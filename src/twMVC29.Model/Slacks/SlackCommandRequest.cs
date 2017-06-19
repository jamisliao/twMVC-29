using Newtonsoft.Json;
namespace twMVC29.Model
{
    public class SlackCommandRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("text")]
        public string Area { get; set; }
    }
}