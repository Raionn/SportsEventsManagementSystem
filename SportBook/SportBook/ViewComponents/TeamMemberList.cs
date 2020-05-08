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
    public class TeamMemberList : ViewComponent
    {
        private readonly SportbookDatabaseContext _context;

        public TeamMemberList(SportbookDatabaseContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(int teamId, int userId)
        {
            ViewData["CurrentUser"] = GetCurrentUser();
            var team = _context.Team.Find(teamId);
            ViewData["TeamOwner"] = _context.User.Where(x => x.UserId == team.FkOwner).FirstOrDefault();
            if (userId > 0)
            {
                var user = _context.TeamMember.FirstOrDefault(x => x.FkUser == userId);

                if (user == null)
                {
                    _context.TeamMember.Add(new TeamMember { FkTeam = teamId, FkUser = userId });
                    _context.SaveChanges();
                }
                else
                {
                    _context.TeamMember.Remove(user);
                    _context.SaveChanges();
                }

            }
            var teamMemberList = _context.TeamMember.Where(p => p.FkTeam == teamId).Include(x => x.FkUserNavigation);
            return View(teamMemberList);
        }
        //private List<TeamMember> GetItemsAsync(int teamId, int userId)
        //{
        //    if (userId > 0)
        //    {
        //        var user = _context.TeamMember.FirstOrDefault(x => x.FkUser == userId);

        //        if (user == null)
        //        {
        //            _context.TeamMember.Add(new TeamMember { FkTeam = teamId, FkUser = userId });
        //            _context.SaveChanges();
        //        }
        //        else
        //        {
        //            _context.TeamMember.Remove(user);
        //            _context.SaveChanges();
        //        }

        //    }
        //    var teamMemberList = _context.TeamMember.Where(p => p.FkTeam == teamId).Include(x => x.FkUserNavigation);
        //    //var usersInTeam = (from users in _context.User
        //    //                    join teamMembers in teamMemberList on users.UserId equals teamMembers.FkUser
        //    //                    select users).ToList();
        //    return teamMemberList;
        //}
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
