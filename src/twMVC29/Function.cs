using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using twMVC29.Model;
using twMVC29.Model.YouBike;
using twMVC29.Model.Slacks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace twMVC29
{
    public class Function
    {
        private static HttpClient _httpClient = new HttpClient();
        static void Main()
        {
            var text = "token=tDmqKIwv6xtKMmoF9AU5DEc3&team_id=T3CQHTN4W&team_domain=asosnotification&channel_id=D3C2P4YKV&channel_name=directmessage&user_id=U3C779HJR&user_name=jamisliao&command=%2Farea&text=%E4%B8%AD%E5%92%8C%E5%8D%80+true&response_url=https%3A%2F%2Fhooks.slack.com%2Fcommands%2FT3CQHTN4W%2F199849202002%2FBzeyeu9lfua81lfrFjlf7MqE";
            //var result = new Function().FunctionHandler(new LambdaRequest{Body = text});

        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(LambdaRequest request, ILambdaContext context)
        {
            context.Logger.Log(request.Body);
            var parameter = request.Body.Split('&').ToList().First(p => p.Contains("text"));
            var tmp = parameter.Split('=');
            var value = System.Net.WebUtility.UrlDecode(tmp[1]);
            var areaName = value.Split(' ')[0];
            var isShowMap = Convert.ToBoolean(value.Split(' ')[1]);

            var uri = "http://data.ntpc.gov.tw/api/v1/rest/datastore/382000000A-000352-001";
            var response = _httpClient.GetAsync(uri).Result;
            var jsonContent = response.Content.ReadAsStringAsync().Result;
            context.Logger.Log(jsonContent);
            var result = JsonConvert.DeserializeObject<UBikeResponse>(jsonContent);
            
            var areaStations = result.Result.Stations.Where(p => p.Area == areaName).ToList();

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

                if(isShowMap)
                {
                    attachment.ImageUrl = $"https://maps.googleapis.com/maps/api/staticmap?center={item.Lat},{item.Lng}&zoom=18&size=200x200&maptype=roadmap&markers=color:red%7Clabel:%7C{item.Lat},{item.Lng}&scale=2";
                }
                slackMessag.Attachments.Add(attachment);
            }

            var json = JsonConvert.SerializeObject(slackMessag);
            //context.Logger.Log(json);

            var url = "https://hooks.slack.com/services/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";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var slackResponse = _httpClient.PostAsync(url, content).Result;

            var apiResponse = new APIGatewayProxyResponse
            {
                StatusCode = (int)slackResponse.StatusCode,
                Body = "OK",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            return apiResponse;
        }
    }
}
