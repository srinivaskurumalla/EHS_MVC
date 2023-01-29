using EHS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EHS_MVC.Controllers
{
    
    public class BuyersController : Controller
    {
        public string SelectedValue { get; set; }
        private readonly IConfiguration _configuration;
        public BuyersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string selectedValue = "All")
        {
            SelectedValue = selectedValue;
            PropertyViewModel propertyViewModel = null;
            List<SellerHouseDetailsViewModel> houses = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "All";
                }


                var result = await client.GetAsync($"Buyers/GetHouseByType/{selectedValue}");


                if (result.IsSuccessStatusCode)
                {


                    houses = await result.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                }
                else
                {
                    houses = new List<SellerHouseDetailsViewModel>();
                }
                
                

                propertyViewModel = new PropertyViewModel
                {
                    HouseViewModels = houses,
                    Values = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "All", Text = "All" },
                            new SelectListItem { Value = "Villa", Text = "Villa" },
                            new SelectListItem { Value = "Flat", Text = "Flat" },
                            new SelectListItem { Value = "Bungalow", Text = "Bungalow" }
                        }
                };
            }

            return View(propertyViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            SellerHouseDetailsViewModel house = null;
            UserDetailsViewModel seller = null;
            SellerDetailsDtoViewModel houseWithSellerDetails = null;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var houseDetails = await client.GetAsync($"Buyers/GetHouseDetails/{id}");

                // var houseAndSellerDetails = await client.GetAsync($"Seller/GetSellerDetails/{id}");


                if (houseDetails.IsSuccessStatusCode)
                {
                    house = await houseDetails.Content.ReadAsAsync<SellerHouseDetailsViewModel>();

                    var sellerId = house.UserDetailsId;
                    var sellerDetails = await client.GetAsync($"Seller/GetSellerById/{sellerId}");

                    seller = await sellerDetails.Content.ReadAsAsync<UserDetailsViewModel>();

                    //  var result = await houseAndSellerDetails.Content.ReadAsAsync<UserDetailsViewModel>();

                }
                houseWithSellerDetails = new SellerDetailsDtoViewModel
                {
                    HouseDetails = house,
                    SellerDetails = seller
                };
                return View(houseWithSellerDetails);
            }

        }
    }
}
