using System;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace ScopeViewer
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings 
    {
        public Settings()
        {
            InitializeComponent();
            ReadSetting();
            UpdateSetting();
        }

        public static void SaveSettingBase()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "ViewerSettings.xml"
            };

            FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
            XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.Unicode)
            {
                Formatting = Formatting.Indented
            };

            xmlOut.WriteStartDocument();
            xmlOut.WriteStartElement("Setup");
            /////////////////////////////////////////
            //Settings menu Graph
            xmlOut.WriteStartElement("Graph");
            /////////////////////////////////////////
            xmlOut.WriteStartElement("ShowDigital", (Setting.ShowDigital).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("PointInLine", (Setting.PointInLine).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMinor", (Setting.XMinor).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMinorShow", (Setting.XMinorShow).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMinor", (Setting.YMinor).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMinorShow", (Setting.YMinorShow).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMajor", (Setting.XMajor).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMajorShow", (Setting.XMajorShow).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMajor", (Setting.YMajor).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMajorShow", (Setting.YMajorShow).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("ShowLegend", (Setting.ShowLegend).ToString());
            xmlOut.WriteEndElement();

            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
            xmlOut.WriteStartElement("SizeLegend", (Setting.SizeLegend).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("Position", (Setting.Position).ToString());
            xmlOut.WriteEndElement();

            /////////////////////////////////////////
            xmlOut.WriteEndElement();

            /////////////////////////////////////////
            //Settings Window Application
            xmlOut.WriteStartElement("Window");
            /////////////////////////////////////////
            xmlOut.WriteStartElement("WindowHeight", (MainWindow.MainWindowHeight).ToString("####"));
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("WindowWidth", (MainWindow.MainWindowWidth).ToString("####"));
            xmlOut.WriteEndElement();

            if (MainWindow.MainWindowState == 1)
            {
                xmlOut.WriteStartElement("WondowState", (MainWindow.MainWindowState).ToString());
                xmlOut.WriteEndElement();
            }
            else
            {
                xmlOut.WriteStartElement("WondowState", (MainWindow.MainWindowState).ToString());
                xmlOut.WriteEndElement();
            }

            ///////////////////////////////////////// 
            xmlOut.WriteEndElement();
            /////////////////////////////////////////
            xmlOut.WriteEndElement();
            xmlOut.WriteEndDocument();
            xmlOut.Close();
            fs.Close();
        }

        private void SaveSetting()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FileName = "ViewerSettings.xml"
            };

            FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
            XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.Unicode)
            {
                Formatting = Formatting.Indented
            };

            xmlOut.WriteStartDocument();
            xmlOut.WriteStartElement("Setup");
            /////////////////////////////////////////
            //Settings menu Graph
            xmlOut.WriteStartElement("Graph");
            /////////////////////////////////////////
            xmlOut.WriteStartElement("ShowDigital", (ShowDigitalCheckBox.IsChecked).ToString());
            xmlOut.WriteEndElement();

            // ReSharper disable once RedundantToStringCall
            if (PointInLineComboBox.SelectedIndex == 0)
            {
                xmlOut.WriteStartElement("PointInLine", (150).ToString());
                xmlOut.WriteEndElement();
            }
            else if (PointInLineComboBox.SelectedIndex == 1)
            {
                xmlOut.WriteStartElement("PointInLine", (400).ToString());
                xmlOut.WriteEndElement();
            }
            else if (PointInLineComboBox.SelectedIndex == 2)
            {
                xmlOut.WriteStartElement("PointInLine", (1000).ToString());
                xmlOut.WriteEndElement();
            }
            else
            {
                xmlOut.WriteStartElement("PointInLine", (1000000).ToString());
                xmlOut.WriteEndElement();
            }
            
            xmlOut.WriteStartElement("XMinor", (XMinorComboBox.SelectedIndex).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMinorShow", (XMinorCheckBox.IsChecked).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMinor", (YMinorComboBox.SelectedIndex).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMinorShow", (YMinorCheckBox.IsChecked).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMajor", (XMajorComboBox.SelectedIndex).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("XMajorShow", (XMajorCheckBox.IsChecked).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMajor", (YMajorComboBox.SelectedIndex).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("YMajorShow", (YMajorCheckBox.IsChecked).ToString());
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("ShowLegend", (ShowLegendCheckBox.IsChecked).ToString());
            xmlOut.WriteEndElement();

            // ReSharper disable once SpecifyACultureInStringConversionExplicitly
            xmlOut.WriteStartElement("SizeLegend", Convert.ToInt32(ShowLegendSlider.Value).ToString());
            xmlOut.WriteEndElement();

            {
                if(Position0.IsChecked == true)
                {
                    xmlOut.WriteStartElement("Position", (0).ToString());
                    xmlOut.WriteEndElement();
                }
                if (Position1.IsChecked == true)
                {
                    xmlOut.WriteStartElement("Position", (1).ToString());
                    xmlOut.WriteEndElement();
                }
                if (Position2.IsChecked == true)
                {
                    xmlOut.WriteStartElement("Position", (2).ToString());
                    xmlOut.WriteEndElement();
                }
                if (Position3.IsChecked == true)
                {
                    xmlOut.WriteStartElement("Position", (3).ToString());
                    xmlOut.WriteEndElement();
                }
            }
            /////////////////////////////////////////
            xmlOut.WriteEndElement();

            /////////////////////////////////////////
            //Settings Window Application
            xmlOut.WriteStartElement("Window");
            /////////////////////////////////////////
            xmlOut.WriteStartElement("WindowHeight", (MainWindow.MainWindowHeight).ToString("####"));
            xmlOut.WriteEndElement();

            xmlOut.WriteStartElement("WindowWidth", (MainWindow.MainWindowWidth).ToString("####"));
            xmlOut.WriteEndElement();

            if (MainWindow.MainWindowState == 1)
            {
                xmlOut.WriteStartElement("WondowState", (MainWindow.MainWindowState).ToString());
                xmlOut.WriteEndElement();
            }
            else
            {
                xmlOut.WriteStartElement("WondowState", (MainWindow.MainWindowState).ToString());
                xmlOut.WriteEndElement();
            }

            ///////////////////////////////////////// 
            xmlOut.WriteEndElement();
            /////////////////////////////////////////
            xmlOut.WriteEndElement();
            xmlOut.WriteEndDocument();
            xmlOut.Close();
            fs.Close();
        }

        private void ReadSetting()
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
            if (Setting.ShowLegend)
            {
                ShowLegendCheckBox.IsChecked = true;
                checkedLegend = true;
            }
            if (Setting.Position == 2)
            {
                Position2.IsChecked = true;
                h = 0; v = 0;
            }
            if (Setting.Position == 1)
            {
                Position1.IsChecked = true;
                h = 1; v = 0;
            }
            if (Setting.Position == 3)
            {
                Position3.IsChecked = true;
                h = 0; v = 1;
            }
            if (Setting.Position == 0)
            {
                Position0.IsChecked = true;
                h = 1; v = 1;
            }
            ShowLegendSlider.Value = Setting.SizeLegend;
            foreach (GraphPanel t in MainWindow.GraphPanelList)
            {
                t.LegendShow(checkedLegend, Convert.ToInt32(Setting.SizeLegend), h, v);
            }
        }

        private void AxisChange()
        {
            foreach (GraphPanel t in MainWindow.GraphPanelList)
            {
                XMinorCheckBox.IsChecked = Setting.XMinorShow;
                XMinorComboBox.SelectedIndex = Setting.XMinor;
                t.GridAxisChange(Setting.XMinorShow, Setting.XMinor, 0);

                XMajorCheckBox.IsChecked = Setting.XMajorShow;
                XMajorComboBox.SelectedIndex = Setting.XMajor;
                t.GridAxisChange(Setting.XMajorShow, Setting.XMajor, 1);

                YMinorCheckBox.IsChecked = Setting.YMinorShow;
                YMinorComboBox.SelectedIndex = Setting.YMinor;
                t.GridAxisChange(Setting.YMinorShow, Setting.YMinor, 2);

                YMajorCheckBox.IsChecked = Setting.YMajorShow;
                YMajorComboBox.SelectedIndex = Setting.YMajor;
                t.GridAxisChange(Setting.YMajorShow, Setting.YMajor, 3);
            }
        }

        private void LineInChash()
        {
            ShowDigitalCheckBox.IsChecked = Setting.ShowDigital;
            //    PointInLineTextBox.Text = Convert.ToString(Setting.PointInLine);

            if (Setting.PointInLine <= 150)
            {
                foreach (GraphPanel t in MainWindow.GraphPanelList)
                {
                    PointInLineComboBox.SelectedIndex = 0;
                    t.PointInLineChange(150, Setting.ShowDigital);
                    Setting.PointInLine = 150;
                }
            }

            else if (Setting.PointInLine <= 400)
            {
                foreach (GraphPanel t in MainWindow.GraphPanelList)
                {
                    PointInLineComboBox.SelectedIndex = 1;
                    t.PointInLineChange(400, Setting.ShowDigital);
                    Setting.PointInLine = 400;
                }
            }

            else if (Setting.PointInLine <= 1000)
            {
                foreach (GraphPanel t in MainWindow.GraphPanelList)
                {
                    PointInLineComboBox.SelectedIndex = 2;
                    t.PointInLineChange(1000, Setting.ShowDigital);
                    Setting.PointInLine = 1000;
                }
            }

            if (Setting.PointInLine > 1000)
            {
                foreach (GraphPanel t in MainWindow.GraphPanelList)
                {
                    PointInLineComboBox.SelectedIndex = 3;
                    t.PointInLineChange(1000000, Setting.ShowDigital);
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {     
            TabControl.SelectedIndex = ((TreeViewItem)e.NewValue).TabIndex;
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            SaveSetting();
            ReadSetting();
            UpdateSetting();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SaveSetting();
            ReadSetting();
            UpdateSetting();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.SettingsObj = null;
        }

        /*
        private void ShowDigitalCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Setting.ShowDigital = ShowDigitalCheckBox.IsChecked == true;
        }
        */
    }
}
