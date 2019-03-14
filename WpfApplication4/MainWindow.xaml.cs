using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Layout;
// using ScopeViewer.BinaryFormatReader;

using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;


namespace ScopeViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			SrartSettings();
		}

		private void SrartSettings()
		{
			//Проверка на существование настроек
			Setting.InitSetting();

			WindowState = Setting.WondowState == 1 ? WindowState.Maximized : WindowState.Normal;
			MainWindowState = Setting.WondowState == 1 ? 1 : 0;

			if (Setting.WindowHeight != 0 || Setting.WindowWidth != 0)
			{
				Height = Setting.WindowHeight;
				Width = Setting.WindowWidth;
				MainWindowHeight = Height;
				MainWindowWidth = Width;
			}
		}

		private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			MainWindowHeight = Height;
			MainWindowWidth = Width;
			MainWindowState = WindowState == WindowState.Maximized ? 1 : 0;
		}

		Oscil _oscil = new Oscil();

		public static double MainWindowHeight;
		public static double MainWindowWidth;
		public static int MainWindowState;

		public static List<Oscil> OscilList = new List<Oscil>();
		public static List<OscilGraph> OscilChannelList = new List<OscilGraph>();
		public static List<GraphPanel> GraphPanelList = new List<GraphPanel>();
		public static List<WindowsFormsHost> WindowsFormsHostList = new List<WindowsFormsHost>();
		public static List<LayoutDocument> LayoutDocumentList = new List<LayoutDocument>();

		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		public static Graph GraphObj = new Graph();
		// ReSharper disable once FieldCanBeMadeReadOnly.Global
		public static Analysis AnalysisObj = new Analysis();

		public static Settings SettingsObj;

		bool _openWindow;
		bool _graphButtonStatus;
		bool _analysisButtonStatus;

		private void OpenAnimation()
		{
			DoubleAnimation openAnimation = new DoubleAnimation
			{
				From = 0,
				To = 250,
				Duration = new Duration(TimeSpan.FromSeconds(0.02))
			};
			ConfigPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, openAnimation);
		}
		private void CloseAnimation()
		{
			DoubleAnimation closeAnimation = new DoubleAnimation
			{
				From = 250,
				To = 0,
				Duration = new Duration(TimeSpan.FromSeconds(0.02))
			};
			ConfigPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, closeAnimation);
		}

		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private void ResetColorGraphButton()
		{
			GraphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
			GraphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
			GraphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
		}
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private void SetColorGraphButton()
		{
			GraphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
			GraphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
			GraphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
		}
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private void ResetColorAnalysisButton()
		{
			AnalysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
			AnalysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
			AnalysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
		}
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private void SetColorrAnalysisButton()
		{
			AnalysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
			AnalysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
			AnalysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
		}

		private void OpenWindow()
		{
			if (_graphButtonStatus == false && _analysisButtonStatus == false) _openWindow = false;
		}

		private void graphButton_Click(object sender, RoutedEventArgs e)
		{
			_graphButton();
		}

		private void _graphButton()
		{
			OpenWindow();

			if (OscilChannelList.Count != 0)
			{
				_analysisButtonStatus = false;
			}

			ConfigStackPanel.Children.Remove(AnalysisObj);
			ResetColorAnalysisButton();

			if (_graphButtonStatus == false)
			{
				if (OscilChannelList.Count == 0)
				{
					if (_openWindow)
					{
						_analysisButton();
					}
					return;
				}

				_graphButtonStatus = true;
				SetColorGraphButton();
				if (_openWindow == false) OpenAnimation();
				_openWindow = true;
				ConfigPanel.Width = new GridLength(250, GridUnitType.Pixel);
				ConfigStackPanel.Children.Add(GraphObj);

				return;
			}
			if (_graphButtonStatus)
			{
				_graphButtonStatus = false;
				ResetColorGraphButton();
				CloseAnimation();
				ConfigPanel.Width = new GridLength(0, GridUnitType.Pixel);
				ConfigStackPanel.Children.Remove(GraphObj);
			}
		}

		private void analysisButton_Click(object sender, RoutedEventArgs e)
		{
			_analysisButton();
		}

		private void _analysisButton()
		{
			OpenWindow();

			if (OscilChannelList.Count != 0)
			{
				_graphButtonStatus = false;
			}

			ConfigStackPanel.Children.Remove(GraphObj);
			ResetColorGraphButton();

			if (_analysisButtonStatus == false)
			{
				if (OscilChannelList.Count == 0)
				{
					if (_openWindow)
					{
						_graphButton();
					}
					return;
				}

				_analysisButtonStatus = true;
				SetColorrAnalysisButton();
				if (_openWindow == false) OpenAnimation();
				_openWindow = true;
				ConfigPanel.Width = new GridLength(250, GridUnitType.Pixel);
				ConfigStackPanel.Children.Add(AnalysisObj);
				return;
			}
			if (_analysisButtonStatus)
			{
				_analysisButtonStatus = false;
				ResetColorAnalysisButton();
				CloseAnimation();
				ConfigPanel.Width = new GridLength(0, GridUnitType.Pixel);
				ConfigStackPanel.Children.Remove(AnalysisObj);
			}
		}

		public void OpenAuto(string str)
		{
			string strP = str.Split('.').Last();

			OpenFileDialog ofd = new OpenFileDialog
			{
				FileName = str
			};

			if (strP == "txt")
			{
				ofd.FilterIndex = 1;
			}
			else if (strP == "cfg")
			{
				ofd.FilterIndex = 2;
			}
            else if(strP == "osc")
            {
                ofd.FilterIndex = 3;
            }
			OpenFile(ofd);
		}

		private void openButton_Click(object sender, RoutedEventArgs e)
		{
			if (!Setting.LoadSetting)
			{
				MessageBox.Show("Не удалось открыть файл настроек!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			OpenFileDialog ofd = new OpenFileDialog
			{
				DefaultExt = ".txt",
				Filter = "Текстовый файл|*.txt|Comtrade|*.cfg|BinaryFormat|*.osc|All files (*.*)|*.*"
			};

			if (ofd.ShowDialog() == true)
			{
				OpenFile(ofd);
			}
		}

		private void OpenFile(OpenFileDialog ofd)
		{
			_oscil = new Oscil();

			//Чтение .txt
			string str;
			if (ofd.FilterIndex == 1)
			{
				try
				{
					StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding("utf-8"));
					_oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);
					_oscil.OscilStampDateTrigger = DateTime.Parse(sr.ReadLine());
					_oscil.OscilSampleRate = Convert.ToDouble(sr.ReadLine());     //Частота выборки 
					_oscil.OscilHistotyCount = Convert.ToDouble(sr.ReadLine());   //колличество на предысторию 
					_oscil.OscilStampDateStart = _oscil.OscilStampDateTrigger.AddMilliseconds(-(1000 * _oscil.OscilHistotyCount / _oscil.OscilSampleRate));
					str = sr.ReadLine();
					if (str != null)
					{
						string[] str1 = str.Split('\t');
						for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelNames.Add(Convert.ToString(str1[i]));
					}
					_oscil.OscilChannelCount = Convert.ToUInt16(_oscil.ChannelNames.Count);

					str = sr.ReadLine(); //Цвета
					if (str != null)
					{
						string[] str1 = str.Split('\t');
						for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelColor.Add(Convert.ToString(str1[i]));
					}

					for (int j = 0; !sr.EndOfStream; j++)
					{
						str = sr.ReadLine();
						if (str != null)
						{
							string[] str2 = str.Split('\t');
							_oscil.OscilData.Add(new List<double>());
							for (int i = 1; i < str2.Length - 1; i++)
							{
								_oscil.OscilData[j].Add(Convert.ToDouble(str2[i]));
							}
						}
					}
					_oscil.OscilEndSample = Convert.ToUInt32(_oscil.OscilData.Count);
					_oscil.OscilStampDateEnd = _oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (_oscil.OscilEndSample - _oscil.OscilHistotyCount) / _oscil.OscilSampleRate);
					for (int i = 0; i < _oscil.OscilChannelCount; i++)
					{
						_oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
						_oscil.ChannelDimension.Add(" ");
						_oscil.ChannelPhase.Add("");
						_oscil.ChannelCcbm.Add("");
						_oscil.ChannelMin.Add("");
						_oscil.ChannelMax.Add("");
					}

					_oscil.OscilStationName = "";
					_oscil.OscilRecordingDevice = "";

					_oscil.OscilTimeCode = "";
					_oscil.OscilLocalCode = "";

					_oscil.OscilTmqCode = "";
					_oscil.OscilLeapsec = "";

					sr.Close();
				}
				catch (Exception)
				{
					try
					{
						StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding("utf-8"));


						_oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);  //Строка названия осциллограммы

						string stringTime = sr.ReadLine();
						string[] parseStringTime = stringTime?.Split(' ');
						if (parseStringTime?.Length == 4)
						{
							string[] splitString = parseStringTime[3].Split(':');
							if (splitString.Length == 4)
							{
								parseStringTime[3] = $@"{splitString[0]}:{splitString[1]}:{splitString[2]}.{splitString[3]}";
							}
							stringTime = parseStringTime[2] + " " + parseStringTime[3];
						}


						_oscil.OscilStampDateTrigger = DateTime.Parse(stringTime);             //Строка штампа времени осциллограммы 

						double sampleRate = 0;
						double historyPercent = 0;

						//int OscilSampleRate = ;
						switch (Convert.ToInt32(sr.ReadLine()))
						{
							case 1:
								{
									sampleRate = 4000;
								}
								break;
							case 3:
								{
									sampleRate = 2000;
								}
								break;
							case 7:
								{
									sampleRate = 1000;
								}
								break;
						}

						//int HistoryPercent = Convert.ToInt32(sr.ReadLine());

						switch (Convert.ToInt32(sr.ReadLine()))
						{
							case 0x4000:
								{
									historyPercent = 25;
								}
								break;
							case 0x8000:
								{
									historyPercent = 50;
								}
								break;
							case 0xC000:
								{
									historyPercent = 75;
								}
								break;
						}

						str = sr.ReadLine(); // Строка названия каналов
						if (str != null)
						{
							string[] str1 = str.Split('\t');
							for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelNames.Add(Convert.ToString(str1[i]));
						}
						_oscil.OscilChannelCount = Convert.ToUInt16(_oscil.ChannelNames.Count);

						for (int i = 0; i < 8; i++)
						{
							sr.ReadLine();
						}

						for (int j = 0; !sr.EndOfStream; j++)
						{
							str = sr.ReadLine();
							if (str != null)
							{
								string[] str2 = str.Split('\t');
								_oscil.OscilData.Add(new List<double>());
								for (int i = 1; i < str2.Length - 1; i++)
								{
									_oscil.OscilData[j].Add(Convert.ToDouble(str2[i]));
								}
							}
						}
						_oscil.OscilEndSample = Convert.ToUInt32(_oscil.OscilData.Count);

						_oscil.OscilSampleRate = sampleRate;     //Частота выборки 
						_oscil.OscilHistotyCount = _oscil.OscilEndSample * historyPercent / 100;   //колличество на предысторию 
						_oscil.OscilStampDateStart = _oscil.OscilStampDateTrigger.AddMilliseconds(-(1000 * _oscil.OscilHistotyCount / _oscil.OscilSampleRate));
						_oscil.OscilStampDateEnd = _oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (_oscil.OscilEndSample - _oscil.OscilHistotyCount) / _oscil.OscilSampleRate);

						for (int i = 0; i < _oscil.OscilChannelCount; i++)
						{
							_oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
							_oscil.ChannelDimension.Add(" ");
						}

						for (int i = 0; i < _oscil.OscilChannelCount; i++)
						{
							_oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
							_oscil.ChannelDimension.Add(" ");
							_oscil.ChannelPhase.Add("");
							_oscil.ChannelCcbm.Add("");
							_oscil.ChannelMin.Add("");
							_oscil.ChannelMax.Add("");
						}

						_oscil.OscilStationName = "";
						_oscil.OscilRecordingDevice = "";

						_oscil.OscilTimeCode = "";
						_oscil.OscilLocalCode = "";

						_oscil.OscilTmqCode = "";
						_oscil.OscilLeapsec = "";

						sr.Close();
					}
					catch (Exception)
					{
						try
						{
							StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding("utf-8"));
							_oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);
							_oscil.OscilStampDateTrigger = DateTime.Parse(sr.ReadLine());
							_oscil.OscilSampleRate = Convert.ToDouble(sr.ReadLine());     //Частота выборки 
							_oscil.OscilHistotyCount = Convert.ToDouble(sr.ReadLine());   //колличество на предысторию 
							_oscil.OscilStampDateStart = _oscil.OscilStampDateTrigger.AddMilliseconds(-(1000 * _oscil.OscilHistotyCount / _oscil.OscilSampleRate));
							str = sr.ReadLine();
							if (str != null)
							{
								string[] str1 = str.Split('\t');
								for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelNames.Add(Convert.ToString(str1[i]));
							}
							_oscil.OscilChannelCount = Convert.ToUInt16(_oscil.ChannelNames.Count);

							str = sr.ReadLine(); //Цвета
							if (str != null)
							{
								string[] str1 = str.Split('\t');
								for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelColor.Add(Convert.ToString(str1[i]));
							}

							for (int j = 0; !sr.EndOfStream; j++)
							{
								str = sr.ReadLine();
								if (str != null)
								{
									string[] str2 = str.Split('\t');
									_oscil.OscilData.Add(new List<double>());
									for (int i = 1; i < str2.Length - 1; i++)
									{
										_oscil.OscilData[j].Add(Convert.ToDouble(str2[i]));
									}
								}
							}
							_oscil.OscilEndSample = Convert.ToUInt32(_oscil.OscilData.Count);
							_oscil.OscilStampDateEnd = _oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (_oscil.OscilEndSample - _oscil.OscilHistotyCount) / _oscil.OscilSampleRate);
							for (int i = 0; i < _oscil.OscilChannelCount; i++)
							{
								_oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
								_oscil.ChannelDimension.Add(" ");
								_oscil.ChannelPhase.Add("");
								_oscil.ChannelCcbm.Add("");
								_oscil.ChannelMin.Add("");
								_oscil.ChannelMax.Add("");
							}

							_oscil.OscilStationName = "";
							_oscil.OscilRecordingDevice = "";

							_oscil.OscilTimeCode = "";
							_oscil.OscilLocalCode = "";

							_oscil.OscilTmqCode = "";
							_oscil.OscilLeapsec = "";

							sr.Close();
						}
						catch
						{
							try
							{
								StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding("utf-8"));

								_oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);  //Строка названия осциллограммы

								string stringTime = sr.ReadLine();
								string[] parseStringTime = stringTime.Split(' ');
								if (parseStringTime.Length == 4)
								{
									string[] splitString = parseStringTime[3].Split(':');
									if (splitString.Length == 4)
									{
										parseStringTime[3] = $@"{splitString[0]}:{splitString[1]}:{splitString[2]}.{splitString[3]}";
									}
									stringTime = parseStringTime[2] + " " + parseStringTime[3];
								}
								else if (parseStringTime.Length == 2)
								{
									string[] splitString = parseStringTime[1].Split(':');
									if (splitString.Length == 4)
									{
										parseStringTime[3] = $@"{splitString[0]}:{splitString[1]}:{splitString[2]}.{splitString[3]}";
									}
									stringTime = parseStringTime[0] + " " + parseStringTime[1];
								}

								_oscil.OscilStampDateTrigger = DateTime.Parse(stringTime);

								double sampleRate = 0;
								double historyPercent = 0;

								//int OscilSampleRate = ;
								switch (Convert.ToInt32(sr.ReadLine()))
								{
									case 1:
										{
											sampleRate = 4000;
										}
										break;
									case 3:
										{
											sampleRate = 2000;
										}
										break;
									case 7:
										{
											sampleRate = 1000;
										}
										break;
								}

								OpenOldFormat openOldObj = new OpenOldFormat(sampleRate);                      //Запускам выбор параметров для осциллограммы старой версии


								DialogResult dlgr = openOldObj.ShowDialog();

								if (dlgr == System.Windows.Forms.DialogResult.OK)
								{
									sampleRate = OpenOldFormat.SampleRate;
									historyPercent = OpenOldFormat.HistoryPercent;
								}

								str = sr.ReadLine(); // Строка названия каналов
								if (str != null)
								{
									string[] str1 = str.Split('\t');
									for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelNames.Add(Convert.ToString(str1[i]));
								}
								_oscil.OscilChannelCount = Convert.ToUInt16(_oscil.ChannelNames.Count);

								for (int i = 0; i < 8; i++)
								{
									sr.ReadLine();
								}

								for (int j = 0; !sr.EndOfStream; j++)
								{
									str = sr.ReadLine();
									if (str != null)
									{
										string[] str2 = str.Split('\t');
										_oscil.OscilData.Add(new List<double>());
										for (int i = 1; i < str2.Length - 1; i++)
										{
											_oscil.OscilData[j].Add(Convert.ToDouble(str2[i]));
										}
									}
								}
								_oscil.OscilEndSample = Convert.ToUInt32(_oscil.OscilData.Count);

								_oscil.OscilSampleRate = sampleRate;     //Частота выборки 
								_oscil.OscilHistotyCount = _oscil.OscilEndSample * historyPercent / 100;   //колличество на предысторию 
								_oscil.OscilStampDateStart = _oscil.OscilStampDateTrigger.AddMilliseconds(-(1000 * _oscil.OscilHistotyCount / _oscil.OscilSampleRate));
								_oscil.OscilStampDateEnd = _oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (_oscil.OscilEndSample - _oscil.OscilHistotyCount) / _oscil.OscilSampleRate);

								for (int i = 0; i < _oscil.OscilChannelCount; i++)
								{
									_oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
									_oscil.ChannelDimension.Add(" ");
								}

								for (int i = 0; i < _oscil.OscilChannelCount; i++)
								{
									_oscil.ChannelType.Add(false);  //Значит сигнал аналоговый
									_oscil.ChannelDimension.Add(" ");
									_oscil.ChannelPhase.Add("");
									_oscil.ChannelCcbm.Add("");
									_oscil.ChannelMin.Add("");
									_oscil.ChannelMax.Add("");
								}

								_oscil.OscilStationName = "";
								_oscil.OscilRecordingDevice = "";

								_oscil.OscilTimeCode = "";
								_oscil.OscilLocalCode = "";

								_oscil.OscilTmqCode = "";
								_oscil.OscilLeapsec = "";

								sr.Close();
							}
							catch
							{
								MessageBox.Show("Ошибка при чтении файла", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
								return;
							}
						}
					}
				}
			}

			//Чтение .comtrade
			if (ofd.FilterIndex == 2)
			{
				try
				{
					StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding("utf-8"));
					_oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);
					str = sr.ReadLine();
					var line1 = str.Split(',');
					_oscil.OscilStationName = line1[0].Normalize();
					_oscil.OscilRecordingDevice = line1[1].Normalize();
					_oscil.OscilRevisionYear = line1[2].Normalize();
					str = sr.ReadLine();
					Regex regex = new Regex(@"\d");
					// ReSharper disable once AssignNullToNotNullAttribute
					MatchCollection matches = regex.Matches(str);
					_oscil.OscilChannelCount = Convert.ToUInt16(matches[0].Value);
					for (int i = 0; i < _oscil.OscilChannelCount; i++)
					{
						_oscil.ChannelType.Add(i >= Convert.ToInt32(matches[1].Value));
					}
					//Аналоговые каналы
					for (int i = 0; i < Convert.ToInt32(matches[1].Value); i++)
					{
						str = sr.ReadLine();
						if (str != null)
						{
							string[] str1 = str.Split(',');
							_oscil.ChannelNames.Add(Convert.ToString(str1[1]));
							_oscil.ChannelDimension.Add(Convert.ToString(str1[4]));
							//Info about chanel
							_oscil.ChannelColor.Add("ffffff");
							_oscil.ChannelPhase.Add(Convert.ToString(str1[2]));
							_oscil.ChannelCcbm.Add(Convert.ToString(str1[3]));
							_oscil.ChannelMin.Add(Convert.ToString(str1[8]));
							_oscil.ChannelMax.Add(Convert.ToString(str1[9]));
						}
					}
					for (int i = 0; i < Convert.ToInt32(matches[2].Value); i++)
					{
						str = sr.ReadLine();
						if (str != null)
						{
							string[] str1 = str.Split(',');
							_oscil.ChannelNames.Add(Convert.ToString(str1[1]));
							_oscil.ChannelDimension.Add(" ");
							//Info about chanel
							_oscil.ChannelColor.Add("ffffff");
							_oscil.ChannelPhase.Add(Convert.ToString(str1[2]));
							_oscil.ChannelCcbm.Add(Convert.ToString(str1[3]));
							_oscil.ChannelMin.Add("");
							_oscil.ChannelMax.Add("");
						}
					}
					var line5 = sr.ReadLine();
					_oscil.OscilNominalFrequency = line5;
					var line6 = sr.ReadLine();
					_oscil.OscilNRates = line6;
					str = sr.ReadLine();
					if (str != null)
					{
						string[] str2 = str.Split(',');
						_oscil.OscilSampleRate = Convert.ToDouble(str2[0]);
						_oscil.OscilEndSample = Convert.ToUInt32(str2[1]);
					}
					_oscil.OscilStampDateStart = DateTime.Parse(sr.ReadLine());
					_oscil.OscilStampDateTrigger = DateTime.Parse(sr.ReadLine());
					_oscil.OscilHistotyCount = Convert.ToInt32((_oscil.OscilStampDateTrigger.Second - _oscil.OscilStampDateStart.Second +
						(double)(_oscil.OscilStampDateTrigger.Millisecond - _oscil.OscilStampDateStart.Millisecond) / 1000) * _oscil.OscilSampleRate);
					_oscil.OscilStampDateEnd = _oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (_oscil.OscilEndSample - _oscil.OscilHistotyCount) / _oscil.OscilSampleRate);
					var line10 = sr.ReadLine();
					_oscil.OscilFT = line10;
					var line11 = sr.ReadLine();
					_oscil.OscilTimemult = line11;
					if (_oscil.OscilRevisionYear == "2013")
					{
						var line12 = sr.ReadLine().Split(',');
						_oscil.OscilTimeCode = line12[0];
						_oscil.OscilLocalCode = line12[1];
						var line13 = sr.ReadLine().Split(',');
						_oscil.OscilTmqCode = line13[0];
						_oscil.OscilLeapsec = line13[1];
					}

					sr.Close();

					var namefile = Path.GetFileNameWithoutExtension(ofd.FileName);
					var pathfile = Path.GetDirectoryName(ofd.FileName);

					string pathDateFile = pathfile + "\\" + namefile + ".dat";

					StreamReader srd = new StreamReader(pathDateFile, Encoding.GetEncoding("utf-8"));
					for (int j = 0; !srd.EndOfStream; j++)
					{
						str = srd.ReadLine();
						if (str != null)
						{
							string[] str3 = str.Split(',');
							_oscil.OscilData.Add(new List<double>());
							for (int i = 2; i < str3.Length; i++)
							{
								str3[i] = str3[i].Replace(".", ",");
								_oscil.OscilData[j].Add(Convert.ToDouble(str3[i]));
							}
						}
					}
					srd.Close();
				}
				catch (Exception)
				{
					MessageBox.Show("Ошибка при чтении файла", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
			}

            //Binary reading
            if(ofd.FilterIndex == 3)
            {
                try
                {
                    using (var fs = File.OpenRead(ofd.FileName))
                    {
                        using (var binaryReader = new BinaryReader(fs))
                        {
                            _oscil = BinaryFormatReader.ReadHeader(binaryReader);
                            if (_oscil == null) throw new Exception();
                            _oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);
                        }
                    }
                    //BinaryReader.
                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось открыть файл!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

			OscilList.Add(_oscil);

			AddOscilChannnel(_oscil, false);
		}

		private void AddOscilChannnel(Oscil oscil, bool dig)
		{
			OscilGraph oscilGraph = new OscilGraph();

			oscilGraph.OscilConfigAdd(oscil.OscilNames);
			for (int i = 0; i < oscil.OscilChannelCount; i++)
			{
				var color = _oscil.ChannelColor.Count == 0 ? "FFFFFF" : _oscil.ChannelColor[i];

				oscilGraph.GraphConfigAdd(oscil.ChannelNames[i], oscil.ChannelDimension[i], oscil.ChannelType[i], color);
			}

			OscilChannelList.Add(oscilGraph);

			GraphObj.GraphStackPanel.Children.Add(oscilGraph.LayoutOscilPanel);
			for (int i = 0; i < oscil.OscilChannelCount; i++)
			{
				GraphObj.GraphStackPanel.Children.Add(oscilGraph.LayoutPanel[i]);
			}

			GraphPanelList.Add(new GraphPanel(Setting.CoursorBinding));

			SetSetting();

			WindowsFormsHostList.Add(new WindowsFormsHost());
			LayoutDocumentList.Add(new LayoutDocument());
			LayoutDocumentList[LayoutDocumentList.Count - 1].Content = WindowsFormsHostList[WindowsFormsHostList.Count - 1];
			LayoutDocumentList[LayoutDocumentList.Count - 1].Title = oscil.OscilNames;
			LayoutDocumentList[LayoutDocumentList.Count - 1].CanFloat = true;
			LayoutDocumentList[LayoutDocumentList.Count - 1].CanClose = true;
			LayoutDocumentList[LayoutDocumentList.Count - 1].Closed += OnClosed;

			LayoutGraph.Children.Add(LayoutDocumentList[LayoutDocumentList.Count - 1]);
			WindowsFormsHostList[WindowsFormsHostList.Count - 1].Child = GraphPanelList[GraphPanelList.Count - 1];

			for (int i = 0; i < OscilChannelList[OscilChannelList.Count - 1].NameLabel.Count; i++)
			{
				SolidColorBrush brush = (SolidColorBrush)OscilChannelList[OscilChannelList.Count - 1].ColorEllipse[i].Fill;
				System.Drawing.Color color = System.Drawing.Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
				GraphPanelList[GraphPanelList.Count - 1].AddGraph(i, color, dig);
			}

			GraphPanelList[GraphPanelList.Count - 1].Scale(1.0f);
		}

		private void OnClosed(object sender, EventArgs eventArgs)
		{
			for (int i = 0; i < LayoutDocumentList.Count; i++)
			{
				if (Equals(LayoutDocumentList[i], (LayoutDocument)sender))
				{

					OscilList.Remove(OscilList[i]);
					GraphPanelList[i].DelCursor();

					GraphObj.GraphStackPanel.Children.Remove(OscilChannelList[i].LayoutOscilPanel);
					for (int j = OscilChannelList[i].LayoutPanel.Count - 1; j >= 0; j--)
						GraphObj.GraphStackPanel.Children.Remove(OscilChannelList[i].LayoutPanel[j]);
					OscilChannelList.Remove(OscilChannelList[i]);
					GraphPanelList.Remove(GraphPanelList[i]);
					WindowsFormsHostList.Remove(WindowsFormsHostList[i]);
					LayoutDocumentList.Remove(LayoutDocumentList[i]);

					while (i < OscilChannelList.Count)
					{
						OscilChannelList[i].OscilName.Content = "Осциллограмма №" + (i + 1);
						i++;
					}
				}
			}
		}

		private void SetSetting()
		{
			Setting.InitSetting();
			GraphPanelList[GraphPanelList.Count - 1].PointInLineChange(Setting.PointInLine, Setting.ShowDigital);
			if (Setting.PointInLine < 50) GraphPanelList[GraphPanelList.Count - 1].PointInLineChange(50, Setting.ShowDigital);
			GraphPanelList[GraphPanelList.Count - 1].GridAxisChange(Setting.XMinorShow, Setting.XMinor, 0);
			GraphPanelList[GraphPanelList.Count - 1].GridAxisChange(Setting.XMajorShow, Setting.XMajor, 1);
			GraphPanelList[GraphPanelList.Count - 1].GridAxisChange(Setting.YMinorShow, Setting.YMinor, 2);
			GraphPanelList[GraphPanelList.Count - 1].GridAxisChange(Setting.YMajorShow, Setting.YMajor, 3);

			bool checkedLegend = false;
			int h = 1, v = 1;
			if (Setting.ShowLegend) checkedLegend = true;
			if (Setting.Position == 2)
			{
				h = 0; v = 0;
			}
			if (Setting.Position == 1)
			{
				h = 1; v = 0;
			}
			if (Setting.Position == 3)
			{
				h = 0; v = 1;
			}
			if (Setting.Position == 0)
			{
				h = 1; v = 1;
			}
			GraphPanelList[GraphPanelList.Count - 1].LegendShow(checkedLegend, Convert.ToInt32(Setting.SizeLegend), h, v);
		}

		private void settings_Click(object sender, RoutedEventArgs e)
		{
			if (SettingsObj != null)
			{
				SettingsObj.Topmost = true;
			}
			else
			{
				SettingsObj = new Settings
				{
					Topmost = true
				};
			}
			SettingsObj.Show();
		}

		public static void DelateOscil(int i)
		{
			LayoutDocumentList[i].Close();
		}

		public static void ActivateOscil(int i)
		{
			LayoutDocumentList[i].IsSelected = true;
		}
		

		private void AddGraph_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//Создание новой осциллограммы OscilList
			_oscil = new Oscil
			{
				OscilNames = "Осциллограмма"
			};

			for (var k = 0; k < OscilChannelList.Count; k++)
			{
				for (var i = 0; i < OscilChannelList[k].TypeComboBox.Count; i++)
				{
					if (OscilChannelList[k].SelectCheckBox[i].IsChecked != true) continue;
					_oscil.ChannelNames.Add(OscilList[k].ChannelNames[i]);
					_oscil.ChannelDimension.Add(OscilList[k].ChannelDimension[i]);

					if (_oscil.OscilChannelCount > 0 && ((Math.Abs(_oscil.OscilSampleRate - OscilList[k].OscilSampleRate) > 0) || (Math.Abs(_oscil.OscilHistotyCount - OscilList[k].OscilHistotyCount) > 0) || (_oscil.OscilEndSample != OscilList[k].OscilEndSample)))
					{
						MessageBox.Show("Каналы не совместимы", "Ошибка", MessageBoxButton.OK);
						return;
					}

					if (_oscil.OscilChannelCount == 0)
					{
						_oscil.OscilStampDateTrigger = OscilList[k].OscilStampDateTrigger;
						_oscil.OscilSampleRate = OscilList[k].OscilSampleRate;
						_oscil.OscilHistotyCount = OscilList[k].OscilHistotyCount;
						_oscil.OscilEndSample = OscilList[k].OscilEndSample;
						_oscil.OscilStampDateStart = _oscil.OscilStampDateTrigger.AddMilliseconds(-(1000 * _oscil.OscilHistotyCount / _oscil.OscilSampleRate));
						_oscil.OscilStampDateEnd =
							_oscil.OscilStampDateTrigger.AddMilliseconds(1000 * (_oscil.OscilEndSample - _oscil.OscilHistotyCount) / _oscil.OscilSampleRate);

						for (int j = 0; j < _oscil.OscilEndSample; j++)
							_oscil.OscilData.Add(new List<double>());
					}

					_oscil.OscilChannelCount += 1;
					_oscil.ChannelType.Add(OscilList[k].ChannelType[i]);
					_oscil.ChannelColor.Add(OscilList[k].ChannelColor[i]);

					for (int j = 0; j < _oscil.OscilEndSample; j++)
					{
						_oscil.OscilData[j].Add(OscilList[k].OscilData[j][i]);
					}
					OscilChannelList[k].SelectCheckBox[i].IsChecked = false;
				}
			}

			if (_oscil.OscilChannelCount == 0) return;

			OscilList.Add(_oscil);

			AddOscilChannnel(_oscil, false);
		}

		private void AddGraph_MouseEnter(object sender, MouseEventArgs e)
		{
			AddGraph.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Chromatography-48_2.png")));
		}

		private void AddGraph_MouseLeave(object sender, MouseEventArgs e)
		{
			AddGraph.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Chromatography-48.png")));
		}

		private void AddDigitalChannel_MouseDown(object sender, MouseButtonEventArgs e)
		{
			for (int numOsc = 0; numOsc < OscilChannelList.Count; numOsc++)
			{
				for (int numCh = 0; numCh < OscilChannelList[numOsc].TypeComboBox.Count; numCh++)
				{
					if (OscilChannelList[numOsc].SelectCheckBox[numCh].IsChecked == true && OscilChannelList[numOsc].TypeTypeComboBox[numCh].Text == "Digital")
					{
						GraphPanelList[numOsc].AddDigitalChannel(numCh, numOsc, System.Drawing.Color.Blue);
						OscilChannelList[numOsc].SelectCheckBox[numCh].IsChecked = false;
						break;
					}
				}
			}
		}

		private void AddDigitalChannel_MouseEnter(object sender, MouseEventArgs e)
		{
			AddDigitalChannel.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Dig_Leave.png")));
		}

		private void AddDigitalChannel_MouseLeave(object sender, MouseEventArgs e)
		{
			AddDigitalChannel.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Dig_Add.png")));
		}

		int maxGapCount = 10;

		private void UnionScope_MouseDown(object sender, MouseButtonEventArgs e)
		{
			List<int> scopeIndex1 = new List<int>();
			List<int> scopeIndex2 = new List<int>();
			int scope1 = 0;
			int scope2 = 0;

			//Формируем список объединяйимых каналов
			for (int k = 0; k < OscilChannelList.Count; k++)
			{
				if (scopeIndex2.Count == 0 && scopeIndex1.Count != 0)
				{
					for (int i = 0; i < OscilChannelList[k].SelectCheckBox.Count; i++)
					{
						if (OscilChannelList[k].SelectCheckBox[i].IsChecked == true)
						{
							scope2 = k;
							scopeIndex2.Add(i);
							OscilChannelList[k].SelectCheckBox[i].IsChecked = false;
						}
					}
				}
				if (scopeIndex1.Count == 0)
				{
					for (int i = 0; i < OscilChannelList[k].SelectCheckBox.Count; i++)
					{
						if (OscilChannelList[k].SelectCheckBox[i].IsChecked == true)
						{
							scope1 = k;
							scopeIndex1.Add(i);
							OscilChannelList[k].SelectCheckBox[i].IsChecked = false;
						}
					}
				}
			}

			//Проверка на соответствие друг-другу списка объединяймых параметров
			if (scope1 == scope2)
			{
				MessageBox.Show("Нечего объединять", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (Math.Abs(OscilList[scope1].OscilSampleRate - OscilList[scope2].OscilSampleRate) > 0)  //Проверка на совпадение частоты выборки 
			{
				MessageBox.Show("Каналы не совместимы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (!scopeIndex1.SequenceEqual(scopeIndex2)) //Проверка на соответствие каналов к объединению
			{
				MessageBox.Show("Выбранные каналы неляьзя объеденить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			double time1 = Math.Abs(OscilList[scope1].OscilStampDateEnd.Second - OscilList[scope2].OscilStampDateStart.Second + (double)(OscilList[scope1].OscilStampDateEnd.Millisecond - OscilList[scope2].OscilStampDateStart.Millisecond) / 1000);
			double time2 = Math.Abs(OscilList[scope2].OscilStampDateEnd.Second - OscilList[scope1].OscilStampDateStart.Second + (double)(OscilList[scope2].OscilStampDateEnd.Millisecond - OscilList[scope1].OscilStampDateStart.Millisecond) / 1000);


			if (time1 > maxGapCount / OscilList[scope1].OscilSampleRate && time2 > maxGapCount / OscilList[scope1].OscilSampleRate) //Проверка на временной отсуп между двумя осциллограммами 
			{
				MessageBox.Show("Интервал времени между объединяймыми каналами слишком велик", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			//Объединение
			int firstScope = OscilList[scope1].OscilStampDateTrigger < OscilList[scope2].OscilStampDateTrigger ? scope1 : scope2;
			int secondScope = OscilList[scope1].OscilStampDateTrigger > OscilList[scope2].OscilStampDateTrigger ? scope1 : scope2;
			int numCountTemp = Convert.ToInt32(((OscilList[secondScope].OscilStampDateStart.Second - OscilList[firstScope].OscilStampDateEnd.Second)
				+ (double)(OscilList[secondScope].OscilStampDateStart.Millisecond - OscilList[firstScope].OscilStampDateEnd.Millisecond) / 1000) * OscilList[firstScope].OscilSampleRate);

			_oscil = new Oscil
			{
				OscilNames = "Осциллограмма объединенная",
				OscilStampDateTrigger = OscilList[firstScope].OscilStampDateTrigger,
				OscilStampDateStart = OscilList[firstScope].OscilStampDateTrigger.AddMilliseconds(-(1000 * OscilList[firstScope].OscilHistotyCount / OscilList[firstScope].OscilSampleRate)),
				OscilStampDateEnd = OscilList[firstScope].OscilStampDateTrigger.AddMilliseconds(1000 * (OscilList[firstScope].OscilEndSample + OscilList[secondScope].OscilEndSample - OscilList[firstScope].OscilHistotyCount) / OscilList[firstScope].OscilSampleRate),
				OscilSampleRate = OscilList[firstScope].OscilSampleRate,
				OscilHistotyCount = OscilList[firstScope].OscilHistotyCount,
				OscilEndSample = OscilList[firstScope].OscilEndSample + OscilList[secondScope].OscilEndSample + Convert.ToUInt32(numCountTemp)
			};

			for (int j = 0; j < _oscil.OscilEndSample; j++)
			{
				_oscil.OscilData.Add(new List<double>());
			}

			foreach (int scopeIndex in scopeIndex1)
			{
				_oscil.ChannelNames.Add(OscilList[firstScope].ChannelNames[scopeIndex]);
				_oscil.ChannelDimension.Add(OscilList[firstScope].ChannelDimension[scopeIndex]);
				_oscil.ChannelType.Add(OscilList[firstScope].ChannelType[scopeIndex]);
				_oscil.ChannelColor.Add(OscilList[firstScope].ChannelColor[scopeIndex]);

				for (int j = 0; j < OscilList[firstScope].OscilEndSample; j++)
				{
					_oscil.OscilData[j].Add(OscilList[firstScope].OscilData[j][scopeIndex]);
				}

				for (int j = 0; j < numCountTemp; j++)
				{
					_oscil.OscilData[j + Convert.ToInt32(OscilList[firstScope].OscilEndSample)].Add(OscilList[firstScope].OscilData[OscilList[firstScope].OscilData.Count - 1][scopeIndex]);
				}

				for (int j = 0; j < OscilList[secondScope].OscilEndSample; j++)
				{
					_oscil.OscilData[j + numCountTemp + Convert.ToInt32(OscilList[firstScope].OscilEndSample)].Add(OscilList[secondScope].OscilData[j][scopeIndex]);
				}

				_oscil.OscilChannelCount += 1;
			}

			OscilList.Add(_oscil);
			AddOscilChannnel(_oscil, false);

			//Очищаем список объединяйимых каналов
			scopeIndex1.Clear();
			scopeIndex2.Clear();
		}

		private void UnionScope_MouseEnter(object sender, MouseEventArgs e)
		{
			UnionScope.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Line Chart-48(3).png")));
		}

		private void UnionScope_MouseLeave(object sender, MouseEventArgs e)
		{
			UnionScope.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Line Chart-48(2).png")));
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = SettingsObj != null;
			Settings.SaveSettingBase();
		}
	}
}
