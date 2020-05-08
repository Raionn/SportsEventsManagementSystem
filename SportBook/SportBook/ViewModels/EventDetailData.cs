using ChallongeSharp.Models.ChallongeModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SportBook.ViewModels
{
    public class EventDetailData
    {
        public List<User> Users { get; set; }
        public Event Event { get; set; }
        public Models.Participant Participant {get;set;}
        public EventInvitation Invitation { get; set; }
        public List<Models.Participant> Participants { get; set; }

        public EventDetailData(List<User> users, Event @event, Models.Participant participant,
            EventInvitation invitation, List<Models.Participant> participants)
        {
            Users = users;
            Event = @event;
            Participant = participant;
            Invitation = invitation;
            Participants = participants;
        }

    }
}
