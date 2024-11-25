namespace BackendAPIBookingHotel.Model
{
	public class Service
	{
		public int ServiceID{get;set;}
		public string Title{get;set;}
		public string Description{get;set;}
		public bool? IsAvailable{get;set;}
		public int? ServiceType {get;set;}
		public decimal? Price{ get;set; }

		public ICollection<BookingDetail> BookingDetails { get; set; }

	}
}
