namespace AlphaPoint.Api.Models
{
    /// <summary>
    /// Represents the type of L2 information price data.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// New data.
        /// </summary>
        New = 0,

        /// <summary>
        /// Updated data.
        /// </summary>
        Update = 1,

        /// <summary>
        /// Deleted data.
        /// </summary>
        Delete = 2,
    }
}
