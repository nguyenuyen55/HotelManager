using BackendAPIBookingHotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.DTO.Room
{
    public class RoomResponse
    {

        public int RoomID { get; set; }
        public int HotelID { get; set; }
        public string RoomNumber { get; set; }
        public int RoomSquare { get; set; }
        public bool IsActive { get; set; }
        public int RoomDetailID { get; set; }
        public Hotel Hotel { get; set; }

        public virtual List<ImageRooms> ImageRooms { get; set; }

        public virtual RoomDetail RoomDetail { get; set; }
    }
}
