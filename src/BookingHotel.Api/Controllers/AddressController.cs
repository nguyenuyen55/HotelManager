using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("createAddress")]
        public async Task<IActionResult> CreateAddress(Address_InsertRequestData model)
        {
            try
            {
                var result = await _addressService.CreateAddressAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
