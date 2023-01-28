using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EHS_MVC.Models
{
    public class SignUpViewModel
    {

        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the User Name")]
        [MaxLength(50)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User name should be minimum of 3 characters and maximum of 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter the Password")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password is too short.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter the Password")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password is too short.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter the Email Address")]
        //[EmailAddress(ErrorMessage = "please enter a valid email address")]
        public string Email { get; set; }


        [Required]
        //[Phone]
        public string PhoneNumber { get; set; }
        [Required]
        //[MaxLength(50)]
        public string FullName { get; set; }

        public string Role { get; set; }

        //public bool IsActive { get; set; }

        //public bool IsRemember { get; set; }
    }
}
