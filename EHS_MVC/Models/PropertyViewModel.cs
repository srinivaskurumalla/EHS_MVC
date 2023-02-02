using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EHS_MVC.Models
{
    public class PropertyViewModel
    {
        public string SelectedValue { get; set; }
        public IEnumerable<SelectListItem> Values { get; set; }
        public List<SellerHouseDetailsViewModel>  HouseViewModels { get; set; }
        public List<CityViewModel> CityViewModels  { get; set; }
        public int? CityId { get; set; }
        public string SortOrder { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
        public int SellerId { get; set; }
        public int UserDetailsId { get; set; }
        public List<HouseImage> HouseImages { get; set; }
        public string ImageName { get; set; }
        public int HouseId { get; set; }
        public List<BuyerCartModel> buyerCartModels { get; set; }
       
        public string SortColumn { get; set; }
      

    }
}
