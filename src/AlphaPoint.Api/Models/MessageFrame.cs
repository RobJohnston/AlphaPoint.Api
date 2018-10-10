using Newtonsoft.Json;

namespace AlphaPoint.Api.Models
{
    /// <summary>
    /// Represents a frame object that wraps all requests and responses.
    /// </summary>
    public class MessageFrame
    {
        /// <summary>
        /// The type of the message.
        /// </summary>
        [JsonProperty("m")]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// The sequence number identifies an individual request or request-and-response pair.
        /// </summary>
        /// <remarks>
        /// A non-zero sequence number is required, but the numbering scheme you use is up to you.
        /// </remarks>
        [JsonProperty("i")]
        public long SequenceNumber { get; set; }

        /// <summary>
        /// The function name is the name of the function being called or that the server responds to.
        /// </summary>
        /// <remarks>
        /// The server response echoes the request.
        /// </remarks>
        [JsonProperty("n")]
        public string FunctionName { get; set; }

        /// <summary>
        /// Payload is a JSON-formatted string containing the data being sent with the message.
        /// </summary>
        [JsonProperty("o")]
        public string Payload { get; set; }
    }
}