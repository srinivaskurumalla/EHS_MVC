using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EHS_MVC.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage ="Please enter your name")]
        public string Username { get; set; }

        [Required(ErrorMessage ="please enter your password")]
        public string Password { get; set; }
    }
}
