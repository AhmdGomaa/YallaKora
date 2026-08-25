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
    public class Booking
    {
        public string Type { get; set; } = string.Empty;
        [Key]
        public int BookingId { get; set; }
        public int CourtId { get; set; }
        public int UserId { get; set; }
        public bool WantEquipment { get; set; }
        public decimal EquipmentPrice { get; set; }
        public DateTime Date {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }= string.Empty;
        public decimal TotalPrice { get; set; }
        public int? OpponentUserId { get; set; }
        public string BookingDay { get; set; } = string.Empty;

        public Court? Court { get; set; }
        public User? User { get; set; }

    }
}
