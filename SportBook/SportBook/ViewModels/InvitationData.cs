using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.ViewModels
{
    public class InvitationData
    {
        public List<EventDataInvitation> EventInvitation { get; set; }

        public List<TeamDataInvitation> TeamInvitation { get; set; }

        public InvitationData(List<EventDataInvitation> eventInvitation, List<TeamDataInvitation> teamInvitation)
        {
            EventInvitation = eventInvitation;
            TeamInvitation = teamInvitation;
        }





    }
    public class EventDataInvitation
    {
        public int EventInvitationId { get; set; }
        public bool IsAccepted { get; set; }
        public int FkUser { get; set; }
        public int FkEvent { get; set; }
        public string EventName { get; set; }
        public string Url { get; set; }
        public EventDataInvitation(int invitationId, bool isAccepted, int fkUser, int fkEvent, string eventName, string url)
        {
            EventInvitationId = invitationId;
            IsAccepted = isAccepted;
            FkUser = fkUser;
            FkEvent = fkEvent;
            EventName = eventName;
            Url = url;
        }

    }
    public class TeamDataInvitation
    {
        public int TeamInvitationId { get; set; }
        public bool IsAccepted { get; set; }
        public int FkUser { get; set; }
        public int FkTeam { get; set; }
        public string TeamName { get; set; }
        public string Url { get; set; }

        public TeamDataInvitation(int invitationId, bool isAccepted, int fkUser, int fkTeam, string teamName, string url)
        {
            TeamInvitationId = invitationId;
            IsAccepted = isAccepted;
            FkUser = fkUser;
            FkTeam = fkTeam;
            TeamName = teamName;
            Url = url;
        }

    }
}
