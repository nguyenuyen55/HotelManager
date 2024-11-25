using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        //// 1. Get All Contacts
        //[HttpGet("getAllContacts")]
        //public async Task<IActionResult> GetAllContacts()
        //{
        //    var contacts = await _contactService.GetAllContactsAsync();
        //    if (contacts == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(contacts);
        //}

        //// 5. Delete Contact
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteContact(int id)
        //{
        //    try
        //    {
        //        await _contactService.DeleteContactAsync(id);
        //        return Ok("Contact Deleted successfully!!!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}

        //// 6. Uninsactive Contact
        //[HttpPut("{id}/active")]
        //public async Task<IActionResult> UninsactiveContact(int id)
        //{
        //    var result = await _contactService.UninsactiveContactAsync(id);
        //    if (!result)
        //    {
        //        return NotFound(); // Trả về 404 nếu không tìm thấy người dùng
        //    }

        //    return Ok("Active contact successfully!!!");
        //}

        [HttpPost("createContact")]
        public async Task<IActionResult> CreateContact(Contact_InsertRequestData model)
        {
            try
            {
                var result = await _contactService.CreateContactAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //// API lấy thông tin người dùng theo ID
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetContactById(int id)
        //{
        //    try
        //    {
        //        var contact = await _contactService.GetContactByIdAsync(id);
        //        return Ok(contact);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Nếu có lỗi, trả về BadRequest với thông báo lỗi
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        //[HttpPut("updateContact/{id}")]
        //public async Task<IActionResult> UpdateContact(int id, [FromForm] UpdateContactDto model)
        //{
        //    try
        //    {
        //        var result = await _contactService.UpdateContactAsync(id, model);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}
    }
}
