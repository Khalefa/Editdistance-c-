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
            else if (alg == "P2J")
                passjoinII.ComputeMultiMatch(words, th, eps);
            else if (alg == "MPJ")
                p = passjoinIII.ComputeMyMatch(words, th, eps);
            else if (alg == "HPJ")
                p = passjoinIII.ComputeHistMatch(words, th, eps);
            else if(alg=="GJ")
                p=Grams.Grams.ComputeMatches(words, eps, th);
                
            Global.resut = p.first;
            Global.count = p.second;

            TimeSpan ts = DateTime.Now - t;
            Global.time = ts;
            Console.Write(Global.print());
        }
        public static void test_lev()
        {
            int k = 0;
            string a = "a";//caushik chakrabar";
            string b = "a_s";//kausic chakduri";

            k = Verification.Lev.lengthawareVer(a, b, 3);
            Console.WriteLine(k);
        }
        public static void GetStat(){
            Filenames();
            int indx = 1;
            ArrayList words = readinput(filename[indx]);
            StreamWriter sw = getfile("s.txt");
            sw.WriteLine(filename[indx]);
            sw.WriteLine(words.Count);
            Stats.stat.getstat(words, sw);
            sw.Close();
        }
        static string[] filename;
        static string data_dir;
        static void Filenames(){
            filename = new string[10];
           data_dir = @"C:\Users\khalefa\SkyDrive\Alex Work\Work\Edit Distance\datasets\";
            filename[0] = data_dir + "word.format";
            filename[1] = data_dir + "dblpall.format";
            filename[2] = @"c:\data\tiny.txt";
            filename[3] = @"c:\data\words.txt";
            filename[4] = data_dir + "word.format.1000";
            filename[5] = @"c:\data\paper.txt";
        }
        static void runGramExperiments()
        {
            StreamWriter sw = getfile("r_Grams.txt");
            

            int q = 4;
            int indx = 0;
            Global.exact = false;
            Filenames();
            ArrayList words = readinput(filename[indx]);
            sw.WriteLine("\t q\t th  \t cout \t time " );
            for (int x = 3; x < 10; x++)
            
            for (int t = 0; t < 10; t++)
            {
                DateTime tt = DateTime.Now;
                var cc=Grams.Grams.ComputeMatches(words, x, t);
                TimeSpan ts = DateTime.Now - tt;
                sw.WriteLine("Grams:\t" + x + "\t" + t + "\t" + cc+"\t"+ts);
            }
            sw.Close();
        }
        static void runExperiments()
        {
            int th = 2;
            int indx = 0;
            Global.exact = true;
            Filenames();
            ArrayList words = readinput(filename[indx]);
            StreamWriter sw = getfile("rV2.txt");
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
                run("GJ", filename[indx], th, e, words);
                sw.WriteLine(Global.print());
            }
            sw.Close();

        }
        static StreamWriter getfile(string f)
        {
            StreamWriter sw;
            if (File.Exists(data_dir + f))
            {
                sw = new StreamWriter(data_dir + f, true);
                sw.WriteLine("--------------------------------------------");
            }
            else
            {
                sw = new StreamWriter(data_dir + f);
                sw.WriteLine(Global.header());
            }
            sw.AutoFlush = true;
            return sw;
        }
        static void Main(string[] args)
        {
            //test_lev();
            // return;
            runExperiments();
            //GetStat();            
            
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


