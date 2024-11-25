// UserService.cs

using System.Security.Cryptography;
using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.DTO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<UserDtoNew>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Repository<User>().GetAllAsync(u => u.isActive);

        if (users == null || !users.Any())
        {
            return new List<UserDtoNew>();
        }

        var userDtos = new List<UserDtoNew>();

        foreach (var u in users)
        {
            // Lấy Person tương ứng với User
            var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == u.UserID);

            // Kiểm tra person có null không
            if (person == null)
            {
                // Log lỗi hoặc xử lý nếu cần
                Console.WriteLine($"No person found for UserID: {u.UserID}");
                continue; // Bỏ qua user này
            }

            // Lấy tất cả Email liên quan đến Person
            var emails = await _unitOfWork.Repository<Email>().GetAllAsync(e => e.PersonID == person.PersonID);
            var primaryEmail = emails?.FirstOrDefault(e => e.IsPrimary)?.EmailAddress; // Lấy Email chính


            // Lấy tất cả UserRoles liên quan đến User
            var userRoles = await _unitOfWork.Repository<UserRole>().GetAllAsync(r => r.UserID == u.UserID);

            // Tạo danh sách các tên vai trò
            var roleNames = new List<string>();

            foreach (var userRole in userRoles)
            {
                // Lấy Role dựa trên RoleID
                var role = await _unitOfWork.Repository<Role>().GetAsync(r => r.RoleID == userRole.RoleID);
                if (role != null)
                {
                    roleNames.Add(role.RoleName);
                }
            }

            userDtos.Add(new UserDtoNew
            {
                UserID = u.UserID,
                Username = u.Username,
                Email = emails != null && emails.Any() ? string.Join(", ", emails.Select(e => e.EmailAddress)) : "No Email", // Nếu không có email thì hiển thị "No Email"
                ImageUrl = u.ImageUrl ?? "No Image", // Nếu không có ảnh thì hiển thị "No Image"
                RoleName = roleNames.Any() ? string.Join(", ", roleNames) : "No Role"// Nếu không có vai trò thì hiển thị "No Role"
            });
        }

        return userDtos;
    }

    public async Task<UserDtoNew> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserID == id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == user.UserID);
        var emails = await _unitOfWork.Repository<Email>().GetAllAsync(e => e.PersonID == person.PersonID);
        var primaryEmail = emails?.FirstOrDefault(e => e.IsPrimary)?.EmailAddress;

        var userRoles = await _unitOfWork.Repository<UserRole>().GetAllAsync(r => r.UserID == user.UserID);
        var roleNames = new List<string>();

        foreach (var userRole in userRoles)
        {
            var role = await _unitOfWork.Repository<Role>().GetAsync(r => r.RoleID == userRole.RoleID);
            if (role != null)
            {
                roleNames.Add(role.RoleName);
            }
        }

        return new UserDtoNew
        {
            UserID = user.UserID,
            Username = user.Username,
            Email = emails != null && emails.Any() ? string.Join(", ", emails.Select(e => e.EmailAddress)) : "No Email",
            ImageUrl = user.ImageUrl ?? "No Image",
            RoleName = roleNames.Any() ? string.Join(", ", roleNames) : "No Role"
        };
    }

    public async Task<string> CreateUserAsync(RegisterDto model)
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

                // Kiểm tra xem username đã tồn tại chưa
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
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    var fileName = $"{Guid.NewGuid()}_{DateTime.Now.AddHours(7):yyyyMMdd_HHmmss}_{Path.GetFileName(model.Image.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    imageUrl = $"/Images/{fileName}";
                }

                // Tạo mới User
                var user = new User
                {
                    UserID = person.PersonID,
                    Username = model.UserName,
                    PasswordHash = hashedPassword,
                    PasswordSalt = Convert.ToBase64String(salt),
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

                // Thêm vào bảng Customer
                var customer = new Customer
                {
                    CustomerID = user.UserID,
                    RegistrationDate = user.CreateDate,
                    CustomerSpecificInfo = string.Empty,
                };
                await _unitOfWork.Repository<Customer>().AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
                return "User created successfully!";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("User creation failed: " + ex.Message);
            }
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserID == id);
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        // Đặt isActive thành false
        user.isActive = false;

        // Cập nhật người dùng trong cơ sở dữ liệu
        _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

    }

    public async Task<string> UpdateUserAsync(int userId, UpdateUserDto model)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                // Kiểm tra xem người dùng có tồn tại không
                var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserID == userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                // Cập nhật thông tin cá nhân nếu có
                var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == user.UserID);
                if (person != null)
                {
                    person.FirstName = model.FirstName ?? person.FirstName;
                    person.LastName = model.LastName ?? person.LastName;
                    await _unitOfWork.SaveChangesAsync();
                }

                // Cập nhật Email nếu có
                var email = await _unitOfWork.Repository<Email>().GetAsync(e => e.PersonID == person.PersonID);
                if (email != null && model.Email != null)
                {
                    email.EmailAddress = model.Email;
                    await _unitOfWork.SaveChangesAsync();
                }

                // Cập nhật mật khẩu nếu cần thiết
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var (hashedPassword, salt) = HashPassword(model.Password);
                    user.PasswordHash = hashedPassword;
                    user.PasswordSalt = Convert.ToBase64String(salt);
                }

                // Xử lý cập nhật ảnh
                if (model.Image != null)
                {
                    var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    if (!Directory.Exists(imageDirectory))
                    {
                        Directory.CreateDirectory(imageDirectory);
                    }

                    var fileName = $"{Guid.NewGuid()}_{DateTime.Now.AddHours(7):yyyyMMdd_HHmmss}_{Path.GetFileName(model.Image.FileName)}";
                    var filePath = Path.Combine(imageDirectory, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    user.ImageUrl = $"/Images/{fileName}";
                }

                await _unitOfWork.Repository<User>().UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
                return "User updated successfully!";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("User update failed: " + ex.Message);
            }
        }
    }

    public async Task<bool> UninsactiveUserAsync(int userId)
    {
        var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserID == userId);

        if (user == null)
        {
            return false; // Người dùng không tồn tại
        }

        user.isActive = true; // Khôi phục trạng thái thành hoạt động

        _unitOfWork.Repository<User>().UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(); // Lưu thay đổi

        return true; // Trả về true nếu thành công
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


}
