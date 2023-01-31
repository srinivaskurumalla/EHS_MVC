using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace EHS_MVC.Models
{

    public class BuyerCartModel
    {
        [Required]
        public int Id { get; set; }
        
        public int UserDetaisId { get; set; }

        public int HouseId { get; set; }
    }

    
    
}
