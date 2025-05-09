using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;

namespace Restaurant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Map your domain entities to tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffSchedule> StaffSchedules { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<POSLog> POSLogs { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<TimeOffRequest> TimeOffRequests { get; set; }
        public DbSet<ShiftSwapRequest> ShiftSwapRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optionally add custom configurations here.
            // For example, you might configure a check constraint for NumberOfGuests:
            // modelBuilder.Entity<Reservation>()
            //     .Property(r => r.NumberOfGuests)
            //     .IsRequired();

        }
    }
}
