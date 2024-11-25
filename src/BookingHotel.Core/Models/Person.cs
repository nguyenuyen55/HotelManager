namespace BackendAPIBookingHotel.Model
{
	public class Person
	{
		public int PersonID { get; set; }

		public string FirstName { get; set; }


		public string LastName { get; set; }

		public DateTime DOB { get; set; }

		public ICollection<Email> Emails { get; set; }

		public ICollection<Phone> Phones { get; set; }

		public ICollection<Address> Addresses { get; set; }
		
	}
}
