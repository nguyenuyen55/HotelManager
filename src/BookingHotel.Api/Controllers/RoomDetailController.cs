using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Services;
using BookingHotel.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomDetailController : ControllerBase
    {

        private readonly IRoomDetailService _roomDetailService;
        private readonly IUnitOfWork _unitOfWork;

        public RoomDetailController(IRoomDetailService roomService, IUnitOfWork unitOfWork)
        {
            _roomDetailService = roomService;
            _unitOfWork = unitOfWork;
        }



        // GET api/<RoomController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var reponse = new RetureReponse();
            try
            {
                var roomDetail = await _roomDetailService.getRoomDetailById(id);
                if (roomDetail == null)
                {
                    reponse.returnCode = 404;
                    reponse.returnMessage = "Không tìm thấy phòng";
                    return NotFound(reponse);
                }
                var RoomDetailDTO = new RoomDetailDTO()
                {
                    RoomFittings = roomDetail.RoomFittings,
                    RoomType = roomDetail.RoomType,
                    RoomView = roomDetail.RoomView,
                    PricePerNight=roomDetail.PricePerNight,
                    IsAvailable = roomDetail.IsAvailable
                };
                return Ok(RoomDetailDTO);

            }
            catch (Exception ex)
            {
                reponse.returnCode = 500;
                reponse.returnMessage = "Lỗi dữ liệu " + ex.Message;
                return BadRequest(reponse);
            }

        }
        // GET api/<RoomController>/5
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var reponse = new RetureReponse();
            try
            {
                var roomDetails = await _roomDetailService.getAll();

                if (roomDetails.Count > 0)
                {
                    //var roomDetailDTO = roomDetails.Select(x => new RoomDetailDTO()
                    //{
                    //    RoomFittings = x.RoomFittings,
                    //    RoomType = x.RoomType,
                    //    RoomView = x.RoomView,
                    //    PricePerNight = x.PricePerNight,
                    //    IsAvailable = x.IsAvailable
                    //});
                    return Ok(roomDetails);
                }
                reponse.returnCode = 404;
                reponse.returnMessage = "Không dữ liệu nào được tìm thấy";
                return NotFound(reponse);

            }
            catch (Exception ex)
            {
                reponse.returnCode = 500;
                reponse.returnMessage = "Lỗi dữ liệu " + ex.Message;
                return BadRequest(reponse);
            }

        }
        // POST api/<RoomController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoomDetailDTO roomDetailRequest)
        {

            var returnRespone = new RetureReponse();

            try
            {
              
                returnRespone = _roomDetailService.InsertRoomDetail(roomDetailRequest).Result;
                return Ok(returnRespone);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi thêm dữ liệu " + ex.Message);
            }

        }

        // POST api/<RoomController>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(int id, [FromBody] RoomDetailDTO roomRequest)
        {
            var returnRespone = new RetureReponse();

            try
            {
                var room = await _roomDetailService.getRoomDetailById(id);
                if (room == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Không tìm thấy chi tiết phòng";
                    return NotFound(returnRespone);
                }
               
                returnRespone = _roomDetailService.UpdateRoomDetail(id, roomRequest).Result;
                return Ok(returnRespone);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi cập nhật dữ liệu " + ex.Message);
            }

        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {

            var returnRespone = new RetureReponse();

            try
            {
                var roomDetail = await _roomDetailService.getRoomDetailById(id);
                if (roomDetail == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Không tìm thấy phòng";
                    return NotFound(returnRespone);
                }
                await _roomDetailService.DeleteRoomDetail(id);
                returnRespone.returnCode = 200;
                returnRespone.returnMessage = "Xóa phòng thành công";
                return Ok(returnRespone);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi xóa dữ liệu " + ex.Message);
            }
        }
    }
}
