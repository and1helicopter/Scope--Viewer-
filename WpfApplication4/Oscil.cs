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

        public static DateTime StampDateStart;
        public static DateTime StampDateTrigger;
        public static DateTime StampDateEnd;
        public static double SampleRate;
        public static double HistotyCount;

        public static ushort ChannelCount;
        public static uint NumCount;
        public static List <bool> TypeChannel = new List <bool>();

        public static List<List<double>> Data = new List<List<double>>();
    }
}
