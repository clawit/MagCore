using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagCore.Sdk.Common
{
    internal static class ApiReq
    {
        internal static string _url = "http://localhost:6000/";

        internal static HttpClient CreateReq()
        {
            var client = new HttpClient() { BaseAddress = new Uri(_url) };
            return client;
        }

        internal static async Task<HttpResponseMessage> WithMethod(this HttpClient request, string url, string method, HttpContent content = null)
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
                default:
                    break;
            }

            return response;
        }

        internal static HttpStatusCode GetResult(this Task<HttpResponseMessage> response, out string result)
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
}
