using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataGetter.Models
{
    public class WikiItemsCount
    {
        [JsonPropertyName("cargoquery")]
        public List<Cargoquery> Cargoquerys { get; set; }

        public class Cargoquery
        {
            [JsonPropertyName("title")]
            public Title Title { get; set; }
        }

        public class Title
        {
            [JsonPropertyName("count(*)")]
            public string Count { get; set; }
        }
    }
}