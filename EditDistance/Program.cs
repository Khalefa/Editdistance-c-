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

        static void Main(string[] args)
        {
            string[] filename = new string[10];
            string dir=@"C:\Users\khalefa\SkyDrive\Alex Work\Work\Edit Distance\datasets\";
            filename[0] = dir+"word.format";
            filename[1] = dir+"dblpall.format";
            filename[2] = @"c:\data\tiny.txt";
            filename[3] = @"c:\data\words.txt";
            filename[4] = dir + "word.format.1000";
            filename[5] = @"c:\data\paper.txt";
            int indx = 0;
            ArrayList words = readinput(filename[indx]);
            //get number of grams
          /*  DateTime t1 = DateTime.Now;
            Grams.Gram.GetGrams(words, 2);
            TimeSpan ts1 = DateTime.Now - t1;
            Console.Write(ts1);
            */
            for (int e = 1; e < 8; e++)
            {
                DateTime t = DateTime.Now;
                passjoinII.ComputeMultiMatch(words, 3, e);
                //passjoinII.ComputeMultiMatch_old (words, 3);
                TimeSpan ts = DateTime.Now - t;
                Console.Write(ts);
            }
            //Radix.engine.run(words);
            //ArrayList m= passjoin.getlengths(9, 6);
            //passjoin.parition("sad", 1);
            //passjoin.parition("sads", 1);
            //ArrayList i= passjoin.permute(m);n
        }
    }
}

