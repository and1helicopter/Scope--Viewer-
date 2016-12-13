using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ScopeViewer;

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
        }

        Oscil _oscil = new Oscil();
        public static List<Oscil> OscilList = new List<Oscil>();

        public static Graph GraphObj = new Graph();
        public static Analysis AnalysisObj = new Analysis();
        bool _openWindow;
        bool _graphButtonStatus;
        bool _styleButtonStatus;
        bool _analysisButtonStatus;

        private void OpenAnimation()
        {
            DoubleAnimation openAnimation = new DoubleAnimation
            {
                From = 0,
                To = 250,
                Duration = new Duration(TimeSpan.FromSeconds(0.01))
            };
            ConfigPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, openAnimation);
        }
        private void CloseAnimation()
        {
            DoubleAnimation closeAnimation = new DoubleAnimation
            {
                From = 250,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.01))
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
            if (_graphButtonStatus == false && _styleButtonStatus == false && _analysisButtonStatus == false) _openWindow = false;
        }

        private void graphButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow();

            _styleButtonStatus = false;
            _analysisButtonStatus = false;
            ConfigStackPanel.Children.Remove(AnalysisObj);
            ResetColorAnalysisButton();

            if (_graphButtonStatus == false)
            {
                _graphButtonStatus = true;
                SetColorGraphButton();
                if(_openWindow == false) OpenAnimation();
                _openWindow = true;
                ConfigPanel.Width = new GridLength(250, GridUnitType.Pixel);
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
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
            OpenWindow();

            _graphButtonStatus = false;
            _styleButtonStatus = false;
            ConfigStackPanel.Children.Remove(GraphObj);
            ResetColorGraphButton();

            if (_analysisButtonStatus == false)
            {
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
            OpenFileDialog ofd = new OpenFileDialog
            {
                FileName = str,
            };
            OpenFile(ofd);
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Текстовый файл|*.txt|Comtrade|*.cfg|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                OpenFile(ofd);
            }
        }

        private void OpenFile(OpenFileDialog ofd)
        {
            _oscil = new Oscil();

            Graph.StampTriggerClear();
            Graph.CursorClear();

            AnalysisObj.AnalysisCursorClear();
            CursorCreate = false;

            //Чтение .txt
            string str;
            if (ofd.FilterIndex == 1)
            {
                try
                {
                    StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8);
                    _oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);
                    _oscil.StampDateTrigger = DateTime.Parse(sr.ReadLine());
                    _oscil.SampleRate = Convert.ToDouble(sr.ReadLine());     //Частота выборки 
                    _oscil.HistotyCount = Convert.ToDouble(sr.ReadLine());   //колличество на предысторию 
                    _oscil.StampDateStart = _oscil.StampDateTrigger.AddMilliseconds(-(100 * _oscil.HistotyCount / _oscil.SampleRate));
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        string[] str1 = str.Split('\t');
                        for (int i = 1; i < str1.Length - 1; i++) _oscil.ChannelNames.Add(Convert.ToString(str1[i]));
                    }
                    _oscil.ChannelCount = Convert.ToUInt16(_oscil.ChannelNames.Count);
                    for (int j = 0; !sr.EndOfStream; j++)
                    {
                        str = sr.ReadLine();
                        if (str != null)
                        {
                            string[] str2 = str.Split('\t');
                            _oscil.Data.Add(new List<double>());
                            for (int i = 1; i < str2.Length - 1; i++)
                            {
                                _oscil.Data[j].Add(Convert.ToDouble(str2[i]));
                            }
                        }
                    }
                    _oscil.NumCount = Convert.ToUInt32(_oscil.Data.Count);
                    for (int i = 0; i < _oscil.ChannelCount; i++)
                    {
                        _oscil.TypeChannel.Add(false);  //Значит сигнал аналоговый
                        _oscil.Dimension.Add("NONE");
                    }
                    sr.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при чтении файла", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            //Чтение .comtrade
            if (ofd.FilterIndex == 2)
            {
                try
                {
                    StreamReader sr = new StreamReader(ofd.FileName, Encoding.Default);
                    _oscil.OscilNames = Path.GetFileNameWithoutExtension(ofd.FileName);
                    sr.ReadLine();
                    str = sr.ReadLine();
                    Regex regex = new Regex(@"\d");
                    // ReSharper disable once AssignNullToNotNullAttribute
                    MatchCollection matches = regex.Matches(str);
                    _oscil.ChannelCount = Convert.ToUInt16(matches[0].Value);
                    for (int i = 0; i < _oscil.ChannelCount; i++)
                    {
                        _oscil.TypeChannel.Add(i >= Convert.ToInt32(matches[1].Value));
                    }
                    //Аналоговые каналы
                    for (int i = 0; i < Convert.ToInt32(matches[1].Value); i++)
                    {
                        str = sr.ReadLine();
                        if (str != null)
                        {
                            string[] str1 = str.Split(',');
                            _oscil.ChannelNames.Add(Convert.ToString(str1[1]));
                            _oscil.Dimension.Add(Convert.ToString(str1[4]));
                        }
                    }
                    for (int i = 0; i < Convert.ToInt32(matches[2].Value); i++)
                    {
                        str = sr.ReadLine();
                        if (str != null)
                        {
                            string[] str1 = str.Split(',');
                            _oscil.ChannelNames.Add(Convert.ToString(str1[1]));
                        }
                        _oscil.Dimension.Add("NONE");
                    }
                    sr.ReadLine();
                    sr.ReadLine();
                    str = sr.ReadLine();
                    if (str != null)
                    {
                        string[] str2 = str.Split(',');
                        _oscil.SampleRate = Convert.ToDouble(str2[0]);
                        _oscil.NumCount = Convert.ToUInt32(str2[1]);
                    }
                    _oscil.StampDateStart = DateTime.Parse(sr.ReadLine());
                    _oscil.StampDateTrigger = DateTime.Parse(sr.ReadLine());
                    sr.Close();

                    var namefile = Path.GetFileNameWithoutExtension(ofd.FileName);
                    var pathfile = Path.GetDirectoryName(ofd.FileName);

                    string pathDateFile = pathfile + "\\" + namefile + ".dat";

                    StreamReader srd = new StreamReader(pathDateFile, Encoding.Default);
                    for (int j = 0; !srd.EndOfStream; j++)
                    {
                        str = srd.ReadLine();
                        if (str != null)
                        {
                            string[] str3 = str.Split(',');
                            _oscil.Data.Add(new List<double>());
                            for (int i = 2; i < str3.Length; i++)
                            {
                                str3[i] = str3[i].Replace(".", ",");
                                _oscil.Data[j].Add(Convert.ToDouble(str3[i]));
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

            OscilList.Add(_oscil);



            GraphObj.OscilConfigAdd(_oscil.OscilNames);
            for (int i = 0; i < _oscil.ChannelCount; i++)
            {
                Graph.AddGraph(i);
                GraphObj.GraphConfigAdd(_oscil.ChannelNames[i], _oscil.Dimension[i], OscilList.Count - 1, _oscil.TypeChannel[i]);
            }
        }

        public static GraphPanel Graph; 

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Graph = new GraphPanel();
            GrPanel.Child = Graph;
            OpenSetting();
            UpdateSetting();
        }

        private void OpenSetting()
        {
            Setting.InitSetting();
        }

        private void UpdateSetting()
        {
            LegendChange();
            AxisChange();
            LineInChash();
        }

        private void LegendChange()
        {
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
            Graph.LegendShow(checkedLegend, Convert.ToInt32(Setting.SizeLegend), h, v);
        }

        private void AxisChange()
        {
            Graph.GridAxisChange(Setting.XMinorShow, Setting.XMinor, 0);
            Graph.GridAxisChange(Setting.XMajorShow, Setting.XMajor, 1);
            Graph.GridAxisChange(Setting.YMinorShow, Setting.YMinor, 2);
            Graph.GridAxisChange(Setting.YMajorShow, Setting.YMajor, 3);
        }

        private void LineInChash()
        {
           Graph.PointInLineChange(Setting.PointInLine, Setting.ShowDigital);
        }
  
        Settings _settingsObj; 

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            _settingsObj = new Settings();
            _settingsObj.UpdatePointPerChannelTextBox();
            _settingsObj.Show();
        }

        private void AddGraph_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddGraph_MouseEnter(object sender, MouseEventArgs e)
        {
            AddGraph.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Chromatography-48(1).png")));
        }

        private void AddGraph_MouseLeave(object sender, MouseEventArgs e)
        {
            AddGraph.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Chromatography-48.png")));
        }

        public static bool CursorCreate;

        private void AddCoursor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AddCoursorEvent();
        }

        public void AddCoursorEvent() {
            if (OscilList.Count == 0) return;
            if (CursorCreate == false)
            {
                Graph.CursorClear();
                Graph.CursorAdd();
                AnalysisObj.AnalysisCursorAdd();
                CursorCreate = true;
                AddCoursor.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Line-48(1).png")));
            }
            else
            {
                Graph.CursorClear();
                AnalysisObj.AnalysisCursorClear();
                CursorCreate = false;
                AddCoursor.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Line-48.png")));
            }
        }

        private void AddCoursor_MouseEnter(object sender, MouseEventArgs e)
        {
            if (CursorCreate == false) AddCoursor.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Line-48.png")));
        }

        private void AddCoursor_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CursorCreate == false) AddCoursor.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Line-48(2).png")));
        }

        public static bool StampTriggerCreate;
        private void StampTrigger_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StampTriggerEvent();
        }

        public void StampTriggerEvent()
        {
            if (OscilList.Count == 0)
            {
                StampTrigger.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Horizontal Line-48(2).png")));
                return;
            }
            if (StampTriggerCreate == false)
            {
                Graph.StampTriggerClear();
                Graph.LineStampTrigger();
                StampTriggerCreate = true;
                StampTrigger.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Horizontal Line-48.png")));

            }
            else
            {
                Graph.StampTriggerClear();
                StampTriggerCreate = false;
                StampTrigger.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Horizontal Line-48(2).png")));

            }
        }



        private void StampTrigger_MouseEnter(object sender, MouseEventArgs e)
        {
            if (StampTriggerCreate == false) StampTrigger.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Horizontal Line-48(1).png")));
        }

        private void StampTrigger_MouseLeave(object sender, MouseEventArgs e)
        {
            if (StampTriggerCreate == false) StampTrigger.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Horizontal Line-48(2).png")));
        }

        bool _changeScale;

        private void ChangeScale_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_changeScale == false)
            {
                Graph.ChangeScale();
                _changeScale = true;
                ChangeScale.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Resize-48.png")));

            }
            else
            {
                Graph.ChangeScale();
                _changeScale = false;
                ChangeScale.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Width-48.png")));

            }
        }

        private void ChangeScale_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void ChangeScale_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
