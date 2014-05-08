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
        static RTree<string>[] rts;
        public static int level = 6;
        public static bool exact = true;
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
        static void grams(ArrayList words, int q)
        {
            Hashtable ht = new Hashtable();
            foreach (string w in words)
            {
                foreach (string ws in Util.grams(w, q))
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
            
            }
            foreach (string w in words)
            {
                long c = 0;
                foreach (string ws in Util.grams(w, q))
                {
                    ArrayList l = (ArrayList)ht[ws];
                    c += l.Count;
                }
                Console.WriteLine(w + " " + c);
            }
            
        }
        static void run()
        {
            ArrayList words = readinput("c:\\data\\web2.txt");
            double ec = words.Count;
            DateTime st = DateTime.Now;
            grams(words,3);
            
            DateTime t = DateTime.Now;
            TimeSpan ts = t - st;

             t = DateTime.Now;
            //long word_c = searchRtrees(words);
            DateTime t2 = DateTime.Now;
            TimeSpan ts2 = t2 - t;
            Console.WriteLine("level " + level + " exact " + exact);
            Console.WriteLine(ts);
            //Console.WriteLine(ts1);
            Console.WriteLine(ts2);
           // Console.WriteLine("Count " + word_c);
        }
        static long searchRtrees(ArrayList words)
        {
            long word_c = 0;
            //SpatialIndex((string)words[111]);
            int ii = 0;
            foreach (String w in words)
            {
                DateTime tt0 = DateTime.Now;
                List<Rectangle> rects = getPoint(w, 1);
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
                word_c += allwords.Count;
                ii++;
                if (ii > 10000) break;
            }
            return word_c;
        }
        static void hashIndex(string s)
        {

        }
        static List<Rectangle> getPoint(string w, int d)
        {
            List<Rectangle> pnts = new List<Rectangle>();
            int[] hist = Util.hist(w);


            for (int i = 0; i <= level; i++)
            {
                int[] m = Util.summarize_hist(hist, i);
                Point p = null;
                if (exact)
                    p = new Point(m, m.Length);
                else

                    p = new Point(m[0], m[0 + m.Length / 2]);
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
            for (int i = 0; i <= level; i++)
                a.Add(i);
            //a.Add(2);
            foreach (int j in a)
            {
                int[] m = Util.summarize_hist(hist, j);
                //rts[j].Add(new Rectangle(new Point(m[0], m[0 + m.Length / 2])), w);
                // rts[j].Add(new Rectangle(new Point(m,m.Length)), w);
                //   for (int i = 0; i < m.Length / 2; i++)
                {
                    if (exact)
                        rts[j].Add(new Rectangle(new Point(m, m.Length)), w);
                    else
                        rts[j].Add(new Rectangle(new Point(m[0], m[0 + m.Length / 2])), w);
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


            rts = new RTree<string>[32];
            for (int i = 0; i < 32; i++) rts[i] = new RTree<string>(100, 50);
            run();


        }
    }
}

