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
        public SelectList Users { get; set; }
        public Event Event { get; set; }
        public Models.Participant Participant {get;set;}
        public EventInvitation Invitation { get; set; }

        public EventDetailData(SelectList users, Event @event, Models.Participant participant, EventInvitation invitation)
        {
            Users = users;
            Event = @event;
            Participant = participant;
            Invitation = invitation;
        }

    }
}
