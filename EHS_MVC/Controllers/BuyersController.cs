using EHS_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public async Task<IActionResult> Index([FromQuery] string selectedValue = "All")
        {
            SelectedValue = selectedValue;
            PropertyViewModel propertyViewModel = new();
            List<SellerHouseDetailsViewModel> houses = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "All";
                }


                var result = await client.GetAsync($"Buyers/GetHouseByType/{selectedValue}");
                var tempimages = await client.GetAsync("HouseImages/GetAllHouseImages");


                if (result.IsSuccessStatusCode)
                {
                    var allImages = await tempimages.Content.ReadAsAsync<List<HouseImage>>();
                    propertyViewModel.HouseImages = allImages;

                    houses = await result.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                }
                else
                {
                    houses = new List<SellerHouseDetailsViewModel>();

                }





                propertyViewModel.HouseViewModels = houses;
                propertyViewModel.Values = new List<SelectListItem>
                        {
                            new SelectListItem { Value = "All", Text = "All" },
                            new SelectListItem { Value = "Villa", Text = "Villa" },
                            new SelectListItem { Value = "Flat", Text = "Flat" },
                            new SelectListItem { Value = "Bungalow", Text = "Bungalow" }
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

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var user = HttpContext.Session.GetString("sellerName");
                var userDetails = await client.GetAsync($"Buyers/GetUserId/{user}");
                var userDetailsId = await userDetails.Content.ReadAsAsync<BuyerDetailsModel>();

                BuyerCartModel buyer = new BuyerCartModel
                {
                    HouseId = id,
                    UserDetaisId = userDetailsId.Id

                };
                var res = await client.PostAsJsonAsync("Buyers/AddToCart", buyer);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Cart(PropertyViewModel propertyView)
        {
            PropertyViewModel propertyViewModel = new();
            List<SellerHouseDetailsViewModel> ho = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

               // var user = HttpContext.Session.GetString("UserName");
                var users = HttpContext.Session.GetString("sellerName");
                var userDetails = await client.GetAsync($"Buyers/GetUserId/{users}");
                var userDetailsId = await userDetails.Content.ReadAsAsync<BuyerDetailsModel>();
                var housecheck = await client.GetAsync($"Buyers/GetAllHousesforBuyer");
                var houses = await housecheck.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                var res = await client.GetAsync($"Buyers/GetMyCart/{userDetailsId.Id}");
                var tempimages = await client.GetAsync("HouseImages/GetAllHouseImages");

                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var allImages = await tempimages.Content.ReadAsAsync<List<HouseImage>>();
                    propertyViewModel.HouseImages = allImages;
                    var cartDetails = await res.Content.ReadAsAsync<List<BuyerCartModel>>();



                    propertyViewModel.buyerCartModels = cartDetails;
                    propertyViewModel.HouseViewModels = houses;


                    return View(propertyViewModel);
                }

            }
            propertyViewModel = new PropertyViewModel
            {
                HouseImages = new(),
                HouseViewModels = new(),
                buyerCartModels = new()
            };





            return View(propertyViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromCart(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var users = HttpContext.Session.GetString("sellerName");
                var userDetails = await client.GetAsync($"Buyers/GetUserId/{users}");
                var userDetailsId = await userDetails.Content.ReadAsAsync<BuyerDetailsModel>();
                await client.DeleteAsync($"Buyers/DeleteFromCart/{id},{userDetailsId.Id}");


            }
            return RedirectToAction("Cart");
        }


    }
}