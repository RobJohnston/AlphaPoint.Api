using AlphaPoint.Api.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaPoint.Api
{
    public class AlphaPointWebSocketClient : IDisposable
    {
        #region Fields

        private CancellationTokenSource _cancelation;
        private ClientWebSocket _client;
        private bool _disposing = false;
        private readonly Uri _url;
        private readonly int _reconnectDelayInMilliseconds = 30000;

        #endregion

        #region Constructors

        public AlphaPointWebSocketClient(Uri url)
        {
            _url = url;
        }

        #endregion

        #region Events

        public event EventHandler<ReceivedMessageFrameEventArgs> ReceivedMessageFrame;

        #endregion

        #region Public methods

        public Task Start()
        {
            Trace.WriteLine("Starting.");
            _cancelation = new CancellationTokenSource();

            return StartClient(_url, _cancelation.Token);
        }

        public async Task Send(MessageFrame frame)
        {
            Trace.WriteLine(string.Format("Sending frame {0}: {1}", frame.SequenceNumber, frame.Payload));
            var json = JsonConvert.SerializeObject(frame);
            var request = Encoding.UTF8.GetBytes(json);
            var messageSegment = new ArraySegment<byte>(request);
            var client = await GetClient();
            await client.SendAsync(messageSegment, WebSocketMessageType.Text, true, _cancelation.Token);
        }

        public void Dispose()
        {
            _disposing = true;
            Trace.WriteLine("Disposing.");
            _cancelation?.Cancel();
            _client?.Abort();
            _client?.Dispose();
            _cancelation?.Dispose();
            Trace.WriteLine("Disposed.");
        }

        #endregion

        #region Private methods

        private async Task StartClient(Uri uri, CancellationToken token)
        {
            try
            {
                _client = new ClientWebSocket();
                await _client.ConnectAsync(uri, token);
                Listen(_client, token);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                await Task.Delay(_reconnectDelayInMilliseconds, token);
                await Reconnect();
            }
        }

        private async Task<ClientWebSocket> GetClient()
        {
            if (_client == null || (_client.State != WebSocketState.Open && _client.State != WebSocketState.Connecting))
            {
                await Reconnect();
            }
            return _client;
        }

        private async void Listen(ClientWebSocket client, CancellationToken token)
        {
            var bufferSize = 4096;
            var bufferAmplifier = 20;
            var temporaryBuffer = new byte[bufferSize];
            var buffer = new byte[bufferSize * bufferAmplifier];
            var offset = 0;
            var result = string.Empty;
            WebSocketReceiveResult response;

            Trace.WriteLine("Listening...");

            while (client.State == WebSocketState.Open)
            {
                do
                {
                    response = await client.ReceiveAsync(new ArraySegment<byte>(temporaryBuffer), token);
                    temporaryBuffer.CopyTo(buffer, offset);
                    offset += response.Count;
                    temporaryBuffer = new byte[bufferSize];
                } while (!response.EndOfMessage);

                if (response.MessageType != WebSocketMessageType.Close)
                {
                    result = Encoding.UTF8.GetString(buffer);
                    Trace.WriteLine(string.Format("Received message : {0}." + Environment.NewLine, result));

                    var json = Encoding.ASCII.GetString(buffer).TrimEnd('\0');
                    var frame = JsonConvert.DeserializeObject<MessageFrame>(json);

                    ReceivedMessageFrame?.Invoke(this, new ReceivedMessageFrameEventArgs(frame));

                    // Clear the buffer array.
                    buffer = new byte[bufferSize * bufferAmplifier];
                    offset = 0;
                }
                else
                {
                    var message = "Close response received.";
                    Trace.WriteLine(message);

                    await
                        client.CloseAsync(WebSocketCloseStatus.NormalClosure, message, CancellationToken.None);
                }
            }
        }

        private async Task Reconnect()
        {
            if (!_disposing)
            {
                Trace.WriteLine("Reconnecting.");
                _cancelation.Cancel();

                _cancelation = new CancellationTokenSource();
                await StartClient(_url, _cancelation.Token);
            }
        }

        #endregion
    }
}
