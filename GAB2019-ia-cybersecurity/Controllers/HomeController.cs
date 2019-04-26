using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GAB2019_ia_cybersecurity.Models;

namespace GAB2019_ia_cybersecurity.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Home/Login/FirstLogin.cshtml");
        }

        public IActionResult Terminators()
        {
            return View("~/Views/Home/Login/Terminators.cshtml");
        }
        public IActionResult Finish()
        {
            return View("~/Views/Home/Login/Finish.cshtml");
        }

        public IActionResult SecondCake()
        {
            return View("~/Views/Home/Login/SecondCake.cshtml");
        }
        public IActionResult ThirdLogin()
        {
            return View("~/Views/Home/Login/ThirdLogin.cshtml");
        }


        public IActionResult SecondLogin()
        {
            return View("~/Views/Home/Login/SecondLogin.cshtml");
        }

        public IActionResult Dashboard()
        {
            ViewData["Message"] = "Your dashboard page.";

            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
