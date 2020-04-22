using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportBook.Models;

namespace SportBook.Controllers
{
    [Authorize]
    public class GeneralController : Controller
    {

        private readonly ILogger<GeneralController> _logger;
        private readonly SportbookContext _context;
        public GeneralController(ILogger<GeneralController> logger, SportbookContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult MyTeams()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginValidation(
        [Bind("Username,Firstname,Lastname,Birthdate,Password,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View("Login", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationValidation(
        [Bind("Username,Firstname,Lastname,Birthdate,Password,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "General");
            }
            return View("Registration", user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}