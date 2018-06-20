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
        private static string _url = "http://localhost:6000/";

        public static HttpClient CreateReq()
        {
            var client = new HttpClient() { BaseAddress = new Uri(_url) }; 
            return client;
        }

        public static async Task<HttpResponseMessage> AddMethod(this HttpClient request, string url, string method, HttpContent content = null)
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

        public static string GetResult(this Task<HttpResponseMessage> response)
        {
            var res = response.Result.EnsureSuccessStatusCode();
            var contentType = res.Content.Headers.ContentType;
            if (string.IsNullOrEmpty(contentType.CharSet))
                contentType.CharSet = "GBK";

            return res.Content.ReadAsStringAsync().Result;
        }
    }
}
