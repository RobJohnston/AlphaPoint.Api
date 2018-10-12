using AlphaPoint.Api.Models.Responses;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace AlphaPoint.Api
{
    public partial class AlphaPointClient
    {
        /// <summary>
        /// Gets the earliest ticker time of the trading day.
        /// </summary>
        /// <param name="instrumentId">The ID of an instrument on the Order Management System for which the earliest ticker time is being requested.</param>
        /// <returns>
        /// Time of the earliest ticker tick, in POSIX format.  Despite the array, the response returns a single value.
        /// </returns>
        public async Task<List<long>> GetEarliestTickTimeAsync(int instrumentId)
        {
            dynamic payload = new ExpandoObject();
            payload.OMSId = _omsId;
            payload.InstrumentId = instrumentId;

            return await QueryAsync<List<long>>("GetEarliestTickTime", payload);
        }

        /// <summary>
        /// Used to keep a connection alive.
        /// </summary>
        /// <returns>Response is PONG.</returns>
        public async Task<Pong> PingAsync()
        {
            dynamic payload = null;

            return await QueryAsync<Pong>("Ping", payload);
        }
    }
}
