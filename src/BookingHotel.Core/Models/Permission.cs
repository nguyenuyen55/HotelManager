using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendAPIBookingHotel.Model;

namespace BookingHotel.Core.Models
{
    public class Permission
    {
         public int PermissionId { get; set; } // Change to int to match Role
        public string? Screen_Url { get; set; }
        public string? Create { get; set; }
        public string? Insert { get; set; }
        public string? Delete { get; set; }
        public string? Approve { get; set; }
        public string? RoleId { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}