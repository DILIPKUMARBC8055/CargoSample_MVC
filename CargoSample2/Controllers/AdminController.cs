using CargoSample2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class AdminController : Controller
    {
        public readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {

            _configuration= configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<AdminViewModel> admins = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Admins/GetAllAdmin");
                if (result.IsSuccessStatusCode)
                {
                    admins = await result.Content.ReadAsAsync<List<AdminViewModel>>();
                }

            }
            return View(admins);

        }

        public async Task<IActionResult> Details(int id)
        {
            AdminViewModel admin = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Admins/GetAllAdmin");
                if (result.IsSuccessStatusCode)
                {
                    var Adminlist = await result.Content.ReadAsAsync<List<AdminViewModel>>();
                    admin = Adminlist.Where(c => c.Id == id).FirstOrDefault();
                    if (admin != null)
                    {
                        return View(admin);
                    }
                }

            }

            return null;

        }

        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminViewModel admin)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"Admins/CreateAdmin", admin);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(admin);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AdminResponses admin = new AdminResponses();
            if (ModelState.IsValid)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Admins/GetAdminById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        admin = await result.Content.ReadAsAsync<AdminResponses>();
                        return View(admin.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Admin does not exist");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminViewModel admin)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Admins/UpdateAdmin/{admin.Id}", admin);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Server Error, Please try later");
                    }
                }
            }
            return View(admin);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                AdminViewModel admin = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Admins/GetAllAdmin");
                    if (result.IsSuccessStatusCode)
                    {
                        var adminList = await result.Content.ReadAsAsync<List<AdminViewModel>>();
                        admin = adminList.Where(c => c.Id == id).FirstOrDefault();
                        if (admin != null)
                        {
                            return View(admin);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Admin doesn't exist");
                        }
                    }
                }

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AdminViewModel admin)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Admins/DeleteAdmin/{admin.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(admin);


        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Admins/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);
                        return RedirectToAction("Index", "Admin");
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
            return RedirectToAction("Index", "Home");
        }


    }
}
