using System;
using System.Collections.Generic;


namespace ScopeViewer
{
	public class Oscil
	{
		public string OscilNames;

		public string OscilStationName;
		public string OscilRecordingDevice;
		public string OscilRevisionYear;

		public ushort OscilChannelCount;

		public List<bool> ChannelType = new List<bool>();
		public List<string> ChannelColor = new List<string>();

		public List<string> ChannelNames = new List<string>();
		public List<string> ChannelPhase = new List<string>();
		public List<string> ChannelCcbm = new List<string>();
		public List<string> ChannelDimension = new List<string>();
		public List<string> ChannelMin = new List<string>();
		public List<string> ChannelMax = new List<string>();

		public string OscilNominalFrequency = "50";
		public string OscilNRates = "1";
		public double OscilSampleRate;
		public uint OscilEndSample;

		public DateTime OscilStampDateStart;
		public DateTime OscilStampDateTrigger;
		public DateTime OscilStampDateEnd;

		public string OscilFT = "ASCII";
		public string OscilTimemult = "1";

		public string OscilTimeCode;
		public string OscilLocalCode;

		public string OscilTmqCode;
		public string OscilLeapsec;

		public double OscilHistotyCount;


		public List<List<double>> OscilData = new List<List<double>>();
	}
}
