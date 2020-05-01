using ChallongeSharp.Models.ChallongeModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public class Tournament
    {
        [JsonProperty("require_score_agreement")]
        public bool? RequireScoreAgreement { get; set; }
        [JsonProperty("rr_pts_for_game_tie")]
        public string RrPtsForGameTie { get; set; }
        [JsonProperty("rr_pts_for_game_win")]
        public string RrPtsForGameWin { get; set; }
        [JsonProperty("rr_pts_for_match_tie")]
        public string RrPtsForMatchTie { get; set; }
        [JsonProperty("rr_pts_for_match_win")]
        public string RrPtsForMatchWin { get; set; }
        [JsonProperty("sequential_pairings")]
        public bool? SequentialPairings { get; set; }
        [JsonProperty("show_rounds")]
        public bool? ShowRounds { get; set; }
        [JsonProperty("signup_cap")]
        public int? SignupCap { get; set; }
        [JsonProperty("start_at")]
        public DateTime? StartAt { get; set; }
        [JsonProperty("started_at")]
        public DateTime? StartedAt { get; set; }
        [JsonProperty("started_checking_in_at")]
        public DateTime? StartedCheckingInAt { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("swiss_rounds")]
        public long? SwissRounds { get; set; }
        [JsonProperty("ranked_by")]
        public string RankedBy { get; set; }
        [JsonProperty("teams")]
        public bool? Teams { get; set; }
        [JsonProperty("tournament_type")]
        public string TournamentType { get; set; }
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("description_source")]
        public string DescriptionSource { get; set; }
        [JsonProperty("subdomain")]
        public string Subdomain { get; set; }
        [JsonProperty("full_challonge_url")]
        public string FullChallongeUrl { get; set; }
        [JsonProperty("live_image_url")]
        public string LiveImageUrl { get; set; }
        [JsonProperty("sign_up_url")]
        public string SignUpUrl { get; set; }
        [JsonProperty("review_before_finalizing")]
        public bool? ReviewBeforeFinalizing { get; set; }
        [JsonProperty("accepting_predictions")]
        public bool? AcceptingPredictions { get; set; }
        [JsonProperty("participants_locked")]
        public bool? ParticipantsLocked { get; set; }
        [JsonProperty("game_name")]
        public string GameName { get; set; }
        [JsonProperty("participants_swappable")]
        public bool? ParticipantsSwappable { get; set; }
        [JsonProperty("tie_breaks")]
        public List<string> TieBreaks { get; set; }
        [JsonProperty("quick_advance")]
        public bool? QuickAdvance { get; set; }
        [JsonProperty("pts_for_match_win")]
        public string PtsForMatchWin { get; set; }
        [JsonProperty("pts_for_match_tie")]
        public string PtsForMatchTie { get; set; }
        [JsonProperty("accept_attachments")]
        public bool? AcceptAttachments { get; set; }
        [JsonProperty("allow_participant_match_reporting")]
        public bool? AllowParticipantMatchReporting { get; set; }
        [JsonProperty("anonymous_voting")]
        public bool? AnonymousVoting { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("check_in_duration")]
        public DateTime? CheckInDuration { get; set; }
        [JsonProperty("completed_at")]
        public DateTime? CompletedAt { get; set; }
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("created_by_api")]
        public bool? CreatedByApi { get; set; }
        [JsonProperty("credit_capped")]
        public bool? CreditCapped { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("game_id")]
        public long? GameId { get; set; }
        [JsonProperty("group_stages_enabled")]
        public bool? GroupStagesEnabled { get; set; }
        [JsonProperty("hide_forum")]
        public bool? HideForum { get; set; }
        [JsonProperty("hide_seeds")]
        public bool? HideSeeds { get; set; }
        [JsonProperty("hold_third_place_match")]
        public bool? HoldThirdPlaceMatch { get; set; }
        [JsonProperty("id")]
        public long? Id { get; set; }
        [JsonProperty("max_predictions_per_user")]
        public long? MaxPredictionsPerUser { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("notify_users_when_matches_open")]
        public bool? NotifyUsersWhenMatchesOpen { get; set; }
        [JsonProperty("notify_users_when_the_tournament_ends")]
        public bool? NotifyUsersWhenTheTournamentEnds { get; set; }
        [JsonProperty("open_signup")]
        public bool? OpenSignup { get; set; }
        [JsonProperty("participants_count")]
        public long? ParticipantsCount { get; set; }
        [JsonProperty("prediction_method")]
        public long? PredictionMethod { get; set; }
        [JsonProperty("predictions_opened_at")]
        public DateTime? PredictionsOpenedAt { get; set; }
        [JsonProperty("private")]
        public bool? Private { get; set; }
        [JsonProperty("progress_meter")]
        public long? ProgressMeter { get; set; }
        [JsonProperty("pts_for_bye")]
        public string PtsForBye { get; set; }
        [JsonProperty("pts_for_game_tie")]
        public string PtsForGameTie { get; set; }
        [JsonProperty("pts_for_game_win")]
        public string PtsForGameWin { get; set; }
        [JsonProperty("team_convertable")]
        public bool? TeamConvertable { get; set; }
        [JsonProperty("group_stages_were_started")]
        public bool? GroupStagesWereStarted { get; set; }

        [JsonProperty("locked_at")]
        public string LockedAt { get; set; }

        [JsonProperty("event_id")]
        public string EventId { get; set; }

        [JsonProperty("public_predictions_before_start_time")]
        public bool PublicPredictionsBeforeStartTime { get; set; }

        [JsonProperty("ranked")]
        public bool Ranked { get; set; }

        [JsonProperty("grand_finals_modifier")]
        public string GrandFinalsModifier { get; set; }

        [JsonProperty("predict_the_losers_bracket")]
        public bool PredictTheLosersBracket { get; set; }

        [JsonProperty("spam")]
        public bool Spam { get; set; }


        [JsonProperty("ham")]
        public bool Ham { get; set; }

        [JsonProperty("rr_iterations")]
        public string RRIterations { get; set; }

        [JsonProperty("tournament_registration_id")]
        public int TournamentRegistrationId { get; set; }

        [JsonProperty("donation_contest_enabled")]
        public bool DonationContestEnabled { get; set; }

        [JsonProperty("mandatory_donation")]
        public bool MandatoryDonation { get; set; }

        [JsonProperty("auto_assign_stations")]
        public bool AutoAssignStations { get; set; }

        [JsonProperty("only_start_matches_with_stations")]
        public bool OnlyStartMatchesWithStations { get; set; }

        [JsonProperty("registration_fee")]
        public double RegistrationFee { get; set; }

        [JsonProperty("registration_type")]
        public string RegistrationType { get; set; }

        [JsonProperty("split_participants")]
        public bool SplitParticipants { get; set; }

        [JsonProperty("non_elimination_tournament_data")]
        public List<ParticipantsPerMatch> NonEliminationTournamentData { get; set; }

        public class ParticipantsPerMatch {

            [JsonProperty("participants_per_match")]
            public List<ParticipantsPerMatch> ParticipantsOrIDK { get; set; }
        }


    }
}
