using System;
using Poe整理倉庫v2.Enums;

namespace Poe整理倉庫v2.Helper
{
    internal class RarityHelper
    {
        public static Rarity StringToRarity(string s)
        {
            switch (s)
            {
                case "Normal":
                case "普通":
                    return Rarity.Normal;

                case "Magic":
                case "魔法":
                    return Rarity.Magic;

                case "Rare":
                case "稀有":
                    return Rarity.Rare;

                case "Unique":
                case "傳奇":
                    return Rarity.Unique;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}