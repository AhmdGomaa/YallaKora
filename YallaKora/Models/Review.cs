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
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public User? FromUser { get; set; }
        public User? ToUser { get; set; }

    }
}
