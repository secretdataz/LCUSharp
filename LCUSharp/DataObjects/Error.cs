using JsonFx.Json;

namespace LCUSharp.DataObjects
{
    public class Error
    {
        [JsonName("errorCode")]
        public string errorCode { get; set; }
        [JsonName("httpStatus")]
        public int httpStatus { get; set; }

        /*[JsonName("implementationDetails")]
        public string[] implementationDetails { get; set; }*/
        [JsonName("message")]
        public string message { get; set; }

    }
}
