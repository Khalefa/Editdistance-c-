using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using RTree;


namespace EditDistance
{
    class Program
    {
        static ArrayList readinput(string file)
        {
            HashSet<string> words = new HashSet<string>();

            StreamReader r = new StreamReader(file);
            while (!r.EndOfStream)
            {
                words.Add(r.ReadLine().ToLower());
            }
            r.Close();
            return new ArrayList(words.ToArray<string>());
        }
        
       
       
        static void hashIndex(string s)
        {

        }
    
   
        static void Main(string[] args)
        {
            string s = "beautiful";
            int[] hist = Util.hist(s);
            int[] m0 = Util.summarize_hist(hist, 0);
            int[] m1 = Util.summarize_hist(hist, 1);
            int[] m2 = Util.summarize_hist(hist, 2);
            int[] m3 = Util.summarize_hist(hist, 3);
            int[] m4 = Util.summarize_hist(hist, 4);


           


        }
    }
}

