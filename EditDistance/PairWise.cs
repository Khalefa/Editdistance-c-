using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verification;
namespace EditDistance
{
    class PairWise
    {
        static public void compute(ArrayList words, int thr)
        {
            for (int i = 0; i < words.Count; i++)
            {
                for (int j = i + 1; j < words.Count; j++)
                {
                    int l = Lev.editdistance((string)words[i], (string)words[j], thr);
                    if (l <= thr)
                    {
                        Console.WriteLine((string)words[i], (string)words[j], l);
                    }
                }
            }
        }
    }
}
