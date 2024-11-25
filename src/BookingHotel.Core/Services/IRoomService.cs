using BackendAPIBookingHotel.Model;
using BookingHotel.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Services
{
    public interface IRoomService
    {
        Task<List<Room>> getAll();
        Task<Room> getRoomById(int id);
        Task<List<Room>> getListRoomsByHotelId(int idHotel);
        Task<RetureReponse> InsertRoom(RoomDTO roomDTO);
        Task<RetureReponse> UpdateRoom(int idRoom, RoomDTO roomDTO);
        Task<RetureReponse> DeleteRoom(int idRoom);
    }
}
