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
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }
        public int CurrentPlayers { get; set; }
        public string Status { get; set; } = string.Empty;
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }

    }
}
