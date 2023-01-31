using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EHS_MVC.Models
{
    public class HouseImageViewModel
    {
        public int Id { get; set; }
        [Required]
        public IFormFile CoverImage { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public int HouseId { get; set; }
        public SellerHouseDetailsViewModel House { get; set; }
        public int SellerId { get; set; }
    }

}
