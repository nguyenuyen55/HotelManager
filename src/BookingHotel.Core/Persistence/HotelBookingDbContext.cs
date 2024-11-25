using BackendAPIBookingHotel.Model;
using BookingHotel.Core.Models;
using Microsoft.EntityFrameworkCore;

/*
    dotnet ef migrations add UpdateUserTableTT --startup-project ../BookingHotel.Api
    dotnet ef database update --startup-project ../BookingHotel.Api
*/
namespace BookingHotel.Core.Persistence
{
    public class HotelBookingDbContext : DbContext
    {
        public HotelBookingDbContext(DbContextOptions<HotelBookingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<CancellationPolicy> CancellationPolicies { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomDetail> RoomDetails { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<BedRoom> BedRooms { get; set; }
        public DbSet<ImageRooms> ImageRooms { get; set; }
        public DbSet<Contact> Contacts { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thêm tiền tố "BE072024_HB_" cho tất cả các bảng
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName($"BE072024_HB_{entityType.GetTableName()}");
            }
            modelBuilder.Entity<Address>()
                .HasKey(a => a.AddressID);

            modelBuilder.Entity<CancellationPolicy>()
                .HasKey(cp => cp.CancellationPolicyID);

            modelBuilder.Entity<Hotel>()
                .HasKey(h => h.HotelID);

            modelBuilder.Entity<Person>()
                .HasKey(p => p.PersonID);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleID);

      
            // Seed dữ liệu mặc định cho bảng Roles
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleID = 1,
                    RoleName = "Admin",
                    Description = "Administrator role with full permissions"
                },
                new Role
                {
                    RoleID = 2,
                    RoleName = "User",
                    Description = "Regular user with limited permissions"
                },
                new Role
                {
                    RoleID = 3,
                    RoleName = "Customer",
                    Description = "Customer role with permissions to book and view hotels"
                },
                new Role
                {
                    RoleID = 4,
                    RoleName = "Staff",
                    Description = "Staff role with permissions to manage hotel operations"
                }
            );

            modelBuilder.Entity<Service>()
                .HasKey(s => s.ServiceID);

            modelBuilder.Entity<Admin>()
                .HasKey(a => a.AdminID);

            modelBuilder.Entity<Customer>()
                .HasKey(c => c.CustomerID);

            modelBuilder.Entity<Email>()
                .HasKey(e => e.EmailID);

            modelBuilder.Entity<Phone>()
                .HasKey(p => p.PhoneID);

            modelBuilder.Entity<Room>()
                .HasKey(r => r.RoomID);

            modelBuilder.Entity<RoomDetail>()
                .HasKey(rd => rd.RoomDetailID);

            modelBuilder.Entity<Staff>()
                .HasKey(s => s.StaffID);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);
            modelBuilder.Entity<User>()
            .Property(u => u.UserID)
            .ValueGeneratedNever();

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.UserRoleId);

            modelBuilder.Entity<Booking>()
                .HasKey(b => b.BookingID);

            modelBuilder.Entity<BookingDetail>()
                .HasKey(bd => bd.ID);

            modelBuilder.Entity<Deposit>()
                .HasKey(d => d.DepositID);

            modelBuilder.Entity<Invoice>()
                .HasKey(i => i.InvoiceID);
            modelBuilder.Entity<Bed>()
              .HasKey(i => i.BedID);

            // Các cấu hình khóa ngoại
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Persons)
                .WithMany(p => p.Addresses)
                .HasForeignKey(a => a.PersonID)
                .OnDelete(DeleteBehavior.Restrict); // Hoặc DeleteBehavior.NoAction nếu cần

            modelBuilder.Entity<Email>()
                .HasOne(e => e.Person)
                .WithMany(p => p.Emails)
                .HasForeignKey(e => e.PersonID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Phone>()
                .HasOne(p => p.Person)
                .WithMany(p => p.Phones)
                .HasForeignKey(p => p.PersonID)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Admin>()
            //    .HasOne(a => a.Person)
            //    .WithOne()
            //    .HasForeignKey<Admin>(a => a.AdminID)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Person)
                .WithMany()
                .HasForeignKey(c => c.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Hotel)
                .WithMany(h => h.Staffs)
                .HasForeignKey(s => s.HotelID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelID)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Room>()
            //    .HasOne(r => r.RoomDetail)
            //    .WithMany(rd => rd.Rooms)
            //    .HasForeignKey(r => r.RoomDetailId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình 1-N giữa Permission và Role
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Permission)   // Mỗi Role có một Permission
                .WithMany(p => p.Roles)      // Một Permission có nhiều Role
                .HasForeignKey(r => r.PermissionId)
                .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa Permission nếu có Role liên kết

            modelBuilder.Entity<BedRoom>()
               .HasKey(br => new { br.RoomID, br.BedID });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Booking>()
            //    .HasOne(b => b.Customer)
            //    .WithMany(c => c.Bookings)
            //    .HasForeignKey(b => b.Co)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingDetail>()
                .HasOne(bd => bd.Booking)
                .WithMany(b => b.BookingDetails)
                .HasForeignKey(bd => bd.BookingID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingDetail>()
                .HasOne(bd => bd.Service)
                .WithMany(s => s.BookingDetails)
                .HasForeignKey(bd => bd.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.Booking)
                .WithMany(b => b.Deposits)
                .HasForeignKey(d => d.BookingID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Deposit>()
                .HasOne(d => d.CancellationPolicy)
                .WithMany(cp => cp.Deposits)
                .HasForeignKey(d => d.CancellationPolicyID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Booking)
                .WithMany(b => b.Invoices)
                .HasForeignKey(i => i.BookingID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
