using Newtonsoft.Json;

namespace AlphaPoint.Api.Models.Responses
{
    public class Product
    {
        /// <summary>
        /// The ID of the Order Management System that offers the product
        /// </summary>
        public int OmsId { get; set; }

        /// <summary>
        /// The ID of the product.
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// “Nickname” or shortened name of the product. For example, NZD (New Zealand Dollar).
        /// </summary>
        [JsonProperty("Product")]
        public string Nickname { get; set; }

        /// <summary>
        /// Full and official name of the product. For example, New Zealand Dollar.
        /// </summary>
        public string ProductFullName { get; set; }

        /// <summary>
        /// The nature of the product.
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// The number of decimal places in which the product is divided..
        /// </summary>
        public int DecimalPlaces { get; set; }

        /// <summary>
        /// Minimum tradable quantity of the product.
        /// </summary>
        /// <remarks>
        /// See also <c>GetInstrumentAsync</c>, where this value is called <c>QuantityIncrement</c>.
        /// </remarks>
        public decimal TickSize { get; set; }

        /// <summary>
        /// Shows whether trading the product incurs fees.
        /// </summary>
        public bool NoFees { get; set; }
    }
}
