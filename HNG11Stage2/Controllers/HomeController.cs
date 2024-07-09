using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HNG11Stage2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Home() =>
       Ok("Hello");
    }
}
