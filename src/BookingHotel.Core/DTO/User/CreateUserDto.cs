using Microsoft.AspNetCore.Http;

public class CreateUserDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
    public IFormFile Image { get; set; }
}