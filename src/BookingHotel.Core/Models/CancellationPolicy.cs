namespace BackendAPIBookingHotel.Model
{
	public class CancellationPolicy
	{
		public int CancellationPolicyID { get; set; }

		public int CancellationPeriod { get; set; }

		public decimal RefundPercentage { get; set; }

		public decimal? PenaltyFee { get; set; }

		public string Description { get; set; }

		public ICollection<Deposit> Deposits { get; set; }
	}
}
