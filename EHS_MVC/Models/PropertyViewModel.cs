using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EHS_MVC.Models
{
    public class PropertyViewModel
    {
        public string SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
        public List<HouseViewModel>  HouseViewModels { get; set; }
        public List<HouseViewModel> FilterByCity { get; set; }
    }
}
