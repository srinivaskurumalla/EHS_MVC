using EHS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        [HttpGet]

        public async Task<IActionResult> Index([FromQuery] string selectedValue = "All")

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


                var result = await client.GetAsync($"Admins/GetProperties/{selectedValue}");


                if (result.IsSuccessStatusCode)
                {


                    houses = await result.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                }

                propertyViewModel = new PropertyViewModel
                {
                    HouseViewModels = houses,
                    Values = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "All", Text = "All" },
                            new SelectListItem { Value = "Rejected", Text = "Rejected" },
                            new SelectListItem { Value = "Approved", Text = "Approved" },
                            new SelectListItem { Value = "Pending", Text = "Pending" }
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

      /*  public async Task<IActionResult> HousesByCity(int id)
        {
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync($"House/GetHousesByCityId/{id}");

                if(res.IsSuccessStatusCode)
                {
                    rt
                }
            }
        }*/
    }
}
