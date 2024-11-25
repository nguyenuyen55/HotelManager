namespace BackendAPIBookingHotel.Model
{
	public class Phone
	{
		public int PhoneID {get; set; }
		public int PersonID {get; set; }
		public string PhoneNumber {get; set; }
		public string PhoneType {get; set; }
		public bool IsPrimary { get; set; }

		public Person Person { get; set; }
	}
}
