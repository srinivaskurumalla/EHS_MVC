using FakeItEasy;
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
        [RegularExpression("^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$", ErrorMessage = "Invalid Email Id")]


        public string Password { get; set; }

        [Required(ErrorMessage = "please confirm your password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password doesn't match")]


        public string  ConfirmPassword { get; set; }

       

       
    }
}
