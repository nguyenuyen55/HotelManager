using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPIBookingHotel.Model
{
	public class Customer
	{
		[Key]
		public int CustomerID {get; set;}
		public DateTime? RegistrationDate {get; set;}
		public string? CustomerSpecificInfo { get; set; }

		public Person? Person { get; set; }
		public ICollection<Booking>? Bookings { get; set; }
	}

}
