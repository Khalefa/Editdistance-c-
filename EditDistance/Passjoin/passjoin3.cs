using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Verification;
using System.IO;
namespace EditDistance.Passjoin
{

    public class PairLong
    {
        public long first;
        public long second;
        public PairLong(long f, long s)
        {
            first = f;
            second = s;
        }

        public PairLong()
        {
            // TODO: Complete member initialization
        }
    }

    //we use array instead of hashset for 
    public class passjoinIII
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
        public static Hashtable invlists = new Hashtable();
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
                    if (k < (th + eps) - 1)
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
        static invertedList GetList(Hashtable ht, int i, int len)
        {
            pair x = new pair();
            x.l = len;
            x.i = i;
            invertedList L = null;
            if (ht.ContainsKey(x))
                L = (invertedList)ht[x];
            return L;
        }
        static invertedList getList(Hashtable ht, int i, int len)
        {
            pair x = new pair();
            x.l = len;

            x.i = i;
            invertedList L = null;
            if (ht.ContainsKey(x))
                L = (invertedList)ht[x];
            else
            {
                L = new invertedList();
                ht.Add(x, L);
            }

            return L;
        }


        static public void AddPart(Hashtable ht, string s, int indx, string[] ps)
        {
            int m = 0;
            int start = 0;
            foreach (string p in ps)
            {
                //get the invert list if exists; otherwise creates a new one
                invertedList L = getList(ht, m, s.Length);
                L.add(p, indx);
                if (p != null)
                {
                    L.length = p.Length;
                    L.start = start;
                    start = start + p.Length;
                }
                m++;
            }

        }

        static public List<int> getMatches(int epslion, int th, string s, Hashtable ht, int j, int[] matches, int[] indx)
        {
            List<int> m = new List<int>();

            for (int l = s.Length - th; l <= s.Length; l++)
            {
                for (int i = 0; i < th + epslion; i++)
                {
                    invertedList L = GetList(ht, i, l);
                    if (L == null) continue;
                    if (L.length == 0) continue;
                    int pi = L.start;
                    int delta = s.Length - l;

                    //iterate throw
                    /*  int lowerbound = (int)Math.Max(pi - (i + 1 - 1), pi + delta - (th+epslion - i-1));
                      int upperbound = (int)Math.Min(pi + (i + 1 - 1), pi + delta + (th+epslion - i-1));*/

                    int lowerbound = pi - (int)((th - delta) / 2);
                    int upperbound = pi + (int)((th + delta) / 2);

                    lowerbound = (int)Math.Max(0, lowerbound);
                    upperbound = (int)Math.Min(s.Length - L.length, upperbound);

                    for (int k = lowerbound; k <= upperbound; k++)
                    {
                        string tmp = s.Substring(k, L.length);
                        if (L.ht.ContainsKey(tmp))
                        {
                            List<int> il = (List<int>)L.ht[tmp];
                            foreach (int x in il)
                            {
                                if (indx[x] == j)
                                {
                                    matches[x]++;
                                    if (matches[x] == epslion) m.Add(x);
                                }
                                else
                                {
                                    indx[x] = j;
                                    matches[x] = 1;
                                    if (matches[x] == epslion) m.Add(x);
                                }
                            }
                        }
                    }
                }
            }
            return m;
        }

        static public void getMatches_noreturn(int th, string s, Hashtable ht, int j, int[] matches, int[] indx)
        {
            int epslion = 1;
            for (int l = s.Length - th; l <= s.Length; l++)
            {
                int delta = s.Length - l;
                for (int i = 0; i < th + epslion; i++)
                {
                    invertedList L = GetList(ht, i, l);
                    if (L == null) continue;
                    if (L.length == 0) continue;
                    int pi = L.start;                    

                    //iterate throw
                    /*  int lowerbound = (int)Math.Max(pi - (i + 1 - 1), pi + delta - (th+epslion - i-1));
                      int upperbound = (int)Math.Min(pi + (i + 1 - 1), pi + delta + (th+epslion - i-1));*/

                    int lowerbound = pi - (int)((th - delta) / 2);
                    int upperbound = pi + (int)((th + delta) / 2);

                    lowerbound = (int)Math.Max(0, lowerbound);
                    upperbound = (int)Math.Min(s.Length - L.length, upperbound);

                    for (int k = lowerbound; k <= upperbound; k++)
                    {
                        string tmp = s.Substring(k, L.length);
                        if (L.ht.ContainsKey(tmp))
                        {
                            List<int> il = (List<int>)L.ht[tmp];
                            foreach (int x in il)
                            {
                                indx[x] = j;
                                matches[x] = 0;
                            }
                        }
                    }
                }
            }
        }
        static public List<int> getMatches_withmemo(int epslion, int th, string s, Hashtable ht, int j, int[] matches, int[] indx)
        {
            List<int> m = new List<int>();
            for (int l = s.Length - th; l <= s.Length; l++)
            {
                for (int i = 0; i < th + epslion; i++)
                {
                    invertedList L = GetList(ht, i, l);
                    if (L == null) continue;
                    if (L.length == 0) continue;
                    int pi = L.start;
                    int delta = s.Length - l;

                    //iterate throw
                    /*  int lowerbound = (int)Math.Max(pi - (i + 1 - 1), pi + delta - (th+epslion - i-1));
                      int upperbound = (int)Math.Min(pi + (i + 1 - 1), pi + delta + (th+epslion - i-1));*/

                    int lowerbound = pi - (int)((th - delta) / 2);
                    int upperbound = pi + (int)((th + delta) / 2);

                    lowerbound = (int)Math.Max(0, lowerbound);
                    upperbound = (int)Math.Min(s.Length - L.length, upperbound);

                    for (int k = lowerbound; k <= upperbound; k++)
                    {
                        string tmp = s.Substring(k, L.length);
                        if (L.ht.ContainsKey(tmp))
                        {
                            List<int> il = (List<int>)L.ht[tmp];
                            foreach (int x in il)
                            {
                                if (indx[x] == j)
                                {
                                    matches[x]++;
                                    if (matches[x] == epslion) m.Add(x);
                                }

                            }
                        }
                    }
                }
            }
            return m;
        }

        static public PairLong ComputeMatch(ArrayList words, int th, int eps = 1)
        {
            Global.alg = "P3J";
            long rcnt = 0;
            long candidta_cnt = 0;
            int[] matches_arr = new int[words.Count];
            int[] indx = new int[words.Count];
           
            words.Sort(new StringComparer());

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            for (int j = (int)(0); j < words.Count; j++)
            {
                int e = eps;
                if (j % progress == 0)
                {
                    Console.Write(".");
                }

                string s = (string)words[j];
                string ss = s;
                //two cases: if s is shorter than threshold
                if (s.Length >= th + eps)
                {
                    //this is ok
                    ss = s;
                }
                else if (s.Length < th + 1)
                {
                    //append the difference
                    ss="";
                    for(int i=0;i<th+1-s.Length;i++)
                        ss=ss+' ';
                    ss=s+ss;
                    e = 1;
                }
                else
                {
                    //reduce eps to  work fine
                    e = (s.Length)-th;
                }
                
                List<int> l = getMatches(e, th, ss, invlists, j, matches_arr, indx);
                candidta_cnt += l.Count;
                if (Global.exact)
                {
                    foreach (int p in l)
                    {
                        if (Lev.editdistance(s, (string)words[p], th) <= th)
                        {
                            rcnt++;
                        }
                    }
                }
                #region parition
                string[] ps1 = parition(ss, th, e);
                //adding parition to the index
                AddPart(invlists, ss, j, ps1);
                #endregion
            }

            invlists = new Hashtable();
            return new PairLong(rcnt, candidta_cnt);
        }
        static public PairLong ComputeMyMatch(ArrayList words, int th, int eps = 1)
        {
            Global.alg = "MPJ";
            long rcnt = 0;
            long candidta_cnt = 0;
            int[] matches_arr = new int[words.Count];
            int[] indx = new int[words.Count];
            invlists = new Hashtable();
            invertedlists = new Hashtable();
            words.Sort(new StringComparer());

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            for (int j = (int)(0); j < words.Count; j++)
            {
                int e = eps;
                if (j % progress == 0)
                {
                    Console.Write(".");
                }

                string s = (string)words[j];
                string ss = s;
                //two cases: if s is shorter than threshold
                if (s.Length >= th + eps)
                {
                    //this is ok
                    ss = s;
                }
                else if (s.Length < th + 1)
                {
                    //append the difference
                    ss = "";
                    for (int i = 0; i < th + 1 - s.Length; i++)
                        ss = ss + ' ';
                    ss = s + ss;
                    e = 1;
                }
                else
                {
                    //reduce eps to  work fine
                    e = (s.Length) - th;
                }
             
                getMatches_noreturn(th, ss, invlists, j, matches_arr, indx);
                List<int> l = getMatches_withmemo(e, th, ss, invertedlists, j, matches_arr, indx);
                candidta_cnt += l.Count;
                if (Global.exact)
                {
                    foreach (int p in l)
                    {
                        if (Lev.editdistance(s, (string)words[p], th) <= th)
                        {
                            rcnt++;
                        }
                    }
                }
                #region parition
                string[] ps1 = parition(ss, th, 1);
                AddPart(invlists, ss, j, ps1);
                string[] ps = parition(ss, th, e);
                AddPart(invertedlists, ss, j, ps);

                #endregion
            }

            return new PairLong(rcnt, candidta_cnt);
        }

        /* for (long p = 0; p < matches_arr.LongLength; p++)
         {
             //if (Lev.editdistance((string)words[indx], (string)words[p], th) <= th){
             matches_arr[p]++;
             //}
         }
         for (long p = 0; p < matches_arr.LongLength; p++)
         {
          int lvl= matches_arr[p];
          count[lvl]++;
         }*/
    }
}
