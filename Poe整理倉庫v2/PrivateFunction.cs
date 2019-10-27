using System;
using System.Collections.Generic;
using System.Drawing;

namespace Poe整理倉庫v2
{
    public static class PrivateFunction
    {
        public static bool IsChineseContain(string s)
        {
            foreach (char c in s)
            {
                if ((int)c > 127)
                    return true;
            }
            return false;
        }
        public static string GetStringAfterSomething(string s, string beforeWhat)
        {
            string temp = s.Substring(s.IndexOf(beforeWhat) + 1, s.Length - s.IndexOf(beforeWhat) - 1);
            return temp;
        }
        /// <summary>
        /// 依照自訂進位制進行進位
        /// </summary>
        /// <param name="list">原始陣列</param>
        /// <param name="carryX">X進位制</param>
        /// <returns></returns>
        public static List<int> CheckCarry(List<int> list, int carryX)
        {
            //List<int> temp = new List<int>(list);
            //bool haveCarry = false;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                //餘數
                int remainder = 0;
                //商數
                int quotient = Math.DivRem(list[i], carryX, out remainder);
                if (quotient > 0)
                {

                    if (i == 0)
                        list.Insert(0, quotient);
                    else
                        list[i - 1] += quotient;
                    list[i] = remainder;
                }
            }
            return list;
        }
        /// <summary>
        /// 計算陣列中每個數字的總和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static int CalcArrayTotal(int[] array)
        {
            int total = 0;
            foreach (var t in array)
                total += t;
            return total;
        }
        /// <summary>
        /// 兩陣列相加(A+B)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>A+B</returns>
        public static int[] ArrayPlus(int[] A, int[] B)
        {
            int[] temp = new int[A.Length];
            if (A.Length.Equals(B.Length))
                for (int i = 0; i < A.Length; i++)
                {
                    temp[i] = A[i] + B[i];
                }
            return temp;
        }
        /// <summary>
        /// 兩陣列相減(A-B)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>A-B</returns>
        public static int[] ArrayMinus(int[] A, int[] B)
        {
            int[] temp = new int[A.Length];
            if (A.Length.Equals(B.Length))
                for (int i = 0; i < A.Length; i++)
                {
                    temp[i] = A[i] - B[i];
                }
            return temp;
        }
        public static int GetHashCode(int[] array)
        {
            if (array != null)
            {
                int hash = 17;
                foreach (var item in array)
                {
                    hash = hash * 23 + item.GetHashCode();
                }
                return hash;
            }
            return 0;
        }
        /// <summary>
        /// 判斷數值是否在兩者之間
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static bool IsWithin(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
        /// <summary>
        /// 判斷數值是否在兩者之間
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static bool IsWithin(this Point value, Point minimum, Point maximum)
        {
            if (Point.Equals(value, minimum))
                return true;
            return value.X >= minimum.X && value.Y >= minimum.Y && value.X <= maximum.X && value.Y <= maximum.Y;
        }
    }
}
