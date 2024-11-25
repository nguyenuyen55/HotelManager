using Microsoft.AspNetCore.Http;

namespace BackendAPIBookingHotel.Model
{
	public class Booking
	{

		public int BookingID {get ; set ;}


		public int RoomID { get; set; }
		public int? ContactID {get ; set ;}
		public int? UserID { get; set;}
		public int? DepositID {get ; set ;}
		
		//public DateTime BookingDate {get ; set ;}
		public DateTime FromDate { get; set;}
		public DateTime? CheckInDate {get ; set ;}
		public DateTime? CheckOutDate {get ; set ;}
		public string BookingStatus {get ; set ;}
		public DateTime ToDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool isActive { get; set; } = true;
		public string? Note { get ; set ;} 


        public Room Room { get; set; }
		//public Customer Customer { get; set; }
		public ICollection<BookingDetail> BookingDetails { get; set; }
		public ICollection<Deposit> Deposits { get; set; }
		public ICollection<Invoice> Invoices { get; set; }

	}

    public class Booking_InsertRequestData
    {
        public int RoomID { get; set; }
        public int? ContactID { get; set; }
        public int? UserID { get; set; }
        public int? DepositID { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string BookingStatus { get; set; }
        public DateTime ToDate { get; set; }
        public string? Note { get; set; }

    }

	public class BookingResponseDto
{
    public int BookingID { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string BookingStatus { get; set; }
    public string CustomerName { get; set; }
    public string RoomType { get; set; }
    public string HotelName {get; set; }
}
}
