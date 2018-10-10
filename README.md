# AlphaPoint.Api
A .Net Standard client for the AlphaPoint API.  Full documentation here: https://alphapoint.github.io/slate

This version is a wrapper around the WebSocket to simplify the sending and receiving of message frames.
A future version will use this as a base to more easily consume API methods and return well-defined objects.

## Example usage

```csharp
using AlphaPoint.Api;
using AlphaPoint.Api.Models;
using AlphaPoint.Api.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                Console.WriteLine("\nPress <enter> key to exit.");
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
                    //Console.WriteLine("  o > " + e.MessageFrameResponse.Payload);
                    Console.WriteLine();

                    // Demo how to work with the result.
                    if (e.MessageFrameResponse.FunctionName == "GetL2Snapshot")
                    {
                        var l2 = e.MessageFrameResponse.PayloadAs<List<Level2Snapshot>>();

                        Console.WriteLine($"{"Date/Time",22} |{"Type",8} |{"Price",10} |{ "Quantity",11} |{"Side",5}");
                        Console.WriteLine(new string('-', 65));

                        foreach (var item in l2)
                        {
                            Console.WriteLine($"{PosixTimeStampToDateTime(item.ActionDateTime),22} |" +
                                $"{item.ActionType,8} |" +
                                $"{item.Price.ToString("N2"),10} |" +
                                $"{item.Quantity.ToString("N8"),11} |" +
                                $"{item.Side,5}");
                        }
                    }

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

        public static DateTime PosixTimeStampToDateTime(double timeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(timeStamp).ToLocalTime();
            return dt;
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

             Date/Time |    Type |     Price |   Quantity | Side
-----------------------------------------------------------------
 2018-10-10 6:52:31 PM |     New |  8,530.00 | 0.70344880 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,500.00 | 0.01300000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,460.01 | 0.63737062 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,459.89 | 0.57906748 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,459.37 | 0.02523204 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,459.12 | 0.13396543 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,458.86 | 0.39436493 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,458.48 | 0.00572819 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,457.97 | 0.21305503 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,457.84 | 0.07534153 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,457.33 | 0.18856173 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,457.07 | 0.00572370 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,456.82 | 0.49919611 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,456.69 | 0.03960801 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,456.30 | 0.38466649 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,456.18 | 0.07527020 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,455.66 | 1.60153236 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,455.28 | 0.75478451 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,455.15 | 1.49758832 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,455.03 | 7.00000000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,453.87 | 0.16392300 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,453.36 | 0.99839222 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,453.23 | 0.49919611 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,453.11 | 1.95684874 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,452.98 | 2.25353822 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,452.08 | 0.49919611 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,451.96 | 0.03801526 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,246.70 | 0.41800000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,246.45 | 0.25000000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,238.48 | 0.51100000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,238.23 | 0.32000000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,237.60 | 0.30000000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,200.00 | 0.21392684 |  Buy
 2018-10-10 6:52:31 PM |     New |      0.01 |31.00000000 |  Buy
 2018-10-10 6:52:31 PM |     New |  8,671.92 | 0.10826416 | Sell
 2018-10-10 6:52:31 PM |     New |  8,672.05 | 1.42836521 | Sell
 2018-10-10 6:52:31 PM |     New |  8,672.19 | 2.00000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,672.32 | 0.72176110 | Sell
 2018-10-10 6:52:31 PM |     New |  8,672.59 | 2.00000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,762.29 | 7.00000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,762.42 | 2.00000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,762.55 | 0.06077132 | Sell
 2018-10-10 6:52:31 PM |     New |  8,762.69 | 0.76951331 | Sell
 2018-10-10 6:52:31 PM |     New |  8,762.82 | 0.00999000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,763.75 | 0.02879279 | Sell
 2018-10-10 6:52:31 PM |     New |  8,764.14 | 0.03079200 | Sell
 2018-10-10 6:52:31 PM |     New |  8,764.28 | 3.00000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,765.34 | 1.00000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,765.60 | 3.20000000 | Sell
 2018-10-10 6:52:31 PM |     New |  8,765.73 | 2.42500103 | Sell
 2018-10-10 6:52:31 PM |     New |  8,765.87 | 0.06077132 | Sell
 2018-10-10 6:52:31 PM |     New |  8,766.00 | 1.13603811 | Sell
 2018-10-10 6:52:31 PM |     New |  8,766.53 | 0.48787545 | Sell
 2018-10-10 6:52:31 PM |     New |  8,766.66 | 0.14720710 | Sell
 2018-10-10 6:52:31 PM |     New |  8,766.79 | 0.19520208 | Sell
 2018-10-10 6:52:31 PM |     New |  8,767.46 | 0.07479691 | Sell
 2018-10-10 6:52:31 PM |     New |  8,767.59 | 0.54720711 | Sell
 2018-10-10 6:52:31 PM |     New |  8,767.72 | 0.73224120 | Sell
 2018-10-10 6:52:31 PM |     New |  8,768.38 | 0.68658606 | Sell
 2018-10-10 6:52:31 PM |     New |  8,768.52 | 0.41307854 | Sell
 2018-10-10 6:52:31 PM |     New |  8,768.65 | 0.19520208 | Sell
 2018-10-10 6:52:31 PM |     New |  8,769.44 | 0.19651065 | Sell
 2018-10-10 6:52:31 PM |     New |  8,769.71 | 0.41720609 | Sell
 2018-10-10 6:52:31 PM |     New |  8,769.84 | 7.00000000 | Sell

Press <enter> key to exit.

```

## My related projects

* [Ndax.Api](https://github.com/RobJohnston/Ndax.Api)