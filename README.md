# AlphaPoint.Api
A .Net Standard client for the AlphaPoint API.  Full documentation here: https://alphapoint.github.io/slate

This client will take care of the message framing and sequence numbering required by the API 
and abstracts-away dealing with the low-level WebSocket.

If you need to make a call to a function that I haven't yet incorporated, it can be done by using the 
included `AlphaPointWebSocketClient` class directly.

Future versions will include more of the API calls.

## Example usage

```csharp
using System;
using System.Threading.Tasks;
using AlphaPoint.Api;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var task = Task.Run(() => MainAsync());
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                // Keep the console window open.
                Console.WriteLine("\nPress <enter> key to exit.");
                var keyInfo = Console.ReadKey();
                while (keyInfo.Key != ConsoleKey.Enter)
                    keyInfo = Console.ReadKey();
            }
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Hello NDAX!\n");

            var url = "wss://api.ndax.io/WSGateway/";

            using (var client = new AlphaPointClient(new Uri(url)))
            {
                // Ping the server to test connectivity.
                var ping = await client.PingAsync();
                Console.Write(ping.Msg);
                Console.WriteLine("\n");

                // See what instruments are available here.
                var instruments = await client.GetInstrumentsAsync();

                foreach(var instrument in instruments)
                {
                    Console.WriteLine(string.Format("Instrument ID {0} is {1}.", 
                        instrument.InstrumentId, instrument.Symbol));
                }

                Console.WriteLine();

                // Show part of the Level 2 Snapshot information for Instrument ID 1.
                var snapshot = await client.GetL2SnapshotAsync(1);
                Console.WriteLine($"{"Date/Time",22} |{"Type",8} |{"Price",10} |{ "Quantity",11} |{"Side",5}");
                Console.WriteLine(new string('-', 65));

                foreach (var item in snapshot)
                {
                    Console.WriteLine($"{PosixTimeStampToDateTime(item.ActionDateTime),22} |" +
                               $"{item.ActionType,8} |" +
                               $"{item.Price.ToString("N2"),10} |" +
                               $"{item.Quantity.ToString("N8"),11} |" +
                               $"{item.Side,5}");
                }
            }
        }

        public static DateTime PosixTimeStampToDateTime(double timeStamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(timeStamp).ToLocalTime();
            return dt;
        }
    }
}
```

### Output
```
Hello NDAX!

PONG

Instrument ID 1 is BTCCAD.
Instrument ID 2 is BCHCAD.
Instrument ID 3 is ETHCAD.
Instrument ID 4 is XRPCAD.
Instrument ID 5 is LTCCAD.
Instrument ID 74 is BTCUSD.
Instrument ID 75 is EOSCAD.

             Date/Time |    Type |     Price |   Quantity | Side
-----------------------------------------------------------------
 2018-10-11 8:36:38 PM |     New |  8,100.00 | 0.02980000 |  Buy
 2018-10-11 8:36:38 PM |     New |  8,010.00 | 0.03000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,984.60 | 7.00000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,984.47 | 3.71426262 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,983.96 | 1.07221356 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,983.45 | 0.05537446 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,983.20 | 0.40000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.94 | 0.06537152 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.81 | 0.12789257 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.68 | 3.57287860 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.56 | 3.00000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.43 | 5.37413070 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.30 | 0.25052271 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,982.05 | 0.28000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,981.79 | 0.20842018 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,980.77 | 1.20000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,980.64 | 0.02995682 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,980.26 | 1.27051435 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,979.88 | 0.01213154 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,979.75 | 0.59401625 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,979.49 | 1.03200000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,978.98 | 0.40000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,978.86 | 3.57287860 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,977.96 | 3.00000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,977.45 | 0.30000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,977.20 | 1.00000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,976.94 | 0.50000309 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,950.00 | 0.05100000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,900.00 | 0.03000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,800.00 | 0.03000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  7,700.00 | 0.03330000 |  Buy
 2018-10-11 8:36:38 PM |     New |      0.01 |31.00000000 |  Buy
 2018-10-11 8:36:38 PM |     New |  8,229.16 | 0.08023071 | Sell
 2018-10-11 8:36:38 PM |     New |  8,233.23 | 0.54091185 | Sell
 2018-10-11 8:36:38 PM |     New |  8,233.37 | 0.02871839 | Sell
 2018-10-11 8:36:38 PM |     New |  8,233.63 | 0.41213155 | Sell
 2018-10-11 8:36:38 PM |     New |  8,234.42 | 0.17950371 | Sell
 2018-10-11 8:36:38 PM |     New |  8,234.55 | 4.10000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,235.34 | 0.23980893 | Sell
 2018-10-11 8:36:38 PM |     New |  8,235.73 | 0.07993197 | Sell
 2018-10-11 8:36:38 PM |     New |  8,235.86 | 1.11906448 | Sell
 2018-10-11 8:36:38 PM |     New |  8,236.00 | 0.31974523 | Sell
 2018-10-11 8:36:38 PM |     New |  8,236.26 | 0.04262798 | Sell
 2018-10-11 8:36:38 PM |     New |  8,236.65 | 3.63949046 | Sell
 2018-10-11 8:36:38 PM |     New |  8,236.79 | 0.58249845 | Sell
 2018-10-11 8:36:38 PM |     New |  8,236.92 | 5.72000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,237.31 | 1.15986947 | Sell
 2018-10-11 8:36:38 PM |     New |  8,237.84 | 1.10960522 | Sell
 2018-10-11 8:36:38 PM |     New |  8,237.97 | 1.27898091 | Sell
 2018-10-11 8:36:38 PM |     New |  8,238.23 | 0.72482859 | Sell
 2018-10-11 8:36:38 PM |     New |  8,238.89 | 0.08023071 | Sell
 2018-10-11 8:36:38 PM |     New |  8,239.15 | 5.75000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,239.28 | 0.40000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,240.07 | 1.00000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,240.20 | 1.71000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,240.86 | 0.30000000 | Sell
 2018-10-11 8:36:38 PM |     New |  8,241.65 | 0.30000000 | Sell

Press <enter> key to exit.

```

## My related projects

* [Ndax.Api](https://github.com/RobJohnston/Ndax.Api)