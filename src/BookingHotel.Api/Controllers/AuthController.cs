using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookingHotel.Core.DTO;
using Swashbuckle.AspNetCore.Annotations;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Registers a new customer.
    /// </summary>
    /// <param name="model">The registration details.</param>
    /// <returns>A message indicating the result of the registration.</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Registers a new user", Description = "Registers a new user with the provided details.")]
    [SwaggerResponse(200, "Registration successful", typeof(object))]
    [SwaggerResponse(400, "Registration failed", typeof(object))]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        try
        {
            var message = await _authService.RegisterAsync(model);
            return Ok(new { message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

     /// <summary>
    /// Tạo admin để test api
    /// </summary>
    [HttpPost("register-for-admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto model)
    {
        try
        {
            var message = await _authService.RegisterAdminAsync(model);
            return Ok(new { message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        try
        {
            var tokens = await _authService.LoginAsync(model);

            // Store RefreshToken in a secure HttpOnly cookie
            Response.Cookies.Append("RefreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensures the cookie is sent over HTTPS only
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7) // Same expiration as the token
            });


            return Ok(new
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                UserInfo = tokens.UserInfo // Đảm bảo rằng thông tin người dùng cũng được trả về
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }


    /// <summary>
    /// Gets user info and new access token by refresh token from cookies. API này login xong thì tự động gọi được
    /// </summary>
    [HttpGet("refresh-token")]
    public async Task<IActionResult> GetUserByRefreshToken()
    {
        try
        {
            // Get refresh token from the cookie
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("Refresh token is missing.");

            var tokens = await _authService.GetUserByRefreshTokenAsync(refreshToken);

            // Return a new access token and user info
            return Ok(new
            {
                AccessToken = tokens.AccessToken,
                UserInfo = tokens.UserInfo
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [SwaggerOperation(Summary = "Logout the user", Description = "Logs out the user by invalidating the refresh token.")]
    [SwaggerResponse(200, "Logout successful", typeof(object))]
    [SwaggerResponse(400, "Logout failed", typeof(object))]
    public async Task<IActionResult> Logout()
    {
        try
        {
            // Lấy refresh token từ cookie
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { message = "No refresh token found in the cookies." });
            }

            // Lấy user dựa trên refresh token
            var user = await _authService.GetUserByRefreshTokenAsync(refreshToken);

            if (user.UserInfo == null)
            {
                return BadRequest(new { message = "Invalid refresh token." });
            }

            // Xóa refresh token của người dùng trong DB
            await _authService.LogoutUserAsync(user.UserInfo.UserID);

            // Xóa refresh token trong cookie
            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Logout successful!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
