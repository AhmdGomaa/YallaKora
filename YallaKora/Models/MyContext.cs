using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace YallaKora.Models
{
    public class MyContext : DbContext
    {
        string connectionString = "Server=AHMED-LAPTOP;Database=YallaKora;Trusted_Connection=True;TrustServerCertificate=True;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<SlotPlayer> SlotPlayers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ToUser)
                .WithMany()
                .HasForeignKey(r => r.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);


           
            modelBuilder.Entity<Slot>()
                 .HasMany<SlotPlayer>()
                 .WithOne(sp => sp.Slot)
                 .HasForeignKey(sp => sp.SlotId)
                 .OnDelete(DeleteBehavior.Restrict);   

            modelBuilder.Entity<User>()
                .HasMany<SlotPlayer>()
                .WithOne(sp => sp.User)
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Restrict);  


            modelBuilder.Entity<Court>().HasData(
                new Court { CourtId = 1, CourtName = "Court 1", IsAvailable = true, PricePerHour = 200 },
                new Court { CourtId = 2, CourtName = "Court 2", IsAvailable = true, PricePerHour = 200 },
                new Court { CourtId = 3, CourtName = "Court 3", IsAvailable = true, PricePerHour = 400 },
                new Court { CourtId = 4, CourtName = "Court 4", IsAvailable = false, PricePerHour = 1000 });

            modelBuilder.Entity<User>().HasData(
                new User { FirstName = "Ahmed", LastName = "Gomaa", UserName = "Ahmed Gomaa", Age = 20, Email = "ahmed@gmail.com", Password = "1234", Role = "Admin" ,Address="ringroad", PhoneNumber = "01032455" , UserId=1 , UserPosition = "defender" , ProfileImage= "D:\\.NET Web diplome\\Advanced C#\\YallaKora\\YallaKora\\wwwroot\\ball-soccer-soccer-ball-1530417 (1).jpg" },
                new User { FirstName = "Ayman", LastName = "Refaat", UserName = "Ayman_Refaat23", Age = 45, Email = "Ayman@gmail.com", Password = "12345", Role = "User",
                    Address = "ringroad " , PhoneNumber="01032455" , UserId=2 , UserPosition="defender" ,
                    ProfileImage = "D:\\.NET Web diplome\\Advanced C#\\YallaKora\\YallaKora\\wwwroot\\ball-soccer-soccer-ball-1530417 (1).jpg"
                }

             );

        }


    }
}
