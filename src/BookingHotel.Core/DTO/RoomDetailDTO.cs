using BackendAPIBookingHotel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.DTO
{
    public class RoomDetailDTO
    {
           public string RoomFittings {get;set;}
           public string RoomView     {get;set;}
           public string RoomType     {get;set;}
           public int PricePerNight{get;set;}
           public int IsAvailable { get; set; } = 1;
        
    }
}
