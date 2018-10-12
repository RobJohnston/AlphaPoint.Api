using AlphaPoint.Api.Models.Responses;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace AlphaPoint.Api
{
    public partial class AlphaPointClient
    {
        /// <summary>
        /// Provides a current Level 2 snapshot of a specific instrument trading on an Order Management System to a user-determined market depth.
        /// The Level 2 snapshot allows the user to specify the level of market depth information on either side of the bid and ask.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Depth in this call is "depth of market," the number of buyers and sellers at greater or lesser prices in the order book for the instrument.
        /// </para>
        /// <para>
        /// The Level2UpdateEvent contains the same data, but is sent by the OMS when trades occur. 
        /// To receive Level2UpdateEvents, a user must subscribe to Level2UpdateEvents.
        /// </para>
        /// </remarks>
        /// <param name="instrumentId">The ID of the instrument that is the subject of the snapshot.</param>
        /// <param name="depth">The number of buyers and sellers at greater or lesser prices in the order book for the instrument.</param>
        /// <returns>The response is a single object for one specific instrument.</returns>
        public async Task<List<Level2Snapshot>> GetL2SnapshotAsync(int instrumentId, int depth = 100)
        {
            dynamic payload = new ExpandoObject();
            payload.OMSId = _omsId;
            payload.InstrumentId = instrumentId;
            payload.Depth = depth;

            return await QueryAsync<List<Level2Snapshot>>("GetL2Snapshot", payload);
        }
    }
}
