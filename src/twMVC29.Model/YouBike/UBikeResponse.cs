using Newtonsoft.Json;

namespace twMVC29.Model.YouBike
{
    public class UBikeResponse
    {
        [JsonProperty("result")]
        public TwBikeStations Result { get; set; }
    }
}