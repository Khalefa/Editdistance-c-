﻿using System;
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
        static RTree<string>[] rts;
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
        static void run()
        {
            ArrayList words = readinput("c:\\data\\web2.txt");
            double ec = words.Count;
            DateTime st = DateTime.Now;
            Hashtable ht = new Hashtable();
            foreach (string w in words)
            {

                foreach (string ws in Util.grams(w, 3))
                {
                    if (!ht.ContainsKey(ws))
                    {
                        ArrayList l = new ArrayList();
                        l.Add(w);
                        ht.Add(ws, l);
                    }
                    else
                    {
                        ArrayList l = (ArrayList)ht[ws];
                        l.Add(w);
                        ht[ws] = l;
                    }
                }
                SpatialIndex((string)w);

            }


            //SpatialIndex((string)words[111]);
            DateTime t0 = DateTime.Now;
            TimeSpan ts = t0 - st;
            //List<pair> pps= rts[0].joins(rts[0], 1);
            DateTime t1 = DateTime.Now;
            TimeSpan ts1 = t1 - t0;
            int ii = 0;
            foreach (String w in words)
            {
                DateTime tt0 = DateTime.Now;
                List<Rectangle> rects = getPoint(w, 1, 1);
                HashSet<string> allwords = new HashSet<string>();
                foreach (string s in words) allwords.Add(s);
                int i = 0;
                foreach (Rectangle r in rects)
                {
                    List<string> objects = rts[i].Intersects(r);
                    i++;
                    HashSet<string> o = new HashSet<string>();
                    foreach (string s in objects) { o.Add(s); }
                    allwords.IntersectWith(o);
                }
                TimeSpan tts1 = DateTime.Now - tt0;
                Console.WriteLine(w + " :" + allwords.Count + " " + tts1);
                ii++;
                if (ii > 100) break;
            }
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t1;

            Console.WriteLine(ts);
            Console.WriteLine(ts1);
            Console.WriteLine(ts2);
        }
        static void hashIndex(string s)
        {

        }
        static List<Rectangle> getPoint(string w, int j, int d)
        {
            List<Rectangle> pnts = new List<Rectangle>();
            int[] hist = Util.hist(w);
            int[] m = Util.summarize_hist(hist, j);

            for (int i = 0; i < m.Length / 2; i++)
            {
                Point p = new Point(m[i], m[i + m.Length / 2]);
                Rectangle r = new Rectangle(p);

                pnts.Add(r.extend(d));
            }
            return pnts;
        }
        static void SpatialIndex(string w)
        {
            int[] hist = Util.hist(w);
            ArrayList a = new ArrayList();
            //a.Add(0);
            //a.Add(1);
            //a.Add(2);
            a.Add(1);
            foreach (int j in a)
            {
                int[] m = Util.summarize_hist(hist, j);

                for (int i = 0; i < m.Length / 2; i++)
                {
                    rts[i].Add(new Rectangle(new Point(m[i], m[i + m.Length / 2])), w);
                }
            }


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


            rts = new RTree<string>[10];
            for (int i = 0; i < 10; i++) rts[i] = new RTree<string>();
            run();


        }
    }
}

