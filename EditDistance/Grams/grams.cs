using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace EditDistance.Grams
{
    class Gram
    {
       
        static void GetGrams(ArrayList words, int q)
        {
            Hashtable ht = new Hashtable();
            foreach (string w in words)
            {
                foreach (string ws in Util.grams(w, q))
                {
                    if (!ht.ContainsKey(ws))
                    {
                        ArrayList l = new ArrayList();
                        l.Add(w);
                        ht.Add(ws, l);
                    }
                    else
                    {
                        ArrayList l = (ArrayList)ht[ws];
                        l.Add(w);
                        ht[ws] = l;
                    }
                }

            }
            foreach (string w in words)
            {
                long c = 0;
                HashSet<string> hs = new HashSet<string>();
                foreach (string ws in Util.grams(w, q))
                {
                    ArrayList l = (ArrayList)ht[ws];
                    foreach (string s in l)
                        if (!hs.Contains(s)) hs.Add(s);
                }
                Console.WriteLine(w + " " + hs.Count);
            }

        }

    }
}
