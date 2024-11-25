using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services
{
    public class RoomDetailService : IRoomDetailService
    {

        private readonly IUnitOfWork _unitOfWork;

        public RoomDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RetureReponse> DeleteRoomDetail(int idRoomDetail)
        {
            var reponse = new RetureReponse();
            await _unitOfWork.Repository<RoomDetail>().DeleteAsync(idRoomDetail);
            await _unitOfWork.SaveChangesAsync();
            reponse.returnCode = 200;
            reponse.returnMessage = "Xóa Phòng khách sạn thành công";
            return reponse;
        }

        public async Task<List<RoomDetail>> getAll()
        {
            var list = new List<RoomDetail>();
            list = (List<RoomDetail>)await _unitOfWork.Repository<RoomDetail>().GetAllAsync();
            return list;
        }

        public Task<RoomDetail> getRoomDetailById(int id)
        {
            var room = _unitOfWork.Repository<RoomDetail>().GetByIdAsync(id);
            return room;
        }

        public async Task<RetureReponse> InsertRoomDetail(RoomDetailDTO roomDetailDTO)
        {
            var reponse = new RetureReponse();

            var roomDetail = new RoomDetail()
            {
                RoomFittings = roomDetailDTO.RoomFittings,
                RoomView=roomDetailDTO.RoomView,
                RoomType=roomDetailDTO.RoomType,
                PricePerNight=roomDetailDTO.PricePerNight,
                IsAvailable=roomDetailDTO.IsAvailable
            };
            await _unitOfWork.Repository<RoomDetail>().AddAsync(roomDetail);
            await _unitOfWork.SaveChangesAsync();
            reponse.returnCode = 200;
            reponse.returnMessage = "Thêm chi tiết phòng khách sạn thành công";
            return reponse;
        }

        public async Task<RetureReponse> UpdateRoomDetail(int idRoomDetail, RoomDetailDTO roomDetailDTO)
        {
            var reponse = new RetureReponse();
            var roomDetail = await _unitOfWork.Repository<RoomDetail>().GetByIdAsync(idRoomDetail);
            roomDetail.RoomFittings = roomDetailDTO.RoomFittings;
            roomDetail.RoomView = roomDetailDTO.RoomView;
            roomDetail.RoomType = roomDetailDTO.RoomType;
            roomDetail.PricePerNight = roomDetailDTO.PricePerNight;
            roomDetail.IsAvailable = roomDetailDTO.IsAvailable;
            await _unitOfWork.SaveChangesAsync();
            reponse.returnCode = 200;
            reponse.returnMessage = "Cập nhật chi tiết phòng khách sạn thành công";
            return reponse;
        }
    }
}
