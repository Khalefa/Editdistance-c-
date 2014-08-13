using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    class word
    {
        public word(string s)
        {
            w = s;
        }
        public string w;
        public int getlen()
        {
            return w.Length;
        }
        public int getlen2()
        {
            int l2 = 0;
            char[] c = w.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                char cc = c[i];
                int j = cc - 'a';                
                if (j < 14 && j>=0) l2++;
            }
            return l2;
        }
    }
}
