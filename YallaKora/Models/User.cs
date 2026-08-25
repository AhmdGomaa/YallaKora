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
    public class User
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = String.Empty;
        [Required]
        public string UserName { get; set; } = String.Empty;

        public int Age { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }= string.Empty;
        [Required]
        public string Role { get; set; }= string.Empty;
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = String.Empty;
        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        public string Password { get; set; } = String.Empty;
        public string Address {  get; set; } = String.Empty;
        public string UserPosition {  get; set; } = String.Empty;
        public string ProfileImage { get; set; } = string.Empty;
    }
}
