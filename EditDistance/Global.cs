using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditDistance
{
    public  class Global
    {
        public static TimeSpan time;
        public static string alg;
        public static int threshold;
        public static int epslion;
        public static long resut;
        public static long count;
        public static bool exact;
        public static string dataset;
        public static string ver_alg;

        public static string header()
        {
            return "dataset\talg\tver_alg\tth\teps\texact\tcount\tresult\ttime";
        }
        public static string print()
        {
            return dataset + '\t' + alg + '\t' +ver_alg+ '\t' + threshold + '\t' + epslion + '\t' + exact + '\t' + count + '\t' + resut + '\t' + time.TotalSeconds;
        }
    }
}
