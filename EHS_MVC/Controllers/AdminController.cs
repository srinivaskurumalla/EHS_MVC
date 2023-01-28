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
    public class AdminController : Controller
    {
        public string SelectedValue { get; set; }
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public async Task<IActionResult> Index([FromQuery]string selectedValue = "All")

        {
            SelectedValue = selectedValue;
            PropertyViewModel propertyViewModel = null;
            List<HouseViewModel> houses = new();

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


                    houses = await result.Content.ReadAsAsync<List<HouseViewModel>>();
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

       

    }
}
