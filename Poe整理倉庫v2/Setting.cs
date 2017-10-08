using Newtonsoft.Json;
namespace Poe整理倉庫v2
{
    public partial class Setting
    {
        [JsonProperty("Hotkey_Start")]
        public int HotkeyStart { get; set; }
        
        [JsonProperty("Hotkey_Stop")]
        public int HotkeyStop { get; set; }

        [JsonProperty("Delay1")]
        public int Delay1 { get; set; }

        [JsonProperty("Delay2")]
        public int Delay2 { get; set; }

        [JsonProperty("Delay_Scan")]
        public int Delay_Scan { get; set; }

        [JsonProperty("Direction")]
        public string Direction { get; set; }

        [JsonProperty("LowQ")]
        public int LowQ { get; set; }

        [JsonProperty("Species")]
        public string[] Species { get; set; }

        [JsonProperty("Priority")]
        public string[] Priority { get; set; }

    }


    public partial class Setting
    {
        public static Setting FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Setting>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this Setting self)
        {
            return JsonConvert.SerializeObject(self,Formatting.Indented, Converter.Settings);
        }
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
