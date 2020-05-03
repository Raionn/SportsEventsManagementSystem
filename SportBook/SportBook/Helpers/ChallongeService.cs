using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SportBook.Models;
using Microsoft.Extensions.Configuration;

namespace SportBook.Helpers
{
    public class ChallongeService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration Configuration;

        //public IEnumerable<ChallongeData> ChallongeTourney { get; private set; }

        public bool GetBranchesError { get; private set; }

        public ChallongeService(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            Configuration = config;
        }

        public async Task<Task> OnGet()
        {

            var request = new HttpRequestMessage(HttpMethod.Get,
                "https://api.challonge.com/v1/tournaments.json?include_participants=1&api_key=Hgtdj8IxMetjfk7zbJNQDLHKbzduQxqKACLlVKAl");
            //request.Headers.Add("Accept", "application/vnd.github.v3+json");
            //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            var client = _clientFactory.CreateClient();
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            };
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var reponseString = await response.Content.ReadAsStringAsync();
                try
                {
                    var data = await JsonSerializer.DeserializeAsync<IEnumerable<TournamentItem>>(responseStream, options);
                }
                catch (Exception ex)
                {

                    string exce = ex.Message;
                }
                

                //var data1 = JsonSerializer.Deserialize<IEnumerable<ChallongeData>>(reponseString);
            }
            else
            {
                GetBranchesError = true;
            }


            return Task.CompletedTask;
        }

        public async Task<Tournament> OnPostTournament(Tournament tour, int id)
        {
            string api_key = Configuration.GetValue<string>("API_Keys:Challonge_Key");
            string url = String.Format("https://api.challonge.com/v1/tournaments.json?api_key={0}",api_key);
            var result = tour;

            var tournament = new PostTournament() {
                StartAt = tour.StartTime,
                Name = tour.Name,
                Url = "Sportbook" + id
            };

            //var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();
            var requestMessage = JsonSerializer.Serialize<PostTournament>(tournament);
            var response = await client.PostAsync(url, new StringContent(requestMessage, Encoding.UTF8, "application/json"));
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var reponseString = await response.Content.ReadAsStringAsync();
            try
            {
                var data = await JsonSerializer.DeserializeAsync<TournamentItem>(responseStream);
                result.ExternalID = (int)data.Tournament.Id; result.TournamentUrl = data.Tournament.FullChallongeUrl;
                return result;
            }
            catch (Exception ex)
            {

                string exce = ex.Message;
            }

            return result;
        }

        public async Task<Task> OnPutTournament(Tournament tour)
        {
            string api_key = Configuration.GetValue<string>("API_Keys:Challonge_Key");
            string url = String.Format("https://api.challonge.com/v1/tournaments/{1}.json?api_key={0}", api_key, tour.ExternalID);

            var tournament = new PutTournament()
            {
                StartAt = tour.StartTime,
                Name = tour.Name
            };

            //var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();
            var requestMessage = JsonSerializer.Serialize<PutTournament>(tournament);
            var response = await client.PutAsync(url, new StringContent(requestMessage, Encoding.UTF8, "application/json"));
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var reponseString = await response.Content.ReadAsStringAsync();
            try
            {
                var data = await JsonSerializer.DeserializeAsync<TournamentItem>(responseStream);
            }
            catch (Exception ex)
            {

                string exce = ex.Message;
            }

            return Task.CompletedTask;
        }

        public async Task<Task> OnDeleteTournament(Tournament tour)
        {
            string api_key = Configuration.GetValue<string>("API_Keys:Challonge_Key");
            string url = String.Format("https://api.challonge.com/v1/tournaments/{1}.json?api_key={0}", api_key, tour.ExternalID);

            //var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();

            var response = await client.DeleteAsync(url);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var reponseString = await response.Content.ReadAsStringAsync();
            try
            {
                var data = await JsonSerializer.DeserializeAsync<TournamentItem>(responseStream);
            }
            catch (Exception ex)
            {

                string exce = ex.Message;
            }

            return Task.CompletedTask;
        }

        public async Task<int> OnPostParticipant(int tournamentExternalId, string participantName)
        {
            string api_key = Configuration.GetValue<string>("API_Keys:Challonge_Key");
            string url = String.Format("https://api.challonge.com/v1/tournaments/{1}/participants.json?api_key={0}", api_key, tournamentExternalId);
            var result = 0;

            var participant = new Participant()
            {
                Name = participantName
            };

            //var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();
            var requestMessage = JsonSerializer.Serialize<Participant>(participant);
            var response = await client.PostAsync(url, new StringContent(requestMessage, Encoding.UTF8, "application/json"));
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var reponseString = await response.Content.ReadAsStringAsync();
            try
            {
                var data = await JsonSerializer.DeserializeAsync<ParticipantItem>(responseStream);
                result = (int)data.Participant.Id;
                return result;
            }
            catch (Exception ex)
            {

                string exce = ex.Message;
            }

            return result;
        }

        public async Task<Task> OnDeleteParticipant(TournamentMember member, int tournamentId)
        {
            string api_key = Configuration.GetValue<string>("API_Keys:Challonge_Key");
            string url = String.Format("https://api.challonge.com/v1/tournaments/{1}/participants/{2}.json?api_key={0}", api_key, tournamentId, member.ExternalID);

            //var request = new HttpRequestMessage(HttpMethod.Post, url);
            var client = _clientFactory.CreateClient();

            var response = await client.DeleteAsync(url);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var reponseString = await response.Content.ReadAsStringAsync();
            try
            {
                var data = await JsonSerializer.DeserializeAsync<TournamentItem>(responseStream);
            }
            catch (Exception ex)
            {

                string exce = ex.Message;
            }

            return Task.CompletedTask;
        }


    }
}
