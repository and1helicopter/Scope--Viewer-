using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Xceed.Wpf.AvalonDock.Layout;


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
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static List<Oscil> OscilList = new List<Oscil>();
        public static List<OscilGraph> OscilChannelList = new List<OscilGraph>();
        public static List<GraphPanel> GraphPanelList = new List<GraphPanel>();
        public static List<WindowsFormsHost> WindowsFormsHostList = new List<WindowsFormsHost>();
        public static List<LayoutDocument> LayoutDocumentList = new List<LayoutDocument>();

        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static Graph GraphObj = new Graph();
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static Analysis AnalysisObj = new Analysis();

        Settings _settingsObj;

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
                if (OscilChannelList.Count == 0)
                {
                    return;
                }

                _graphButtonStatus = true;
                SetColorGraphButton();
                if(_openWindow == false) OpenAnimation();
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
            OpenWindow();

            _graphButtonStatus = false;

            ConfigStackPanel.Children.Remove(GraphObj);
            ResetColorGraphButton();

            if (_analysisButtonStatus == false)
            {
                if (OscilChannelList.Count == 0)
                {
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

            AddOscilChannnel(_oscil, false);
        }

        private void AddOscilChannnel(Oscil oscil, bool dig)
        {
            OscilGraph oscilGraph = new OscilGraph();

            oscilGraph.OscilConfigAdd(oscil.OscilNames);
            for (int i = 0; i < oscil.ChannelCount; i++)
                oscilGraph.GraphConfigAdd(oscil.ChannelNames[i], oscil.Dimension[i], oscil.TypeChannel[i]);

            OscilChannelList.Add(oscilGraph);

            GraphObj.GraphStackPanel.Children.Add(oscilGraph.LayoutOscilPanel);
            for (int i = 0; i < oscil.ChannelCount; i++)
                GraphObj.GraphStackPanel.Children.Add(oscilGraph.LayoutPanel[i]);

            GraphPanelList.Add(new GraphPanel());

            SetSetting();

            WindowsFormsHostList.Add(new WindowsFormsHost());
            LayoutDocumentList.Add(new LayoutDocument());
            LayoutDocumentList[LayoutDocumentList.Count - 1].Content = WindowsFormsHostList[WindowsFormsHostList.Count - 1];
            LayoutDocumentList[LayoutDocumentList.Count - 1].Title = oscil.OscilNames;
            LayoutDocumentList[LayoutDocumentList.Count - 1].CanFloat = true;
            LayoutDocumentList[LayoutDocumentList.Count - 1].CanClose = false;
            
            LayoutGraph.Children.Add(LayoutDocumentList[LayoutDocumentList.Count - 1]);
            WindowsFormsHostList[WindowsFormsHostList.Count - 1].Child = GraphPanelList[GraphPanelList.Count - 1];

            for (int i = 0; i < OscilChannelList[OscilChannelList.Count - 1].NameLabel.Count; i++)
            {
                SolidColorBrush brush = (SolidColorBrush) OscilChannelList[OscilChannelList.Count - 1].ColorEllipse[i].Fill;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
                GraphPanelList[GraphPanelList.Count - 1].AddGraph(i, color, dig);
            }
        }

        private void SetSetting()
        {
            Setting.InitSetting();
            GraphPanelList[GraphPanelList.Count - 1].PointInLineChange(Setting.PointInLine, Setting.ShowDigital);
            if(Setting.PointInLine < 50) GraphPanelList[GraphPanelList.Count - 1].PointInLineChange(50, Setting.ShowDigital);
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
            _settingsObj = new Settings();
            _settingsObj.UpdatePointPerChannelTextBox();
            _settingsObj.Show();
        }

        public static void DelateOscil(int i)
        {
            OscilList.Remove(OscilList[i]);
            GraphPanelList[i].DelCursor();

            GraphObj.GraphStackPanel.Children.Remove(OscilChannelList[i].LayoutOscilPanel);
            for (int j = OscilChannelList[i].LayoutPanel.Count - 1; j >= 0; j--)
                GraphObj.GraphStackPanel.Children.Remove(OscilChannelList[i].LayoutPanel[j]);
            OscilChannelList.Remove(OscilChannelList[i]);
            GraphPanelList.Remove(GraphPanelList[i]);
            WindowsFormsHostList.Remove(WindowsFormsHostList[i]);
            LayoutDocumentList[i].Close();
            LayoutDocumentList.Remove(LayoutDocumentList[i]);

            while (i < OscilChannelList.Count)
            {
                OscilChannelList[i].OscilName.Content = "Осциллограмма №" + (i + 1);

                    i++;
            }


        }

        private void AddGraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Создание новой осциллограммы OscilList
            _oscil = new Oscil();

            _oscil.OscilNames += 1;

            for (int k = 0; k < OscilChannelList.Count; k++)
            {
                for (int i = 0; i < OscilChannelList[k].TypeComboBox.Count; i++)
                {
                    if (OscilChannelList[k].SelectCheckBox[i].IsChecked == true)
                    {
                        _oscil.ChannelNames.Add(OscilList[k].ChannelNames[i]);
                        _oscil.Dimension.Add(OscilList[k].Dimension[i]);

                        if (_oscil.ChannelCount > 0 && ((_oscil.SampleRate != OscilList[k].SampleRate) || (_oscil.HistotyCount != OscilList[k].HistotyCount) || (_oscil.NumCount != OscilList[k].NumCount)))
                        {
                            MessageBox.Show("Каналы не совместимы", "Ошибка",MessageBoxButton.OK);
                            return;
                        }

                        if(_oscil.ChannelCount == 0)
                        {
                            // StampDateStart;
                            // StampDateTrigger;
                            // StampDateEnd;
                            // SampleRate;
                            // HistotyCount;

                            _oscil.StampDateTrigger = OscilList[k].StampDateTrigger;
                            _oscil.SampleRate = OscilList[k].SampleRate;
                            _oscil.HistotyCount = OscilList[k].HistotyCount;
                            _oscil.NumCount = OscilList[k].NumCount;
                            _oscil.StampDateStart = _oscil.StampDateTrigger.AddMilliseconds(-(100 * _oscil.HistotyCount / _oscil.SampleRate));

                            for (int j = 0; j < _oscil.NumCount; j++)
                                _oscil.Data.Add(new List<double>());
                        }

                        _oscil.ChannelCount += 1;
                        _oscil.TypeChannel.Add(OscilList[k].TypeChannel[i]);

                        for (int j = 0; j < _oscil.NumCount; j++)
                        {
                            _oscil.Data[j].Add(OscilList[k].Data[j][i]);
                        }
                    }
                }
            }

            if (_oscil.ChannelCount == 0) return;

            OscilList.Add(_oscil);

            AddOscilChannnel(_oscil, false);
        }

        private void AddGraph_MouseEnter(object sender, MouseEventArgs e)
        {
            AddGraph.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Chromatography-48(1).png")));
        }

        private void AddGraph_MouseLeave(object sender, MouseEventArgs e)
        {
            AddGraph.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Chromatography-48.png")));
        }

        private void AddDigitalChannel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            for (int k = 0; k < OscilChannelList.Count; k++)
            {
                for (int i = 0; i < OscilChannelList[k].TypeComboBox.Count; i++)
                {
                    if (OscilChannelList[k].SelectCheckBox[i].IsChecked == true)
                    {


                        GraphPanelList[k].AddDigitalChannel(i,k, System.Drawing.Color.Blue);
                        break;
                    }
                }
            }
        }

        private void AddDigitalChannel_MouseEnter(object sender, MouseEventArgs e)
        {
            AddDigitalChannel.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Cancel 4 Digits-48(1).png")));
        }

        private void AddDigitalChannel_MouseLeave(object sender, MouseEventArgs e)
        {
            AddDigitalChannel.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/Cancel 4 Digits-48.png")));
        }
    
    }
}
