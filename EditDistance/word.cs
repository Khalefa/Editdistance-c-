using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    class word
    {
     public       string w;
     public int getlen(){
         return w.Length;
     }
     public int getlen2()
     {
         int l2 = 0;
         char []c = w.ToCharArray();
         for (int i = 0; i < c.Length; i++)
         {
             char cc = c[i];
             int j = cc - 'a';
             if (j < 13) l2++;
         }
         return w.Length;
     }
    }
}
