using EHS_MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

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

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
