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
        static void run(string dataset,int th, int eps,ArrayList words){
            Global.dataset = dataset;
            Global.threshold = th;
            
            Global.epslion = eps;
            DateTime t = DateTime.Now;
            PairLong p= passjoinIII.ComputeMatch(words, th,eps);
            Global.resut = p.first;
            Global.count = p.second;
            //passjoinII.ComputeMultiMatch_old (words, 3);
            TimeSpan ts = DateTime.Now - t;
            Global.time = ts;
            Console.Write(Global.print());
        }
        static void Main(string[] args)
        {
            string[] filename = new string[10];
            string dir = @"C:\Users\khalefa\SkyDrive\Alex Work\Work\Edit Distance\datasets\";
            filename[0] = dir + "word.format";
            filename[1] = dir + "dblpall.format";
            filename[2] = @"c:\data\tiny.txt";
            filename[3] = @"c:\data\words.txt";
            filename[4] = dir + "word.format.1000";
            filename[5] = @"c:\data\paper.txt";
            int indx = 0;
            Global.exact = false;
            ArrayList words = readinput(filename[indx]);
            StreamWriter sw;
            if (Directory.Exists(dir + "r"))
            {
                sw = new StreamWriter(dir + "r", true);
            }
            else
            {
                sw = new StreamWriter(dir + "r");
                sw.WriteLine(Global.header());
            }
            for (int e = 1; e < 9; e=e*2)
            {
                run(filename[indx], 3, e, words);
                sw.WriteLine(Global.print());
            }
            sw.Close();
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
        }
    }
}

