using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public class EventInvite
    {
        [JsonPropertyName("FkUser")]
        public string FkUser { get; set; }
        [JsonPropertyName("FkEvent")]
        public string FkEvent { get; set; }
    }
}
