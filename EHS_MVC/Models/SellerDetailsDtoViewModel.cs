using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace EHS_MVC.Models
{
    public class SellerDetailsDtoViewModel
    {
        public SellerHouseDetailsViewModel HouseDetails { get; set; }
      
        //Seller Foreign key
        public UserDetailsViewModel SellerDetails { get; set; }
      
        public CityViewModel CityViewModel { get; set; }
        public int CityId { get; set; }
        public string Remarks{ get; set; }

    }

}
