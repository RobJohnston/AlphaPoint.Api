using AlphaPoint.Api.Models.Responses;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace AlphaPoint.Api
{
    public partial class AlphaPointClient
    {
        /// <summary>
        /// Retrieves the details about a specific product on the trading venue.
        /// </summary>
        /// <param name="productId">The ID of the product on the specified Order Management System.</param>
        /// <returns>The response returns a single product available on the Order Management System.</returns>
        /// <seealso cref="GetInstrumentAsync(long)"/>
        /// <seealso cref="GetInstrumentsAsync()"/>
        /// <seealso cref="GetProductsAsync()"/>
        public async Task<Product> GetProductAsync(long productId)
        {
            dynamic payload = new ExpandoObject();
            payload.OMSId = _omsId;
            payload.ProductId = productId;

            return await QueryAsync<Product>("GetProduct", payload);
        }

        /// <summary>
        /// Retrieves an array of products available on the trading venue.
        /// </summary
        /// <returns>
        /// The response returns an array of objects, one object for each product available on the Order Management System.
        /// </returns>
        /// <seealso cref="GetInstrumentAsync(long)"/>
        /// <seealso cref="GetInstrumentsAsync()"/>
        /// <seealso cref="GetProductAsync(long)"/>
        public async Task<List<Product>> GetProductsAsync()
        {
            dynamic payload = new ExpandoObject();
            payload.OMSId = _omsId;

            return await QueryAsync<List<Product>>("GetProducts", payload);
        }
    }
}
