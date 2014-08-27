using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;

namespace EditDistance
{
    [DebuggerDisplay("'{first}' '{second}'")]
    public class PairLong
    {
        public long first;
        public long second;
        public PairLong(long f, long s)
        {
            first = f;
            second = s;
        }
        public static PairLong operator +(PairLong c1, PairLong c2)
        {
            return new PairLong(c1.first + c2.first, c1.second + c2.second);
        }
        public PairLong()
        {
            first = 0;
            second = 0;
        }
        public override string ToString()
        {
            return first + "\t" + second;
        }
    }
    
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
        public Dictionary<string, List<int>> ht = new Dictionary<string, List<int>>();
        public int length;
        public int start;
        public void del()
        {
            ht = null;
        }

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
    class WordComparer : IComparer
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

}

