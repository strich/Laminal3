using Laminal.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Stl.Fusion.Server;

namespace Laminal.Server.Controllers
{
    [ApiController, JsonifyErrors, UseDefaultSession]
    [Route("api/[controller]/[action]")]
    public class BaseAPIController : ControllerBase
    {
        internal ILogger<BaseAPIController> _logger;
        internal IConfiguration _configuration;

        public BaseAPIController(ILogger<BaseAPIController> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }
    }
}
