using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataGetter.Models
{
    public class PoedbTradeModel
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("data")]
        public List<Datum> Data { get; set; }

        public class Datum
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("us")]
            public string Us { get; set; }

            [JsonPropertyName("lang")]
            public string Lang { get; set; }
        }
    }
}