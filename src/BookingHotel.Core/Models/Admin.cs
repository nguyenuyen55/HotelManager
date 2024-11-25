using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPIBookingHotel.Model
{
	public class Admin
	{
        [Key]
        public int? AdminID { get; set; }
        public string? Position { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string? AdminSpecificInfo { get; set; }
    }
}
