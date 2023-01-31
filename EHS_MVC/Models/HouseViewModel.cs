using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EHS_MVC.Models
{
    public class SellerHouseDetailsViewModel
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string PropertyType { get; set; }

        [Required]
        [MaxLength(50)]
        public string PropertyName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string Region { get; set; }

        [Required]
        [MaxLength(50)]
        public string PropertyOption { get; set; }
        public string Description { get; set; }

        [Required]

        public decimal PriceRange { get; set; }
        [Required]

        public decimal InitialDeposit { get; set; }
        public string Landmark { get; set; }
        public DateTime UploadDate { get; set; }

        //Seller Foreign key
        public int UserDetailsId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }

        //navingation property
        public UserDetailsViewModel UserDetailsViewModel { get; set; }
        public CityViewModel CityViewModel { get; set; }

        public List<CityViewModel> CityViewModels { get; set; }
        // public int HouseImageId { get; set; }
        public ICollection<HouseImage> HouseImages { get; set; }
        public int HouseId { get; set; }

        public string Status { get; set; }

    }


    //House Image Details

    public class HouseImage
    {
        public int Id { get; set; }
        public string CoverImageUrl { get; set; }
        public string ImageName { get; set; }
        //House Foreign key
        public int HouseId { get; set; }
        public int SellerId { get; set; }
        //navingation property
        public SellerHouseDetailsViewModel HouseViewModel { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CityViewModel
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        //Foreign key
        public int StateId { get; set; }
        public State State { get; set; }

        public ICollection<SellerHouseDetailsViewModel> Houses { get; set; }
    }
    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; }

        //  public Seller seller { get; set; }
        public ICollection<CityViewModel> Cities { get; set; }

    }

}