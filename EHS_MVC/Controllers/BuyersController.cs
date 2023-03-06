using EHS_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EHS_MVC.Controllers
{

    public class BuyersController : Controller
    {
        public string SelectedValue { get; set; }
        public string PriceRange { get; set; }
        public string PropertyOption { get; set; }

        private readonly IConfiguration _configuration;
        public BuyersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string propertyOption,[FromQuery] string selectedValue, [FromQuery] string priceRange = "-1")
        {
            SelectedValue = selectedValue;
            PriceRange= priceRange;
            PropertyOption = propertyOption;
            PropertyViewModel propertyViewModel = new();
            List<SellerHouseDetailsViewModel> houses = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "All";
                    propertyOption = "null";
                   
                }
               


                var result = await client.GetAsync($"Buyers/GetHouseByType/{selectedValue}");
                var tempimages = await client.GetAsync("HouseImages/GetAllHouseImages");

                decimal priceInt = Convert.ToDecimal(priceRange); //Convert.ToInt32(priceRange);
                if (result.IsSuccessStatusCode)
                {
                    var allImages = await tempimages.Content.ReadAsAsync<List<HouseImage>>();
                    propertyViewModel.HouseImages = allImages;

                    houses = await result.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();
                    if (priceInt > 0 && priceInt <30000 )
                    {
                        if (priceInt == 1)
                            priceInt = 0;
                        houses = houses.FindAll(h => h.PriceRange >= priceInt && h.PriceRange < priceInt + 10000);
                        
                    }
                    if(priceInt == 30000)
                    {
                        houses = houses.FindAll(h => h.PriceRange >= priceInt);

                    }
                    if(propertyOption != "null")
                    {
                        houses = houses.FindAll(h => h.PropertyOption == propertyOption);
                    }
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
                            new SelectListItem { Value = "Bungalow", Text = "Bungalow" },
                            new SelectListItem { Value = "IndependentHouse", Text = "IndependentHouse" },
                        };
                propertyViewModel.PriceValues = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0" ,Text="--Price--"},
                    new SelectListItem { Value = "1" ,Text="0-10000"},
                    new SelectListItem { Value = "10000" ,Text="10000-20000"},
                    new SelectListItem { Value = "20000" ,Text="20000-30000"},
                    new SelectListItem { Value = "30000" ,Text="Greater than 30000"},
                };
                propertyViewModel.Option = new List<SelectListItem>
                {
                    new SelectListItem { Value = "null" ,Text="--Property Type--"},
                    new SelectListItem { Value = "Sell" ,Text="Sell"},
                    new SelectListItem { Value = "Rent" ,Text="Rent"},
                  
                };

            }

            return View(propertyViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            SellerHouseDetailsViewModel house = null;
            UserDetailsViewModel seller = null;
            SellerDetailsDtoViewModel houseWithSellerDetails = new();


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

                    var cityNameResult = await client.GetAsync($"Cities/GetcityById/{house.CityId}");
                    var cityName = await cityNameResult.Content.ReadAsAsync<CityViewModel>();

                    house.CityName = cityName.CityName;


                    seller = await sellerDetails.Content.ReadAsAsync<UserDetailsViewModel>();

                    //  var result = await houseAndSellerDetails.Content.ReadAsAsync<UserDetailsViewModel>();

                }
                var checkAllImages = await client.GetAsync("HouseImages/GetAllHouseImages");
                if (checkAllImages.IsSuccessStatusCode)
                {
                    var AllImages = await checkAllImages.Content.ReadAsAsync<List<HouseImageViewModel>>();
                    houseWithSellerDetails.HouseImages = AllImages;

                }




                houseWithSellerDetails.HouseDetails = house;
                houseWithSellerDetails.SellerDetails = seller;





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
                if (userDetails.IsSuccessStatusCode)
                {
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
                else
                {
                    return RedirectToAction("Login", "Accounts");
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