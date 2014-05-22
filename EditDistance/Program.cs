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

        static void Main(string[] args)
        {
            string []filename = new string[10];
            filename[0]= @"C:\Users\khalefa\SkyDrive\Alex Work\Work\Edit Distance\datasets\dblpall.format";
            filename[1] = @"c:\data\tiny.txt";
             filename[2]=@"c:\data\words.txt";
            ArrayList words = readinput(filename[2]);
            Radix.engine.run(words);

        }
    }
}

