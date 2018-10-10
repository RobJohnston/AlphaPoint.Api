# AlphaPoint.Api
A .Net Standard client for the AlphaPoint API.  Full documentation here: https://alphapoint.github.io/slate

This version is a wrapper around the WebSocket to simplify the sending and receiving of message frames.
A future version will use this as a base to more easily consume API methods and return well-defined objects.

## Example usage

```csharp
using AlphaPoint.Api;
using AlphaPoint.Api.Models;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly AutoResetEvent ExitEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            try
            {
                var task = Task.Run(() => MainAsync());
                task.Wait();

                // Keep the console window open.
                Console.WriteLine("Press <enter> key to exit.");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                while (keyInfo.Key != ConsoleKey.Enter)
                    keyInfo = Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Hello NDAX!");

            var uri = "wss://api.ndax.io/WSGateway/";

            using (var socket = new AlphaPointWebSocketClient(new Uri(uri)))
            {
                // Handle events
                socket.ReceivedMessageFrame += (o, e) =>
                {
                    Console.WriteLine("MessageFrame received:");
                    Console.WriteLine("  m > " + e.MessageFrameResponse.MessageType);
                    Console.WriteLine("  i > " + e.MessageFrameResponse.SequenceNumber);
                    Console.WriteLine("  n > " + e.MessageFrameResponse.FunctionName);
                    Console.WriteLine("  o > " + e.MessageFrameResponse.Payload);

                    ExitEvent.Set();
                };

                // Create the payload
                dynamic payload = new System.Dynamic.ExpandoObject();
                payload.OMSId = 1;
                payload.InstrumentId = 1;
                payload.Depth = 100;

                // Wrap a message into a frame object.
                var frame = new MessageFrame()
                {
                    MessageType = MessageType.Request,
                    SequenceNumber = 2,
                    FunctionName = "GetL2Snapshot",
                    Payload = JsonConvert.SerializeObject(payload),
                };

                // Start the client and send the message.
                await socket.Start();
                await socket.Send(frame);

                ExitEvent.WaitOne();
            }
        }
    }
}
```

### Output
```
Hello NDAX!
MessageFrame received:
  m > Reply
  i > 2
  n > GetL2Snapshot
  o > [[33653795,1,1539134020119,0,8550,1,8449.81,1,7,0],
[33653795,1,1539134020119,0,8550,1,8449.43,1,1.21310602,0],
[33653795,1,1539134020119,0,8550,1,8448.67,1,0.07465268,0],
[33653795,1,1539134020119,0,8550,1,8448.29,1,0.14189868,0],
[33653795,1,1539134020119,0,8550,1,8448.16,1,2.95022608,0],
[33653795,1,1539134020119,0,8550,1,8447.27,1,0.00253175,0],
[33653795,1,1539134020119,0,8550,1,8447.14,1,0.93315848,0],
[33653795,1,1539134020119,0,8550,1,8446.76,1,0.01014973,0],
[33653795,1,1539134020119,0,8550,1,8446.38,1,2.23211508,0],
[33653795,1,1539134020119,0,8550,1,8446.25,1,0.00922703,0],
[33653795,1,1539134020119,0,8550,1,8446,1,1.39973772,0],
[33653795,1,1539134020119,0,8550,1,8444.35,1,4.54448179,0],
[33653795,1,1539134020119,0,8550,1,8444.22,1,0.1403765,0],
[33653795,1,1539134020119,0,8550,1,8443.71,1,0.13997377,0],
[33653795,1,1539134020119,0,8550,1,8443.46,1,1.17992127,0],
[33653795,1,1539134020119,0,8550,1,8443.2,1,7,0],
[33653795,1,1539134020119,0,8550,1,8443.07,1,0.56167038,0],
[33653795,1,1539134020119,0,8550,1,8442.82,1,1.27881168,0],
[33653795,1,1539134020119,0,8550,1,8442.69,1,3.82594977,0],
[33653795,1,1539134020119,0,8550,1,8442.18,1,0.21066443,0],
[33653795,1,1539134020119,0,8550,1,8441.93,1,2.3328962,0],
[33653795,1,1539134020119,0,8550,1,8441.8,1,0.84265773,0],
[33653795,1,1539134020119,0,8550,1,8441.68,1,0.37326339,0],
[33653795,1,1539134020119,0,8550,1,8441.55,1,2.26066101,0],
[33653795,1,1539134020119,0,8550,1,8441.42,1,0.1866317,0],
[33653795,1,1539134020119,0,8550,1,8349.0077,1,1.67684598,0],
[33653795,2,1539134020119,0,8550,2,8200,1,0.21392684,0],
[33653795,0,1539134020119,0,8550,1,0.01,1,31,0],
[33653795,1,1539134020119,0,8550,1,8550,1,1.62613,1],
[33653795,1,1539134020119,0,8550,1,8600,1,2,1],
[33653795,1,1539134020119,0,8550,1,8700,1,2,1],
[33653795,1,1539134020119,0,8550,1,8751.73,1,7,1],
[33653795,1,1539134020119,0,8550,1,8751.86,1,0.38479167,1],
[33653795,1,1539134020119,0,8550,1,8752.25,1,0.1,1],
[33653795,1,1539134020119,0,8550,1,8752.38,1,0.12,1],
[33653795,1,1539134020119,0,8550,1,8752.78,1,0.41444587,1],[
33653795,1,1539134020119,0,8550,1,8753.96,1,0.011,1],
[33653795,1,1539134020119,0,8550,1,8754.1,1,1.20780263,1],
[33653795,1,1539134020119,0,8550,1,8754.23,1,0.01,1],
[33653795,1,1539134020119,0,8550,1,8756.2,1,0.17919283,1],
[33653795,1,1539134020119,0,8550,1,8756.33,1,0.4,1],
[33653795,1,1539134020119,0,8550,1,8756.47,1,0.27796986,1],
[33653795,1,1539134020119,0,8550,1,8756.6,1,0.63324654,1],
[33653795,1,1539134020119,0,8550,1,8756.73,1,0.18190475,1],
[33653795,1,1539134020119,0,8550,1,8757.26,1,0.02199081,1],
[33653795,1,1539134020119,0,8550,1,8757.39,1,1.98,1],
[33653795,1,1539134020119,0,8550,1,8757.52,1,0.28644275,1],
[33653795,1,1539134020119,0,8550,1,8757.65,1,0.12800312,1],
[33653795,1,1539134020119,0,8550,1,8757.78,1,0.3,1],
[33653795,1,1539134020119,0,8550,1,8757.92,1,0.026,1],
[33653795,1,1539134020119,0,8550,1,8758.05,1,0.22575335,1],
[33653795,1,1539134020119,0,8550,1,8758.18,1,7,1],
[33653795,1,1539134020119,0,8550,1,8758.44,1,0.2,1],
[33653795,1,1539134020119,0,8550,1,8758.57,1,0.13885664,1],
[33653795,1,1539134020119,0,8550,1,8758.71,1,0.65440634,1],
[33653795,1,1539134020119,0,8550,1,8758.84,1,0.1772403,1],
[33653795,1,1539134020119,0,8550,1,8800,1,2,1]]
Press <enter> key to exit.
```

## My related projects

* [Ndax.Api](https://github.com/RobJohnston/Ndax.Api)