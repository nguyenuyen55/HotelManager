using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using Microsoft.AspNetCore.Mvc;

public interface IContactService
{
    //Task<IEnumerable<ContactDtoNew>> GetAllContactsAsync();
    //Task<ContactDtoNew> GetContactByIdAsync(int id);
    Task<int> CreateContactAsync(Contact_InsertRequestData model);
    //Task<bool> UninsactiveContactAsync(int userId); // Thêm phương thức này
    //Task DeleteContactAsync(int id);

    //Task<string> UpdateContactAsync(int userId, UpdateContactDto model);
    // Các phương thức khác...
}
