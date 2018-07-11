using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MagCore.Model;
using Microsoft.AspNetCore.Mvc;
using SystemCommonLibrary.Json;

namespace MagCore.Server.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        // GET api/game
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return Core.Server.GameList();
        }

        // GET api/game/5b4512fb673f4a638fe2907b7483c0ab
        [HttpGet("{id}")]
        public string Get(string id)
        {
            var game = Core.Server.Game(id);
            if (game == null)
                return "{}";
            else
                return game.ToJson();
        }

        // POST api/game - New game
        [HttpPost]
        public ContentResult Post([FromBody]dynamic json)
        {
            try
            {
                var map = json.Map.ToString();
                var game = Core.Server.NewGame(map);
                if (!string.IsNullOrEmpty(game))
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.OK, Content = game };
                else
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest };
            }
            catch
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        // PATCH api/game - Join game
        [HttpPatch]
        public ContentResult Patch([FromBody]dynamic json)
        {
            try
            {
                var game = json.Game.ToString();
                var player = json.Player.ToString();
                if (Core.Server.Join(game, player))
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.OK };
                else
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.Forbidden };
            }
            catch
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest };
            }
        }

        // Put api/game/5b4512fb673f4a638fe2907b7483c0ab - Start game
        [HttpPut("{id}")]
        public ContentResult Put(string id)
        {
            if (Core.Server.StartGame(id))
                return new ContentResult() { StatusCode = (int)HttpStatusCode.OK };
            else
                return new ContentResult() { StatusCode = (int)HttpStatusCode.Forbidden };
        }
    }
}
