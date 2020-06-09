using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportBook.Helpers;
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
        private readonly EmailHelper _emailHelper;

        public ScrimEnemyTeamViewComponent(SportbookDatabaseContext context, EmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int eventId, int teamId)
        {
            var team = _context.Team.Find(teamId);
            var scrimEvent = _context.Event.Find(eventId);
            var owner = _context.User.Find(scrimEvent.FkOwner);

            var currentUser = GetCurrentUser();
            var participant = _context.Participant.FirstOrDefault(x => x.FkTeam == teamId && x.FkEvent == eventId);
            if (participant == null)
            {
                if (team != null && owner != null)
                {
                    var emailModel = new EmailModel(owner.Email, // To  
"Scrim opponent accepted match", // Subject  
String.Format("Your event <a href=\"https://sportbook.azurewebsites.net/Esports/TeamEvent?id={4}\" target=\"_blank\"><b>{0}</b></a> was accepted by opponent team <b>{1}</b> <br> Event start time: {2} <br> Event end time: {3}", scrimEvent.Title, team.Name, scrimEvent.StartTime, scrimEvent.EndTime, scrimEvent.EventId) // Message  
);
                    _emailHelper.SendEmail(emailModel);
                }
                _context.Participant.Add(new Models.Participant() { FkEvent = eventId, FkTeam = teamId, FkUser = currentUser.UserId });
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
