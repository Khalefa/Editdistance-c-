using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    class prefix
    {
        public static Hashtable getprefix(ArrayList words, int p)
        {
            Hashtable ht = new Hashtable();
            foreach (string word in words)
            {
                //get prefix
                string prefix="";
                if (p > word.Length)
                {
                    prefix = word;
                }
                else
                {
                    prefix = word.Substring(0, p);
                }
                if (ht.ContainsKey(prefix))
                {
                    int x = (int)ht[prefix];
                    ht[prefix] = x + 1;
                }
                else
                {
                    ht.Add(prefix, 1);
                }
            }
            return ht;
        }
    }
}
