using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportBook.Models;

namespace SportBook.Controllers
{
    [Route("[action]")]
    public class GeneralController : Controller
    {
        private readonly ILogger<GeneralController> _logger;
        public GeneralController(ILogger<GeneralController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult LoginValidation()
        {
            //validate login information
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegistrationValidation()
        {
            //validate login information
            return RedirectToAction("Login", "General");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}