using System.Text.Json;
using System.Text.Json.Serialization;

namespace Poe整理倉庫v2
{
    public partial class Setting
    {
        [JsonPropertyName("Hotkey_Scan")]
        public int HotkeyScan { get; set; }

        [JsonPropertyName("Hotkey_Start")]
        public int HotkeyStart { get; set; }

        [JsonPropertyName("Hotkey_Stop")]
        public int HotkeyStop { get; set; }

        [JsonPropertyName("Delay1")]
        public int Delay1 { get; set; }

        [JsonPropertyName("Delay2")]
        public int Delay2 { get; set; }

        [JsonPropertyName("Delay_Scan")]
        public int Delay_Scan { get; set; }

        [JsonPropertyName("Direction")]
        public string Direction { get; set; }

        [JsonPropertyName("LowQ")]
        public int LowQ { get; set; }

        [JsonPropertyName("Species")]
        public string[] Species { get; set; }

        [JsonPropertyName("Priority")]
        public string[] Priority { get; set; }
    }

    public partial class Setting
    {
        public static Setting FromJson(string json)
        {
            return JsonSerializer.Deserialize<Setting>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this Setting self)
        {
            return JsonSerializer.Serialize(self, Converter.Settings);
        }
    }

    public class Converter
    {
        public static readonly JsonSerializerOptions Settings = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    }
}