using Azure;
using BackendAPIBookingHotel.Model;
using BookingHotel.Core;
using BookingHotel.Core.DTO;
using BookingHotel.Core.Models;
using BookingHotel.Core.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingHotel.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IUnitOfWork _unitOfWork;

        public RoomController(IRoomService roomService, IUnitOfWork unitOfWork)
        {
            _roomService=roomService;
            _unitOfWork= unitOfWork;
        }



        // GET api/<RoomController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var reponse= new RetureReponse();
            try
            {
                var room = await _roomService.getRoomById(id);
                if(room == null)
                {
                    reponse.returnCode = 404;
                    reponse.returnMessage = "Không tìm thấy phòng";
                    return NotFound(reponse);
                }
                foreach (var img in room.ImageRooms)
                {
                    img.NameFileImg = $"{Request.Scheme}://{Request.Host}/Images/{img.NameFileImg}";
                }
                return Ok(room);
            
            }catch (Exception ex)
            {
                reponse.returnCode = 500;
                reponse.returnMessage = "Lỗi dữ liệu "+ex.Message;
                return BadRequest(reponse);
            }
          
        }
        [HttpGet("GetAllRoomByHotel/{idHotel}")]
        public async Task<IActionResult> GetRoomByIdHotel(int idHotel)
        {
            var reponse = new RetureReponse();
            try
            {
                var rooms = await _roomService.getListRoomsByHotelId(idHotel);
                if (rooms.Count == 0)
                {
                    reponse.returnCode = 404;
                    reponse.returnMessage = "Không tìm thấy phòng nào";
                    return NotFound(reponse);
                }
                var listRoomResponse= new List<RoomDTOResponse>();
                foreach(var itemRoom in rooms)
                {
                    var bed = _unitOfWork.Repository<BedRoom>().GetAllAsync().Result.Where(x => x.RoomID == itemRoom.RoomID).FirstOrDefault();
                    var listImgInDb = _unitOfWork.Repository<ImageRooms>().GetAllAsync().Result.Where(x => x.RoomID == itemRoom.RoomID).ToList();

                    //var listImage= listImgInDb.Select(x=>new List<string>
                    //{

                    //})
                    var RoomDTO = new RoomDTOResponse()
                    {
                        hotelID = itemRoom.HotelID,
                        roomNumber = itemRoom.RoomNumber,
                        roomSquare = itemRoom.RoomSquare,
                        isActive = itemRoom.IsActive,
                        idBed = bed.BedID,
                        iddetail = itemRoom.RoomDetailID,
                        quantity = bed.Quantity,
                        ImageList = listImgInDb.Select(x => $"{Request.Scheme}://{Request.Host}/Images/{x.NameFileImg}").ToList()
                    };
                    listRoomResponse.Add(RoomDTO);
                }
               
                return Ok(listRoomResponse);

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
                var room = await _roomService.getAll();
                
               if (room.Count>0)
                {
                    var listRoom = new List<RoomDTO>();
                    foreach (var itemRoom in room)
                    {
                        foreach (var img in itemRoom.ImageRooms)
                        {
                            img.NameFileImg = $"{Request.Scheme}://{Request.Host}/Images/{img.NameFileImg}";
                        }
                    }
                    //var roomDTO = room.Select(x => new RoomDTO()
                    //{
                    //    hotelID = x.HotelID,
                    //    roomNumber = x.RoomNumber,
                    //    roomSquare = x.RoomSquare,
                    //    isActive = x.IsActive,

                    //});
                    return Ok(room);
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
        public async Task<IActionResult> Post([FromForm] RoomDTO roomRequest)
        {

            var returnRespone = new RetureReponse();

            try
            {
                var hotel = _unitOfWork.Repository<Hotel>().GetByIdAsync(roomRequest.hotelID).Result;
                if (hotel == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Khách sạn không tìm thấy";
                    return NotFound(returnRespone);
                }
                var detailHotel = _unitOfWork.Repository<RoomDetail>().GetByIdAsync(roomRequest.iddetail).Result;
                if (detailHotel == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Chi tiết phòng không tìm thấy";
                    return NotFound(returnRespone);
                }
                if (roomRequest.Images.Count == 0)
                {
                    returnRespone.returnCode = 400;
                    returnRespone.returnMessage = "Vui lòng chọn ảnh";
                    return BadRequest(returnRespone);

                }

                var bed = _unitOfWork.Repository<Bed>().GetByIdAsync(roomRequest.idBed).Result;
                if (bed == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Loại giường không tìm thấy";
                    return NotFound(returnRespone);
                }
                returnRespone = _roomService.InsertRoom(roomRequest).Result;
                return Ok(returnRespone);
            }catch(Exception ex)
            {
                return BadRequest("Lỗi khi thêm dữ liệu "+ex.Message );
            }
           
        }

        // POST api/<RoomController>
        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(int id,[FromForm] RoomDTO roomRequest )
        {
            var returnRespone = new RetureReponse();

            try
            {
                var room = await _roomService.getRoomById(id);
                if (room == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Không tìm thấy phòng";
                    return NotFound(returnRespone);
                }
                var hotel = _unitOfWork.Repository<Hotel>().GetByIdAsync(roomRequest.hotelID).Result;
                if (hotel == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Khách sạn không tìm thấy";
                    return NotFound(returnRespone);
                }
                var detailHotel = _unitOfWork.Repository<RoomDetail>().GetByIdAsync(roomRequest.iddetail).Result;
                if (detailHotel == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Chi tiết phòng không tìm thấy";
                    return NotFound(returnRespone);
                }
                //if (roomRequest.Images==null)
                //{
                //    returnRespone.returnCode = 400;
                //    returnRespone.returnMessage = "Vui lòng chọn ảnh";
                //    return BadRequest(returnRespone);

                //}

                var bed = _unitOfWork.Repository<Bed>().GetByIdAsync(roomRequest.idBed).Result;
                if (bed == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Loại giường không tìm thấy";
                    return NotFound(returnRespone);
                }
                returnRespone = _roomService.UpdateRoom(id,roomRequest).Result;
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
                var room = await _roomService.getRoomById(id);
                if (room == null)
                {
                    returnRespone.returnCode = 404;
                    returnRespone.returnMessage = "Không tìm thấy phòng";
                    return NotFound(returnRespone);
                }
                await  _roomService.DeleteRoom(id);
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
