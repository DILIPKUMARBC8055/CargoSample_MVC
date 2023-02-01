using CargoSample2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class CargoOrdersController : Controller
    {
        public readonly IConfiguration _configuration;
        public CargoOrdersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public async Task<IActionResult> Index()
        {
            List<CargoOrderViewModel> orders = new List<CargoOrderViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                if (result.IsSuccessStatusCode)
                {
                    orders = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                }

            }
            return View(orders);
        }
        public async Task<ActionResult> Create()
        {
            CargoOrderViewModel cargo = new CargoOrderViewModel
            {
                CargoStatus = await this.GetStatus(),
                CargoType = await this.GetAllCargoTypes(),
                City = await this.GetCities()
            };
            return View(cargo);
        }

        [HttpGet]
        public async Task<IActionResult> CalculatePrice(int id,double Weight)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    CargoType cargoType = cargolist.Where(c => c.Id == id).FirstOrDefault();
                    double price = 0;
                    if (Weight > double.Parse(cargoType.Weight))
                    {
                        double extraWeight = double.Parse(cargoType.Weight) - Weight;
                        price = double.Parse(cargoType.Price) * double.Parse(cargoType.Weight);
                        price += extraWeight * double.Parse(cargoType.ExtraPrice) * double.Parse(cargoType.ExtraWeight);


                    }
                    else
                    {
                        price = double.Parse(cargoType.Price) * Weight ;

                    }
                    ViewBag.Price = price;
                    return PartialView("_CalculatePrice");




                }
            }
            return View();
        }


        public async Task<IActionResult> GetCargoById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    CargoType cargoType = cargolist.Where(c => c.Id == id).FirstOrDefault();
                    return PartialView("_PartialGetCargoById", cargoType);


                }

            }
            return null;

        }

        [NonAction]

        public async Task<List<CargoStatusViewModel>> GetStatus()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoStatus/GetAllStatus");
                if (result.IsSuccessStatusCode)
                {
                    var statusList = await result.Content.ReadAsAsync<List<CargoStatusViewModel>>();
                    return statusList;

                }

            }
            return null;
        }

        [NonAction]
        public async Task<List<CargoType>> GetAllCargoTypes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("CargoType/GetAllCargoTypes");
                if (result.IsSuccessStatusCode)
                {
                    var cargolist = await result.Content.ReadAsAsync<List<CargoType>>();
                    return cargolist;

                }

            }
            return null;
        }
        [NonAction]
        public async Task<List<CityViewModel>> GetCities()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);

                var result = await client.GetAsync("Cities/GetAllCities");
                if (result.IsSuccessStatusCode)
                {
                    var citylist = await result.Content.ReadAsAsync<List<CityViewModel>>();
                    return citylist;

                }

            }
            return null;

        }


        [HttpPost]

        public async Task<IActionResult> Create(CargoOrderViewModel cargoOrder)
        {
            if (ModelState.IsValid)
            {
                cargoOrder.CargoStatusId = "1";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"CargoOrderDetails/Create", cargoOrder);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(cargoOrder);
        }

        public async Task<IActionResult> Details(int id)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                if (result.IsSuccessStatusCode)
                {
                    var orderList = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                    CargoOrderViewModel cargoOrder = orderList.Where(c => c.Id == id).FirstOrDefault();
                    if (cargoOrder != null)
                    {
                        return View(cargoOrder);
                    }

                }
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CargoOrderViewModel cargoOrder = new CargoOrderViewModel();
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                    if (result.IsSuccessStatusCode)
                    {
                        var orderList = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                        cargoOrder = orderList.Where(c => c.Id == id).FirstOrDefault();
                        if (cargoOrder != null)
                        {
                            return View(cargoOrder);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cargo order does not exist");
                        }
                    }
                }
                return View(cargoOrder);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CargoOrderViewModel cargoOrder)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"CargoOrderDetails/UpdateCargoOrderDetail/{cargoOrder.Id}", cargoOrder);
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
            return View(cargoOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                CargoOrderViewModel cargoOrder = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("CargoOrderDetails/GetAllCargoOrderDetails");
                    if (result.IsSuccessStatusCode)
                    {
                        var orderList = await result.Content.ReadAsAsync<List<CargoOrderViewModel>>();
                        cargoOrder = orderList.Where(c => c.Id == id).FirstOrDefault();
                        if (cargoOrder != null)
                        {
                            return View(cargoOrder);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cargo Order doesn't exist");
                        }

                    }
                }
            }
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(CargoOrderViewModel cargoOrder)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"/api/CargoOrderDetails/DeleteCargoOrderDetail/{cargoOrder.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View(cargoOrder);
        }





    }
}
