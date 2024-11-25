using BookingHotel.Core.DTO;
using BackendAPIBookingHotel.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using BookingHotel.Core;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


public class AuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<string> RegisterAsync(RegisterDto model)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                // Kiểm tra xem email đã tồn tại chưa
                var existingEmail = await _unitOfWork.Repository<Email>().GetAsync(e => e.EmailAddress == model.Email);
                if (existingEmail != null)
                {
                    throw new Exception("Email already exists.");
                }

                // Kiểm tra xem username đã tồn tại hay chưa
                var existingUser = await _unitOfWork.Repository<User>().GetAsync(u => u.Username == model.UserName);
                if (existingUser != null)
                {
                    throw new Exception("Username already exists.");
                }

                // Tạo mới Person
                var person = new Person
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                await _unitOfWork.Repository<Person>().AddAsync(person);
                await _unitOfWork.SaveChangesAsync();

                // Tạo mới Email
                var email = new Email
                {
                    EmailAddress = model.Email,
                    PersonID = person.PersonID,
                    EmailType = model.EmailType,
                };
                await _unitOfWork.Repository<Email>().AddAsync(email);
                await _unitOfWork.SaveChangesAsync();

                // Tạo salt và hash mật khẩu
                var (hashedPassword, salt) = HashPassword(model.Password);

                // Xử lý ảnh
                string imageUrl = null;

                if (model.Image != null)
                {
                    // Đảm bảo thư mục Images tồn tại
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    // Lưu ảnh vào wwwroot/Images
                    var fileName = $"{Guid.NewGuid()}_{DateTime.Now.AddHours(7):yyyyMMdd_HHmmss}_{Path.GetFileName(model.Image.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    // Đường dẫn URL của ảnh
                    imageUrl = $"/Images/{fileName}";
                }

                // Tạo mới User
                var user = new User
                {
                    UserID = person.PersonID,
                    Username = model.UserName,
                    PasswordHash = hashedPassword,
                    PasswordSalt = Convert.ToBase64String(salt), // Lưu salt
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    ImageUrl = imageUrl
                };
                await _unitOfWork.Repository<User>().AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Thêm Role cho User là Customer
                var userRole = new UserRole
                {
                    UserID = user.UserID,
                    RoleID = 3 // Gán role mặc định là Customer
                };

                await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
                await _unitOfWork.SaveChangesAsync();

                // Gán vào Customer
                var customer = new Customer
                {
                    CustomerID = user.UserID,
                    RegistrationDate = user.CreateDate,
                    CustomerSpecificInfo = string.Empty,
                };
                await _unitOfWork.Repository<Customer>().AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();

                // Nếu tất cả đều thành công, commit transaction
                await transaction.CommitAsync();

                return "User registered successfully!";
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, rollback transaction
                await transaction.RollbackAsync();
                throw new Exception("Registration failed: " + ex.Message);
            }
        }
    }

    public async Task<string> RegisterAdminAsync(RegisterDto model)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                // Kiểm tra xem email đã tồn tại chưa
                var existingEmail = await _unitOfWork.Repository<Email>().GetAsync(e => e.EmailAddress == model.Email);
                if (existingEmail != null)
                {
                    throw new Exception("Email already exists.");
                }

                // Kiểm tra xem username đã tồn tại hay chưa
                var existingUser = await _unitOfWork.Repository<User>().GetAsync(u => u.Username == model.UserName);
                if (existingUser != null)
                {
                    throw new Exception("Username already exists.");
                }

                // Tạo mới Person
                var person = new Person
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                await _unitOfWork.Repository<Person>().AddAsync(person);
                await _unitOfWork.SaveChangesAsync();

                // Tạo mới Email
                var email = new Email
                {
                    EmailAddress = model.Email,
                    PersonID = person.PersonID,
                    EmailType = model.EmailType,
                };
                await _unitOfWork.Repository<Email>().AddAsync(email);
                await _unitOfWork.SaveChangesAsync();

                // Tạo salt và hash mật khẩu
                var (hashedPassword, salt) = HashPassword(model.Password);

                // Xử lý ảnh
                string imageUrl = null;

                if (model.Image != null)
                {
                    // Đảm bảo thư mục Images tồn tại
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    // Lưu ảnh vào wwwroot/Images
                    var fileName = $"{Guid.NewGuid()}_{DateTime.Now.AddHours(7):yyyyMMdd_HHmmss}_{Path.GetFileName(model.Image.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    // Đường dẫn URL của ảnh
                    imageUrl = $"/Images/{fileName}";
                }

                // Tạo mới User
                var user = new User
                {
                    UserID = person.PersonID,
                    Username = model.UserName,
                    PasswordHash = hashedPassword,
                    PasswordSalt = Convert.ToBase64String(salt), // Lưu salt
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    ImageUrl = imageUrl
                };
                await _unitOfWork.Repository<User>().AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                // Thêm Role cho User là Admin
                var userRole = new UserRole
                {
                    UserID = user.UserID,
                    RoleID = 1 // Gán role mặc định là Admin
                };

                await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
                await _unitOfWork.SaveChangesAsync();

                // Gán vào Customer
                var Admin = new Admin
                {
                    Position = "",
                    AssignedDate = DateTime.UtcNow.AddHours(7),
                    AdminSpecificInfo = string.Empty,
                };
                await _unitOfWork.Repository<Admin>().AddAsync(Admin);
                await _unitOfWork.SaveChangesAsync();

                // Nếu tất cả đều thành công, commit transaction
                await transaction.CommitAsync();

                return "User registered successfully!";
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, rollback transaction
                await transaction.RollbackAsync();
                throw new Exception("Registration failed: " + ex.Message);
            }
        }
    }

    public async Task<(string AccessToken, string RefreshToken, UserDto UserInfo)> LoginAsync(LoginDto model)
    {
        var user = await _unitOfWork.Repository<User>().GetAsync(u => u.Username == model.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("UserName Or Password is invalid!!!");
        }

        var saltBytes = Convert.FromBase64String(user.PasswordSalt);

        if (!VerifyPassword(model.Password, user.PasswordHash, saltBytes))
        {
            throw new UnauthorizedAccessException("UserName Or Password is invalid!!!");
        }

        var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == user.UserID);

        if (person == null)
        {
            throw new Exception("Person not found for this user.");
        }

        // Lấy danh sách UserRole
        var userRoles = await _unitOfWork.Repository<UserRole>().GetAllAsync(ur => ur.UserID == user.UserID);

        // Lấy danh sách RoleName từ UserRole
        var roleNames = await Task.WhenAll(userRoles.Select(async ur =>
        {
            var role = await _unitOfWork.Repository<Role>().GetAsync(r => r.RoleID == ur.RoleID);
            return role?.RoleName; // Trả về RoleName
        }));

        var email = await _unitOfWork.Repository<Email>().GetAllAsync(p => p.PersonID == user.UserID);

        if (email == null || !email.Any())
        {
            throw new Exception("No emails found for this user.");
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        new Claim(ClaimTypes.Name, user.Username)
    };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleID.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(7).AddMinutes(30),
            signingCredentials: creds,
            claims: claims
        );

        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshToken_ExpriredTime = DateTime.UtcNow.AddHours(7).AddDays(7).ToString("yyyy-MM-dd HH:mm:ss");

        _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Tạo UserDto để trả về thông tin người dùng, bao gồm cả vai trò
        var userInfo = new UserDto
        {
            UserID = user.UserID,
            Username = user.Username,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Email = string.Join(", ", email.Select(e => e.EmailAddress)),
            UrlImage = !string.IsNullOrEmpty(user.ImageUrl) ? user.ImageUrl : string.Empty,
            Roles = string.Join(", ", roleNames.Where(r => r != null)) // Gán danh sách vai trò thành chuỗi
        };

        return (new JwtSecurityTokenHandler().WriteToken(accessToken), refreshToken, userInfo);
    }

    public async Task<(string AccessToken, UserDto UserInfo)> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var user = await _unitOfWork.Repository<User>().GetAsync(u => u.RefreshToken == refreshToken);

        if (user == null || DateTime.Parse(user.RefreshToken_ExpriredTime) < DateTime.UtcNow.AddHours(7))
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == user.UserID);
        var roles = await _unitOfWork.Repository<UserRole>().GetAllAsync(ur => ur.UserID == user.UserID);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.RoleID.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddHours(7).AddMinutes(30),
            signingCredentials: creds,
            claims: claims
        );

        var userInfo = new UserDto
        {
            UserID = user.UserID,
            Username = user.Username,
            FirstName = person.FirstName,
            LastName = person.LastName
        };

        return (new JwtSecurityTokenHandler().WriteToken(accessToken), userInfo);
    }

    private (string hashedPassword, byte[] salt) HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return (hashed, salt);
    }

    private bool VerifyPassword(string password, string storedHash, byte[] storedSalt)
    {
        string hashedInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: storedSalt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashedInput == storedHash;
    }

    private string GenerateRefreshToken()
    {
        // Tạo mảng byte ngẫu nhiên có độ dài được chỉ định
        var randomNumber = new byte[32];

        // Sử dụng RandomNumberGenerator để tạo số ngẫu nhiên an toàn
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        // Chuyển mảng byte thành chuỗi base64 để dễ dàng lưu trữ
        return Convert.ToBase64String(randomNumber);
    }

    public async Task LogoutUserAsync(int userId)
    {
        // Tìm user dựa trên userID
        var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserID == userId);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Xóa refresh token của người dùng trong DB
        user.RefreshToken = null;
        user.RefreshToken_ExpriredTime = null;

        _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }


}
