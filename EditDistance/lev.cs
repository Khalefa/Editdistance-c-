﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EditDistance;
namespace Verification
{
    class Lev
    {
        public static int editdistance(string a, string b, int limit)
        {
            return Leditdistance(a, b, limit);
        }
        public static int editdistance_old(string a, string b, int limit)
        {
            Global.ver_alg = "V";
            int oo = limit+10;
            if (a.Length > b.Length)
            {
                string t = a;
                a = b;
                b = t;
            }
            int na = a.Length;
            int nb = b.Length;
            if (b.Length < limit) return 0;
            int diff = b.Length - a.Length;

            int[,] T = new int[2, b.Length + 1];

            int ia, ib;

            int cur = 0;
            ia = 0;

            for (ib = 0; ib <= nb; ib++)
                T[cur, ib] = ib;

            cur = 1 - cur;

            int stop_b = limit;
            for (ia = 1; ia <= na; ia++)
            {
                for (ib = 0; ib <= nb; ib++)
                    T[cur, ib] = oo;
                int ib_st = 0;
                stop_b++;
                int ib_en = Math.Min(stop_b, nb);

                if (ib_st == 0)
                {
                    ib = 0;
                    T[cur,ib] = ia;
                    
                }
                ib_st++;
                int min = oo;
                for (ib = ib_st; ib <= ib_en; ib++)
                {
                    int ret = oo;

                    int d1 = T[1 - cur,ib] + 1;
                    int d2 = T[cur,ib - 1] + 1;
                    int d3 = T[1 - cur,ib - 1];
                    if (a[ia - 1] != b[ib - 1]) d3++;

                    if (d1 < ret) ret = d1;
                    if (d2 < ret) ret = d2;
                    if (d3 < ret) ret = d3;
                    if (ret < min) min = ret;
                    T[cur,ib] = ret;
                }
                if (min > limit) 
                    return limit + 1;
                cur = 1 - cur;
            }
            return  T[1 - cur,nb];
        }
        public static int Leditdistance(string a, string b, int limit)
        {
            Global.ver_alg = "Ledit";
            int oo = limit + 10;
            
            if (a.Length > b.Length)
            {
                string t = a;
                a = b;
                b = t;
            }
            
            int na = a.Length;
            int nb = b.Length;
            if (b.Length < limit) return 0;
            
            int diff = b.Length - a.Length;
            if (diff > limit)
            {
                new Exception("aa");
            }
            int[,] T = new int[2, b.Length + 1];

            int ia, ib;

            int cur = 0;
            ia = 0;

            for (ib = 0; ib <= nb; ib++)
                T[cur, ib] = ib;

            cur = 1 - cur;

            int stop_b = (limit+diff)/2;//(limit+th)/2
            for (ia = 1; ia <= na; ia++)
            {
                for (ib = 0; ib <= nb; ib++)
                    T[cur, ib] = oo;
                int ib_st = 0;
                stop_b++;
                int ib_en = Math.Min(stop_b, nb);
                ib_st = ia - (limit - diff) / 2;
                if (ib_st <= 0)
                {                    
                    T[cur, 0] = ia;
                }
                ib_st++;
                int min = oo;
                for (ib = ib_st; ib <= ib_en; ib++)
                {
                    int ret = oo;

                    int d1 = T[1 - cur, ib] + 1;
                    int d2 = T[cur, ib - 1] + 1;
                    int d3 = T[1 - cur, ib - 1];
                    if (a[ia - 1] != b[ib - 1]) d3++;

                    if (d1 < ret) ret = d1;
                    if (d2 < ret) ret = d2;
                    if (d3 < ret) ret = d3;
                    
                    T[cur, ib] = ret;
                    ret = ret + Math.Abs(b.Length - ib - a.Length + ia);
                    if (ret < min) min = ret;
                }
                if (min > limit)
                    return limit + 1;
                cur = 1 - cur;
            }
            return T[1 - cur, nb];
        }
        static int min(int a, int b, int c)
        {
            int d=a;
            if (a > b) d = b;
            if (d > c) return c;
            return d;
        }
        //static public int[,] M;// = new int[r.Length + 1, s.Length + 1];
        static public int lengthawareVer(string r, string s, int th)
        {
            Global.ver_alg = "LengthwareVer";
            if (r.Length > s.Length)
            {
                string t = r;
                r = s;
                s = t;
            }
            if (s.Length < th) return 0;

            //int max_len = ts.Length;
            int[,] M = new int[r.Length + 1, s.Length + 1];
            
            /*for (int i = 0; i < r.Length + 1; i++)
                for (int j = 0; j < s.Length + 1; j++)
                    M[i, j] = th + 10;*/
            int delta = s.Length - r.Length;
            if (delta > th)
            {
                    new Exception("aa");
            }
            
            for (int i = 0; i <=(th+delta) /2 ; i++)
            {
                M[0, i] = i;                
            }
            
            for (int i = 1; i <= r.Length; i++)
            {
                int st=i-(th-delta)/2;
                if(st<1)st=1;
                int en = i + (th + delta) / 2;
                if (en > s.Length) en = s.Length;
                int e_ij=th+10;
                M[i, st - 1] = 100+th;
                for (int j = st; j <= en; j++)
                {
                    int d=0;
                    if (r[i-1]!=s[j-1]) d=1;
                    M[i, j] = min(M[i-1, j]+1, M[i, j-1]+1, M[i-1, j-1 ] + d);
                    int t=M[i,j]+(s.Length-j-r.Length+i);
                    if (e_ij > t)
                        e_ij = t;
            
                   // Console.WriteLine("M[" + i + "," + j + "]=" + M[i, j]+"t "+t);
                }
                if(e_ij>th)return th+1;

            }
            return M[r.Length,s.Length];
        }
        public static int editdistance0(string x, string y, int th)
        {
            int xl = x.Length;
            int yl = y.Length;
            if (x.Length > y.Length)
            {
                string t = y;
                y = x;
                x = t;
            }
            int diff = y.Length - x.Length;

            if (diff > th)
            {
                    new Exception("aa");
                
            }

            int[,] m = new int[x.Length + 1, y.Length + 1];
            //initalization
            for (int i = 0; i < xl; i++)
            {
                m[i, 0] = i;
            }
            for (int j = 0; j < yl; j++)
            {
                m[0, j] = j;
            }
            //D[i,j]=min (D[i,j-1]+1, D 

            for (int i = 1; i <= xl; i++)
            {
                for (int j = 1; j <= yl; j++)
                {
                    int min = 1000;
                    if (x[i - 1] == y[j - 1])
                        min = m[i - 1, j - 1];
                    else
                        min = m[i - 1, j - 1] + 1;
                    //check 
                    if (min > m[i - 1, j] + 1)
                    {
                        min = m[i - 1, j] + 1;
                    }
                    if (min > m[i, j - 1] + 1)
                    {
                        min = m[i, j - 1] + 1;
                    }
                    m[i, j] = min;
                }
            }
            return m[xl, yl];
        }
    }

}
