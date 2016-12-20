﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZedGraph;
using Border = System.Windows.Controls.Border;
using Brushes = System.Windows.Media.Brushes;
using Label = System.Windows.Controls.Label;

namespace ScopeViewer
{
    public class OscilAnalysis
    {
        /// <summary>
        /// Interaction logic for Analysis.xaml
        /// </summary>
        public readonly List<DockPanel> LayoutPanel = new List<DockPanel>();

        private readonly Label _nameLabel = new Label();
        private readonly Border _panelBorder = new Border();

        private readonly List<Label> _nameChannelLabel = new List<Label>();
        private readonly List<Label> _nameStatuslLabel = new List<Label>();
        private readonly List<Label> _nameValue1Label = new List<Label>();
        private readonly List<Label> _nameValue2Label = new List<Label>();

        public void AnalysisCursorAdd(int numOsc)
        {
            _panelBorder.BorderBrush = Brushes.DarkGray;
            _panelBorder.BorderThickness = new Thickness(1.0);
            _panelBorder.Margin = new Thickness(-195, 0, 0, 0);

            LayoutPanel.Add(new DockPanel());
            LayoutPanel[0].Width = 225;
            LayoutPanel[0].Height = 25;
            LayoutPanel[0].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[0].MouseDown += click_LayoutPanelCursor;

            _nameLabel.Content = "Курсоры";
            _nameLabel.VerticalAlignment = VerticalAlignment.Top;
            _nameLabel.FontSize = 12;
            _nameLabel.Height = 25;
            _nameLabel.Width = 180;
            _nameLabel.TabIndex = 0;
            _nameLabel.ToolTip = "Положения курсоров";
            _nameLabel.Margin = new Thickness(0, 0, 0, 0);

            double positonY = 0;
            {
                _nameStatuslLabel.Add(new Label());
                _nameStatuslLabel[0].Content = "Курсор 1                  Курсор 2";
                _nameStatuslLabel[0].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[0].FontSize = 12;
                _nameStatuslLabel[0].Height = 25;
                _nameStatuslLabel[0].Width = 180;
                _nameStatuslLabel[0].ToolTip = "Положние курсоров";
                positonY += 20;
                _nameStatuslLabel[0].Margin = new Thickness(-170, positonY, 0, 0);
            }

            {
                _nameStatuslLabel.Add(new Label());
                XDate x = new XDate(MainWindow.GraphPanelList[numOsc].Cursor1.Location.X);
                _nameStatuslLabel[1].Content = x.DateTime.Second + "." + x.DateTime.Millisecond.ToString("000") +
                                               " сек";
                _nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[1].FontSize = 12;
                _nameStatuslLabel[1].Height = 25;
                _nameStatuslLabel[1].Width = 85;
                _nameStatuslLabel[1].ToolTip = x + ":" + x.DateTime.Second + "." +
                                               x.DateTime.Millisecond.ToString("000");
                positonY += 20;
                _nameStatuslLabel[1].Margin = new Thickness(-275, positonY, 0, 0);
            }

            {
                _nameStatuslLabel.Add(new Label());
                XDate x = new XDate(MainWindow.GraphPanelList[numOsc].Cursor2.Location.X);
                _nameStatuslLabel[2].Content = x.DateTime.Second + "." + x.DateTime.Millisecond.ToString("000") +
                                               " сек";
                _nameStatuslLabel[2].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[2].FontSize = 12;
                _nameStatuslLabel[2].Height = 25;
                _nameStatuslLabel[2].Width = 85;
                _nameStatuslLabel[2].ToolTip = x + ":" + x.DateTime.Second + "." +
                                               x.DateTime.Millisecond.ToString("000");
                _nameStatuslLabel[2].Margin = new Thickness(-80, positonY, 0, 0);
            }

            double positonX = -200;
            for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
            {
                _nameChannelLabel.Add(new Label());
                _nameChannelLabel[j].Content = MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Label.Text;
                _nameChannelLabel[j].VerticalAlignment = VerticalAlignment.Top;
                _nameChannelLabel[j].FontSize = 12;
                _nameChannelLabel[j].Height = 25;
                _nameChannelLabel[j].Width = 180;
                _nameChannelLabel[j].ToolTip = "Название канала";
                positonY += 20;
                _nameChannelLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);

                string str1 = "";
                string str2 = "";

                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
                        MainWindow.GraphPanelList[numOsc].Cursor1.Location.X)
                    {
                        str1 = MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y.ToString("F3");
                        break;
                    }
                }
                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
                        MainWindow.GraphPanelList[numOsc].Cursor2.Location.X)
                    {
                        str2 = MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y.ToString("F3");
                        break;
                    }
                }

                positonY += 20;

                _nameValue1Label.Add(new Label());
                _nameValue1Label[j].Content = str1;
                _nameValue1Label[j].VerticalAlignment = VerticalAlignment.Top;
                _nameValue1Label[j].FontSize = 12;
                _nameValue1Label[j].Height = 25;
                _nameValue1Label[j].Width = 85;
                _nameValue1Label[j].Margin = new Thickness(-280, positonY, 0, 0);

                _nameValue2Label.Add(new Label());
                _nameValue2Label[j].Content = str2;
                _nameValue2Label[j].VerticalAlignment = VerticalAlignment.Top;
                _nameValue2Label[j].FontSize = 12;
                _nameValue2Label[j].Height = 25;
                _nameValue2Label[j].Width = 85;
                _nameValue2Label[j].Margin = new Thickness(-85, positonY, 0, 0);
            }

            LayoutPanel[0].Children.Add(_nameLabel);
            LayoutPanel[0].Children.Add(_nameStatuslLabel[0]);
            LayoutPanel[0].Children.Add(_nameStatuslLabel[1]);
            LayoutPanel[0].Children.Add(_nameStatuslLabel[2]);

            for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
            {
                LayoutPanel[0].Children.Add(_nameChannelLabel[j]);
                LayoutPanel[0].Children.Add(_nameValue1Label[j]);
                LayoutPanel[0].Children.Add(_nameValue2Label[j]);
            }

            LayoutPanel[0].Children.Add(_panelBorder);
        }

        public void AnalysisCursorClear()
        {
            for (int j = _nameChannelLabel.Count - 1; j >= 0; j--)
            {
                LayoutPanel[0].Children.Remove(_nameChannelLabel[j]);
                LayoutPanel[0].Children.Remove(_nameValue1Label[j]);
                LayoutPanel[0].Children.Remove(_nameValue2Label[j]);
            }

            for (int i = 2; i >= 0; i--)
            {
                LayoutPanel[0].Children.Remove(_nameStatuslLabel[i]);
            }

            LayoutPanel[0].Children.Remove(_nameLabel);
            LayoutPanel[0].Children.Remove(_panelBorder);

            _nameStatuslLabel.Remove(_nameStatuslLabel[2]);
            _nameStatuslLabel.Remove(_nameStatuslLabel[1]);
            _nameStatuslLabel.Remove(_nameStatuslLabel[0]);

            _nameChannelLabel.Clear();
            _nameValue1Label.Clear();
            _nameValue2Label.Clear();

            LayoutPanel.Clear();
        }

        public void UpdateCursor(int numOsc)
        {
            for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
            {
                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
                        MainWindow.GraphPanelList[numOsc].Cursor1.Location.X)
                    {
                        var str1 = MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y.ToString("F3");
                        _nameValue1Label[j].Content = str1;
                        break;
                    }
                }
                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
                        MainWindow.GraphPanelList[numOsc].Cursor2.Location.X)
                    {
                        var str2 = MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y.ToString("F3");
                        _nameValue2Label[j].Content = str2;
                        break;
                    }
                }
            }

            XDate x1 = new XDate(MainWindow.GraphPanelList[numOsc].Cursor1.Location.X);
            XDate x2 = new XDate(MainWindow.GraphPanelList[numOsc].Cursor2.Location.X);

            _nameStatuslLabel[1].Content = x1.DateTime.Second + "." + x1.DateTime.Millisecond.ToString("000") + " сек";
            _nameStatuslLabel[1].ToolTip = x1 + ":" + x1.DateTime.Second + "." + x1.DateTime.Millisecond.ToString("000");
            _nameStatuslLabel[2].Content = x2.DateTime.Second + "." + x2.DateTime.Millisecond.ToString("000") + " сек";
            _nameStatuslLabel[2].ToolTip = x2 + ":" + x2.DateTime.Second + "." + x2.DateTime.Millisecond.ToString("000");
        }

        private void click_LayoutPanelCursor(object sender, MouseButtonEventArgs e)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            LayoutPanel[0].Height = LayoutPanel[0].Height == 25 ? 25 + 20 * (_nameChannelLabel.Count + 1) * 2 : 25;
        }
    }
}