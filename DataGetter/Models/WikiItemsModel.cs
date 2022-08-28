using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace DataGetter.Models
{
    public class WikiItemsModel
    {
        [JsonPropertyName("cargoquery")]
        public List<Cargoquery> Cargoquerys { get; set; }

        // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
        public class Cargoquery
        {
            [JsonPropertyName("title")]
            public Title Title { get; set; }
        }

        [Equals(DoNotAddEqualityOperators = true)]
        public class Title
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("class id")]
            public string ClassId { get; set; }

            [JsonPropertyName("size x")]
            public string SizeX { get; set; }

            [JsonPropertyName("size y")]
            public string SizeY { get; set; }

            [JsonPropertyName("rarity id")]
            public string RarityId { get; set; }

            [JsonPropertyName("base item")]
            public string BaseItem { get; set; }

            [JsonPropertyName("tags")]
            public string Tags { get; set; }

            [JsonPropertyName("inventory icon")]
            public string InventoryIcon { get; set; }

            [JsonPropertyName("dexterity percent")]
            public string DexterityPercent { get; set; }

            [JsonPropertyName("strength percent")]
            public string StrengthPercent { get; set; }

            [JsonPropertyName("intelligence percent")]
            public string IntelligencePercent { get; set; }

            [JsonPropertyName("release version")]
            public string ReleaseVersion { get; set; }

            [JsonPropertyName("removal version")]
            public string RemovalVersion { get; set; }

            public string GemColor
            {
                get
                {
                    switch (StrengthPercent)
                    {
                        case var _ when StrengthPercent == null && IntelligencePercent == null && DexterityPercent == null:
                            return "n";

                        case var _ when StrengthPercent != null && IntelligencePercent == null && DexterityPercent == null:
                            return "r";

                        case var _ when StrengthPercent == null && IntelligencePercent == null && DexterityPercent != null:
                            return "g";

                        case var _ when StrengthPercent == null && IntelligencePercent != null && DexterityPercent == null:
                            return "b";

                        default:
                            bool r = int.TryParse(StrengthPercent, out int str);
                            bool g = int.TryParse(DexterityPercent, out int dex);
                            bool b = int.TryParse(IntelligencePercent, out int @int);
                            if (str == 33 && dex == 33 && @int == 34)
                                return "w";
                            var list = new List<(string color, int percent)>() { ("r", str), ("g", dex), ("b", @int), };
                            return list.OrderByDescending(x => x.percent).First().color;
                    }
                }
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode() * ClassId.GetHashCode() * RarityId.GetHashCode() * (Tags == null ? 1 : Tags.GetHashCode()) * InventoryIcon.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return this.GetHashCode() == obj.GetHashCode();
            }
        }
    }
}