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
                   new User { Id = 1, Username = "elf", PasswordHash = "password", Role = "Admin", IsActive = true },
                   new User { Id = 2, Username = "peter", PasswordHash = "password", Role = "User", IsActive = true },
                   new User { Id = 3, Username = "isac", PasswordHash = "password", Role = "Customer", IsActive = true }
               );


            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    Email = "AnnaAndersson@hotmail.com",
                    Phone = "0788995544",
                    FirstName = "Anna",
                    LastName = "Andersson",
                    CreationDate = DateTime.UtcNow.AddDays(-10),
                    UpdateDate = DateTime.UtcNow,
                    UserId = 2
                },
                new Customer
                {
                    CustomerId = 2,
                    Email = "KarlKarlsson@hotmail.com",
                    Phone = "+4698765432",
                    FirstName = "Karl",
                    LastName = "Karlsson",
                    CreationDate = DateTime.UtcNow.AddDays(-8),
                    UpdateDate = DateTime.UtcNow,
                    UserId = 3
                }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyId = 1,
                    CompanyName = "FBG SJUKVÅRD",
                    CompanyEmail = "fbg@sjukvård.se",
                    UserId = 1
                },
                new Company
                {
                    CompanyId = 2,
                    CompanyName = "Friskis & Svettis",
                    CompanyEmail = "contact@friskissvettis.se",
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
                    Title = "Undersökning",
                    Description = "Undersökning för att hitta eventuella skador/sjukdomar",
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
                    Title = "PT-Pass",
                    Description = "Träningspass med våran PT",
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
                    Action = "Skapad",
                    Details = "Möte skapad för Anna Andersson",
                    CustomerId = 1,
                    CompanyId = 1
                },
                new ChangeLog
                {
                    Id = 2,
                    AppointId = 2,
                    ChangedAtDate = DateTime.UtcNow,
                    Action = "Skapad",
                    Details = "Möte skapad för Karl Karlsson",
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