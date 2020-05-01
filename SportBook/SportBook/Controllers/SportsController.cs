using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;

namespace SportBook.Controllers
{

    //[Authorize(Roles ="admin")]
    public class SportsController : Controller
    {
        private readonly SportbookDatabaseContext _context;

        public SportsController(SportbookDatabaseContext context)
        {
            _context = context;
        }
        [Route("[action]")]
        public IActionResult Sports()
        {
            User currentUser = (from s in _context.User select s).Where(s => s.ExternalId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            ViewData["CurrentUser"] = currentUser;
            var events = _context.Event.Include(e => e.FkGameTypeNavigation).Include(e => e.FkLocationNavigation).Include(e => e.FkOwnerNavigation).Where(e => !e.FkGameTypeNavigation.IsOnline);
            return View(events);
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