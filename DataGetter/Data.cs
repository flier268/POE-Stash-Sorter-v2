using SQLite;
using System;

namespace DataGetter
{
    public class Data
    {
        [PrimaryKey]
        public string Name_English { get; set; }

        public string Name_Chinese { get; set; }
        public string Type { get; set; }
        public int Rarity { get; set; }
        public string ImageURL { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string GemColor { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}