using HNG11Stage2.DTOs;
using HNG11Stage2.Services;
using Microsoft.AspNetCore.Mvc;

namespace HNG11Stage2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {

        [HttpPost("/auth/Register")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO model)
        {
            var createUser = await userService.CreateUser(model);
            return StatusCode(createUser.StatusCode, createUser);
        }

        [HttpPost("/auth/Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var login = await userService.Login(model);
            return StatusCode(login.StatusCode, login);
        }
    }
}
