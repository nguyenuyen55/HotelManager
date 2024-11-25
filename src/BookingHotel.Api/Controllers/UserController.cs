using BookingHotel.Core;
using BookingHotel.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 1. Get All Users
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        // 5. Delete User
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok("User Deleted successfully!!!");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 6. Uninsactive User
        [HttpPut("{id}/active")]
        public async Task<IActionResult> UninsactiveUser(int id)
        {
            var result = await _userService.UninsactiveUserAsync(id);
            if (!result)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy người dùng
            }

            return Ok("Active user successfully!!!");
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromForm] RegisterDto model)
        {
            try
            {
                var result = await _userService.CreateUserAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // API lấy thông tin người dùng theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về BadRequest với thông báo lỗi
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UpdateUserDto model)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
