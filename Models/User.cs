using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
  
namespace olympic_app.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; }
  
        [Required]
        public string Password { get; set; }
        public bool isAdmin { get; set; }

    }
}