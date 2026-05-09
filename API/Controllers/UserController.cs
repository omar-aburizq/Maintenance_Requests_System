using Application.Services.UserService;
using Application.Services.UserService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto input)
        {
            await _userService.CreateUser(input);
            return Ok();
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto input)
        {
            await _userService.UpdateUser(id, input);
            return Ok();
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(string? name, string? email)
        {
            var users = await _userService.GetAllUsers(name, email);
            return Ok(users);
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }

        [HttpGet("GetUsersTechnicians")]
        public async Task<IActionResult> GetUsersTechnicians(Guid? categoryId = null)
        {
            var technicians = await _userService.GetUsersTechnicians(categoryId);

            return Ok(technicians);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}
