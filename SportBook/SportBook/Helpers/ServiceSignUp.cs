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
            string username = null;
            string firstName = null;
            string lastName = null;
            string email = null;
            bool isNewUser = false;
            List<Claim> claims = ticketReceivedContext.Principal.Claims.ToList();
            var claim = claims.FirstOrDefault(x => x.Type.EndsWith("isNewUser"));
            string userOId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userInfo = _context.User.FirstOrDefault(s => s.ExternalId == userOId);
            if ((userInfo == null) || bool.Parse(claim.Value))
            {
                isNewUser = true;
            }

            if (isNewUser)
            {
                username = claims.FirstOrDefault(x => x.Type.EndsWith("nickname")).Value;
                //claims.First(x => x.Type == ClaimTypes.)
                if (claims.Find(c => (c.Type == ClaimTypes.NameIdentifier)).Value.StartsWith("google"))
                {
                    if (claims.FirstOrDefault(x => x.Type.Contains("givenname")) != null)
                        firstName = claims.FirstOrDefault(x => x.Type.Contains("givenname")).Value;
                    if (claims.FirstOrDefault(x => x.Type.Contains("surname")) != null)
                        lastName = (claims.FirstOrDefault(x => x.Type.Contains("surname")).Value);
                    email = username + "@gmail.com";
                }
                // claims.FirstOrDefault(x => x.Type.EndsWith("name")).Value;
                if (claims.Find(c => (c.Type == ClaimTypes.NameIdentifier)).Value.StartsWith("auth0"))
                    email = claims.FirstOrDefault(x => x.Type.StartsWith("name")).Value;
                var pictureURL = claims.FirstOrDefault(x => x.Type.EndsWith("picture")).Value;
                User user = new User() { Firstname = firstName, Lastname = lastName, ExternalId = userOId, Username = username, Email =  email, PictureUrl = pictureURL};
                //insert new value into DB
                _context.Add(user);
                await _context.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }
    }
}


