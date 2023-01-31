using EHS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace EHS_MVC.Controllers
{
   // [IgnoreAntiforgeryToken]
   

    public class AdminController : Controller
    {
        public string SelectedValue { get; set; }
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        private string _sortColumn = "PropertyName";
        private string _sortOrder = "asc";


        [AcceptVerbs("GET", "POST")]
       
        public async Task<IActionResult> Index([FromForm] int? id,[FromForm] string selectedValue = "All")
        {
                

            

           

            SelectedValue = selectedValue;
            PropertyViewModel propertyViewModel = null;
            List<SellerHouseDetailsViewModel> houses = new();
            List<CityViewModel> cityViewModels = null;
            List<SellerHouseDetailsViewModel> houses2 = new List<SellerHouseDetailsViewModel>();
            List<SellerHouseDetailsViewModel> combinedHouses = new List<SellerHouseDetailsViewModel>();

           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "All";
                }


                var result = await client.GetAsync($"Admins/GetProperties/{selectedValue}");

                var cities = await client.GetAsync("Cities/GetAllCities");

                var res = await client.GetAsync($"House/GetHousesByCityId/{id}");

                if (result.IsSuccessStatusCode)
                {


                    houses = await result.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                    cityViewModels = await cities.Content.ReadAsAsync<List<CityViewModel>>();
                    
                }

                if (result.IsSuccessStatusCode && id != null)
                {
                   foreach(var h in houses)
                    {
                        if(h.CityId == id)
                        {
                            houses2.Add(h);
                        }
                    }
                  //  houses2 = await res.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                  combinedHouses= houses2;
                }
                else
                {
                    combinedHouses = houses;
                }

               
              
            }
         

        

            propertyViewModel = new PropertyViewModel
            {
                HouseViewModels = combinedHouses,
                Values = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "All", Text = "All" },
                            new SelectListItem { Value = "Rejected", Text = "Rejected" },
                            new SelectListItem { Value = "Approved", Text = "Approved" },
                            new SelectListItem { Value = "Pending", Text = "Pending" }
                        },
                CityViewModels = cityViewModels,
                CityId = id,
             

            };

          
          
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
                var houseDetails = await client.GetAsync($"House/GetHouseById/{id}");
               
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
       
        public async Task<IActionResult> ApproveHouse( int id)
        {
          
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var res = await client.PutAsync($"Admins/ApproveHouse/{id}", null);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Admin");
                }
               

              
                return BadRequest();
            }
        }

        public async Task<IActionResult> RejectHouse(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var res = await client.PutAsync($"Admins/RejectHouse/{id}", null);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Admin");
                }



                return BadRequest();
            }
        }

       

    }
}
