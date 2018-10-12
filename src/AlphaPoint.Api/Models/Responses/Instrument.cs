using System;

namespace AlphaPoint.Api.Models.Responses
{
    /// <summary>
    /// An instrument is a pair of exchanged products (or fractions of them) such as US dollars and ounces of gold.
    /// </summary>
    public class Instrument
    {
        /// <summary>
        /// The ID of the Order Management System on which the instrument is traded.
        /// </summary>
        public int OmsId { get; set; }

        /// <summary>
        /// The ID of the instrument.
        /// </summary>
        public int InstrumentId { get; set; }

        /// <summary>
        /// Trading symbol of the instrument.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The first product comprising the instrument. For example, USD in a USD/BitCoin instrument.
        /// </summary>
        public int Product1 { get; set; }

        /// <summary>
        /// The symbol for Product 1 on the trading venue. For example, USD.
        /// </summary>
        public string Product1Symbol { get; set; }

        /// <summary>
        /// The second product comprising the instrument. For example, BitCoin in a USD/BitCoin instrument.
        /// </summary>
        public int Product2 { get; set; }

        /// <summary>
        /// The symbol for Product 2 on the trading venue. For example, BTC.
        /// </summary>
        public string Product2Symbol { get; set; }

        /// <summary>
        /// The type of the instrument. All instrument types currently are standard, an exchange of one product for another (or unknown, an error condition), but this may expand to new types in the future.
        /// </summary>
        public InstrumentType InstrumentType { get; set; }

        /// <summary>
        /// If the instrument trades on another trading venue to which the user has access, this value is the instrument ID on that other venue.
        /// </summary>
        /// <see cref="VenueId"/>
        public long VenueInstrumentId { get; set; }

        /// <summary>
        /// The ID of the trading venue on which the instrument trades, if not this venue.
        /// </summary>
        /// <see cref="VenueInstrumentId"/>
        public int VenueId { get; set; }

        /// <summary>
        /// The numerical position in which to sort the returned list of instruments on a visual display.
        /// </summary>
        /// <remarks>
        /// If the call returns information about a single instrument, SortIndex should return 0.
        /// </remarks>
        public int SortIndex { get; set; }

        /// <summary>
        /// Is the market for this instrument currently open and operational?
        /// </summary>
        public SessionStatus SessionStatus { get; set; }

        /// <summary>
        /// What was the previous session status for this instrument?
        /// </summary>
        public SessionStatus PreviousSessionStatus { get; set; }

        /// <summary>
        ///  The time and date at which the session status was reported, in ISO 8601 format.
        /// </summary>
        public DateTime SessionStatusDateTime { get; set; }

        /// <summary>
        /// An account trading with itself still incurs fees. If this instrument prevents an account from trading the instrument with itself, the value returns true; otherwise defaults to false.
        /// </summary>
        public bool SelfTradePrevention { get; set; }

        /// <summary>
        /// The number of decimal places for the smallest quantity of the instrument that can trade (analogous to smallest lot size).
        /// </summary>
        /// <remarks>
        /// For example, the smallest increment of a US Dollar that can trade is 0.01 (one cent, or 2 decimal places). 
        /// Current maximum is 8 decimal places.  The default is 0.
        /// </remarks>
        public decimal QuantityIncrement { get; set; }
    }
}
