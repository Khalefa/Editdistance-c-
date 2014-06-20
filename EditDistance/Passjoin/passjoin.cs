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
    public class passjoin
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
                    ArrayList l = (ArrayList)ht[s];
                    l.Add(indx);
                }
                else
                {
                    ArrayList l = new ArrayList();
                    l.Add(indx);
                    ht.Add(s, l);
                }
            }
        }
        public static Hashtable invertedlists = new Hashtable();//stores the inverted listes the keu is a composite of length and 
        public static Hashtable invertedlists_by_length = new Hashtable();
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
        public static string[] parition(string s, int th)
        {
            string[] a = new string[th + 1];
            int d = s.Length / (th + 1);
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
            //added to the inverted by the length

            if (invertedlists_by_length.ContainsKey(len))
            {
                ArrayList l = (ArrayList)invertedlists_by_length[len];
                l.Add(L);
            }
            else
            {
                ArrayList l = new ArrayList();
                l.Add(L);
                invertedlists_by_length.Add(len, l);
            }

            return L;
        }
        //find candidate words
        static public void ComputeMultiMatch(ArrayList words, int th)
        {
            bool print = false;
            long count = 0;
            long exact_count = 0;
            HashSet<pair> pairs = new HashSet<pair>();
            words.Sort(new StringComparer());
            int progress =(int) Math.Ceiling (words.Count / 100.0);
            
            for (int indx = (int)(0.8*words.Count); indx < words.Count; indx++)
            {

                if (indx % progress == 0)
                {
                    Console.WriteLine(indx + " " + words.Count + "(" + 100.0 * indx / words.Count + ")");
                    Console.WriteLine(exact_count + " " + count + " (" + exact_count * 100.0 / count + ")");
                }
             //   if (indx / progress > 10) break;
                string s = (string)words[indx];
                if (print) Console.WriteLine(s);
                //iterate through inverteed index                
                for (int l = s.Length - th; l <= s.Length; l++)
                {
                    for (int i = 0; i < th + 1; i++)
                    {                        
                        invertedList L = GetList(i, l);
                        if (L == null) continue;
                        if (L.length == 0) continue;
                        int pi = L.start;
                        int delta=s.Length-l;
                        //iterate throw
                        int lowerbound=(int) Math.Max(pi - (i + 1 - 1),pi+delta-(th-i));
                        lowerbound = (int)Math.Max(0, lowerbound);
                        int upperbound=(int) Math.Min(pi + (i + 1 - 1),pi+delta+(th-i));
                        upperbound=(int) Math.Min(s.Length-L.length,upperbound);

                        for (int k = lowerbound; k <= upperbound; k++)
                        {                        
                            string tmp = s.Substring(k, L.length);
                            if (print) Console.Write(tmp + " ");
                            if (L.ht.ContainsKey(tmp))
                            {
                                ArrayList il = (ArrayList)L.ht[tmp];
                                foreach (int x in il)
                                    pairs.Add(new pair(indx, x));
                            }
                        }
                        if(print)Console.WriteLine();
                    }
                }
                count += pairs.Count;
                foreach (pair p in pairs)
                {
                    if (Lev.editdistance((string)words[p.i], (string)words[p.l], th) <= th)
                        exact_count++;     
                }
                pairs.Clear();
                #region parition
                //parition s into strings
                string[] ps = parition(s, th);
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
            Console.WriteLine(exact_count);
            Console.WriteLine(count);
        }
        #region old
        static ArrayList ProcessWord(string s, string word, int th)
        {
            ArrayList ws = new ArrayList();
            //iterate throught inverted lists 
            for (int l = s.Length - th; l <= s.Length; l++)
            {
                ArrayList il = (ArrayList)invertedlists_by_length[l];
                if (il != null)
                {
                    foreach (invertedList L in il)
                    {
                        ArrayList pl = (ArrayList)L.ht[word];
                        if (pl != null)
                            foreach (int x in pl)
                            {
                                ws.Add(x);
                            }
                    }
                }
            }
            return ws;
        }
        static public void compute(ArrayList words, int th)
        {
            HashSet<pair> pairs = new HashSet<pair>();

            words.Sort(new StringComparer());
            for (int i = 0; i < words.Count; i++)
            {
                string s = (string)words[i];
                //if (s.Length < 8) continue;
                //ArrayList W = naive(s, th); //change it to delegate
                ArrayList W = lengths(s, th); //change it to delegate
                //finding matching string
                pairs.Clear();
                foreach (string w in W)
                {
                    ArrayList A = ProcessWord(s, w, th);
                    foreach (int a in A)
                        pairs.Add(new pair(i, a));
                }
                #region parition
                //parition s into strings
                string[] ps = parition(s, th);
                //adding parition to the index
                int m = 0;
                int start = 0;
                foreach (string p in ps)
                {
                    //get the invert list if exists; otherwise creates a new one
                    invertedList L = getList(m, s.Length);
                    L.add(p, i);
                    if (p != null)
                    {
                        L.length = p.Length;
                        L.start = start;
                        start = start + p.Length;
                    }
                    m++;
                }
                #endregion
                foreach (pair p in pairs)
                {
                   //   if (Lev.editdistance((string)words[p.i], (string)words[p.l], th) <= th)
                    Console.WriteLine(words[p.i] + " " + words[p.l]);
                }
            }
        }
        static ArrayList naive(string s, int th)
        {
            ArrayList W = new ArrayList();
            for (int l = 1; l <= s.Length; l++)
                for (int i = 0; i + l - 1 < s.Length; i++)
                {
                    W.Add(s.Substring(i, l));
                }
            return W;
        }
        public static ArrayList permute(ArrayList list)
        {
            ArrayList r = new ArrayList();
            int[] i = new int[list.Count];
            int l = 0;
            for (; ; )
            {
                r.Add(i.Clone());
                i[l]++;
                ArrayList pl = (ArrayList)list[l];

                if (i[l] >= pl.Count)
                {
                    for (; ; )
                    {
                        i[l] = 0;
                        l++;

                        if (l >= i.Length) return r;
                        pl = (ArrayList)list[l];
                        i[l]++;
                        if (i[l] < pl.Count) break;
                    }
                    l = 0;
                }
            }
        }
        public static ArrayList lengths(string s, int th)
        {
            ArrayList W = new ArrayList();
            ArrayList Lens = getlengths(s.Length, th);
            HashSet<int> lens = new HashSet<int>();
            foreach (ArrayList t in Lens)
            {
                foreach (int tt in t)
                    lens.Add(tt);
            }
            foreach (int tt in lens)
            {
                if (tt == 0) continue;
                for (int i = 0; i + tt - 1 < s.Length; i++)
                {
                    W.Add(s.Substring(i, tt));
                }
            }
            return W;
        }
        public static ArrayList oldlengths(string s, int th)
        {
            ArrayList lens = getlengths(s.Length, th);
            ArrayList perms = permute(lens);
            ArrayList W = new ArrayList();
            foreach (int[] i in perms)
            {
                int start = 0;
                for (int j = 0; j < i.Length; j++)
                {
                    ArrayList ls = (ArrayList)lens[j];
                    int len = (int)ls[i[j]];
                    W.Add(s.Substring(start, len));
                    start = start + len;
                }
            }
            return W;
        }
        public static ArrayList getlengths(int len, int th)
        {
            ArrayList ll = new ArrayList();
            for (int j = 0; j < th + 1; j++) ll.Add(new ArrayList());
            //get lengths of strings with length l
            for (int l = len - th; l <= len; l++)
            {
                int[] pil = parition(l, th);
                for (int j = 0; j < th + 1; j++)
                {
                    ArrayList m = (ArrayList)ll[j];
                    if (!m.Contains(pil[j]))
                    {
                        m.Add(pil[j]);
                    }
                }
            }
            return ll;
        }
        #endregion
    }
}
