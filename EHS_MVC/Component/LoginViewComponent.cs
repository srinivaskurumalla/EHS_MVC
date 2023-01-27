using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace EHS_MVC.Component
{
    public class LoginViewComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;

        public LoginViewComponent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Accounts/GetName");
                if (result.IsSuccessStatusCode)
                {
                    var name = await result.Content.ReadAsAsync<string>();
                    ViewBag.Name = name;
                    return View("_LoginPartial");
                }
            }
            return View("_LoginPartial");
        }

    }
}
