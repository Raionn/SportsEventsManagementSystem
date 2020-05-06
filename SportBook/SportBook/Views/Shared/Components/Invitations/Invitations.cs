using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportBook.Models;
using SportBook.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportBook.Views.Components.Invitations
{
    public class InvitationsViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public InvitationsViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = GetCurrentUser().UserId;
            var amountEvent = await _context.EventInvitation.Where(u => u.FkUser == userId).Where(u => u.IsAccepted == false).CountAsync();
            var amountTeam = await _context.TeamInvitation.Where(u => u.FkUser == userId).Where(u => u.IsAccepted == false).CountAsync();
            //var items = await GetItemsAsync(maxPriority, isDone);
            return View(amountEvent + amountTeam);
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

        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone &&
        //                         x.Priority <= maxPriority).ToListAsync();
        //}
    }

    public class InvitationListViewComponent : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public InvitationListViewComponent(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = GetCurrentUser().UserId;
            var amountEvent = await _context.EventInvitation.Where(u => u.FkUser == userId).Where(u => u.IsAccepted == false).Include(e => e.FkEventNavigation).Include(e => e.FkEventNavigation.FkGameTypeNavigation).ToListAsync();
            var amountTeam = await _context.TeamInvitation.Where(u => u.FkUser == userId).Where(u => u.IsAccepted == false).Include(t => t.FkTeamNavigation).ToListAsync();
            var eventList = new List<EventDataInvitation>();
            var teamList = new List<TeamDataInvitation>();
            foreach (var item in amountEvent)
            {
                var url = "";
                if (item.FkEventNavigation.FkGameTypeNavigation.IsOnline)
                    url = "../Esports/ViewEvent?id=" + item.FkEvent.ToString();
                else
                    url = "../Sports/ViewEvent?id=" + item.FkEvent.ToString();
                eventList.Add(new EventDataInvitation(item.EventInvitationId, item.IsAccepted, item.FkUser, item.FkEvent, item.FkEventNavigation.Title,url));
            }
            foreach (var item in amountTeam)
            {
                teamList.Add(new TeamDataInvitation(item.TeamInvitationId, item.IsAccepted, item.FkUser, item.FkTeam, item.FkTeamNavigation.Name, ""));
            }
            var data = new InvitationData(eventList, teamList);
            //var items = await GetItemsAsync(maxPriority, isDone);
            return View(data);
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


        //private Task<List<TodoItem>> GetItemsAsync(int maxPriority, bool isDone)
        //{
        //    return db.ToDo.Where(x => x.IsDone == isDone &&
        //                         x.Priority <= maxPriority).ToListAsync();
        //}
    }
}