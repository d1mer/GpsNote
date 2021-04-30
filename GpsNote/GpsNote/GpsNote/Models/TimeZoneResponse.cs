using System.Text.Json.Serialization;

namespace GpsNote.Models
{
    //[Serializable()]
    //[JsonProperty("TimeZoneResponse")]
    public class TimeZoneResponse
    {
        [JsonPropertyName("dstOffset")]
        public int DstOffset { get; set; }

        [JsonPropertyName("rawOffset")]
        public int RawOffset { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("timeZoneId")]
        public string TimeZoneID { get; set; }

        [JsonPropertyName("timeZoneName")]
        public string TimeZoneName { get; set; }
    }
}