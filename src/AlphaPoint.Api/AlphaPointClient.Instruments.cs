using AlphaPoint.Api.Models.Responses;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace AlphaPoint.Api
{
    public partial class AlphaPointClient
    {
        /// <summary>
        /// Retrieves the details of a specific instrument from the Order Management System of the trading venue.
        /// </summary>
        /// <param name="instrumentId">The ID of the instrument.</param>
        /// <returns>The response returns a single instrument available on the Order Management System.</returns>
        /// <seealso cref="GetInstrumentsAsync()"/>
        /// <seealso cref="GetProductAsync(long)"/>
        /// <seealso cref="GetProductsAsync()"/>
        public async Task<Instrument> GetInstrumentAsync(long instrumentId)
        {
            dynamic payload = new ExpandoObject();
            payload.OMSId = _omsId;
            payload.InstrumentId = instrumentId;

            return await QueryAsync<Instrument>("GetInstrument", payload);
        }

        /// <summary>
        /// Retrieves a list of instruments available on the exchange.
        /// </summary>
        /// <returns>
        /// The response is an array of objects listing all the instruments available on the Order Management System.
        /// </returns>
        /// <seealso cref="GetInstrumentAsync(long)"/>
        /// <seealso cref="GetProductAsync(long)"/>
        /// <seealso cref="GetProductsAsync()"/>
        public async Task<List<Instrument>> GetInstrumentsAsync()
        {
            dynamic payload = new ExpandoObject();
            payload.OMSId = _omsId;

            return await QueryAsync<List<Instrument>>("GetInstruments", payload);
        }
    }
}
