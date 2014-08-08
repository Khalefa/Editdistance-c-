using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{

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
            // TODO: Complete member initialization
        }
    }
    
}
