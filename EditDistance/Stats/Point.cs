using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace EditDistance.Stats

{
    [DebuggerDisplay("{ds[0]}  {ds[1]}")]
    public class nPoint:IComparable //represents an n dimensioal point
    {
        int dim;
        public int[] ds;
        public nPoint(int []d)
        {
            dim = d.Length;
            ds = new int[dim];
            for (int i = 0; i < dim; i++)
            {
                ds[i] = d[i];
            }
        }
        public override bool Equals(object obj)
        {
            nPoint p = (nPoint)obj;
            if (p.dim != dim) return false;
            for (int i = 0; i < dim; i++)
                if (p.ds[i] != ds[i]) return false;

            return true;
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                for (int i = 0; i < dim; i++)
                    hash = hash * 16777619 ^ ds[i];
                return hash;
            }
        }
        public override string ToString()
        {
            string s="";
            if(ds!=null)
            for (int i = 0; i < ds.Length; i++)
                s += ds[i] + "\t";
            return s;
        }
        public int CompareTo(object obj)
        {
            int h=GetHashCode();
            int h2=obj.GetHashCode();
            return h.CompareTo(h2);
        }
        //return the difference to another point
        public int diff(nPoint x)
        {
            return Util.diff(x.ds, ds);
        }

        public int absdiff(nPoint x)
        {
            return Util.absdiff(x.ds, ds);
        }
    }
}

