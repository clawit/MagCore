using MagCore.Core;
using MagCore.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MagCore.Server.Controllers
{
    [Route("api/[controller]")]
    public class CellController : Controller
    {
        // Put api/cell - attack cell
        [HttpPut]
        public ContentResult Put([FromBody]dynamic json)
        {
            try
            {
                var param1 = json.Game.ToString().Trim();
                var param2 = json.Player.ToString().Trim();
                int param3 = (int)json.X;
                int param4 = (int)json.Y;

                Core.Game game = Core.Server.Game(param1);
                if (game == null)
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.NotFound, Content = "Game not found" };
                else if (!game.HasPlayer(param2))
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.NotFound, Content = "Player not found" };
                else if (game.State != GameState.Playing)
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.Forbidden, Content = "Game over" };
                else
                {
                    Command cmd = new Command();
                    cmd.Action = Model.Action.Attack;
                    cmd.Sender = param2;
                    cmd.Target = new Position(param3, param4);
                    game.Enqueue(cmd);
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.OK };
                }
            }
            catch
            {
                return new ContentResult() { StatusCode = (int)HttpStatusCode.BadRequest};
            }
        }
    }
}
