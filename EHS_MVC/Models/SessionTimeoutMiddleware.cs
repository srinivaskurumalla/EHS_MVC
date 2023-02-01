using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace EHS_MVC.Models
{
    public class SessionTimeoutMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionTimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Session.GetString("SessionExpiry") == null ||
                DateTime.Parse(context.Session.GetString("SessionExpiry")) < DateTime.Now)
            {
                context.Response.Redirect("/Login");
                return;
            }

            context.Session.SetString("SessionExpiry",
                (DateTime.Now + TimeSpan.FromMinutes(1)).ToString());

            await _next(context);
        }
    }
}