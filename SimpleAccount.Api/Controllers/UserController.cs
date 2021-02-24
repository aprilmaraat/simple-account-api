using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using SimpleAccount.Services;
using SimpleAccount.Utilities;
using SimpleAccount.Models;

namespace SimpleAccount.Api.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private IEmailConfiguration _emailConfiguration;

        public UserController(IEmailConfiguration emailConfiguration, IUserService userService) : base(emailConfiguration)
        {
            _userService = userService;
        }

        [HttpGet("user-list")]
        public async Task<IActionResult> List()
        {
            var response = await _userService.UserList();

            switch (response.State)
            {
                case ResponseState.Exception:
                    return StatusCode(500, response.Exception.Message);
                case ResponseState.Error:
                    return BadRequest(response.MessageText);
                default:
                    return Ok(response);
            }
        }

        [HttpGet("user-detail/{id}")]
        public async Task<IActionResult> UserDetail(int id)
        {
            var response = await _userService.UserDetail(id);

            switch (response.State)
            {
                case ResponseState.Exception:
                    return StatusCode(500, response.Exception.Message);
                case ResponseState.Error:
                    return BadRequest(response.MessageText);
                default:
                    return Ok(response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var response = await _userService.Login(login);

            switch (response.State)
            {
                case ResponseState.Exception:
                    return StatusCode(500, response.Exception.Message);
                case ResponseState.Error:
                    return BadRequest(response.MessageText);
                default:
                    return Ok(response);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            //logger here

            var response = await _userService.Register(user);

            switch (response.State)
            {
                case ResponseState.Exception:
                    return StatusCode(500, response.Exception.Message);
                case ResponseState.Error:
                    return BadRequest(response.MessageText);
                default:
                    return Ok(response);
            }
        }
    }
}
