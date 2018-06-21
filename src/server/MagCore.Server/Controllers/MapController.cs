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
    public class MapController : Controller
    {
        // POST api/map
        [HttpGet("{id}")]
        public ContentResult Get(string id)
        {
            var map = Core.Server.GetMap(id);
            if (map != null)
                return new ContentResult() { StatusCode = (int)HttpStatusCode.OK, Content = map.ToJson() };
            else
                return new ContentResult() { StatusCode = (int)HttpStatusCode.NotFound };
        }
    }
}
