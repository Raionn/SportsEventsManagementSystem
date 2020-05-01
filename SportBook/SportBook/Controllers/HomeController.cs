using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SportBook.Helpers;
using SportBook.Models;
using Microsoft.AspNetCore.Http;

namespace SportBook.Controllers
{
    //[Authorize(Roles ="user, admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ChallongeService chall;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            chall = new ChallongeService(_clientFactory);
        }

        public async Task<IActionResult> Index()
        {
            
            var response = await chall.OnGet();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Schedule()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
