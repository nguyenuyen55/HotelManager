using BackendAPIBookingHotel.Model;
using BookingHotel.Api.Services;
using BookingHotel.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly HotelService _hotelService;
        private readonly ILogger<HotelController> _logger;

        public HotelController(HotelService hotelService, ILogger<HotelController> logger)
        {
            _hotelService = hotelService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetAllHotels([FromQuery] string keyword = "")
        {
            try
            {
                var hotels = await _hotelService.GetAllHotelsAsync(keyword);
                foreach(var hotel in hotels.Data)
                {
                    Console.WriteLine(hotel);
                    if (hotel.Rooms.Count > 0)
                    {
                        var listRoom = new List<RoomDTO>();
                        foreach (var itemRoom in hotel.Rooms)
                        {
                                foreach (var img in itemRoom.ImageRooms)
                                {
                                    img.NameFileImg = $"{Request.Scheme}://{Request.Host}/Images/{img.NameFileImg}";
                                }
                        }
                    }
                }
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllHotels");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var result = await _hotelService.GetHotelByIdAsync(id);

            if (result.StatusCode == 404)
                return NotFound(result.Message);
            foreach (var itemRoom in result.Data.Rooms)
            {
                foreach (var img in itemRoom.ImageRooms)
                {
                    img.NameFileImg = $"{Request.Scheme}://{Request.Host}/Images/{img.NameFileImg}";
                }
            }
            return Ok(result.Data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> InsertHotel(Hotel_InsertRequestData requestData)
        {
            try
            {
                var result = await _hotelService.InsertHotelAsync(requestData);

                // Kiểm tra xem mã trả về từ service là gì, nếu mã là 400 thì trả về BadRequest
                if (result.StatusCode == 400)
                {
                    return BadRequest(new { message = result.Message });
                }

                // Trả về mã 201 khi khách sạn được thêm thành công
                return StatusCode(201, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InsertHotel");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel_InsertRequestData requestData)
        {
            try
            {
                var result = await _hotelService.UpdateHotelAsync(id, requestData);

                // Kiểm tra mã trạng thái trả về từ service
                if (result.StatusCode == 400)
                {
                    return BadRequest(new { message = result.Message });
                }
                else if (result.StatusCode == 404)
                {
                    return NotFound(new { message = result.Message });
                }

                // Trả về dữ liệu khách sạn sau khi cập nhật
                return StatusCode(200, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in UpdateHotel for ID {id}");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteHotel(int id)
        {
            try
            {
                await _hotelService.DeleteHotelAsync(id);
                return Ok("Hotel is deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteHotel for ID {id}");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }
        [HttpPut("sactive/{id}")]
        public async Task<IActionResult> UnIsActiveHotel(int id)
        {
            var result = await _hotelService.UnIsActiveHotelAsync(id);

            if (result.StatusCode == 404)
                return NotFound(result.Message);

            if (result.StatusCode == 400)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
