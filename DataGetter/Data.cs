using System;
using System.Collections.Generic;
using LiteDB;

namespace DataGetter
{
    public class Data
    {
        [BsonId]
        public string Name_English { get; set; }

        public string Name_Chinese { get; set; }
        public string Type { get; set; }
        public string BaseItem { get; set; }
        public int Rarity { get; set; }
        public string WikiUrl { get; set; }
        public string ImageURL { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string GemColor { get; set; }
        public List<string> Tags { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal IntelligencePercent { get; set; }
        public decimal DexterityPercent { get; set; }
        public decimal StrengthPercent { get; set; }
    }
}