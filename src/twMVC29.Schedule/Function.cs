using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Newtonsoft.Json;
using twMVC29.Model.Slacks;
using twMVC29.Model.YouBike;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace twMVC29.Schedule
{
    public class Function
    {
        private static HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(ILambdaContext context)
        {
            var uri = "http://data.ntpc.gov.tw/api/v1/rest/datastore/382000000A-000352-001";
            var response = _httpClient.GetAsync(uri).Result;
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            context.Logger.Log(jsonContent);
            var result = JsonConvert.DeserializeObject<UBikeResponse>(jsonContent);
            
            var areaStations = result.Result.Stations.Where(p => p.Area == "永和區").ToList();

            var slackMessag = new SlackWebHookRequest();
            foreach (var item in areaStations)
            {
                var attachment = new Attachment{
                    Color = "#36a64f",
                    Text = $"*{item.StationName}*",
                    MarkdownIn = new List<string>{"text"},
                    Fields = new List<Field>{
                        new Field{
                            Title = "站點車位數量",
                            Value = $"{item.TotalSpace}",
                            IsShort = true
                        },
                        new Field{
                            Title = "站點可租用數量",
                            Value = $"{item.AvaiableBike}",
                            IsShort = true
                        },
                        new Field{
                            Title = "站點可停車數量",
                            Value = $"{item.Bemp}",
                            IsShort = true
                        }
                    },
                    Footer = $"{item.Address}"
                };

                slackMessag.Attachments.Add(attachment);
            }

            var json = JsonConvert.SerializeObject(slackMessag);
            //context.Logger.Log(json);

            var url = "https://hooks.slack.com/services/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";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var slackResponse = _httpClient.PostAsync(url, content).Result;
        }
    }
}
