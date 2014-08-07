using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
namespace EditDistance.Grams
{
    [DebuggerDisplay("{s}:{freq}")]
    class Gram
    {
        public string s;
       public long freq;
        public Gram(string s, long freq)
        {
            this.s = s;
            this.freq = freq;
        }
    }
    class GramComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            Gram sx = (Gram)x;
            Gram sy = (Gram)y;
            if (sx.freq > sy.freq) return 1;
            else if (sx.freq < sy.freq) return -1;
            else return sx.s.CompareTo(sy.s);
        }
    }
    class Grams{
        public static Hashtable GetCountGrams(ArrayList words, int q)
        {
            long cc = 0;
            Hashtable ht = new Hashtable();
            foreach (string w in words)
            {
                string[] t = Util.grams(w, q);
                cc += t.Length;
                foreach (string ws in t)
                {
                    if (!ht.ContainsKey(ws))
                        ht.Add(ws, 1);
                    
                    else
                        ht[ws] = (int)ht[ws]+1;
                    
                }
            }
            Console.WriteLine(ht.Count);
            return ht;
        }
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
              return;
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
        //get the most not frquent grams
        public static Gram[] getLeastFrquentGrams(Hashtable ht, string s, int q, int th)
        {
            string[]grams_s= Util.grams(s, q);
            Gram[] grams = new Gram[grams_s.Length];
            for (int i = 0; i < grams_s.Length; i++)
            {
                grams[i] = new Gram(grams_s[i], (int)ht[grams_s[i]]);
            }
            Array.Sort(grams,new GramComparer());
            //now return q*th+1 gram
            Array.Resize<Gram>(ref grams,Math.Min(q*th+1,s.Length));
            
            return grams;
        }
        public static void ComputeMatches(ArrayList words,int q, int th){
            //compute the count
            Hashtable ht = GetCountGrams(words, q);
            Hashtable htw = new Hashtable();
            foreach (string s in words)
            {
                Gram[] g = getLeastFrquentGrams(ht, s, q, th);
                //now add them in hash buckets
                foreach (Gram gram in g)
                {
                }
            }
        }
    }
}
