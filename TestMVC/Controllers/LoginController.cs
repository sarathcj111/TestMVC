using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestMVC.Models;

namespace TestMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44329");

                string json = JsonConvert.SerializeObject(model);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var result = await client.PostAsync("Test/verifyUser", httpContent);

                if (result.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("UserName", model.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.loginMsg = "<script>alert('Login failed. Please check credentials');</script>";
                    HttpContext.Session.Clear();
                    return View("Index");
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
