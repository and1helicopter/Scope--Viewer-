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

        Graph graphObj = new Graph();
        Style styleObj = new Style();
        Analysis analysisObj = new Analysis();
        bool graphButtonStatus = false;
        bool styleButtonStatus = false;
        bool analysisButtonStatus = false;

        private void OpenAnimation()
        {
            DoubleAnimation OpenAnimation = new DoubleAnimation();
            OpenAnimation.From = 0;
            OpenAnimation.To = 200;
            OpenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.01));
            configPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, OpenAnimation);
        }
        private void CloseAnimation()
        {
            DoubleAnimation CloseAnimation = new DoubleAnimation();
            CloseAnimation.From = 200;
            CloseAnimation.To = 0;
            CloseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.01));
            configPanel.BeginAnimation(ColumnDefinition.MinWidthProperty, CloseAnimation);
        }

        private void resetColorGraphButton()
        {
            graphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            graphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            graphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void setColorGraphButton()
        {
            graphButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            graphButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            graphButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
        private void resetColorStyleButton()
        {
            styleButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            styleButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            styleButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void setColorStyleButton()
        {
            styleButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            styleButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            styleButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }
        private void resetColorAnalysisButton()
        {
            analysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            analysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            analysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
        }
        private void setColorrAnalysisButton()
        {
            analysisButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            analysisButton.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2196F3"));
            analysisButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        }

        private void graphButton_Click(object sender, RoutedEventArgs e)
        {
            styleButtonStatus = false;
            analysisButtonStatus = false;
            configStackPanel.Children.Remove(styleObj);
            configStackPanel.Children.Remove(analysisObj);
            resetColorStyleButton();
            resetColorAnalysisButton();

            if (graphButtonStatus == false)
            {
                graphButtonStatus = true;
                setColorGraphButton();
                OpenAnimation();
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(graphObj);

                return;
            }
            if (graphButtonStatus == true)
            {
                graphButtonStatus = false;
                resetColorGraphButton();
                CloseAnimation();
                //configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(graphObj);
                return;
            }
        }

        private void styleButton_Click(object sender, RoutedEventArgs e)
        {
            graphButtonStatus = false;
            analysisButtonStatus = false;
            configStackPanel.Children.Remove(graphObj);
            configStackPanel.Children.Remove(analysisObj);
            resetColorGraphButton();
            resetColorAnalysisButton();

            if (styleButtonStatus == false)
            {
                styleButtonStatus = true;
                setColorStyleButton();
                OpenAnimation();
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(styleObj);
                return;
            }
            if (styleButtonStatus == true)
            {
                styleButtonStatus = false;
                resetColorStyleButton();
                CloseAnimation();
                //configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(styleObj);
                return;
            }
        }

        private void analysisButton_Click(object sender, RoutedEventArgs e)
        {
            graphButtonStatus = false;
            styleButtonStatus = false;
            configStackPanel.Children.Remove(graphObj);
            configStackPanel.Children.Remove(styleObj);
            resetColorGraphButton();
            resetColorStyleButton();

            if (analysisButtonStatus == false)
            {
                analysisButtonStatus = true;
                setColorrAnalysisButton();
                OpenAnimation();
                //configPanel.Width = new GridLength(150, GridUnitType.Pixel);
                configStackPanel.Children.Add(analysisObj);
                return;
            }
            if (analysisButtonStatus == true)
            {
                analysisButtonStatus = false;
                resetColorAnalysisButton();
                CloseAnimation();
                //configPanel.Width = new GridLength(0, GridUnitType.Pixel);
                configStackPanel.Children.Remove(analysisObj);
                return;
            }
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            string[] NameChannel = new string[32];
            string str, namefile, pathfile;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Текстовый файл|*.txt|Comtrade|*.cfg|All files (*.*)|*.*"; // Filter files by extension
            if (ofd.ShowDialog() == true)
            {
                graph.removeGraph();


                Oscil.ChannelNames.Clear();
                Oscil.Dimension.Clear();
                Oscil.TypeChannel.Clear();
                Oscil.Data.Clear();


                //Чтение .txt
                if (ofd.FilterIndex == 1)
                {
                    try
                    {
                        StreamReader sr = new StreamReader(ofd.FileName, System.Text.Encoding.UTF8);
                        Oscil.StampDateTrigger = DateTime.Parse(sr.ReadLine());
                        Oscil.SampleRate = Convert.ToDouble(sr.ReadLine());
                        str = sr.ReadLine();
                        string[] str1 = str.Split('\t');
                        for(int i = 1; i < str1.Length - 1; i++) Oscil.ChannelNames.Add(Convert.ToString(str1[i]));
                        Oscil.ChannelCount = Convert.ToUInt16(Oscil.ChannelNames.Count);
                        for(int i = 0; i < 8; i++) sr.ReadLine();
                        for(int j = 0; !sr.EndOfStream; j++) 
                        {
                            str = sr.ReadLine();
                            string[] str2 = str.Split('\t');
                            Oscil.Data.Add(new List<double>());
                            for (int i = 1; i < str2.Length - 1; i++)
                            {
                                Oscil.Data[j].Add(Convert.ToDouble(str2[i]));
                            }   
                        }
                        Oscil.NumCount = Convert.ToUInt32(Oscil.Data.Count);
                        for(int i = 0; i < Oscil.ChannelCount; i++)
                        {
                            Oscil.TypeChannel.Add(false);  //Значит сигнал аналоговый
                            Oscil.Dimension.Add("NONE");
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
                        Oscil.ChannelCount = Convert.ToUInt16(matches[0].Value);
                        for(int i = 0; i < Oscil.ChannelCount; i++)
                        {
                            if (i < Convert.ToInt32(matches[1].Value)) Oscil.TypeChannel.Add(false);
                            else Oscil.TypeChannel.Add(true);
                        }
                        //Аналоговые каналы
                        for (int i = 0; i < Convert.ToInt32(matches[1].Value); i++)
                        {
                            str = sr.ReadLine();
                            string[] str1 = str.Split(',');
                            Oscil.ChannelNames.Add(Convert.ToString(str1[1]));
                            Oscil.Dimension.Add(Convert.ToString(str1[4]));
                        }
                        for (int i = 0; i < Convert.ToInt32(matches[2].Value); i++)
                        {
                            str = sr.ReadLine();
                            string[] str1 = str.Split(',');
                            Oscil.ChannelNames.Add(Convert.ToString(str1[1]));
                            Oscil.Dimension.Add("NONE");
                        }
                        sr.ReadLine();
                        sr.ReadLine();
                        str = sr.ReadLine();
                        string[] str2 = str.Split(',');
                        Oscil.SampleRate = Convert.ToDouble(str2[0]);
                        Oscil.NumCount = Convert.ToUInt32(str2[1]);
                        Oscil.StampDateStart = DateTime.Parse(sr.ReadLine());
                        Oscil.StampDateTrigger = DateTime.Parse(sr.ReadLine()); 
                        sr.Close();

                        namefile = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                        pathfile = System.IO.Path.GetDirectoryName(ofd.FileName);

                        string pathDateFile = pathfile + "\\" + namefile + ".dat";

                        StreamReader srd = new StreamReader(pathDateFile, System.Text.Encoding.Default);
                        for (int j = 0; !srd.EndOfStream; j++)
                        {
                            str = srd.ReadLine();
                            string[] str3= str.Split(',');
                            Oscil.Data.Add(new List<double>());
                            for (int i = 2; i < str3.Length; i++)
                            {
                                str3[i] = str3[i].Replace(".",",");
                                Oscil.Data[j].Add(Convert.ToDouble(str3[i]));
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

                for(int i = 0; i < Oscil.ChannelCount; i++)
                {
                    graphObj.GraphConfigClear();
                }

                for (int i = 0; i < Oscil.ChannelCount; i++)
                {
                    graph.addGraph(i);
                    graphObj.GraphConfigAdd(Oscil.ChannelNames[i], Oscil.Dimension[i]);
                }
            }
        }

        public static WpfApplication4.GraphPanel graph; 

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            graph = new WpfApplication4.GraphPanel();
            GrPanel.Child = graph;
        }

        private void cursor_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
