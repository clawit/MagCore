using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules
{
    public static class ApiReq
    {
        private static string _url = "http://106.75.33.221:6000/";
        //private static string _url = "http://localhost:6000/";

        public static void SetUrl(string url)
        {
            _url = url;
        }


        public static HttpClient CreateReq()
        {
            var client = new HttpClient() { BaseAddress = new Uri(_url) }; 
            return client;
        }

        public static async Task<HttpResponseMessage> WithMethod(this HttpClient request, string url, string method, HttpContent content = null)
        {
            HttpResponseMessage response = null;
            switch (method.Trim().ToUpper())
            {
                case "GET":
                    response = await request.GetAsync(url);
                    break;
                case "POST":
                    response = await request.PostAsync(url, content);
                    break;
                case "PUT":
                    response = await request.PutAsync(url, content);
                    break;
                case "DELETE":
                    response = await request.GetAsync(url);
                    break;
                case "PATCH":
                    response = await request.PatchAsync(url, content);
                    break;
                default:
                    break;
            }

            return response;
        }

        public static HttpStatusCode GetResult(this Task<HttpResponseMessage> response, out string result)
        {
            result = string.Empty;
            try
            {
                var msg = response.Result;
                result = msg.Content.ReadAsStringAsync().Result;
                return msg.StatusCode;
            }
            catch
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }

    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string url, HttpContent content)
        {
            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            HttpResponseMessage response = new HttpResponseMessage();
            // In case you want to set a timeout
            //CancellationToken cancellationToken = new CancellationTokenSource(60).Token;

            try
            {
                response = await client.SendAsync(request);
                // If you want to use the timeout you set
                //response = await client.SendRequestAsync(request).AsTask(cancellationToken);
            }
            catch (TaskCanceledException)
            {
                //timeout
            }

            return response;
        }
    }

}
