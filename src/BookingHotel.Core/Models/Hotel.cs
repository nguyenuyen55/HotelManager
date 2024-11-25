using Microsoft.AspNetCore.Http;

namespace BackendAPIBookingHotel.Model
{
	public class Hotel
	{
		public int HotelID { get; set; }
        public int AddressID { get; set; }

        public string HotelName { get; set; }

		public string Description { get; set; }

		public string? UrlImage { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public bool? isActive{get; set;} = true;


		public ICollection<Room> Rooms { get; set; }

		public ICollection<Staff> Staffs { get; set; }
	}

	public class Hotel_InsertRequestData
	{
		public string? HotelName { get; set; }
		public string? Description { get; set; }

		// Thêm thuộc tính Image để nhận file ảnh
		public IFormFile? Image { get; set; }
        public int AddressID { get; set; }

    }

    public class HotelResponseDto
    {
        public int HotelID { get; set; }
        public string Address { get; set; }

        public string HotelName { get; set; }

        public string Description { get; set; }

        public string? UrlImage { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? isActive { get; set; } = true;


        public ICollection<Room> Rooms { get; set; }

        public ICollection<Staff> Staffs { get; set; }
    }
}
