using Microsoft.AspNetCore.Mvc;
using SyncPocService.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace SyncPocService.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _dbContext;
        public HomeController(AppDbContext context)
        {
            _dbContext = context;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "application home page.";
            return View();
        }

        public IActionResult Users()
        {
            try
            {
                var users = _dbContext.Users.ToList();
                return View(users);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

        }

        public IActionResult Employees()
        {
            try
            {
                var employees = _dbContext.Employees.ToList();
                return View(employees);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

        }


        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
