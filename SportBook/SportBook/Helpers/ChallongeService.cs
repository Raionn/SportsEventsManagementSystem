using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChallongeSharp.Clients;
using ChallongeSharp.Models;
using ChallongeSharp.Clients.Interfaces;
using ChallongeSharp.Models.Configurations;
using ChallongeSharp.Helpers;
using ChallongeSharp;
using Microsoft.Extensions.Options;
using ChallongeSharp.Models.ViewModels;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;

namespace SportBook.Helpers
{
    public class ChallongeService
    {
        private readonly IHttpClientFactory _clientFactory;

        //public IEnumerable<ChallongeData> ChallongeTourney { get; private set; }

        public bool GetBranchesError { get; private set; }

        public ChallongeService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<Task> OnGet()
        {
            //ChallongeConfigurations config = new ChallongeConfigurations();
            //config.ApiKey = "Hgtdj8IxMetjfk7zbJNQDLHKbzduQxqKACLlVKAl";
            //config.Username = "Smacl3r";
            //HttpClient http = new HttpClient();
            //http.BaseAddress = new Uri("https://api.challonge.com/v1/tournaments/8402383.json");
            //ChallongeNameAttribute challongeNameAttribute = new ChallongeNameAttribute("id");
            //ChallongeConnection challongeConnection = new ChallongeConnection();
            //ChallongeClientFactory challongeClientFactory = new ChallongeClientFactory();
            //challongeClientFactory.Create("",);
            //TournamentOptions tournamentOptions = new TournamentOptions();
            //tournamentOptions.IncludeParticipants = false;
            //var options = Options.Create<ChallongeConfigurations>(config);
            //try
            //{
            //    //ChallongeClient challongeClient = new ChallongeClient(http, options);
            //    //var response1 = await challongeClient.GetAsync<IEnumerable<Tournament>>("", tournamentOptions);
            //}
            //catch (Exception ex)
            //{

            //    string message = ex.Message;
            //}






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
                var stuff1 = Newtonsoft.Json.JsonConvert.DeserializeObject(reponseString);
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
    }
}
