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
    public class Court
    {
        public string CourtName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Key]
        public int CourtId { get; set; }
        public bool IsAvailable { get; set; }
        public decimal PricePerHour { get; set; }
        public string CourtImage { get; set; } = string.Empty;

    }
}
