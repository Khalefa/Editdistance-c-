using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Verification;
namespace EditDistance.Passjoin
{
    public class passjoinII
    {
         [DebuggerDisplay("'{l}' '{i}'")]
         class pair
         {
             public int l;
             public int i;
             public pair()
             {
             }
             public pair(int l, int i)
             {
                 this.l = l;
                 this.i = i;
             }
             public override int GetHashCode()
             {
                 return l.GetHashCode() * i.GetHashCode();
             }
             public override bool Equals(object obj)
             {
                 pair p = (pair)obj;
                 return p.l == l && p.i == i;
             }
         }
        class invertedList
        {
            public Hashtable ht = new Hashtable();
            public int length;
            public int start;
            public void add(string s, int indx)
            {
                if (s == null) return;
                if (ht.ContainsKey(s))
                {
                    List<int> l = (List<int>)ht[s];
                    l.Add(indx);
                }
                else
                {
                    List<int> l = new List<int>();
                    l.Add(indx);
                    ht.Add(s, l);
                }
            }
        }
        public static Hashtable invertedlists = new Hashtable();//stores the inverted listes the keu is a composite of length and 

        class StringComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                string sx = (string)x;
                string sy = (string)y;
                if (sx.Length > sy.Length) return 1;
                else if (sx.Length < sy.Length) return -1;
                else return String.Compare(sx, sy);
            }
        }
        public static string[] parition(string s, int th, int eps)
        {
            string[] a = new string[th + eps];
            int d = s.Length / (th + eps);
            string p = "";
            int j = 0;
            int k = 0;
            for (int i = 0; i < s.Length; i++)
            {
                a[k] = a[k] + s[i];
                j++;
                if (j >= d)
                {
                    if (k < th)
                        k++;
                    j = 0;
                    p = "";
                }
            }
            return a;
        }
        public static int[] parition(int sl, int th)
        {
            int[] a = new int[th + 1];
            int d = sl / (th + 1);

            int k = 0;
            for (int i = 0; i < th; i++)
            {
                a[i] = d;
                k += a[i];
            }
            a[th] = sl - k;
            return a;
        }
        static invertedList GetList(int i, int len)
        {
            pair x = new pair();
            x.l = len;
            x.i = i;
            invertedList L = null;
            if (invertedlists.ContainsKey(x))
                L = (invertedList)invertedlists[x];
            return L;
        }
        static invertedList getList(int i, int len)
        {
            pair x = new pair();
            x.l = len;
            x.i = i;
            invertedList L = null;
            if (invertedlists.ContainsKey(x))
                L = (invertedList)invertedlists[x];
            else
            {
                L = new invertedList();
                invertedlists.Add(x, L);
            }

            return L;
        }
        //find candidate words
        static public void ComputeMultiMatch(ArrayList words, int th, int epslion)
        {
            bool print = false;
            long count = 0;
            long exact_count = 0;
            words.Sort(new StringComparer());
            int progress = (int)Math.Ceiling(words.Count / 100.0);

            for (int indx = (int)(0); indx < words.Count; indx++)
            {
                if (indx % progress == 0)
                {
                    float r = (float)100.0 * indx / words.Count;
                    
                    Console.WriteLine(indx + " " + words.Count + "(" + r.ToString("dd")+")");
                    Console.WriteLine(exact_count + " " + count + " (" + exact_count * 100.0 / count + ")");
                }
                string s = (string)words[indx];
                if (print) Console.WriteLine(s);
                //iterate through inverteed index    
                //Dictionary<int, int> matches = new Dictionary<int, int>();
                HashSet<int> matches = new HashSet<int>();
                HashSet<int> pairs = new HashSet<int>();
                for (int l = s.Length - th; l <= s.Length; l++)
                {
                    for (int i = 0; i < th + epslion; i++)
                    {
                        invertedList L = GetList(i, l);
                        if (L == null) continue;
                        if (L.length == 0) continue;
                        int pi = L.start;
                        int delta = s.Length - l;
                        //iterate throw
                        int lowerbound = (int)Math.Max(pi - (i + 1 - 1), pi + delta - (th - i));
                        lowerbound = (int)Math.Max(0, lowerbound);
                        int upperbound = (int)Math.Min(pi + (i + 1 - 1), pi + delta + (th - i));
                        upperbound = (int)Math.Min(s.Length - L.length, upperbound);

                        for (int k = lowerbound; k <= upperbound; k++)
                        {
                            string tmp = s.Substring(k, L.length);
                            if (print) Console.Write(tmp + " ");
                            if (L.ht.ContainsKey(tmp))
                            {
                                List<int> il = (List<int>)L.ht[tmp];

                                foreach (int x in il)
                                {
                                    matches.Add(x);
                                }
                            }
                        }
                        if (print) Console.WriteLine();
                    }
                }

                count += pairs.Count;
                //exact_count += pairs.Count;
                foreach (int p in pairs)
                {
                    //if (Lev.editdistance((string)words[indx], (string)words[p], th) <= th)
                    exact_count++;
                }
                //pairs.Clear();
                #region parition
                //parition s into strings
                string[] ps = parition(s, th, epslion);
                //adding parition to the index
                int m = 0;
                int start = 0;
                foreach (string p in ps)
                {
                    //get the invert list if exists; otherwise creates a new one
                    invertedList L = getList(m, s.Length);
                    L.add(p, indx);
                    if (p != null)
                    {
                        L.length = p.Length;
                        L.start = start;
                        start = start + p.Length;
                    }
                    m++;
                }
                #endregion
            }
        }
    }
}
