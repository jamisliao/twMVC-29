using Newtonsoft.Json;

namespace twMVC29.Model
{
    public class BikeStation
    {
        [JsonProperty("sna")]
        public string StationName { get; set; }

        [JsonProperty("tot")]
        public string TotalSpace { get; set; }

        [JsonProperty("sbi")]
        public string AvaiableBike { get; set; }

        [JsonProperty("sarea")]
        public string Area { get; set; }

        [JsonProperty("mday")]
        public string TimeStamp { get; set; }

        [JsonProperty("lat")]
        public string Lat { get; set; }

        [JsonProperty("lng")]
        public string Lng { get; set; }

        [JsonProperty("ar")]
        public string Address { get; set; }

        [JsonProperty("bemp")]
        public string Bemp { get; set; }
    }
}