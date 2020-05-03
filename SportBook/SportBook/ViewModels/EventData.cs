using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportBook.Models;

namespace SportBook.ViewModels
{
    public class EventData
    {
        public Event Event { get; set; }
        public List<Participant> Participants { get; set; }
        public Participant NewParticipant { get; set; }

        public EventData(Event pEvent, List<Participant> participants, Participant participant)
        {
            this.Event = pEvent;
            this.Participants = participants;
            this.NewParticipant = participant;
        }
    }
}
