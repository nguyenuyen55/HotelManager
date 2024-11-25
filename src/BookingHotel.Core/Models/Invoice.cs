namespace BackendAPIBookingHotel.Model
{
	public class Invoice
	{
		public int InvoiceID{get; set;}
		public int BookingID{get; set;}
		public DateTime InvoiceDate{get; set;}
		public int Amount{get; set;}
		public string PaymentMethod{get; set;}
		public string PaymentStatus { get; set; }

		public Booking Booking { get; set; }

	}
}
