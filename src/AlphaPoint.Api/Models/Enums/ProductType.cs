namespace AlphaPoint.Api.Models
{
    /// <summary>
    /// The nature of the product.
    /// </summary>
    public enum ProductType
    {
        /// <summary>
        /// Unknown.  An error condition.
        /// </summary>
        Unknown = 0,

        NationalCurrency = 1,

        CryptoCurrency = 2,

        Contract = 3
    }
}
