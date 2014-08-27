using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class PJ
    {
        public static Hashtable invertedlists = new Hashtable();//stores the inverted listes the key is a composite of length and 
        public static Hashtable invlist_length = new Hashtable();
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
        static void cleanList(int len)
        {
            ArrayList LL = (ArrayList)invlist_length[len];
            if (LL == null)
            {
                return;
            }
            else
            {
                foreach (invertedList i in LL)
                {
                    i.del();
                }
                LL.Clear();
            }
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
        static invertedList GetandCreateList(Hashtable ht, int i, int len)
        {
            invertedList L = GetList(ht, i, len);
            if (L != null)
                return L;
            else
            {
                L = new invertedList();
                pair x = new pair();
                x.l = len;
                x.i = i;
                ht.Add(x, L);
                //add the list to hashtable
                ArrayList LL = (ArrayList)invlist_length[len];
                if (LL == null)
                {
                    LL = new ArrayList();
                    LL.Add(L);
                    invlist_length.Add(len, LL);
                }
                else
                {
                    LL.Add(L);
                }
                //---
            }
            return L;
        }
        static void adjust_string(string s, ref string ss, int th, ref int eps)
        {
            int e = eps;
            ss = s;
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
            eps = e;
        }
        static public void AddPart(Hashtable ht, string s, int indx, string[] ps)
        {
            int m = 0;
            int start = 0;
            foreach (string p in ps)
            {
                //get the invert list if exists; otherwise creates a new one
                invertedList L = GetandCreateList(ht, m, s.Length);
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
        static public PairLong getPassJoinMatches(int th, string s, Hashtable ht, int j, int[] matches, int[] indx, ref  ArrayList words)
        {
            long rcnt = 0;
            long candidta_cnt = 0;

            for (int l = Math.Max (s.Length - th,1); l <= s.Length; l++)
            {
                int delta = s.Length - l;
                for (int i = 0; i < th + 1; i++)
                {
                    invertedList L = GetList(ht, i, l);
                    if (L == null) continue;
                    if (L.length == 0) continue;
                    int pi = L.start;
                    //iterate throw
                    int lowerbound = (int)Math.Max(pi - (i + 1 - 1), pi + delta - (th + 1 - i - 1));
                    int upperbound = (int)Math.Min(pi + (i + 1 - 1), pi + delta + (th + 1 - i - 1));
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
                                if (indx[x] != j)
                                {
                                    indx[x] = j;
                                    matches[x] = 1;

                                    if (Global.exact)
                                    {
                                        candidta_cnt++;
                                        int t = Verification.Lev.editdistance2(s, (string)words[x], th, k, pi, L.length);

                                        if (t <= th)
                                        {
                                            rcnt++;
                                            //Console.WriteLine(s + ":" + (string)words[x]);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new PairLong(rcnt, candidta_cnt); ;
        }
        static public PairLong Compute(ArrayList words, int th)
        {
            PairLong p = new PairLong();
            Global.alg = "Passjoin";
            int[] matches_arr = new int[words.Count];
            int[] indx = new int[words.Count];
            // 
            for (int i = 0; i < words.Count; i++)
            {
                string s = (string)words[i];
                string ss=s;
                int e = 1;
                adjust_string(s, ref ss, th, ref e);
                words[i] = ss;
            }
            words.Sort(new WordComparer());

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            int len = 0;
            for (int j = (int)(0); j < words.Count; j++)
            {
                if (j % progress == 0)
                {
                    Console.Write(".");
                }

                string s = (string)words[j];
                if (s.Length != len)
                {
                    if (s.Length > th)
                    {
                        cleanList(s.Length - th - 1);
                    }
                }
                p = p + getPassJoinMatches(th, s, invertedlists, j, matches_arr, indx, ref words);
                #region parition
                //string ss=s;
                //int e = 2;
               // adjust_string(s, ref ss, th, ref e);
                
                string[] ps1 = parition(s, th, 1);
                //adding parition to the index
                AddPart(invertedlists, s, j, ps1);
                #endregion
                len = s.Length;
            }
            invertedlists = new Hashtable();
            return p;
        }
    }
}


