using Microsoft.AspNetCore.Http;

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    public string Email { get; set; }
    public string EmailType { get; set; }
    public string Password { get; set; }

    public IFormFile Image { get; set; }
}