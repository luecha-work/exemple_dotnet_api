using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class AccountController : ControllerBase
    {
        public AccountController() { }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello Account");
        }
    }
}
