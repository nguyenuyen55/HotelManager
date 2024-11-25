using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using Microsoft.AspNetCore.Mvc;

public interface IUserService
{
    Task<IEnumerable<UserDtoNew>> GetAllUsersAsync();
    Task<UserDtoNew> GetUserByIdAsync(int id);

    Task<string> CreateUserAsync(RegisterDto model);
    Task<bool> UninsactiveUserAsync(int userId); // Thêm phương thức này
    Task DeleteUserAsync(int id);

    Task<string> UpdateUserAsync(int userId, UpdateUserDto model);
    // Các phương thức khác...
}
