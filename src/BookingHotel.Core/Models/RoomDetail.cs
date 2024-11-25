using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPIBookingHotel.Model
{
	public class RoomDetail
	{
		[Key]
		public int RoomDetailID { get; set; }
        public string RoomFittings{get;set;}
		public string RoomView{get;set;}
		public string RoomType{get;set;}
		public int PricePerNight{get;set;}
		public int IsAvailable{ get; set; }
	}
}
