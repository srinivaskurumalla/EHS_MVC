using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace EHS_MVC.Models
{
    public class SellerDetailsDtoViewModel
    {
        public SellerHouseDetailsViewModel HouseDetails { get; set; }
        /*  public int Id { get; set; }

          // public IEnumerable<SellerHouseDetailsViewModel> HouseDetaisiViewModels { get; set; }

          // public IEnumerable<UserDetailsViewModel> SellerDetails { get; set; }
          public string PropertyType { get; set; }


          public string PropertyName { get; set; }


          public string Address { get; set; }


          public string Region { get; set; }

          public string PropertyOption { get; set; }
          public string Description { get; set; }



          public decimal PriceRange { get; set; }
          public decimal InitialDeposit { get; set; }
          public string Landmark { get; set; }
          public DateTime UploadDate { get; set; }*/

        //Seller Foreign key
        public UserDetailsViewModel SellerDetails { get; set; }
        /* public int UserDetailsId { get; set; }
         public string Email { get; set; }
         public string PhoneNumber { get; set; }
         public string UserName { get; set; }
         public string FullName { get; set; }*/
        public CityViewModel CityViewModel { get; set; }
        public int CityId { get; set; }
    }

}
