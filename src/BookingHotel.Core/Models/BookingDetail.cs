namespace BackendAPIBookingHotel.Model
{
	public class BookingDetail
	{
		public int ID{get; set ;}
		public int ServiceID{get; set ;}
		public int BookingID{get; set ;}
		public int Quantity{get; set ;}
	
		public DateTime DateService { get; set; }


		public Booking Booking { get; set; }
		public Service Service { get; set; }

	}
}
