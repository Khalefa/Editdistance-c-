using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using RTree;
using EditDistance.Passjoin;

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
        static void run(string alg, string dataset, int th, int eps, ArrayList words)
        {
            Global.dataset = dataset;
            Global.threshold = th;

            Global.epslion = eps;
            DateTime t = DateTime.Now;
            PairLong p = new PairLong(); ;

            if (alg == "P3J")
                p = passjoinIII.ComputeMatch(words, th, eps);
            else
                if (alg == "P2J")
                    passjoinII.ComputeMultiMatch(words, th, eps);
                else
                    if (alg == "MPJ")
                        p = passjoinIII.ComputeMyMatch(words, th, eps);
                    else if (alg == "HPJ")
                        p = passjoinIII.ComputeHistMatch(words, th, eps);

            Global.resut = p.first;
            Global.count = p.second;

            TimeSpan ts = DateTime.Now - t;
            Global.time = ts;
            Console.Write(Global.print());
        }
       public static void test_lev(){
           string a = "aaa";
           string b = "abc";
           int k = 0;
               k=Verification.Lev.lengthawarever (a, b, 2);
       }
        static void Main(string[] args)
        {
            test_lev();
            return;
            int th = 3;
            string[] filename = new string[10];
            string dir = @"C:\Users\khalefa\SkyDrive\Alex Work\Work\Edit Distance\datasets\";
            filename[0] = dir + "word.format";
            filename[1] = dir + "dblpall.format";
            filename[2] = @"c:\data\tiny.txt";
            filename[3] = @"c:\data\words.txt";
            filename[4] = dir + "word.format.1000";
            filename[5] = @"c:\data\paper.txt";
            int indx = 0;
            Global.exact = true;

            ArrayList words = readinput(filename[indx]);
            StreamWriter sw;
            if (File.Exists(dir + "r.txt"))
            {
                sw = new StreamWriter(dir + "r.txt", true);
                sw.WriteLine("--------------------------------------------");
            }
            else
            {
                sw = new StreamWriter(dir + "r.txt");
                sw.WriteLine(Global.header());
            }
            sw.AutoFlush = true;
            for (int e = 16; e >= 1; e = e / 2)
            {
                //run("P2J", filename[indx], 3, e, words);
                //sw.WriteLine(Global.print());
                run("HPJ", filename[indx], th, e, words);
                sw.WriteLine(Global.print());
                run("MPJ", filename[indx], th, e, words);
                sw.WriteLine(Global.print());
                run("P3J", filename[indx], th, e, words);
                sw.WriteLine(Global.print());
            }
            sw.Close();
        }
    }
}

/*  for (int e = 1; e < 9; e = e * 2)
  {
      run("PJ", filename[indx], 3, e, words);
      sw.WriteLine(Global.print());
  }*/

/*  //get number of grams
DateTime t1 = DateTime.Now;
Grams.Gram.GetGrams(words, 3);
TimeSpan ts1 = DateTime.Now - t1;
Console.Write(ts1);
*/
// PairWise.compute(words, 3);

//Radix.engine.run(words);
//ArrayList m= passjoin.getlengths(9, 6);
//passjoin.parition("sad", 1);
//passjoin.parition("sads", 1);
//ArrayList i= passjoin.permute(m);n


