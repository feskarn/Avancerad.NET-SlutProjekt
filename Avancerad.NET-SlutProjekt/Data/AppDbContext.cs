using ClassLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Avancerad.NET_SlutProjekt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ChangeLog> ChangeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", PasswordHash = "password", Role = "Admin", IsActive = true },
            new User { Id = 2, Username = "user1", PasswordHash = "hejhej", Role = "User", IsActive = true },
            new User { Id = 3, Username = "user2", PasswordHash = "user2", Role = "User", IsActive = true }
            );


            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Email = "john.wall@hotmail.com",
                    Phone = "+4634567890",
                    FirstName = "John",
                    LastName = "Wall",
                    CreationDate = DateTime.UtcNow.AddDays(-10),
                    UpdateDate = DateTime.UtcNow,
                    UserId = 2
                },
                new Customer
                {
                    CustomerId = 2,
                    Email = "mary.jane@hotmail.com",
                    Phone = "+4698765432",
                    FirstName = "Mary",
                    LastName = "Jane",
                    CreationDate = DateTime.UtcNow.AddDays(-8),
                    UpdateDate = DateTime.UtcNow,
                    UserId = 3
                }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    CompanyName = "Tech Solutions Inc.",
                    CompanyEmail = "info@techsolutions.com",
                    UserId = 1
                },
                new Company
                {
                    CompanyId = 2,
                    CompanyName = "Innovative Designs LLC",
                    CompanyEmail = "contact@innovativedesigns.com",
                    UserId = 1
                }
            );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    AppointId = 1,
                    StartingTime = DateTime.UtcNow.AddDays(5),
                    AppointmentDurationMinutes = 60,
                    CreationDate = DateTime.UtcNow.AddDays(-1),
                    UpdateDate = DateTime.UtcNow,
                    Title = "Consultation Meeting",
                    Description = "Meeting to discuss project requirements.",
                    CompanyId = 1,
                    CustomerId = 1
                },
                new Appointment
                {
                    AppointId = 2,
                    StartingTime = DateTime.UtcNow.AddDays(7),
                    AppointmentDurationMinutes = 30,
                    CreationDate = DateTime.UtcNow.AddDays(-2),
                    UpdateDate = DateTime.UtcNow,
                    Title = "Design Review",
                    Description = "Review of the initial design drafts.",
                    CompanyId = 2,
                    CustomerId = 2
                }
            );

            modelBuilder.Entity<ChangeLog>().HasData(
                new ChangeLog
                {
                    Id = 1,
                    AppointId = 1,
                    ChangedAtDate = DateTime.UtcNow,
                    Action = "Created",
                    Details = "Appointment created for John Wall",
                    CustomerId = 1,
                    CompanyId = 1
                },
                new ChangeLog
                {
                    Id = 2,
                    AppointId = 2,
                    ChangedAtDate = DateTime.UtcNow,
                    Action = "Created",
                    Details = "Appointment created for Mary Jane",
                    CustomerId = 2,
                    CompanyId = 2
                }
            );

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Company)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}