using System;
using System.Net;
using System.Net.Http;
using System.Text;
using MagCore.Core;

namespace MagCore.Server.WxApi
{
    public static class WxMiniGame
    {
        private static string _codeUrl = "wxa/getwxacodeunlimit?access_token=";
        public static HttpContent GetGameCode(string gid)
        {
            try
            {
                string token = WxBase.GetAccessToken();
                string gcode = GuidCompacter.Compact(gid);

                string parms = string.Format("{{\"scene\":\"m-{0}\", \"auto_color\": true, \"width\": 256}}", gcode);
                var content = new StringContent(parms, Encoding.UTF8, "application/json");

                var client = new HttpClient() { BaseAddress = new Uri(WxBase.BaseUrl) };
                var response = client.PostAsync(_codeUrl + token, content).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.Content;
                }
                else
                    throw new Exception("微信服务器错误");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
