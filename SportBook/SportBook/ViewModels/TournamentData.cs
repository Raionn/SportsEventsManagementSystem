using Microsoft.AspNetCore.Mvc.Rendering;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewModels
{
    public class TournamentData
    {
        public Tournament Tournament { get; set; }
        public SelectList Teams { get; set; }
        public TournamentMember TournamentMember { get; set; }

        public TournamentData(Tournament tournament, SelectList teams, TournamentMember participant)
        {
            Tournament = tournament;
            Teams = teams;
            TournamentMember = participant;
        }
    }
}
