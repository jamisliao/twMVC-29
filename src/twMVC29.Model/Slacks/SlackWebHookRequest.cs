using System.Collections.Generic;
using Newtonsoft.Json;

namespace twMVC29.Model.Slacks
{
    public class SlackWebHookRequest
    {
        public SlackWebHookRequest()
        {
            this.Attachments = new List<Attachment>();
        }
        
        [JsonProperty("attachments")]
        public List<Attachment> Attachments  { get; set; }
    }
}