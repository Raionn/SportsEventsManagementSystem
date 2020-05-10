using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportBook.ViewComponents
{
    public class Chatroom : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;
        public Chatroom(SportbookDatabaseContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(string chatGroup)
        {
            ViewData["CurrentUser"] = GetCurrentUser();
            ViewData["ChatGroup"] = chatGroup;
            return View();
        }

        private User GetCurrentUser()
        {
            var externalId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _context.User
                           .Where(s => s.ExternalId == externalId)
                           .FirstOrDefaultAsync();
            user.Wait();
            return user.Result;
        }
    }
}
