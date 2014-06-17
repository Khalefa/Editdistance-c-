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
            ArrayList words = readinput(filename[0]);
            DateTime t = DateTime.Now;
            passjoin.ComputeMultiMatch (words, 4);
            TimeSpan ts = DateTime.Now - t;
            Console.Write(ts);
            //Radix.engine.run(words);
            //ArrayList m= passjoin.getlengths(9, 6);
            //passjoin.parition("sad", 1);
            //passjoin.parition("sads", 1);
            //ArrayList i= passjoin.permute(m);
        }
    }
}

