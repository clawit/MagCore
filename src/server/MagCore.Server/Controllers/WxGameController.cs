using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MagCore.Core;
using MagCore.Server.WxApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MagCore.Server.Controllers
{
    [Route("api/[controller]")]
    public class WxGameController : Controller
    { 
        // GET api/game/5b4512fb673f4a638fe2907b7483c0ab
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            try
            {
                var content = WxMiniGame.GetGameCode(id);
                var result = content.ReadAsByteArrayAsync().Result;
                return new FileContentResult(result, "image/jpeg");
            } 
            catch (Exception ex)
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest, Content = ex.Message };
            }
        }

        // GET api/game/Y5b4512fb673f4/id
        [HttpGet("{code}/id")]
        public ContentResult GetId(string code)
        {
            try
            {
                string id = GuidCompacter.Uncompact(code);
                return new ContentResult() { StatusCode = (int)HttpStatusCode.OK, Content = id };
            }
            catch(Exception ex)
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest, Content = ex.Message };
            }
        }
    }
}
