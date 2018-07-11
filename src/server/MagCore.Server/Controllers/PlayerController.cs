using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MagCore.Model;
using Microsoft.AspNetCore.Mvc;
using SystemCommonLibrary.Json;

namespace MagCore.Server.Controllers
{
    [Route("api/[controller]")]
    public class PlayerController : Controller
    {
        // POST api/player
        [HttpPost]
        public ContentResult Post([FromBody]dynamic json)
        {
            try
            {
                var name = json.Name.ToString().Trim();
                PlayerColor color = (PlayerColor)json.Color;
                var player = Core.Players.NewPlayer(name, color);
                if (player != null)
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.OK, Content = player };
                else
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.Conflict };
            }
            catch
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        // GET api/player/5b4512fb673f4a638fe2907b8483c0ab
        [HttpGet("{id}")]
        public ContentResult Get(string id)
        {
            if (Core.Players.All.ContainsKey(id))
            {
                var player = Core.Players.All[id];
                return new ContentResult() { StatusCode = (int)HttpStatusCode.OK, Content = player.ToJson() };
            }
            else
                return new ContentResult() { StatusCode = (int)HttpStatusCode.NotFound };
        }
    }
}
