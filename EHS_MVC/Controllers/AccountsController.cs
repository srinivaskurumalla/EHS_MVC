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

                        string userName = login.Username;
                        HttpContext.Session.SetString("sellerName", userName);

                        // TempData["UserName"] = login.Username;
                        string role = await ExtractRole();
                        if (role == "Seller")
                        {


                            return RedirectToAction("Index", "House");

                        }
                        else if(role == "ADMIN")
                        {
                            return RedirectToAction("Index", "Admin");

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");

                        }
                    }
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }
            return View(login);
        }

        [NonAction]
        public async Task<string> ExtractRole()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["Apiurl:api"]);
              //  var result = await client.GetAsync("Accounts/GetName");

                var roleResult = await client.GetAsync("Accounts/GetRole");
                if ( roleResult.IsSuccessStatusCode)
                {
                   // var name = await result.Content.ReadAsAsync<string>();
                   // ViewBag.Name = name;

                    var role = await roleResult.Content.ReadAsAsync<string>();
                   // ViewBag.Role = role;


                    return role;
                }
                return null;
            }
        } 

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
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