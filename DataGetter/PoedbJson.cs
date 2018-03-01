using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;

namespace DataGetter
{
    public partial class poedbJson
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("data")]
        public List<List<string>> Data { get; set; }

        [JsonProperty("columns")]
        public List<Column> Columns { get; set; }
    }

    public partial class Column
    {
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class poedbJson
    {
        public static poedbJson FromJson(string json) => JsonConvert.DeserializeObject<poedbJson>(json, DataGetter.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this poedbJson self) => JsonConvert.SerializeObject(self, DataGetter.Converter.Settings);
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }
}