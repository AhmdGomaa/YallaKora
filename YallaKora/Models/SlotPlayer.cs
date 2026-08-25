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
    public class SlotPlayer
    {
        [Key]
        public int SlotPlayerId { get; set; }
        public int SlotId { get; set; }
        public int UserId { get; set; }
        public string Position { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public Slot? Slot { get; set; }
        public User? User { get; set; }

    }
}
