using ScopeViewer.Format;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScopeViewer
{
    public static class BinaryFormatReader
    {        
        internal static Oscil ReadHeader(BinaryReader binaryReader)
        {
            var oscil = new Oscil();
            int _count64 = 0, _count32 = 0, _count16 = 0;

            try
            {
                //Read header 1
                uint OscilMemorySize = binaryReader.ReadUInt32();
                ushort OscilSampleRate = binaryReader.ReadUInt16();
                ushort OscilSampleSize = binaryReader.ReadUInt16();
                uint OscilLenght = binaryReader.ReadUInt32();
                uint OscilHistoryCount = binaryReader.ReadUInt32();
                uint OscilRemainingCount = binaryReader.ReadUInt32();
                binaryReader.ReadBytes(12);
                uint OscilEnd = binaryReader.ReadUInt32();
                ushort[] OscilDateTime = new ushort[4];
                OscilDateTime[0] = binaryReader.ReadUInt16();
                OscilDateTime[1] = binaryReader.ReadUInt16();
                OscilDateTime[2] = binaryReader.ReadUInt16();
                OscilDateTime[3] = binaryReader.ReadUInt16();
                binaryReader.ReadBytes(20);

                //Read header 2
                var OscilTypeData = new uint[32];
                for (var i = 0; i < 32; i++)
                {
                    OscilTypeData[i] = binaryReader.ReadUInt16();
                }
                var OscilAddr = new uint[32];
                for (var i = 0; i < 32; i++)
                {
                    OscilAddr[i] = binaryReader.ReadUInt16();
                }
                uint OscilSize = binaryReader.ReadUInt32();
                ushort OscilQuantity = binaryReader.ReadUInt16();
                ushort OscilChNum = binaryReader.ReadUInt16();
                ushort OscilHistoryPercent = binaryReader.ReadUInt16();
                ushort OscilFreqDiv = binaryReader.ReadUInt16();
                ushort OscilEnable = binaryReader.ReadUInt16();
                binaryReader.ReadBytes(2);
                var OscilChNumName = new string[32];
                for (var i = 0; i < 32; i++)
                {
                    var str = Encoding.Default.GetChars(binaryReader.ReadBytes(32));
                    OscilChNumName[i] = new string(str);
                }
                binaryReader.ReadBytes(1392);

                // OscilSampleSize
                List<ushort[]> paramsLines = new List<ushort[]>();
                for (var i = 0; i < OscilLenght; i++)
                {
                    var tempLine = new ushort[OscilSampleSize >> 1];
                    for (var j = 0; j < OscilSampleSize >> 1; j++)
                    {
                        tempLine[j] = binaryReader.ReadUInt16();
                    }
                    paramsLines.Add(tempLine);
                }

                var lex = binaryReader.BaseStream.Length;
                var l = 0;
                List<ushort[]> paramsSortLines = new List<ushort[]>();
                for (var i = 0; i < paramsLines.Count; i++)
                {
                    if ((i + OscilEnd + 1) >= paramsLines.Count)
                    {
                        paramsSortLines.Add(new ushort[OscilSampleSize >> 1]);
                        paramsSortLines[i] = paramsLines[l++];
                    }
                    else
                    {
                        paramsSortLines.Add(new ushort[OscilSampleSize >> 1]);
                        paramsSortLines[i] = paramsLines[i + (int)OscilEnd + 1];
                    }
                }

                //DateTime
                string str1 = (OscilDateTime[0] & 0x3F).ToString("X2") + "/" + ((OscilDateTime[0] >> 8) & 0x1F).ToString("X2") + @"/20" + (OscilDateTime[1] & 0xFF).ToString("X2");
                string str2 = (OscilDateTime[3] & 0x3F).ToString("X2") + ":" + ((OscilDateTime[2] >> 8) & 0x7F).ToString("X2") + @":" + (OscilDateTime[2] & 0x7F).ToString("X2");
                string str3 = ((OscilDateTime[3] >> 6) & 0x3E7).ToString("D3");
                oscil.OscilStampDateTrigger = DateTime.Parse($"{str1},{str2}.{str3}");

                //OscilSampleRate
                oscil.OscilSampleRate = Convert.ToDouble(OscilSampleRate / OscilFreqDiv);     //Частота выборки 
                oscil.OscilHistotyCount = Convert.ToDouble(OscilHistoryCount);   //колличество на предысторию 
                oscil.OscilStampDateStart = oscil.OscilStampDateTrigger.AddMilliseconds(-(1000 * oscil.OscilHistotyCount / oscil.OscilSampleRate));
                oscil.OscilChannelCount = OscilChNum;
                // Name Channel
                for (var i = 0; i < OscilChNum; i++)
                {
                    oscil.ChannelNames.Add(OscilChNumName[i].Substring(0, 29).Replace("\0", String.Empty));
                }
                // Colors Channel
                for (var i = 0; i < OscilChNum; i++)
                {
                    var name = Encoding.Default.GetBytes(OscilChNumName[i].Substring(29, 3));
                    oscil.ChannelColor.Add(BitConverter.ToString(name).Replace("-", string.Empty).ToLower());
                }
                ///
                /// Convert format
                ///
                FormatConverter.ReadFormats(null);



                // Value Channel
                for (var j = 0; j < paramsSortLines.Count; j++)
                {
                    var convertVal = FileParamLine(paramsSortLines[j], j);
                    if (convertVal != null)
                    {
                        string[] strTemp = convertVal.Split('\t');
                        oscil.OscilData.Add(new List<double>());
                        for (int i = 1; i < strTemp.Length - 1; i++)
                        {
                            oscil.OscilData[j].Add(Convert.ToDouble(strTemp[i]));
                        }
                    }
                }
                               
                oscil.OscilEndSample = Convert.ToUInt32(oscil.OscilData.Count);
                oscil.OscilStampDateEnd = oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (oscil.OscilEndSample - oscil.OscilHistotyCount) / oscil.OscilSampleRate);
                for (int i = 0; i < oscil.OscilChannelCount; i++)
                {
                    oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
                    oscil.ChannelDimension.Add(" ");
                    oscil.ChannelPhase.Add("");
                    oscil.ChannelCcbm.Add("");
                    oscil.ChannelMin.Add("");
                    oscil.ChannelMax.Add("");
                }

                oscil.OscilStationName = "";
                oscil.OscilRecordingDevice = "";

                oscil.OscilTimeCode = "";
                oscil.OscilLocalCode = "";

                oscil.OscilTmqCode = "";
                oscil.OscilLeapsec = "";



                string FileParamLine(ushort[] paramLine, int lineNum)
                {
                    int i;
                    // ChFormats();
                    string str = lineNum + "\t";
                    for (i = 0, _count64 = 0, _count32 = 0, _count16 = 0; i < OscilChNum; i++)
                    {
                        var ulTemp = ParseArr(i, paramLine);
                        str = str + FormatConverter.GetValue(ulTemp, (byte)OscilTypeData[i]) + "\t";
                    }
                    return str;
                }

                ulong ParseArr(int i, ushort[] paramLine)
                {
                    ulong ulTemp = 0;
                    
                    if (OscilTypeData[i] >> 8 == 3)
                    {
                        ulTemp = 0;
                        ulTemp += (ulong)(paramLine[_count64 + 0]) << 8 * 2;
                        ulTemp += (ulong)(paramLine[_count64 + 1]) << 8 * 3;
                        ulTemp += (ulong)(paramLine[_count64 + 2]) << 8 * 0;
                        ulTemp += (ulong)(paramLine[_count64 + 3]) << 8 * 1;
                        _count64 += 4;
                    }
                    if (OscilTypeData[i] >> 8 == 2)
                    {
                        ulTemp = 0;
                        ulTemp += (ulong)(paramLine[_count64 + _count32 + 0]) << 8 * 0;
                        ulTemp += (ulong)(paramLine[_count64 + _count32 + 1]) << 8 * 1;
                        _count32 += 2;
                    }
                    if (OscilTypeData[i] >> 8 == 1)
                    {
                        ulTemp = paramLine[_count64 + _count32 + _count16];
                        _count16 += 1;
                    }
                    return ulTemp;
                }

            }
            catch 
            {
                return null;
            }

            return oscil;

        }


        //Read data


        //Формирование строк всех загруженных данных
        //private List<ushort[]> InitParamsLines()
        //{
        //    List<ushort[]> paramsLines = new List<ushort[]>();
        //    List<ushort[]> paramsSortLines = new List<ushort[]>();
        //    int k = 0;
        //    int j = 0;
        //    int l = 0;
        //    foreach (ushort[] t in _downloadedData)
        //    {
        //        while (j < 32)
        //        {
        //            if (k == 0) paramsLines.Add(new ushort[ScopeConfig.SampleSize >> 1]);
        //            while (k < (ScopeConfig.SampleSize >> 1) && j < 32)
        //            {
        //                paramsLines[paramsLines.Count - 1][k] = t[j];
        //                k++;
        //                j++;
        //            }
        //            if (k == (ScopeConfig.SampleSize >> 1)) k = 0;
        //        }
        //        j = 0;
        //    }
        //    // paramsLines.RemoveAt(paramsLines.Count-1);
        //    //Формирую список начиная с предыстории 
        //    for (int i = 0; i < paramsLines.Count; i++)
        //    {
        //        if ((i + (int)_startLoadSample + 1) >= paramsLines.Count)
        //        {
        //            paramsSortLines.Add(new ushort[ScopeConfig.SampleSize >> 1]);
        //            paramsSortLines[i] = paramsLines[l];
        //            l++;
        //        }
        //        else
        //        {
        //            paramsSortLines.Add(new ushort[ScopeConfig.SampleSize >> 1]);
        //            paramsSortLines[i] = paramsLines[i + (int)_startLoadSample + 1];
        //        }
        //    }
        //    return paramsSortLines;
        //}
    }
}
