using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingHotel.Core.Models
{
    public class Bed
    {
        [Key]
        public int BedID { get; set; }
        public string BedName { get; set; }
    }
}
