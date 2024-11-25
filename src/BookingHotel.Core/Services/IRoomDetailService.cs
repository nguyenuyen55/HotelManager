using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services
{
    public interface IRoomDetailService
    {
        Task<List<RoomDetail>> getAll();
        Task<RoomDetail> getRoomDetailById(int id);
        Task<RetureReponse> InsertRoomDetail(RoomDetailDTO roomDetailDTO);
        Task<RetureReponse> UpdateRoomDetail(int idRoomDetail, RoomDetailDTO roomDetailDTO);
        Task<RetureReponse> DeleteRoomDetail(int idRoomDetail);
    }
}
