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
      public class passjoinIV
    {
        public static Hashtable invertedlists = new Hashtable();//stores the inverted listes the keu is a composite of length and 
        public static Hashtable invlists = new Hashtable();
        public static Hashtable invlist_length = new Hashtable();        
        public static string[] parition(string s, int th, int eps)
        {
            string[] a = new string[th + eps];
            for (int i = 0; i < th + eps; i++)
            {
                a[i] = "";
            }
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
        static int adjust_len(string s, int th, int eps)
        {
            if (s.Length >= th + eps)
            {
                return eps;
            }
            else if (s.Length < th + 1)
            {
                //append the difference
                return 1;
            }
            else
            {
                //reduce eps to  work fine
                return (s.Length) - th;
            }
            
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
        static public PairLong getMatches(string s, int j, Hashtable ht,int th,int eps, int[] matches, int[] indx, ref  ArrayList words)
        {
            long rcnt = 0;
            long candidta_cnt = 0;
                
            for (int l = s.Length - th; l <= s.Length; l++)
            {
                int delta = s.Length - l;
                for (int i = 0; i < th + 1; i++)
                {
                    invertedList L = GetList(ht, i, l);
                    if (L == null) continue;
                    if (L.length == 0) continue;
                    int pi = L.start;
                    //iterate throw
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
                                    matches[x] ++;
                                    
                                } else {
                                
                                    indx[x] = j;
                                    matches[x] = 1;

                                }
                                if(indx[x]==j && matches[x]==eps){
                                    candidta_cnt++;
                                    if (Global.exact)
                                    {                                        
                                        int t = Verification.Lev.editdistance2((string)words[j], (string)words[x], th, k, pi, L.length);
                                        if (t <= th)  rcnt++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return new PairLong(rcnt,candidta_cnt);
        }
        static public PairLong ComputeMatch(ArrayList words, int th, int eps = 1)
        {
            PairLong p = new PairLong();
            Global.alg = "P4J";
            int[] matches_arr = new int[words.Count];
            int[] indx = new int[words.Count];

            words.Sort(new WordComparer());

            int progress = (int)Math.Ceiling(words.Count / 100.0);
            int len = 0;
            for (int j = (int)(0); j < words.Count; j++)
            {
                int e = eps;
                if (j % progress == 0)
                {
                    Console.Write(".");
                }

                string s = (string)words[j];
                string ss = s;
                adjust_string(s, ref ss, th, ref e);
                if (s.Length != len)
                {
                    if (s.Length > th)
                    {
                        cleanList(s.Length - th - 1);
                    }
                }
                p = p + getMatches(ss,j,invlists, th,e, matches_arr, indx, ref words);
                #region parition
                string[] ps1 = parition(ss, th, e);
                //adding parition to the index
                AddPart(invlists, s, j, ps1);
                #endregion
                len = s.Length;
            }
            invlists = new Hashtable();
            return p;
        }
        static public void getMatches_noreturn(int th, string s, Hashtable ht, int j, int[] matches, int[] indx)
        {
            int epslion = 1;
            for (int l = Math.Max(0,s.Length - th); l <= s.Length; l++)
            {
                int delta = s.Length - l;
                for (int i = 0; i < Math.Min(th + epslion,l); i++)
                {
                    invertedList L = GetList(ht, i, l);
                    if (L == null) continue;
                    if (L.length == 0) continue;
                    int pi = L.start;

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
                                indx[x] = j;
                                matches[x] = 0;
                            }
                        }
                    }
                }
            }
        }
        static public List<int> getMatches_withmemo(int epslion, int th, string s, Hashtable ht, int j, int[] matches, int[] indx, ArrayList words)
        {
            List<int> m = new List<int>();

            for (int l = Math.Max(s.Length - th,1); l <= s.Length; l++)
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
            //find the wrong matched
            /*if(j>0)
            for (int i = 0; i < indx.Length; i++)
            {
                if((indx[i]==j) && (matches[i]<epslion) ){
                    if (Lev.editdistance((string)words[i], (string)words[j], th) <=th)
                
                    Console.WriteLine("aa");
                }
            }*/
            return m;
        }
        static public PairLong ComputeMyMatch(StreamWriter sw, ArrayList words, int th, int eps = 1)
        {
            Global.alg = "MPJ4";
            long rcnt = 0;
            long candidta_cnt = 0;
            int[] matches_arr = new int[words.Count];
            int[] indx = new int[words.Count];
            invlists = new Hashtable();
            invertedlists = new Hashtable();

            for (int i = 0; i < words.Count; i++)
            {
                string s = (string)words[i];
                string ss = s;
                int e = eps;
                adjust_string(s, ref ss, th, ref e);
                words[i] = ss;
            }
            words.Sort(new WordComparer());

            int progress = (int)Math.Ceiling(words.Count / 10.0);
            for (int j = (int)(0); j < words.Count; j++)
            {
                string s = (string)words[j];
                if (j % progress == 0)
                {
                    Console.Write(".");
                }

                int e = adjust_len(s, th, eps);
                
                getMatches_noreturn(th, s, invlists, j, matches_arr, indx);
                List<int> l = getMatches_withmemo(e, th, s, invertedlists, j, matches_arr, indx,words);
                candidta_cnt += l.Count;
                if (Global.exact)
                {
                    foreach (int p in l)
                    {
                        if (Lev.editdistance(s.Trim(), (string)words[p], th) <= th)
                        {
                            sw.WriteLine(eps+"\t"+ s + "\t" + (string)words[p]);
                            rcnt++;
                        }
                    }
                }
                #region parition
                string[] ps1 = parition(s, th, 1);
                AddPart(invlists, s, j, ps1);
                string[] ps = parition(s, th, e);
                AddPart(invertedlists, s, j, ps);
                #endregion
            }
            return new PairLong(rcnt, candidta_cnt);
        }
   }
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
