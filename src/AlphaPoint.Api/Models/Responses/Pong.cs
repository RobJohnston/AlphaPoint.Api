using Newtonsoft.Json;

namespace AlphaPoint.Api.Models.Responses
{
    /// <summary>
    /// Represents the response of a ping request.
    /// </summary>
    public class Pong
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }
    }
}
