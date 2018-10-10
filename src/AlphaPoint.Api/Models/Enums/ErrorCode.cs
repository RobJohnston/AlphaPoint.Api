namespace AlphaPoint.Api.Models
{
    /// <summary>
    /// Return value indicating successful or unsuccessful receipt of a call.
    /// </summary>
    public enum ErrorCode
    {
        Success = 0,

        /// <summary>
        /// Not authorized (error code 20)
        /// </summary>
        NotAuthorized = 20,

        /// <summary>
        /// Invalid request (error code 100)
        /// </summary>
        InvalidRequest = 100,

        /// <summary>
        /// Operation failed (error code 101)
        /// </summary>
        OperationFailed = 101,

        /// <summary>
        /// Server error (error code 102)
        /// </summary>
        ServerError = 102,

        /// <summary>
        /// Resource not found (error code 104).
        /// </summary>
        ResourceNotFound = 104,
    }
}
