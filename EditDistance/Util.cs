using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    public class Util
    {

        static bool isalpha(char c)
        {
            if ((c >= 'a') && (c <= 'z')) return true;
            return false;
        }

        static int ALPHABET_SIZE = 27;
        public static int[] hist(string s)
        {
            int[] h = new int[ALPHABET_SIZE];
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (isalpha(c))
                {
                    h[c - 'a']++;
                }
                else
                    h[ALPHABET_SIZE - 1]++;
            }

            return h;
        }
        public static int[] summarize_hist(int[] hist, int l)
        {
            int x;
            x = 0;
            
            double d = Math.Pow(2, l );
            if (d > ALPHABET_SIZE) d = ALPHABET_SIZE;
            double range = Math.Ceiling(ALPHABET_SIZE / d);
            int r = (int)range;
            int n = (int)(d);

            int[] a = new int[n];
            int j = 0;
            int k = 0;
            for (int i = 0; i < ALPHABET_SIZE; i++)
            {
                x += hist[i];
                j++;
                if ((j >= r) || (i == ALPHABET_SIZE - 1))
                {
                    j = 0;
                    a[k++] = x;
                    x = 0;
                }
            }
            return a;   
        }
        static public int[] getPoint(string s,int l)
        {
            int[] ht = hist(s);
            int x, y;
            x = y = 0;
            int[] ar = new int[2];
           

            double d = Math.Pow(2, l + 1);
            if (d > ALPHABET_SIZE) d = Math.Pow(2, Math.Ceiling(Math.Log( ALPHABET_SIZE,2)));
            double range = Math.Ceiling(ALPHABET_SIZE / d);
            int r = (int)range;

            for (int i = 0; i < r; i++)
            {
                x += ht[i];
                y += ht[ALPHABET_SIZE / 2 + i];
            }
            ar[0] = x;
            ar[1] = y;
            return ar;
        }
        public static string[] grams(string s, int q)
        {
            string []str=new string[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                string ss="";
                for (int g = i; g < Math.Min(q, s.Length); g++)
                    ss += s[g];
                str[i] = ss;

            }
            return str;
        }
    }
}
