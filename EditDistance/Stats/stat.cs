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
        public static long get_comparsion(int th,int [] hist)
        {
            long cnt = 0;
            for (int i = 0; i < hist.Length; i++)
            {
                for(int i_=i;i_<=Math.Min( i+th,hist.Length-1);i_++)
                {
                    if (i_ == i) cnt += hist[i] *( hist[i_] - 1)/2;
                    else
                        if(i_-i<=th)
                        cnt += hist[i] * (hist[i_] );
                }
            }
            return cnt;
        }
        public static long get_comparsion(int th, int[,] hist, int min_length)
        {
            long cnt = 0;
            for (int i = 0; i < hist.GetLength(0); i++)
            {
                 for (int j = 0; j <=i; j++)
                {
                    if(hist[i, j]==0)continue;
                    for (int i_ = i; i_ <= Math.Min( i+2*th,hist.GetLength(0)-1); i_++)
                    {
                        for (int j_ = j; j_ <= Math.Min(j +2* th, hist.GetLength(1) - 1); j_++)
                            if (hist[i_, j_] == 0) continue;
                            else
                            {
                                if (i == i_ && j == j_)
                                    cnt += (hist[i, j] * (hist[i, j] - 1) / 2);
                                else                                
                                        if (i_ - i <= th)
                                        {
                                            int z_s = i + min_length - j;
                                            int z_s2 = i_ + min_length - j_;
                                            int diff = (Math.Abs(z_s2 - z_s) + i_ - i) / 2;
                                            if (diff <= th)
                                                cnt += hist[i, j] * hist[i_, j_];
                                        }
                            }
                    }
                }
            }
            return cnt;
        }

        public static void getstat(ArrayList words, StreamWriter sw)
        {

            //build hist of lengeths
            words.Sort(new StringComparer());
            int min_length = ((string)words[0]).Length;
            int max_length = ((string)words[words.Count-1]).Length;
            int[] hist = new int[max_length - min_length + 1];
            foreach (string s in words)
            {
                hist[s.Length - min_length]++;
            }
            for (int i = 0; i < hist.Length; i++)
            {
                sw.WriteLine(min_length+i+"\t"+hist[i]);
            }
            
            int [,]h2= new  int[max_length-min_length+1,max_length+1];
            foreach (string s in words)
            {
                word w = new word(s);

                h2[s.Length-min_length,w.getlen2()]++;
            }
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
            //
            sw.WriteLine("----------------------------------------------------------------------");
            for(int t=1;t<10;t++)
                sw.WriteLine("th:"+t+"\t"+get_comparsion(t,hist) +"\t"+get_comparsion(t,h2,min_length));
             
 
            Hashtable ht=Grams.Grams.GetCountGrams(words, 4);
            sw.WriteLine("gram "+4+"count "+ht.Count);
            foreach (var x in ht.Keys)
            {
                sw.WriteLine(x+"\t"+ht[x]);
               
            }

        }
    }
}

