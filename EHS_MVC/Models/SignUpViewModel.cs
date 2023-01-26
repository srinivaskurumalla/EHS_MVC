using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EHS_MVC.Models
{
    public class SignUpViewModel
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your name")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your Email")]

        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Mobile Number")]

        public long MobileNo { get; set; }
        [Required(ErrorMessage = "Please enter your Password")]

        public string Password { get; set; }

        [Required(ErrorMessage = "please confirm your password")]

        public string  ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        public bool IsRemember { get; set; }
    }
}
