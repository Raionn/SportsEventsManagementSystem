using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewModels
{
    public class TeamWithMembers
    {

        public List<TeamMember> TeamMembers { get; set; }
        public Team Team { get; set; }

        public TeamWithMembers(List<TeamMember> teamMembers, Team team)
        {
            TeamMembers = teamMembers;
            Team = team;
        }


    }
}
