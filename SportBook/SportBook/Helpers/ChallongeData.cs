using ChallongeSharp.Models.ChallongeModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public class TournamentItem
    {
        [JsonPropertyName("tournament")]
        public ChallongeTournament Tournament { get; set; }
    }
    public class ChallongeTournament
    {
        [JsonPropertyName("full_challonge_url")]
        public string FullChallongeUrl { get; set; }
        [JsonPropertyName("start_at")]
        public DateTime? StartAt { get; set; }
        [JsonPropertyName("started_at")]
        public DateTime? StartedAt { get; set; }
        [JsonPropertyName("tournament_type")]
        public string TournamentType { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("subdomain")]
        public string Subdomain { get; set; }

        [JsonPropertyName("live_image_url")]
        public string LiveImageUrl { get; set; }
        [JsonPropertyName("sign_up_url")]
        public string SignUpUrl { get; set; }

        [JsonPropertyName("game_name")]
        public string GameName { get; set; }

        [JsonPropertyName("allow_participant_match_reporting")]
        public bool? AllowParticipantMatchReporting { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("game_id")]
        public long? GameId { get; set; }

        [JsonPropertyName("id")]
        public long? Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }



    }

    public class PostTournament
    {
        [JsonPropertyName("start_at")]
        public DateTime? StartAt { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

    }

    public class PutTournament
    {
        [JsonPropertyName("start_at")]
        public DateTime? StartAt { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

    }

    public class Participant
    {
        [JsonPropertyName("tournament_id")]
        public long? TournamentId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public long? Id { get; set; }

    }

    public class ParticipantItem
    {
        [JsonPropertyName("participant")]
        public Participant Participant { get; set; }
    }

    public class PostParticipant
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }


    }
}
