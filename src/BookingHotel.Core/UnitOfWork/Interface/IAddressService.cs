using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using Microsoft.AspNetCore.Mvc;

public interface IAddressService
{
    //Task<IEnumerable<AddressDtoNew>> GetAllAddresssAsync();
    //Task<AddressDtoNew> GetAddressByIdAsync(int id);
    Task<int> CreateAddressAsync(Address_InsertRequestData model);
    //Task<bool> UninsactiveAddressAsync(int userId); // Thêm phương thức này
    //Task DeleteAddressAsync(int id);

    //Task<string> UpdateAddressAsync(int userId, UpdateAddressDto model);
    // Các phương thức khác...
}
