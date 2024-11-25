using BookingHotel.Core.Models;

namespace BackendAPIBookingHotel.Model
{
	public class Role
	{
		public int RoleID { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }

        // Foreign key from Permission table
        public int? PermissionId { get; set; }
        public Permission? Permission { get; set; } // Each Role has one Permission
	}	
}
