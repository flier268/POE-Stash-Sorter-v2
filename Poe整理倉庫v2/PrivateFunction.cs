using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poe整理倉庫v2
{
    public class PrivateFunction
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
    }
}
