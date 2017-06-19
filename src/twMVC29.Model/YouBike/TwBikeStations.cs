using System.Collections.Generic;
using Newtonsoft.Json;

namespace twMVC29.Model.YouBike
{
    public class TwBikeStations
    {
        [JsonProperty("records")]
        public List<BikeStation> Stations  { get; set; }
    }
}   