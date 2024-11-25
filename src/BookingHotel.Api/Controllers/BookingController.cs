using BackendAPIBookingHotel.Model;
using BookingBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly ILogger<BookingController> _logger;

        public BookingController(BookingService bookingService, ILogger<BookingController> logger)
        {
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetAllBookings([FromQuery] string keyword = "")
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync(keyword);
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllBookings");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var result = await _bookingService.GetBookingByIdAsync(id);

            if (result.StatusCode == 404)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> InsertBooking(Booking_InsertRequestData requestData)
        {
            try
            {
                var result = await _bookingService.InsertBookingAsync(requestData);

                // Kiểm tra xem mã trả về từ service là gì, nếu mã là 400 thì trả về BadRequest
                if (result.StatusCode == 400)
                {
                    return BadRequest(new { message = result.Message });
                }

                return StatusCode(201, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InsertBooking");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Booking_InsertRequestData requestData)
        {
            try
            {
                var result = await _bookingService.UpdateBookingAsync(id, requestData);

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
                _logger.LogError(ex, $"Error in UpdateBooking for ID {id}");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }


        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteBooking(int id)
        {
            try
            {
                await _bookingService.DeleteBookingAsync(id);
                return Ok("Booking is deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteBooking for ID {id}");
                return StatusCode(500, new { error = "An error occurred while processing your request.", details = ex.Message });
            }
        }
        //[HttpPut("sactive/{id}")]
        //public async Task<IActionResult> UnIsActiveBooking(int id)
        //{
        //    var result = await _bookingService.UnIsActiveBookingAsync(id);

        //    if (result.StatusCode == 404)
        //        return NotFound(result.Message);

        //    if (result.StatusCode == 400)
        //        return BadRequest(result.Message);

        //    return Ok(result.Message);
        //}
    }
}
