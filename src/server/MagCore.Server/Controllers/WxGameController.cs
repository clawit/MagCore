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
        // TO get the code image of Wechat
        // GET api/WxGame/5b4512fb673f4a638fe2907b7483c0ab
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            try
            {
                var game = Core.Server.Game(id);
                if (game == null)
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.NotFound};

                if (game.GameCode != null)
                    return new FileContentResult(game.GameCode, "image/jpeg");
                
                var content = WxMiniGame.GetGameCode(id);
                var result = content.ReadAsByteArrayAsync().Result;

                game.GameCode = result;

                return new FileContentResult(result, "image/jpeg");
            } 
            catch (Exception ex)
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest, Content = ex.Message };
            }
        }

        //To get the code from a game id (for debug)
        // GET api/WxGame/5b4512fb673f4a638fe2907b7483c0ab/code
        [HttpGet("{id}/code")]
        public ContentResult GetCode(string id)
        {
            try
            {
                string gcode = GuidCompacter.Compact(id);
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest, Content = gcode };
            }
            catch (Exception ex)
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest, Content = ex.Message };
            }
        }

        // To get the id from a code
        // GET api/WxGame/Y5b4512fb673f4/id
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
