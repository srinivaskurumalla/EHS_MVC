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
        public List<HouseImage> HouseImages { get; set; }
        public string ImageName { get; set; }
        public int HouseId { get; set; }
      //      public string CoverImageUrl { get; set; }

        // public int MyProperty { get; set; }

        /*  private string sortOrder
          {
              set
              {
                  if(value == null)
                  {
                      SortOrder = "asc";
                  }
                  else
                  {
                      SortOrder = "dsc";
                  }
              }
              get
              {
                  return SortOrder;
              }
          }*/
        public string SortColumn { get; set; }
       /* private string sortColumn
        {
            set
            {
                if(value == null)
                {
                    SortColumn = "PriceRange";
                }
                SortColumn = value;
            }
            get
            {
                return SortColumn;
            }
        }*/

    }
}
