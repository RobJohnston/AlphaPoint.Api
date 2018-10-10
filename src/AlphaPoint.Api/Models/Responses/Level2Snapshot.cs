using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AlphaPoint.Api.Models.Responses
{
    [JsonConverter(typeof(Level2SnapshotConverter))]
    public class Level2Snapshot
    {
        /// <summary>
        /// Market Data Update ID. This sequential ID identifies the order in which the update was created.
        /// </summary>
        public int MdUpdateId { get; set; }

        /// <summary>
        /// The number of accounts that have orders at this price level.
        /// </summary>
        public int Accounts { get; set; }

        /// <summary>
        /// ActionDateTime identifies the time and date that the snapshot was taken or the event occurred, in POSIX format X 1000 (milliseconds since 1 January 1970).
        /// </summary>
        public long ActionDateTime { get; set; }

        /// <summary>
        /// L2 information provides price data. This value shows whether this data is new, an update, or a deletion.
        /// </summary>
        public ActionType ActionType { get; set; }

        /// <summary>
        /// The price at which the instrument was last traded.
        /// </summary>
        public decimal LastTradePrice { get; set; }

        /// <summary>
        /// The number of orders at this price point. Whether it is a Buy or Sell order is shown by <see cref="Side"/>.
        /// </summary>
        public int Orders { get; set; }

        /// <summary>
        /// Bid or Ask price for the Quantity (<see cref="Quantity"/>).
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// ProductPairCode is the same value and used for the same purpose as <c>InstrumentID</c>.
        /// </summary>
        public int ProductPairCode { get; set; }

        /// <summary>
        /// Quantity available at a given Bid or Ask price (<see cref="Price"/>).
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// One of: 0 Buy, 1 Sell, 2 Short (reserved for future use), 3 Unknown (error condition).
        /// </summary>
        public Side Side { get; set; }

        private class Level2SnapshotConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JArray ja = JArray.Load(reader);
                Level2Snapshot data = new Level2Snapshot
                {
                    MdUpdateId = (int)ja[0],
                    Accounts = (int)ja[1],
                    ActionDateTime = (long)ja[2],
                    ActionType = (ActionType)Enum.Parse(typeof(ActionType), (string)ja[3]),
                    LastTradePrice = (decimal)ja[4],
                    Orders = (int)ja[5],
                    Price = (decimal)ja[6],
                    ProductPairCode = (int)ja[7],
                    Quantity = (decimal)ja[8],
                    Side = (Side)Enum.Parse(typeof(Side), (string)ja[9]),
                };
                return data;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
