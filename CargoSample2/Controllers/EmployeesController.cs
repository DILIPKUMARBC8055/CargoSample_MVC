﻿using CargoSample2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CargoSample2.Controllers
{
    public class EmployeesController : Controller
    {

        public readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // GET: EmployeesController
        public async Task<IActionResult> Index()
        {
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            using(var client=new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Employees/GetAllEmployees");
                if (result.IsSuccessStatusCode)
                {
                    employees=await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                }
            }
            return View(employees);
           
        }

        // GET: EmployeesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            EmployeeViewModel employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Employees/GetAllEmployees");
                if (result.IsSuccessStatusCode)
                {
                    var Emplist = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                    employee = Emplist.Where(c => c.EmpId == id).FirstOrDefault();
                    if (employee != null)
                    {
                        return View(employee);
                    }
                }

            }
            return null;
        }

        // GET: EmployeesController/Create
        public async Task<ActionResult> Create()
        {
            return View();
        }

        // POST: EmployeesController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync($"Employees/Create", employee);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");

                    }



                }

            }
            return View(employee);
        }

        // GET: EmployeesController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            EmployeeResponses employee = new EmployeeResponses();
            if (ModelState.IsValid)
            {
                
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Employees/GetEmployeeById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        employee = await result.Content.ReadAsAsync<EmployeeResponses>();
                        return View(employee.value);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Employee does not exist");
                    }
                }
            }
            return View(employee.value);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Employees/UpdateEmployee/{employee.EmpId}",employee);
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
            return View(employee);
        }

        // GET: EmployeesController/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                EmployeeViewModel employee = new();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync("Employees/GetAllEmployees");
                    if (result.IsSuccessStatusCode)
                    {
                        var EmpList = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                        employee = EmpList.Where(c => c.EmpId == id).FirstOrDefault();
                        if (employee != null)
                        {
                            return View(employee);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Employee doesn't exist");
                        }




                    }



                }

            }
            return View();
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(EmployeeViewModel employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Employees/DeleteEmployee/{employee.EmpId}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");

                }
            }
            return View(employee);

        }
    }
}