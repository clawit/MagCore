using System;
using System.Collections.Generic;
using System.Linq;
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
        public string Post([FromBody]dynamic json)
        {
            var name = json.Name.ToString().Trim();
            PlayerColor color = (PlayerColor)json.Color;
            return Core.Players.NewPlayer(name, color);
        }
    }
}
