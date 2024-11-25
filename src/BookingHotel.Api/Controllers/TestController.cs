using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingHotel.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestAuthorizeController : ControllerBase
    {
        // Public API, không cần xác thực
        [HttpGet("public")]

        public IActionResult PublicEndpoint()
        {
            return Ok("This endpoint is publicly accessible.");
        }

        [HttpGet("admin")]
        [RoleAuthorize("1")] // Chỉ cho phép người dùng có RoleID là 1
        public IActionResult GetAdminData()
        {
            // Chỉ người dùng có vai trò Admin mới có thể vào đây
            return Ok(new { message = "This is admin data." });
        }

        [HttpGet("customer")]
        [RoleAuthorize("3")] // Chỉ cho phép người dùng có RoleID là 3
        public IActionResult GetUserData()
        {
            return Ok(new { message = "This is user data." });
        }


        [HttpGet("staff")]
        [RoleAuthorize("4")] // Chỉ cho phép người dùng có RoleID là 3
        public IActionResult GetStaffData()
        {
            return Ok(new { message = "This is staff data." });
        }
    }

}