using System;
using System.Collections.Generic;


namespace WpfApplication4
{
    public class Oscil
    {
        public string OscilNames;

        public List<string> ChannelNames = new List<string>();
        public List<string> Dimension = new List<string>();

        public DateTime StampDateStart;
        public DateTime StampDateTrigger;
        public DateTime StampDateEnd;
        public double SampleRate;
        public double HistotyCount;

        public ushort ChannelCount;
        public uint NumCount;
        public List<bool> TypeChannel = new List <bool>();

        public List<List<double>> Data = new List<List<double>>();
    }
}
