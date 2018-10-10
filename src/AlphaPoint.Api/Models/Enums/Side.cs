namespace AlphaPoint.Api.Models
{
    /// <summary>
    /// Represents one side of a trade.  Every trade has two sides.
    /// </summary>
    public enum Side
    {
        /// <summary>
        /// The buy side of a trade..
        /// </summary>
        Buy = 0,

        /// <summary>
        /// The sell side of a trade.
        /// </summary>
        Sell = 1,

        /// <summary>
        /// Short (reserved for future use).
        /// </summary>
        Short = 2,

        /// <summary>
        /// Unknown (error condition).
        /// </summary>
        Unknown = 3
    }
}
