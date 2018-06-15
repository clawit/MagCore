using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemCommonLibrary.Json;

namespace MagCore.Server.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        // GET api/game
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Core.Server.GameList();
        }

        // GET api/game/5b4512fb673f4a638fe2907b7483c0ab
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return Core.Server.Game(id).ToJson();
        }

        // POST api/game
        [HttpPost]
        public string Post([FromBody]dynamic json)
        {
            var map = json.Map.ToString();
            return Core.Server.NewGame(map);
        }

        [HttpPut]
        public void Put([FromBody]dynamic json)
        {
            var game = json.Game.ToString();
            var player = json.Player.ToString();
            Core.Server.Join(game, player);
        }
    }
}
