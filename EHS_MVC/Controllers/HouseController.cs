using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using EHS_MVC.Models;
using Microsoft.AspNetCore.Http;

namespace EHS_MVC.Controllers
{
    public class HouseController : Controller
    {
        private readonly IConfiguration _configuration;

        public HouseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            //string userName = TempData["UserName"].ToString();
            string userName = HttpContext.Session.GetString("sellerName");
            List<SellerHouseDetailsViewModel> House = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"House/GetAllHousesBySellerName/{userName}");
                if (result.IsSuccessStatusCode)
                {

                    House = await result.Content.ReadAsAsync<List<SellerHouseDetailsViewModel>>();

                }

            }
            return View(House);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //List<SellerHouseDetailsViewModel> House = new();
            SignUpViewModel seller = new();
            List<CityViewModel> cityViewModels = null;
            string userName = HttpContext.Session.GetString("sellerName");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Buyers/GetUserId/{userName}");
                var cities = await client.GetAsync("Cities/GetAllCities");
                if (result.IsSuccessStatusCode)
                {

                    seller = await result.Content.ReadAsAsync<SignUpViewModel>();
                    cityViewModels = await cities.Content.ReadAsAsync<List<CityViewModel>>();

                }
            }

            
            //  string userName = HttpContext.Session.GetString("sellerName");
            SellerHouseDetailsViewModel obj = new SellerHouseDetailsViewModel
            {
                UserDetailsId = seller.Id,
                CityViewModels= cityViewModels
                
            };

            return View(obj);
        }



        [HttpPost]
        public async Task<IActionResult> Create(SellerHouseDetailsViewModel House, [FromForm] int cityId)
        {
            SignUpViewModel seller = new();
            List<CityViewModel> cityViewModels = null;
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    /* client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);*/
                    //  client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    House.CityId = cityId;
                    var result = await client.PostAsJsonAsync("House/CreateHouse", House);
                    var cities = await client.GetAsync("Cities/GetAllCities");
                    cityViewModels = await cities.Content.ReadAsAsync<List<CityViewModel>>();
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                       // cityViewModels = await cities.Content.ReadAsAsync<List<CityViewModel>>();
                        return RedirectToAction("Index", "House");

                    }
                }
            }
           // cityViewModels = await cities.Content.ReadAsAsync<List<CityViewModel>>();
            SellerHouseDetailsViewModel obj = new SellerHouseDetailsViewModel
            {
                UserDetailsId = seller.Id,
                CityViewModels = cityViewModels,
            };

           // return View(obj);

            return View(House);
        }

        public async Task<IActionResult> Details(int id)
        {
            SellerHouseDetailsViewModel house = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"House/GetHouseById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    house = await result.Content.ReadAsAsync<SellerHouseDetailsViewModel>();
                }


                string houseId =  id.ToString();
                HttpContext.Session.SetString("houseId", houseId);

            }
            return View(house);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                SellerHouseDetailsViewModel House = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"House/GetHouseById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        House = await result.Content.ReadAsAsync<SellerHouseDetailsViewModel>();
                      
                        //  movie.Genres = await this.GetGenres();
                        return View(House);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Movie doesn't exists");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SellerHouseDetailsViewModel House)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"House/UpdateHouse/{House.Id}", House);
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error. Please try later");
                    }
                }
            }
            //MovieViewModel viewModel = new MovieViewModel
            //{
            //    Genres = await this.GetGenres()
            //};
            return View(House);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                SellerHouseDetailsViewModel House = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"House/GetHouseById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        House = await result.Content.ReadAsAsync<SellerHouseDetailsViewModel>();
                        //  movie.Genres = await this.GetGenres();
                        return View(House);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Movie doesn't exists");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SellerHouseDetailsViewModel House)
        {
            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"House/DeleteHouse/{House.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
        }
    }
}