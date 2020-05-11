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
    public class ConnectedUsernameViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public ConnectedUsernameViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await GetCurrentUser();
            return View(user);
        }
        private async Task<User> GetCurrentUser()
        {
            var externalId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _context.User
                           .Where(s => s.ExternalId == externalId)
                           .FirstOrDefaultAsync();
            return user;
        }

        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone &&
        //                         x.Priority <= maxPriority).ToListAsync();
        //}
    }
}
