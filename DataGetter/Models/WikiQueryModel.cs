using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataGetter.Models
{
    public class WikiQueryModel
    {
        [JsonPropertyName("batchcomplete")]
        public string Batchcomplete { get; set; }

        [JsonPropertyName("query")]
        public QueryModel Query { get; set; }

        [Equals(DoNotAddEqualityOperators = true)]
        public class Node
        {
            [JsonPropertyName("pageid")]
            public int Pageid { get; set; }

            [JsonPropertyName("ns")]
            public int Ns { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("imagerepository")]
            public string Imagerepository { get; set; }

            [JsonPropertyName("imageinfo")]
            public List<Imageinfo> Imageinfo { get; set; }

            //public static bool operator == (CustomGetHashCode left, CustomGetHashCode right) => Operator.Weave(left, right);
            //public static bool operator != (CustomGetHashCode left, CustomGetHashCode right) => Operator.Weave(left, right);
        }

        [Equals(DoNotAddEqualityOperators = true)]
        public class Imageinfo
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("descriptionurl")]
            public string Descriptionurl { get; set; }

            [JsonPropertyName("descriptionshorturl")]
            public string Descriptionshorturl { get; set; }
        }

        //public class Pages
        //{
        //    [JsonPropertyName("105148")]
        //    public _105148 _105148 { get; set; }
        //}

        public class QueryModel
        {
            [JsonPropertyName("pages")]
            public Dictionary<string, Node> Pages { get; set; }
        }
    }
}