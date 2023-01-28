using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace EHS_MVC.Models
{
    public class RolesSignUpViewModel
    {
        public string SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
        public SignUpViewModel SignUpRoles { get; set; }
    }
}
