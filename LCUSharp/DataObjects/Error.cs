using Newtonsoft.Json;

namespace LCUSharp.DataObjects
{
    public class Error
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }
        [JsonProperty("httpStatus")]
        public int HttpStatus { get; set; }
        // public string[] implementationDetails { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
