using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendAPIBookingHotel.Model
{
    public class TypeBed
    {
        [Key]
        public int BedID { get; set; }
        public string NameBed { get; set; }
        public int Quantity { get; set; }

    }
}