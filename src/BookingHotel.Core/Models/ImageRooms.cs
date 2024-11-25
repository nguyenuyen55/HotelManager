using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BackendAPIBookingHotel.Model
{
    public class ImageRooms
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public string NameFileImg { get; set; }
        [JsonIgnore]
        public int RoomID { get; set; }

        [ForeignKey("RoomID")]
        [JsonIgnore]
        public virtual Room Room { get; set; }
    }
}