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

        public static void GetGrams(ArrayList words, int q)
        {
            long cc = 0;
            Hashtable ht = new Hashtable();
            foreach (string w in words)
            {
                string [] t=Util.grams(w, q);
                cc+=t.Length;
                foreach (string ws in t)
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
            Console.WriteLine(ht.Count);
            //   return;
            long c = 0;
            foreach (string w in words)
            {

                HashSet<string> hs = new HashSet<string>();
                //ArrayList l = new ArrayList();
                foreach (string ws in Util.grams(w, q))
                {
                    ArrayList l = (ArrayList)ht[ws];
                    c += l.Count;
                    //foreach (string s in l) ;

                    // hs.Add(s);
                }
                //  Console.WriteLine(w + " " + hs.Count);
            }

        }

    }
}
