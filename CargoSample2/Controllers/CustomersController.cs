using CargoSample2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class CustomersController : Controller
    {
        public readonly IConfiguration _configuration;
        public CustomersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<CustomerViewModel> customers = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Customers/GetAllCustomers");
                if (result.IsSuccessStatusCode)
                {
                    customers = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                }

            }
            return View(customers);
        }


        public async Task<IActionResult> Details(int id)
        {
            CustomerViewModel customer = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Customers/GetAllCustomers");
                if (result.IsSuccessStatusCode)
                {
                    var customerlist = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                    customer=customerlist.Where(c=>c.CustId==id).FirstOrDefault();
                    if (customer != null)
                    {
                        return View(customer);
                    }
                }

            }
        
            return null;




        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"Customers/Create", customer);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CustomerResponses customer = new CustomerResponses();
            if (ModelState.IsValid)
            {
                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Customers/GetCustomerById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        customer = await result.Content.ReadAsAsync<CustomerResponses>();
                        return View(customer.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Customer does not exist");
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerViewModel customer)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Customers/UpdateCustomer/{customer.CustId}",customer);
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
            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                CustomerViewModel customer = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Customers/GetAllCustomers");
                    if (result.IsSuccessStatusCode)
                    {
                        var customerList = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                        customer=customerList.Where(c=>c.CustId==id).FirstOrDefault();
                        if (customer != null)
                        {
                            return View(customer);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Customer doesn't exist");
                        }




                    }
                   


                }
                
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CustomerViewModel customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Customers/DeleteCustomer/{customer.CustId}");
                if(result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(customer);


        }
    }
}
