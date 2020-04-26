using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SportBook.Controllers;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public interface IServiceSignUp
    {
        Task<Task> CreateOnSignUp(TicketReceivedContext ticketReceivedContext);
    }

    public class ServiceSignUp : IServiceSignUp
    {
        private readonly SportbookDatabaseContext _context;
        public ServiceSignUp(SportbookDatabaseContext context)
        {
            _context = context;
        }
        public async Task<Task> CreateOnSignUp(TicketReceivedContext ticketReceivedContext)
        {
            bool isNewUser = false;
            List<Claim> claims = ticketReceivedContext.Principal.Claims.ToList();
            var claim = claims.FirstOrDefault(x => x.Type.EndsWith("isNewUser"));
            string userOId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userInfo = (from s in _context.User
                           select s).Where(s => s.ExternalId == userOId);
            if (userInfo.Count() < 1 || bool.Parse(claim.Value))
            {
                isNewUser = true;
            }

            if (isNewUser)
            {
                var username = claims.FirstOrDefault(x => x.Type.EndsWith("nickname")).Value;
                //var email = claims.FirstOrDefault(x => x.Type.EndsWith("name")).Value;
                var pictureURL = claims.FirstOrDefault(x => x.Type.EndsWith("picture")).Value;
                User user = new User() { ExternalId = userOId, Username = username, /*Email =  email,*/ PictureUrl = pictureURL};
                //insert new value into DB
                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }
    }
}


