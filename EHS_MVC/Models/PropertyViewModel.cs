using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EHS_MVC.Models
{   
    public class PropertyViewModel
    {
        public string SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
        public List<SellerHouseDetailsViewModel>  HouseViewModels { get; set; }
        public List<SellerHouseDetailsViewModel> FilterByCity { get; set; }
        public List<CityViewModel> CityViewModels { get; set; }
        public LoginViewModel loginViewModels { get; set; }
        public List<BuyerCartModel> buyerCartModels { get; set; }
        public List<int> HouseIds { get; set; }
    }
}
