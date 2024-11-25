using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPIBookingHotel.Model
{
	public class Staff
	{
		[Key]
		public int? StaffID { get; set; }
		public int? Position { get; set; }
		public string? HireDate{get; set;}
		public int? HotelID { get; set; }

		public Hotel? Hotel { get; set; }

	}
}
