using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestMVC.Models;

namespace TestMVC.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            var model = new RegisterUserModel();
            return View(model);
        }

        public async Task<IActionResult> RegisterUser(RegisterUserModel model)
        {
            if(model.Password != model.ConfirmPassword)
            {
                ViewBag.registerMsg = "<script>alert('Password and Confirm Password does not match');</script>";
                return View("Index", model);
            }

            if (!await CheckUserNameAvailability(model.Username))
                return View("Index", model);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44329");

                string json = JsonConvert.SerializeObject(model);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var result = await client.PostAsync("Test/registerUser", httpContent);

                if (result.IsSuccessStatusCode)
                {
                    HttpContext.Session.SetString("UserName", model.Username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.registerMsg = "<script>alert('Registration failed. Please try after sometimes');</script>";
                    HttpContext.Session.Clear();
                    return View("Index");
                }
            }
        }

        public async Task<bool> CheckUserNameAvailability(string uName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44329");

                var result = await client.GetAsync($"Test/userNameAvailability?name={uName}");

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    ViewBag.registerMsg = "<script>alert('User Name Not Available. Try Different');</script>";
                    return false;
                }
            }
        }
    }
}
