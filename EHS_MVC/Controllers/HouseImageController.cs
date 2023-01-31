using EHS_MVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EHS_MVC.Controllers
{
    public class HouseImageController : Controller
    {
        private readonly IConfiguration _configuration;

        public HouseImageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                PropertyViewModel propertyViewModel = new();
                string userName = HttpContext.Session.GetString("sellerName");
               var userDetails = await client.GetAsync($"Buyers/GetUserId/{userName}");
               
                var userDetailsId = await userDetails.Content.ReadAsAsync<UserDetailsViewModel>();
               
                var result = await client.GetAsync($"HouseImages/GetAllHouseImages");
                if (result.IsSuccessStatusCode)
                {
                  /*  string houseId = HttpContext.Session.GetString("houseId");
                    int houseId1 = Convert.ToInt32(houseId);
*/
                    

                    propertyViewModel.HouseImages = await result.Content.ReadAsAsync<List<HouseImage>>();
                    
                    //hm.SellerId = houseId1;
                    propertyViewModel.HouseId = Convert.ToInt32(HttpContext.Session.GetString("houseId"));
                    propertyViewModel.SellerId = userDetailsId.Id;
                   
                    

                }

                return View(propertyViewModel);
            }
        }

        [HttpGet]
        public  IActionResult Create()
        {
            HouseImageViewModel houseImageViewModel = new();
            string houseId = HttpContext.Session.GetString("houseId");
            int houseId1 = Convert.ToInt32(houseId);
            houseImageViewModel.HouseId= houseId1;
           // houseImageViewModel
            //stopped here

            return View(houseImageViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HouseImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", model.CoverImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/" + model.CoverImage.FileName;

                // Save the rest of the data to your database
            }
            using (var client = new HttpClient())
            { client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                var result = await client.PostAsJsonAsync("HouseImages/CreateHouseImage", model);
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                   
                    return RedirectToAction("Index", "HouseImage");

                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteHouseImageById(int id)
        {
            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"HouseImages/DeleteHouseImage/{id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","HouseImage");
                }
                else
                {
                    return RedirectToAction("Login", "Accounts");
                }
            }
        }
    }
}
