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
        static public void ComputeMultiMatch_old(ArrayList words, int th)
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
                    //           Console.WriteLine(indx + " " + words.Count + "(" + r.ToString() + ")");
                    Console.WriteLine(exact_count + " \t" + count + "\t (" + exact_count * 100.0 / count + ")");
                }
                string s = (string)words[indx];
                if (print) Console.WriteLine(s);
                //iterate through inverteed index    
                //Dictionary<int, int> matches = new Dictionary<int, int>();
                HashSet<int> matches = new HashSet<int>();
                HashSet<int> pairs = new HashSet<int>();
                for (int l = s.Length - th; l <= s.Length; l++)
                {
                    for (int i = 0; i < th + 1; i++)
                    {
                        invertedList L = GetList(invertedlists, i, l);
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
                                    if (!matches.Add(x))
                                        pairs.Add(x);
                                }
                            }
                        }
                        if (print) Console.WriteLine();
                    }
                }

                count += matches.Count;
                foreach (int p in pairs)
                {
                    if (Lev.editdistance((string)words[indx], (string)words[p], th) <= th)
                        exact_count++;
                }
                #region parition
                //parition s into strings
                string[] ps = parition(s, th, 1);
                //adding parition to the index
                int m = 0;
                int start = 0;
                foreach (string p in ps)
                {
                    //get the invert list if exists; otherwise creates a new one
                    invertedList L = getList(invertedlists, m, s.Length);
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

        /*if (indx % progress == 0)
                {
                    float r = (float)100.0 * indx / words.Count;
                    Console.Write("(" + r.ToString("0.00") + ")");
                    for (int lvl = 0; lvl < epslion + 1; lvl++)
                        Console.Write(count[lvl] + "\t");
                    Console.WriteLine();
                }*/
        static public void test(ArrayList words, int th, int epslion)
        {
            long[] count = new long[epslion + 1];

            words.Sort(new StringComparer());

            string[] xsss = parition((string)words[143896], 3, 4);

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            //StreamWriter sw = new StreamWriter("c:\\data\\out_"+th+"_"+epslion+".txt", true);
            //sw.WriteLine("count:" + words.Count + "\tth:" + th + "\te" + epslion);
            for (int indx = (int)(0); indx < words.Count; indx++)
            {
                if (indx % progress == 0)
                {
                    Console.Write(".");
                }

                for (int i = 0; i < epslion + 1; i++)
                    count[i] = 0;
                HashSet<string> ss = new HashSet<string>();
                //ss.Add("whyalla");
                //ss.Add("amassing");
                ss.Add("inarticulateness_s");
                string s = (string)words[indx];

                HashSet<int>[] matches = new HashSet<int>[epslion + 1];
                for (int i = 0; i < epslion + 1; i++) matches[i] = new HashSet<int>();
                #region matches
                //if (ss.Contains(s))
                {
                    for (int l = s.Length - th; l <= s.Length; l++)
                    {
                        //sw.WriteLine("length  " + l);
                        for (int i = 0; i < th + epslion; i++)
                        {
                            invertedList L = GetList(invertedlists, i, l);
                            if (L == null) continue;
                            if (L.length == 0) continue;
                            int pi = L.start;
                            int delta = s.Length - l;

                            //iterate throw
                            /*  int lowerbound = (int)Math.Max(pi - (i + 1 - 1), pi + delta - (th+epslion - i-1));
                              lowerbound = (int)Math.Max(0, lowerbound);
                              int upperbound = (int)Math.Min(pi + (i + 1 - 1), pi + delta + (th+epslion - i-1));
                              upperbound = (int)Math.Min(s.Length - L.length, upperbound);*/

                            int lowerbound = pi - (int)((th - delta) / 2);
                            int upperbound = pi + (int)((th + delta) / 2);

                            lowerbound = (int)Math.Max(0, lowerbound);
                            upperbound = (int)Math.Min(s.Length - L.length, upperbound);
                            //sw.WriteLine("i:"+i+ "["+lowerbound+","+upperbound+"]");
                            for (int k = lowerbound; k <= upperbound; k++)
                            {
                                string tmp = s.Substring(k, L.length);
                                if (L.ht.ContainsKey(tmp))
                                {
                                    List<int> il = (List<int>)L.ht[tmp];
                                    //sw.Write(k + " " +tmp+" "+ il.Count+"(");
                                    /*foreach (int x in il)
                                    {
                                        sw.Write(x + ",");
                                    }*/
                                    //sw.WriteLine("");
                                    foreach (int x in il)
                                    {
                                        int level = 0;
                                        while (!matches[level].Add(x))
                                        {
                                            level++;
                                            if (level >= epslion) break;
                                        }
                                    }
                                }
                                //  else sw.WriteLine(k + " " +tmp+" "+ "N/A");
                            } //
                        }
                    }
                }
                #endregion

                foreach (int p in matches[epslion - 1])
                {
                    //if (Lev.editdistance((string)words[indx], (string)words[p], th) <= th){
                    matches[epslion].Add(p);
                    //}
                }
                for (int lvl = 0; lvl < epslion + 1; lvl++)
                    count[lvl] = matches[lvl].Count;

                #region parition
                //parition s into strings
                string[] ps = parition(s, th, epslion);
                //adding parition to the index
                int m = 0;
                int start = 0;
                foreach (string p in ps)
                {
                    //get the invert list if exists; otherwise creates a new one
                    invertedList L = getList(invertedlists, m, s.Length);
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
                //save results
                //if (ss.Contains(s))
                {
                    //sw.Write(indx + "\t" + words[indx] + "\t");
                    //for (int i = 0; i < epslion + 1; i++)
                    //{
                    //  sw.Write(count[i] + "\t");
                    //}
                    //sw.WriteLine();
                    /*
                    HashSet<int> hs=matches[epslion];
                    foreach (int j in hs)
                    {
                        
                            sw.Write(words[j]+"("+j+")\t");
                    }
                    sw.WriteLine();*/
                }
            }
            //sw.Close();
            invertedlists = new Hashtable();
        }


        static public void ComputeMultiMatch(ArrayList words, int th, int epslion)
        {
            long[] count = new long[epslion + 1];

            words.Sort(new StringComparer());

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            for (int indx = (int)(0); indx < words.Count; indx++)
            {
                if (indx % progress == 0)
                {
                    Console.Write(".");
                }

                string s = (string)words[indx];

                HashSet<int>[] matches = new HashSet<int>[epslion + 1];
                for (int i = 0; i < epslion + 1; i++) matches[i] = new HashSet<int>();
                #region matches
                for (int l = s.Length - th; l <= s.Length; l++)
                {
                    for (int i = 0; i < th + epslion; i++)
                    {
                        invertedList L = GetList(invertedlists, i, l);
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
                                    int level = 0;
                                    while (!matches[level].Add(x))
                                    {
                                        level++;
                                        if (level >= epslion) break;
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                foreach (int p in matches[epslion - 1])
                {
                    //if (Lev.editdistance((string)words[indx], (string)words[p], th) <= th){
                    matches[epslion].Add(p);
                    //}
                }
                for (int lvl = 0; lvl < epslion + 1; lvl++)
                    count[lvl] += matches[lvl].Count;

                #region parition
                //parition s into strings
                string[] ps = parition(s, th, epslion);
                //adding parition to the index
                int m = 0;
                int start = 0;
                foreach (string p in ps)
                {
                    //get the invert list if exists; otherwise creates a new one
                    invertedList L = getList(invertedlists, m, s.Length);
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
            invertedlists = new Hashtable();
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

        static public HashSet<int>[] getMatches(int epslion, int th, string s, Hashtable ht, HashSet<int> pmatches)
        {
            HashSet<int>[] matches = new HashSet<int>[epslion + 1];
            for (int i = 0; i < epslion + 1; i++) matches[i] = new HashSet<int>();
        
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
                                if (pmatches!=null && !pmatches.Contains(x)) continue;
                                int level = 0;

                                while (!matches[level].Add(x))
                                {
                                    level++;
                                    if (level >= epslion) break;
                                }
                            }
                        }
                    }
                }
            }

            return matches;

        }
        static public void ComputeNewMatch(ArrayList words, int th, int epslion)
        {
            long[] count = new long[epslion + 1];

            words.Sort(new StringComparer());

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            for (int indx = (int)(0); indx < words.Count; indx++)
            {
                if (indx % progress == 0)
                {
                    Console.Write(".");
                }

                string s = (string)words[indx];

                HashSet<int>[] m = getMatches(1, th, s, invlists, null);
                HashSet<int>[] matches = getMatches(epslion, th, s, invertedlists, m[0]);

                foreach (int p in matches[epslion - 1])
                {
                    //if (Lev.editdistance((string)words[indx], (string)words[p], th) <= th){
                    matches[epslion].Add(p);
                    //}
                }
                for (int lvl = 0; lvl < epslion + 1; lvl++)
                    count[lvl] += matches[lvl].Count;

                #region parition
                //parition s into strings
                string[] ps = parition(s, th, epslion);
                //adding parition to the index
                AddPart(invertedlists, s, indx, ps);
                string[] ps1 = parition(s, th, epslion);
                //adding parition to the index
                AddPart(invlists, s, indx, ps1);
                #endregion
            }
            invertedlists = new Hashtable();
            invlists = new Hashtable();
        }
    }
}
