using Newtonsoft.Json;

namespace AlphaPoint.Api.Models.Responses
{
    /// <summary>
    /// A generic response to an API call that verifies that the call was received.
    /// </summary>
    public class GenericResponse
    {
        /// <summary>
        ///  If the call has been successfully received by the Order Management System, result is true; otherwise, it is false.
        /// </summary>
        [JsonProperty("result")]
        bool Result { get; set; }

        /// <summary>
        /// A successful receipt of the call returns null; the errormsg parameter for an unsuccessful call returns one of the following messages:
        ///  Not Authorized (errorcode 20)
        ///  Invalid Request (errorcode 100)
        ///  Operation Failed (errorcode 101)
        ///  Server Error (errorcode 102)
        ///  Resource Not Found (errorcode 104)
        /// </summary>
        [JsonProperty("errormsg")]
        string ErrorMsg { get; set; }

        /// <summary>
        /// A successful receipt of the call returns 0. 
        /// An unsuccessful receipt of the call returns one of the errorcodes shown in the errormsg list.
        /// </summary>
        [JsonProperty("errorcode")]
        ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Message text that the system may send. The content of this parameter is usually null.
        /// </summary>
        [JsonProperty("detail")]
        string Detail { get; set; }
    }
}
