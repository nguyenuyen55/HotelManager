using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingHotel.Core.DTO
{
    public class RoomDTO
    {
        public int hotelID { get; set; }
        public string roomNumber { get; set; }
        public int roomSquare { get; set; }
        public bool isActive { get; set; } = true;

       public int iddetail {get;set;}      
   
       public int idBed {get;set;}   
       public int quantity { get; set; }
        [JsonIgnore]
        public List<IFormFile>? Images { get; set; }
        //ListImage

    }
    public class RoomDTOResponse:RoomDTO
    {
        public List<string>? ImageList { get; set; }
        //ListImage

    }
}
