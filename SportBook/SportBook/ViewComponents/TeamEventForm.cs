using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportBook.ViewComponents
{
    public class TeamEventFormViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public TeamEventFormViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int teamId)
        {
            ViewData["teamId"] = teamId;
            var team = _context.Team.Find(teamId);
            var sportsGameTypes = _context.GameType.Where(x => x.IsOnline == true);
            ViewData["FkGameType"] = new SelectList(sportsGameTypes, "GameTypeId", "Name", sportsGameTypes.Where(x => x.GameTypeId == team.FkGameType));

            return View();
        }
    }
}
