using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EditDistance.Stats
{
    class stat
    {
        public static long get_comparsion(int th, int[] hist)
        {
            long cnt = 0;
            for (int i = 0; i < hist.Length; i++)
            {
                for (int i_ = i; i_ <= Math.Min(i + th + 3, hist.Length - 1); i_++)
                {
                    if (i_ == i) cnt += hist[i] * (hist[i_] - 1) / 2;
                    else
                        if (i_ - i <= th)
                            cnt += hist[i] * hist[i_];
                }
            }
            return cnt;
        }

        public static Hashtable buildquadtree(List<string> words, int d)
        {
            //iterate throughut the words and add 
            Hashtable ht = new Hashtable(); // use hash table as the extent is not known
            foreach (string s in words)
            {
                nPoint dim = Util.getnPoint(s, d);
                if (ht.Contains(dim))
                {
                    List<string> l = (List<string>)ht[dim];
                    l.Add(s);
                }
                else
                {
                    List<string> l = new List<string>();
                    l.Add(s);
                    ht.Add(dim, l);
                }
            }
            return ht;
        }
        static long[] cnt(Hashtable ht, int th)
        {
            //now pairwise compare them
            long[] cnt = new long[th];
            int ii, jj;
            ii = 0;

            foreach (DictionaryEntry e in ht)
            {
                nPoint p = (nPoint)e.Key;
                ii++;
                jj = 0;
                List<string> v = (List<string>)e.Value;
                long tmp = v.Count * (v.Count - 1) / 2;
                for (int cc = 0; cc < th; cc++)
                    cnt[cc] += tmp;
                //sw.WriteLine(p.ToString() +"\t"+v.Count);
                foreach (DictionaryEntry e2 in ht)
                {
                    nPoint p2 = (nPoint)e2.Key;
                    jj++;
                    if (p.CompareTo(p2) != 1) continue;
                    List<string> v2 = (List<string>)e2.Value;
                    long tmp2 = v.Count * v2.Count;
                    int d1 = p.diff(p2);
                    int d2 = p.absdiff(p2);
                    for (int cc = 0; cc < th; cc++)
                    {
                        if (d1 <= 2 * cc && d2 <= cc)
                        {
                            cnt[cc] += tmp2;
                        }
                    }
                    //else
                    //   cnt = cnt;
                }
            }
            return cnt;
        }
        public static void get_error(ArrayList words)
        {
            foreach (string s in words)
            {
                nPoint p = Util.getnPoint(s, 2);
                word w = new word(s);

                if (p.ds[0] != w.getlen2() || (p.ds[0] + p.ds[1]) != w.getlen())
                {
                    Console.WriteLine(s);
                }

            }
        }
        public static long get_comparsion(StreamWriter sw, int th, int[,] hist)
        {
            throw new Exception("bugy");
            long cnt = 0;
            for (int i = 0; i < hist.GetLength(0); i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if (hist[i, j] == 0) continue;
                    cnt += (hist[i, j] * (hist[i, j] - 1) / 2);
                    sw.WriteLine("-cs" + i + "\t" + j + "\t" + hist[i, j]);
                    for (int i_ = i; i_ < hist.GetLength(0); i_++)
                    {
                        for (int j_ = j; j_ < hist.GetLength(1); j_++)
                            if (hist[i_, j_] == 0) continue;
                            else
                            {
                                /*if (i == i_ && j == j_)
                                {
                                    //cnt += (hist[i, j] * (hist[i, j] - 1) / 2);
                                    sw.WriteLine("-cs" + i + "\t" + j +"\t"+hist[i,j]);
                                }
                                else*/
                                if (i_ - i <= th)
                                {
                                    int z_s = i + 0 - j;
                                    int z_s2 = i_ + 0 - j_;
                                    int diff = (Math.Abs(z_s2 - z_s) + i_ - i);
                                    if (diff <= 2 * th)
                                    {
                                        cnt += hist[i, j] * hist[i_, j_];
                                        sw.WriteLine("-c" + i + "\t" + j + "\t\t " + i_ + "\t" + j_ + "\t" + hist[i, j] + "\t" + hist[i_, j_]);
                                    }
                                }
                            }
                    }
                }
            }
            return cnt;
        }
        public static void oldstat(StreamWriter sw)
        {
            int max_length, min_length;
            ArrayList words = new ArrayList();
            max_length = 4;
            min_length = 2;
            int[,] h2 = new int[max_length - min_length + 1, max_length + 1];
            ArrayList[,] hh = new ArrayList[max_length - min_length + 1, max_length + 1];
            foreach (string s in words)
            {
                word w = new word(s);
                if (h2[s.Length - min_length, w.getlen2()] == 0)
                {
                    hh[s.Length - min_length, w.getlen2()] = new ArrayList();
                }
                h2[s.Length - min_length, w.getlen2()]++;
                hh[s.Length - min_length, w.getlen2()].Add(s);
            }

            #region too much printing
            if (false)
            {
                for (int i = 0; i < h2.GetLength(0); i++)
                {
                    sw.Write(i + min_length + "\t");
                    for (int j = 0; j <= i + 1; j++)
                        sw.Write(h2[i, j] + "\t");
                    sw.WriteLine();
                }
            }
            #endregion

        }
        static int[] gethist_onedim(ArrayList words, StreamWriter sw)
        {
            int min_length = ((string)words[0]).Length;
            int max_length = ((string)words[words.Count - 1]).Length;
            int[] hist = new int[max_length - min_length + 1];
            foreach (string s in words)
            {
                hist[s.Length - min_length]++;
            }
            for (int i = 0; i < hist.Length; i++)
            {
                sw.WriteLine(min_length + i + "\t" + hist[i]);
            }
            return hist;
        }
        static Hashtable gethist_ndim(ArrayList words, StreamWriter sw, int n)
        {
            List<string> ws = new List<string>();
            foreach (string s in words)
                ws.Add(s);

            //
            sw.WriteLine("---------------&&&&&&&&&&&&&&&&&&&&-------------------------------------------------------");
            sw.WriteLine("hash for " + Math.Pow(2, n));
            Hashtable htt = buildquadtree(ws, n);
            foreach (DictionaryEntry e in htt)
            {
                List<string> v = (List<string>)e.Value;
                sw.WriteLine(e.Key.ToString() + "\t" + v.Count);
            }
            return htt;
        }
        public static void getstat(ArrayList words, StreamWriter sw)
        {

            //build hist of lengeths
            words.Sort(new StringComparer());
            /*
             * 
             * int []hist=gethist_onedim(words,sw);
            Hashtable htt=gethist_ndim(words,sw,3);
            
            long []cnts = cnt(htt, 10);
            for (int t = 1; t < 10; t++)
            {                
                sw.WriteLine("th:" + t + "\t" + get_comparsion(t, hist) + "\t" + cnts[t-1]);
                
            }
            sw.WriteLine("---------------&&&&&&&&&&&&&&&&&&&&-------------------------------------------------------");
            */
            sw.WriteLine("Prefix selectivety");
            Hashtable ht = prefix.getprefix(words, 4);
            foreach (var x in ht.Keys)
            {
                sw.WriteLine(x + "\t" + ht[x]);
            }
            ArrayList ar = new ArrayList(ht.Keys);
            DateTime t = DateTime.Now;
           
            long c = 0;
            for (int i=0; i<ar.Count;i++ )
            {
                string s = (string)ar[i];
                
                for (int j=i+1; j<ar.Count;j++)
                {
                    string w = (string)ar[j];
                    int tt = Verification.Lev.editdistance(s, w, 4);
                    if (tt <= 4)
                    {
                        //arr.Add(new object[2] { s, w });
                        c += (int)ht[s] * (int)ht[w];
                    }
                }
            }
            TimeSpan ts=DateTime.Now - t;
            Console.WriteLine(ts);
            sw.WriteLine(ts);
            //Hashtable ht = Grams.Grams.GetCountGrams(words, 4);
            //  sw.WriteLine("gram " + 4 + "count " + ht.Count);

            // get_error(words);
        }
    }
}

