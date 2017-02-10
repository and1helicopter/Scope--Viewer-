using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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

            SrartSettings();
        }

        private void SrartSettings()
        {
            //Проверка на существование настроек
            Setting.InitSetting();

            WindowState = Setting.WondowState == 1 ? WindowState.Maximized : WindowState.Minimized;
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
        bool _styleButtonStatus;
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
            if (!Setting.LoadSetting)
            {
                MessageBox.Show("Не удалось открыть файл настроек!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning );
                return;
            }

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
                    _oscil.StampDateStart = _oscil.StampDateTrigger.AddMilliseconds(-(1000 * _oscil.HistotyCount / _oscil.SampleRate));
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
                    _oscil.StampDateEnd = _oscil.StampDateTrigger.AddMilliseconds( 1000 * (_oscil.NumCount - _oscil.HistotyCount) / _oscil.SampleRate);
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
                    _oscil.HistotyCount = Convert.ToInt32((_oscil.StampDateTrigger.Second - _oscil.StampDateStart.Second + 
                        (double)(_oscil.StampDateTrigger.Millisecond - _oscil.StampDateStart.Millisecond) / 1000) * _oscil.SampleRate);
                    _oscil.StampDateEnd = _oscil.StampDateTrigger.AddMilliseconds(1000 * (_oscil.NumCount - _oscil.HistotyCount) / _oscil.SampleRate);
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
            {
                oscilGraph.GraphConfigAdd(oscil.ChannelNames[i], oscil.Dimension[i], oscil.TypeChannel[i]);
            }
                
            OscilChannelList.Add(oscilGraph);

            GraphObj.GraphStackPanel.Children.Add(oscilGraph.LayoutOscilPanel);
            for (int i = 0; i < oscil.ChannelCount; i++)
            {
                GraphObj.GraphStackPanel.Children.Add(oscilGraph.LayoutPanel[i]);
            }

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
            SettingsObj = new Settings
            {
                Topmost = true
            };
            SettingsObj.Show();
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
                    _oscil.Dimension.Add(OscilList[k].Dimension[i]);

                    if (_oscil.ChannelCount > 0 && ((Math.Abs(_oscil.SampleRate - OscilList[k].SampleRate) > 0) || (Math.Abs(_oscil.HistotyCount - OscilList[k].HistotyCount) > 0) || (_oscil.NumCount != OscilList[k].NumCount)))
                    {
                        MessageBox.Show("Каналы не совместимы", "Ошибка",MessageBoxButton.OK);
                        return;
                    }

                    if(_oscil.ChannelCount == 0)
                    {
                        _oscil.StampDateTrigger = OscilList[k].StampDateTrigger;
                        _oscil.SampleRate = OscilList[k].SampleRate;
                        _oscil.HistotyCount = OscilList[k].HistotyCount;
                        _oscil.NumCount = OscilList[k].NumCount;
                        _oscil.StampDateStart = _oscil.StampDateTrigger.AddMilliseconds(-(1000 * _oscil.HistotyCount / _oscil.SampleRate));
                        _oscil.StampDateEnd =
                            _oscil.StampDateTrigger.AddMilliseconds(1000 * (_oscil.NumCount - _oscil.HistotyCount)/_oscil.SampleRate);

                        for (int j = 0; j < _oscil.NumCount; j++)
                            _oscil.Data.Add(new List<double>());
                    }

                    _oscil.ChannelCount += 1;
                    _oscil.TypeChannel.Add(OscilList[k].TypeChannel[i]);

                    for (int j = 0; j < _oscil.NumCount; j++)
                    {
                        _oscil.Data[j].Add(OscilList[k].Data[j][i]);
                    }
                    OscilChannelList[k].SelectCheckBox[i].IsChecked = false;
                }
            }

            if (_oscil.ChannelCount == 0) return;

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
                    if (OscilChannelList[numOsc].SelectCheckBox[numCh].IsChecked == true)
                    {
                        GraphPanelList[numOsc].AddDigitalChannel(numCh,numOsc, System.Drawing.Color.Blue);
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
                MessageBox.Show("Нечего объединять", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error );
                return;
            }
            if (Math.Abs(OscilList[scope1].SampleRate - OscilList[scope2].SampleRate) > 0)  //Проверка на совпадение частоты выборки 
            {
                MessageBox.Show("Каналы не совместимы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error );
                return;
            }
            if (!scopeIndex1.SequenceEqual(scopeIndex2)) //Проверка на соответствие каналов к объединению
            {
                MessageBox.Show("Выбранные каналы неляьзя объеденить", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error );
                return;
            }

            double time1 = Math.Abs(OscilList[scope1].StampDateEnd.Second - OscilList[scope2].StampDateStart.Second + (double)(OscilList[scope1].StampDateEnd.Millisecond - OscilList[scope2].StampDateStart.Millisecond) / 1000);
            double time2 = Math.Abs(OscilList[scope2].StampDateEnd.Second - OscilList[scope1].StampDateStart.Second + (double)(OscilList[scope2].StampDateEnd.Millisecond - OscilList[scope1].StampDateStart.Millisecond) / 1000);


            if (time1 > maxGapCount / OscilList[scope1].SampleRate && time2 > maxGapCount / OscilList[scope1].SampleRate) //Проверка на временной отсуп между двумя осциллограммами 
            {
                MessageBox.Show("Интервал времени между объединяймыми каналами слишком велик", "Ошибка", MessageBoxButton.OK,MessageBoxImage.Error );
                return;
            }

            //Объединение
            int firstScope = OscilList[scope1].StampDateTrigger < OscilList[scope2].StampDateTrigger ? scope1 : scope2;
            int secondScope = OscilList[scope1].StampDateTrigger > OscilList[scope2].StampDateTrigger ? scope1 : scope2;
            int numCountTemp = Convert.ToInt32(((OscilList[secondScope].StampDateStart.Second - OscilList[firstScope].StampDateEnd.Second) 
                + (double)(OscilList[secondScope].StampDateStart.Millisecond - OscilList[firstScope].StampDateEnd.Millisecond)/1000) * OscilList[firstScope].SampleRate);

            _oscil = new Oscil
            {
                OscilNames = "Осциллограмма объединенная",
                StampDateTrigger = OscilList[firstScope].StampDateTrigger,
                StampDateStart = OscilList[firstScope].StampDateTrigger.AddMilliseconds(-(1000 * OscilList[firstScope].HistotyCount / OscilList[firstScope].SampleRate)),
                StampDateEnd = OscilList[firstScope].StampDateTrigger.AddMilliseconds(1000 * (OscilList[firstScope].NumCount + OscilList[secondScope].NumCount - OscilList[firstScope].HistotyCount) / OscilList[firstScope].SampleRate),
                SampleRate = OscilList[firstScope].SampleRate,
                HistotyCount = OscilList[firstScope].HistotyCount,
                NumCount = OscilList[firstScope].NumCount + OscilList[secondScope].NumCount + Convert.ToUInt32(numCountTemp)
            };

            for (int j = 0; j < _oscil.NumCount; j++)
            {
                _oscil.Data.Add(new List<double>());
            }

            foreach (int scopeIndex in scopeIndex1)
            {
                _oscil.ChannelNames.Add(OscilList[firstScope].ChannelNames[scopeIndex]);
                _oscil.Dimension.Add(OscilList[firstScope].Dimension[scopeIndex]);
                _oscil.TypeChannel.Add(OscilList[firstScope].TypeChannel[scopeIndex]);

                for (int j = 0; j < OscilList[firstScope].NumCount; j++)
                {
                    _oscil.Data[j].Add(OscilList[firstScope].Data[j][scopeIndex]);
                }

                for (int j = 0; j < numCountTemp; j++)
                {
                    _oscil.Data[j + Convert.ToInt32(OscilList[firstScope].NumCount)].Add(OscilList[firstScope].Data[OscilList[firstScope].Data.Count - 1][scopeIndex]);
                }

                for (int j = 0; j < OscilList[secondScope].NumCount; j++)
                {
                    _oscil.Data[j + numCountTemp + Convert.ToInt32(OscilList[firstScope].NumCount)].Add(OscilList[secondScope].Data[j][scopeIndex]);
                }

                _oscil.ChannelCount += 1;
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
