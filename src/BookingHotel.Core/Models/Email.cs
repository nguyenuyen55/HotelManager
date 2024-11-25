using System.Net.Mail;

namespace BackendAPIBookingHotel.Model
{
	public class Email
	{
		public int EmailID{get;set;}
		public int PersonID{get;set;}
		public string EmailAddress{get;set;}
		public string EmailType{get;set;}
		public bool IsPrimary { get; set; }

		public Person Person { get; set; }
	}
}
