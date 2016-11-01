using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication4
{
    public static class Oscil
    {
        public static List<string> ChannelNames = new List<string>();
        public static List<string> Dimension = new List<string>();

        public static List<DateTime> StampDateStart = new List<DateTime>();
        public static List<DateTime> StampDateTrigger = new List<DateTime>();
        public static List<DateTime> StampDateEnd = new List<DateTime>();
        public static uint SampleRate;

        public static uint NumCount;
        public static bool[] TypeChannel = new bool[32];

        public static List<List<double>> Data = new List<List<double>>();
    }
}
