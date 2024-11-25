namespace BackendAPIBookingHotel.Model
{
	public class Deposit
	{
		public int DepositID { get; set; }
		public int BookingID { get; set; }
		public DateTime DepositDate { get; set; }
		public int DepositAmount { get; set; }
		public int DepositStatus { get; set; }
		public int CancellationPolicyID { get; set; }

		public Booking Booking { get; set; }
		public CancellationPolicy CancellationPolicy { get; set; }
	}
}
