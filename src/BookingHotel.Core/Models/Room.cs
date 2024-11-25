using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendAPIBookingHotel.Model
{
	public class Room
	{
		[Key]
		public int RoomID { get; set; }
		public int HotelID { get; set; }
		public string RoomNumber { get; set; }
		public int RoomSquare { get; set; }
		public bool IsActive { get; set; }
        public int RoomDetailID { get; set; }
		[JsonIgnore]
        public Hotel Hotel { get; set; }
		public ICollection<Booking> Bookings { get; set; }
		public virtual ICollection<BedRoom> BedRooms { get; set; }
        public virtual ICollection<ImageRooms> ImageRooms { get; set; }

        //them khoa ngoai
        [ForeignKey("RoomDetailID")]
        public virtual RoomDetail RoomDetail { get; set; }


    }


}
