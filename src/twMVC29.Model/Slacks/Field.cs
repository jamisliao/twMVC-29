using Newtonsoft.Json;

namespace twMVC29.Model.Slacks
{
    public class Field
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public bool IsShort { get; set; }
    }
}