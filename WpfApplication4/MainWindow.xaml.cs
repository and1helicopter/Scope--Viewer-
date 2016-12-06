using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;
using Microsoft.Win32;
using ZedGraph;
using System.Windows.Forms.Integration;
using System.Text.RegularExpressions;

namespace WpfApplication4
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

        Oscil oscil = new Oscil();
        public static List<Oscil> _oscilList = new List<Oscil>();

        Graph _graphObj = new Graph();
        Style _styleObj = new Style();
        public static Analysis AnalysisObj = new Analysis();
        bool _openWindow = false;
        bool _graphButtonStatus = false;
        bool _styleButtonStatus = false;
        bool _analysisButtonStatus = false;

        private void OpenAnimation()
        {
            DoubleAnimation openAnimation = new DoubleAnimation();
            openAnimation.From = 0;
            openAnimation.To = 250;
            openAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.0025));
            ConfigPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, openAnimation);
        }
        private void CloseAnimation()
        {
            DoubleAnimation closeAnimation = new DoubleAnimation();
            closeAnimation.From = 250;
            closeAnimation.To = 0;
            closeAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.0025));
            ConfigPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, closeAnimation);
        }

        private void ResetColorGraphButton()
        {
            GraphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            GraphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            GraphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void SetColorGraphButton()
        {
            GraphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            GraphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            GraphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
        private void ResetColorStyleButton()
        {
            StyleButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            StyleButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            StyleButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void SetColorStyleButton()
        {
            StyleButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            StyleButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            StyleButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
        private void ResetColorAnalysisButton()
        {
            AnalysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            AnalysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            AnalysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
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
            ConfigStackPanel.Children.Remove(_styleObj);
            ConfigStackPanel.Children.Remove(AnalysisObj);
            ResetColorStyleButton();
            ResetColorAnalysisButton();

            if (_graphButtonStatus == false)
            {
                _graphButtonStatus = true;
                SetColorGraphButton();
                if(_openWindow == false) OpenAnimation();
                _openWindow = true;
                ConfigPanel.Width = new GridLength(250, GridUnitType.Pixel);
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                ConfigStackPanel.Children.Add(_graphObj);

                return;
            }
            if (_graphButtonStatus == true)
            {
                _graphButtonStatus = false;
                ResetColorGraphButton();
                CloseAnimation();
                ConfigPanel.Width = new GridLength(0, GridUnitType.Pixel);
                ConfigStackPanel.Children.Remove(_graphObj);
                return;
            }
        }

        private void styleButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow();

            _graphButtonStatus = false;
            _analysisButtonStatus = false;
            ConfigStackPanel.Children.Remove(_graphObj);
            ConfigStackPanel.Children.Remove(AnalysisObj);
            ResetColorGraphButton();
            ResetColorAnalysisButton();

            if (_styleButtonStatus == false)
            {
                _styleButtonStatus = true;
                SetColorStyleButton();
                if(_openWindow == false)  OpenAnimation();
                _openWindow = true;
                ConfigPanel.Width = new GridLength(250, GridUnitType.Pixel);
                ConfigStackPanel.Children.Add(_styleObj);
                return;
            }
            if (_styleButtonStatus == true)
            {
                _styleButtonStatus = false;
                ResetColorStyleButton();
                CloseAnimation();
                ConfigPanel.Width = new GridLength(0, GridUnitType.Pixel);
                ConfigStackPanel.Children.Remove(_styleObj);
                return;
            }
        }

        private void analysisButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow();

            _graphButtonStatus = false;
            _styleButtonStatus = false;
            ConfigStackPanel.Children.Remove(_graphObj);
            ConfigStackPanel.Children.Remove(_styleObj);
            ResetColorGraphButton();
            ResetColorStyleButton();

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
            if (_analysisButtonStatus == true)
            {
                _analysisButtonStatus = false;
                ResetColorAnalysisButton();
                CloseAnimation();
                ConfigPanel.Width = new GridLength(0, GridUnitType.Pixel);
                ConfigStackPanel.Children.Remove(AnalysisObj);
                return;
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            string[] nameChannel = new string[32];
            string str, namefile, pathfile;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Текстовый файл|*.txt|Comtrade|*.cfg|All files (*.*)|*.*"; // Filter files by extension
            if (ofd.ShowDialog() == true)
            {
                //Graph.RemoveGraph();

               oscil = new Oscil();

                /* Oscil.ChannelNames.Clear();
                 Oscil.Dimension.Clear();
                 Oscil.TypeChannel.Clear();
                 Oscil.Data.Clear();

                 Graph.ClearListTemp();
                 */
                Graph.StampTriggerClear();
                 Graph.CursorClear();
                 
                AnalysisObj.AnalysisCursorClear();
                _cursorCreate = false;

                //Чтение .txt
                if (ofd.FilterIndex == 1)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.UTF8);
                        oscil.OscilNames = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                        oscil.StampDateTrigger = DateTime.Parse(sr.ReadLine());
                        oscil.SampleRate = Convert.ToDouble(sr.ReadLine());     //Частота выборки 
                        oscil.HistotyCount = Convert.ToDouble(sr.ReadLine());   //колличество на предысторию 
                        oscil.StampDateStart = oscil.StampDateTrigger.AddMilliseconds(-(100 * oscil.HistotyCount / oscil.SampleRate));
                        str = sr.ReadLine();
                        string[] str1 = str.Split('\t');
                        for(int i = 1; i < str1.Length - 1; i++) oscil.ChannelNames.Add(Convert.ToString(str1[i]));
                        oscil.ChannelCount = Convert.ToUInt16(oscil.ChannelNames.Count);
                        for(int j = 0; !sr.EndOfStream; j++) 
                        {
                            str = sr.ReadLine();
                            string[] str2 = str.Split('\t');
                            oscil.Data.Add(new List<double>());
                            for (int i = 1; i < str2.Length - 1; i++)
                            {
                                oscil.Data[j].Add(Convert.ToDouble(str2[i]));
                            }   
                        }
                        oscil.NumCount = Convert.ToUInt32(oscil.Data.Count);
                        for(int i = 0; i < oscil.ChannelCount; i++)
                        {
                            oscil.TypeChannel.Add(false);  //Значит сигнал аналоговый
                            oscil.Dimension.Add("NONE");
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
                        StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.Default);
                        sr.ReadLine();
                        str = sr.ReadLine();
                        Regex regex = new Regex(@"\d");
                        MatchCollection matches = regex.Matches(str);
                        oscil.ChannelCount = Convert.ToUInt16(matches[0].Value);
                        for(int i = 0; i < oscil.ChannelCount; i++)
                        {
                            if (i < Convert.ToInt32(matches[1].Value)) oscil.TypeChannel.Add(false);
                            else oscil.TypeChannel.Add(true);
                        }
                        //Аналоговые каналы
                        for (int i = 0; i < Convert.ToInt32(matches[1].Value); i++)
                        {
                            str = sr.ReadLine();
                            string[] str1 = str.Split(',');
                            oscil.ChannelNames.Add(Convert.ToString(str1[1]));
                            oscil.Dimension.Add(Convert.ToString(str1[4]));
                        }
                        for (int i = 0; i < Convert.ToInt32(matches[2].Value); i++)
                        {
                            str = sr.ReadLine();
                            string[] str1 = str.Split(',');
                            oscil.ChannelNames.Add(Convert.ToString(str1[1]));
                            oscil.Dimension.Add("NONE");
                        }
                        sr.ReadLine();
                        sr.ReadLine();
                        str = sr.ReadLine();
                        string[] str2 = str.Split(',');
                        oscil.SampleRate = Convert.ToDouble(str2[0]);
                        oscil.NumCount = Convert.ToUInt32(str2[1]);
                        oscil.StampDateStart = DateTime.Parse(sr.ReadLine());
                        oscil.StampDateTrigger = DateTime.Parse(sr.ReadLine()); 
                        sr.Close();

                        namefile = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                        pathfile = System.IO.Path.GetDirectoryName(ofd.FileName);

                        string pathDateFile = pathfile + "\\" + namefile + ".dat";

                        StreamReader srd = new StreamReader(pathDateFile, Encoding.Default);
                        for (int j = 0; !srd.EndOfStream; j++)
                        {
                            str = srd.ReadLine();
                            string[] str3= str.Split(',');
                            oscil.Data.Add(new List<double>());
                            for (int i = 2; i < str3.Length; i++)
                            {
                                str3[i] = str3[i].Replace(".",",");
                                oscil.Data[j].Add(Convert.ToDouble(str3[i]));
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

                _oscilList.Add(oscil);



                _graphObj.OscilConfigAdd(oscil.OscilNames);
                for (int i = 0; i < oscil.ChannelCount; i++)
                {
                    Graph.AddGraph(i);
                    _graphObj.GraphConfigAdd(oscil.ChannelNames[i], oscil.Dimension[i], _oscilList.Count - 1);
                }
            }
        }

        public static WpfApplication4.GraphPanel Graph; 

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Graph = new WpfApplication4.GraphPanel();
            GrPanel.Child = Graph;
        }

        bool _cursorCreate = false;

        private void cursor_Click(object sender, RoutedEventArgs e)
        {
            //graph.Cursor1;
            if (_cursorCreate == false)
            {
                Graph.CursorClear();
                Graph.CursorAdd();
                AnalysisObj.AnalysisCursorAdd();
                _cursorCreate = true;
            }
            else
            {
                Graph.CursorClear();
                AnalysisObj.AnalysisCursorClear();
                _cursorCreate = false;
            }
        }

        private void changescale_Click(object sender, RoutedEventArgs e)
        {
            Graph.ChangeScale();
        }

        bool _stampTriggerCreate = false;

        private void StampTrigger_Click(object sender, RoutedEventArgs e)
        {
            if (_stampTriggerCreate == false)
            {
                Graph.StampTriggerClear();
                Graph.LineStampTrigger();
                _stampTriggerCreate = true;
            }
            else
            {
                Graph.StampTriggerClear();
                _stampTriggerCreate = false;
            }
        }

        Settings _settingsObj; 

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            _settingsObj = new Settings();
            _settingsObj.UpdatePointPerChannelTextBox();
            _settingsObj.Show();
        }
    }
}
