using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SportBook.Controllers
{

    //[Authorize(Roles ="admin")]
    public class SportsController : Controller
    {
        [Route("[action]")]
        public IActionResult Sports()
        {
            return View();
        }
        [Route("[controller]/[action]")]
        public IActionResult SportsEvents()
        {
            return View();
        }
        [Route("[controller]/[action]")]
        public IActionResult Teams()
        {
            return View();
        }
    }
}