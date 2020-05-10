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
    public class ScrimEnemyTeamViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public ScrimEnemyTeamViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId, int teamId)
        {
            var team = _context.Team.Find(teamId);
            var currentUser = GetCurrentUser();
            var participant = _context.Participant.FirstOrDefault(x => x.FkTeam == teamId && x.FkEvent == eventId);
            if (participant == null)
            {
                _context.Participant.Add(new Participant() { FkEvent = eventId, FkTeam = teamId, FkUser = currentUser.UserId });
                var teamMembers = _context.TeamMember.Where(x => x.FkTeam == teamId).Include(x => x.FkUserNavigation);
                ViewData["teamMembers"] = teamMembers;
                ViewData["team"] = team;
                ViewData["create"] = true;
            }
            else 
            {
                _context.Participant.Remove(participant);
                ViewData["create"] = false;
            }
            await _context.SaveChangesAsync();

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
