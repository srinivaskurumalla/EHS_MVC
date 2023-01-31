using EHS_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EHS_MVC.Controllers
{

    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Accounts/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);
                        HttpContext.Session.SetString("UserName", login.Username);
                        
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            return View(login);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Login", "Accounts");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            RolesSignUpViewModel model = new RolesSignUpViewModel
            {
                Values = new List<SelectListItem>
                         {
                             new SelectListItem { Value = "Seller", Text = "Seller" },
                             new SelectListItem { Value = "Buyer", Text = "Buyer" }
                         }


            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] RolesSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);


                    SignUpViewModel user = new SignUpViewModel
                    {
                        UserName = model.SignUpRoles.UserName,
                        FullName = model.SignUpRoles.FullName,
                        Password = model.SignUpRoles.Password,
                        ConfirmPassword = model.SignUpRoles.ConfirmPassword,
                        Email = model.SignUpRoles.Email,
                        PhoneNumber = model.SignUpRoles.PhoneNumber,
                        Role = model.SelectedValue


                    };

                    var result = await client.PostAsJsonAsync("Accounts/Register", user);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }

                }
            }
            SignUpViewModel user1 = new SignUpViewModel
            {
                UserName = model.SignUpRoles.UserName,
                FullName = model.SignUpRoles.FullName,
                Password = model.SignUpRoles.Password,
                Email = model.SignUpRoles.Email,
                PhoneNumber = model.SignUpRoles.PhoneNumber,
                Role = model.SelectedValue


            };
            RolesSignUpViewModel model1 = new RolesSignUpViewModel
            {
                SignUpRoles = user1,
                Values = new List<SelectListItem>
                         {
                             new SelectListItem { Value = "Seller", Text = "Seller" },
                             new SelectListItem { Value = "Buyer", Text = "Buyer" }
                         }


            };
            return View(model1);
        }
    }
}