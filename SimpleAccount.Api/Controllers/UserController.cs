using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using SimpleAccount.Services;
using SimpleAccount.Utilities;
using SimpleAccount.Models;

namespace SimpleAccount.Api.Controllers
{
    /// <summary>
    /// API controller for all <see cref="User"/> related transactions.
    /// </summary>
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private IEmailConfiguration _emailConfiguration;

        public UserController(IEmailConfiguration emailConfiguration, IUserService userService) : base(emailConfiguration)
        {
            _userService = userService;
        }

        /// <summary>
        /// API to get user list
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// API to get user data
        /// </summary>
        /// <param name="id">Use Id</param>
        /// <returns></returns>
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

        /// <summary>
        /// API used for logging in or verifying if login data exist.
        /// </summary>
        /// <param name="login">Login data request</param>
        /// <returns></returns>
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

        /// <summary>
        /// API to create <see cref="User"/> data
        /// </summary>
        /// <param name="user">User data provided</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            var response = await _userService.Update(user);

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


        /// <summary>
        /// API to delete the specified Id of <see cref="User"/>
        /// </summary>
        /// <param name="id">User Id to delete</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userService.Delete(id);

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
