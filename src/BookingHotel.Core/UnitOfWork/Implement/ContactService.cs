// ContactService.cs

using System.Security.Cryptography;
using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;

    public ContactService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //public async Task<IEnumerable<ContactDtoNew>> GetAllContactsAsync()
    //{
    //    var users = await _unitOfWork.Repository<Contact>().GetAllAsync(u => u.isActive);

    //    if (users == null || !users.Any())
    //    {
    //        return new List<ContactDtoNew>();
    //    }

    //    var userDtos = new List<ContactDtoNew>();

    //    foreach (var u in users)
    //    {
    //        // Lấy Person tương ứng với Contact
    //        var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == u.ContactID);

    //        // Kiểm tra person có null không
    //        if (person == null)
    //        {
    //            // Log lỗi hoặc xử lý nếu cần
    //            Console.WriteLine($"No person found for ContactID: {u.ContactID}");
    //            continue; // Bỏ qua user này
    //        }

    //        // Lấy tất cả Email liên quan đến Person
    //        var emails = await _unitOfWork.Repository<Email>().GetAllAsync(e => e.PersonID == person.PersonID);
    //        var primaryEmail = emails?.FirstOrDefault(e => e.IsPrimary)?.EmailAddress; // Lấy Email chính


    //        // Lấy tất cả ContactRoles liên quan đến Contact
    //        var userRoles = await _unitOfWork.Repository<ContactRole>().GetAllAsync(r => r.ContactID == u.ContactID);

    //        // Tạo danh sách các tên vai trò
    //        var roleNames = new List<string>();

    //        foreach (var userRole in userRoles)
    //        {
    //            // Lấy Role dựa trên RoleID
    //            var role = await _unitOfWork.Repository<Role>().GetAsync(r => r.RoleID == userRole.RoleID);
    //            if (role != null)
    //            {
    //                roleNames.Add(role.RoleName);
    //            }
    //        }

    //        userDtos.Add(new ContactDtoNew
    //        {
    //            ContactID = u.ContactID,
    //            Contactname = u.Contactname,
    //            Email = emails != null && emails.Any() ? string.Join(", ", emails.Select(e => e.EmailAddress)) : "No Email", // Nếu không có email thì hiển thị "No Email"
    //            ImageUrl = u.ImageUrl ?? "No Image", // Nếu không có ảnh thì hiển thị "No Image"
    //            RoleName = roleNames.Any() ? string.Join(", ", roleNames) : "No Role"// Nếu không có vai trò thì hiển thị "No Role"
    //        });
    //    }

    //    return userDtos;
    //}

    //public async Task<ContactDtoNew> GetContactByIdAsync(int id)
    //{
    //    var user = await _unitOfWork.Repository<Contact>().GetAsync(u => u.ContactID == id);
    //    if (user == null)
    //    {
    //        throw new Exception("Contact not found.");
    //    }

    //    var person = await _unitOfWork.Repository<Person>().GetAsync(p => p.PersonID == user.ContactID);
    //    var emails = await _unitOfWork.Repository<Email>().GetAllAsync(e => e.PersonID == person.PersonID);
    //    var primaryEmail = emails?.FirstOrDefault(e => e.IsPrimary)?.EmailAddress;

    //    var userRoles = await _unitOfWork.Repository<ContactRole>().GetAllAsync(r => r.ContactID == user.ContactID);
    //    var roleNames = new List<string>();

    //    foreach (var userRole in userRoles)
    //    {
    //        var role = await _unitOfWork.Repository<Role>().GetAsync(r => r.RoleID == userRole.RoleID);
    //        if (role != null)
    //        {
    //            roleNames.Add(role.RoleName);
    //        }
    //    }

    //    return new ContactDtoNew
    //    {
    //        ContactID = user.ContactID,
    //        Contactname = user.Contactname,
    //        Email = emails != null && emails.Any() ? string.Join(", ", emails.Select(e => e.EmailAddress)) : "No Email",
    //        ImageUrl = user.ImageUrl ?? "No Image",
    //        RoleName = roleNames.Any() ? string.Join(", ", roleNames) : "No Role"
    //    };
    //}

    public async Task<int> CreateContactAsync(Contact_InsertRequestData model)
    {
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                // Tạo mới Person
                var contact = new Contact
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.EmailAddress,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                };
                await _unitOfWork.Repository<Contact>().AddAsync(contact);
                await _unitOfWork.SaveChangesAsync();



                await transaction.CommitAsync();
                return contact.Id;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Contact creation failed: " + ex.Message);
            }
        }
        throw new NotImplementedException();
    }


}
