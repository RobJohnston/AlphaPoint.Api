using AlphaPoint.Api.Models;
using AlphaPoint.Api.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaPoint.Api
{
    public partial class AlphaPointClient : IDisposable
    {
        private static readonly UTF8Encoding _encoder = new UTF8Encoding();

        #region Fields

        private readonly int _omsId;
        private long _sequenceNumber;
        private AlphaPointWebSocketClient _socket;
        private readonly Uri _uri;

        #endregion

        #region Constructors

        public AlphaPointClient(Uri uri)
        {
            _omsId = 1;
            _sequenceNumber = 0;
            _uri = uri;

            _socket = new AlphaPointWebSocketClient(uri);
            _socket.Start();
        }

        public AlphaPointClient(Uri uri, int omsId) : this(uri)
        {
            _omsId = omsId;
        }

        #endregion

        public void Dispose()
        {
            ((IDisposable)_socket).Dispose();
        }

        #region  Private methods

        internal async Task<T> QueryAsync<T>(string functionName, object payload = null)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            // Wrap all calls in a frame object.
            var frame = new MessageFrame
            {
                FunctionName = functionName,
                MessageType = MessageType.Request,
                SequenceNumber = GetNextSequenceNumber(),
                Payload = JsonConvert.SerializeObject(payload)
            };

            return await SendMessageFrameAsync<T>(frame);
        }

        internal async Task<T> SubscribeAsync<T>(string functionName, object payload = null)
        {
            return await QueryAsync<T>(functionName, payload);
        }

        internal async Task<T> UnsubscribeAsync<T>(string functionName, object payload = null)
        {
            return await QueryAsync<T>(functionName, payload);
        }

        internal async Task<T> SendMessageFrameAsync<T>(MessageFrame frame)
        {
            TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
            Task<T> handlerFinished = tcs.Task;

            _socket.ReceivedMessageFrame += (o, e) =>
            {
                try
                {
                    Debug.Indent();
                    Debug.WriteLine("\nMessageFrame:");
                    Debug.WriteLine("m > " + e.MessageFrameResponse.MessageType);
                    Debug.WriteLine("i > " + e.MessageFrameResponse.SequenceNumber);
                    Debug.WriteLine("n > " + e.MessageFrameResponse.FunctionName);
                    Debug.WriteLine("o > " + e.MessageFrameResponse.Payload);
                    Debug.Unindent();

                    switch (e.MessageFrameResponse.MessageType)
                    {
                        case MessageType.Reply:
                            if (e.MessageFrameResponse.SequenceNumber == frame.SequenceNumber)
                            {
                                var payload = e.MessageFrameResponse.PayloadAs<T>();
                                tcs.SetResult(payload);
                            }
                            break;
                        case MessageType.Event:
                            switch (e.MessageFrameResponse.FunctionName)
                            {
                                //TODO: Handle TickerDataUpdateEvent, OrderTradeEvent, etc.
                                default:
                                    throw new NotImplementedException(e.MessageFrameResponse.FunctionName);
                            }
                            break;
                        case MessageType.Error:
                            var exception = new AlphaPointException(e.MessageFrameResponse.Payload)
                            {
                                FunctionName = e.MessageFrameResponse.FunctionName,
                                Sequence = e.MessageFrameResponse.SequenceNumber,
                            };
                            break;
                        default:
                            throw new NotImplementedException("Unexpected message type: " + e.MessageFrameResponse.MessageType);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            };

            Debug.WriteLine(string.Format("Sending message #{0}... ", frame.SequenceNumber));
            await _socket.Send(frame);

            return await handlerFinished;
        }

        private void Cancel(CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Task cancelled!");
                ct.ThrowIfCancellationRequested();
            }
        }

        internal long GetNextSequenceNumber()
        {
            // Best practice is to carry an even sequence number.
            Interlocked.Add(ref _sequenceNumber, 2);

            return _sequenceNumber;
        }

        #endregion
    }
}
