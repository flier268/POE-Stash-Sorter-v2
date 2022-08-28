using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// 獲取兩個字串的相似度
        /// </summary>
        /// <param name=”sourceString”>第一個字串</param>
        /// <param name=”str”>第二個字串</param>
        /// <returns></returns>
        public static decimal GetSimilarityWith(this string sourceString, string str)
        {
            decimal Kq = 2;
            decimal Kr = 1;
            decimal Ks = 1;

            char[] ss = sourceString.ToCharArray();
            char[] st = str.ToCharArray();

            //獲取交集數量
            int q = ss.Intersect(st).Count();
            int s = ss.Length - q;
            int r = st.Length - q;

            return Kq * q / (Kq * q + Kr * r + Ks * s);
        }
    }
}