using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                double x = MainWindow.GraphPanelList[numOsc].Cursor1.Location.X;
                _nameStatuslLabel[1].Content = x.ToString("F6") + " сек";
                _nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[1].FontSize = 12;
                _nameStatuslLabel[1].Height = 25;
                _nameStatuslLabel[1].Width = 85;
                _nameStatuslLabel[1].ToolTip = (int)x + " сек " + (int)((x - (int)x)*1000) + " мс " +
                    (int)((x*1000 - (int)(x*1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
                positonY += 20;
                _nameStatuslLabel[1].Margin = new Thickness(-275, positonY, 0, 0);
            }

            {
                _nameStatuslLabel.Add(new Label());
                double x = MainWindow.GraphPanelList[numOsc].Cursor2.Location.X;
                _nameStatuslLabel[2].Content = x.ToString("F6") + " сек";
                _nameStatuslLabel[2].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[2].FontSize = 12;
                _nameStatuslLabel[2].Height = 25;
                _nameStatuslLabel[2].Width = 85;
                _nameStatuslLabel[2].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
                    (int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
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

        //Добавляем панель анализа для цифрового канала 
        public void AnalysisCursorAddDig(int numOsc)
        {
            _panelBorder.BorderBrush = Brushes.DarkGray;
            _panelBorder.BorderThickness = new Thickness(1.0);
            _panelBorder.Margin = new Thickness(-190, 0, 0, 0);

            LayoutPanel.Add(new DockPanel());
            LayoutPanel[0].Width = 225;
            LayoutPanel[0].Height = 25;
            LayoutPanel[0].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[0].MouseDown += click_LayoutPanelCursorDig;

            _nameLabel.Content = "Курсор";
            _nameLabel.VerticalAlignment = VerticalAlignment.Top;
            _nameLabel.FontSize = 12;
            _nameLabel.Height = 25;
            _nameLabel.Width = 180;
            _nameLabel.TabIndex = 0;
            _nameLabel.ToolTip = "Положение курсора";
            _nameLabel.Margin = new Thickness(0, 0, 0, 0);

            double positonY = 0;
            {
                _nameStatuslLabel.Add(new Label());
                _nameStatuslLabel[0].Content = "Время:";
                _nameStatuslLabel[0].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[0].FontSize = 12;
                _nameStatuslLabel[0].Height = 25;
                _nameStatuslLabel[0].Width = 80;
                _nameStatuslLabel[0].ToolTip = "Время";
                positonY += 20;
                _nameStatuslLabel[0].Margin = new Thickness(-260, positonY, 0, 0);
            }
            
            {
                _nameStatuslLabel.Add(new Label());
                double x = MainWindow.GraphPanelList[numOsc].CursorDig.Location.X;
                _nameStatuslLabel[1].Content = x.ToString("F6") + " сек";
                _nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[1].FontSize = 12;
                _nameStatuslLabel[1].Height = 25;
                _nameStatuslLabel[1].Width = 85;
                _nameStatuslLabel[1].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
                    (int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
                _nameStatuslLabel[1].Margin = new Thickness(-100, positonY, 0, 0);
            }
            
            {
                _nameStatuslLabel.Add(new Label());
                _nameStatuslLabel[2].Content = "Канал:               Значение:";
                _nameStatuslLabel[2].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[2].FontSize = 12;
                _nameStatuslLabel[2].Height = 25;
                _nameStatuslLabel[2].Width = 180;
                positonY += 20;
                _nameStatuslLabel[2].Margin = new Thickness(-170, positonY, 0, 0);
            }

            {
                _nameChannelLabel.Add(new Label());
                _nameChannelLabel[_nameChannelLabel.Count - 1].Content = MainWindow.GraphPanelList[numOsc].Pane.CurveList[2].Label.Text;
                _nameChannelLabel[_nameChannelLabel.Count - 1].VerticalAlignment = VerticalAlignment.Top;
                _nameChannelLabel[_nameChannelLabel.Count - 1].FontSize = 12;
                _nameChannelLabel[_nameChannelLabel.Count - 1].Height = 25;
                _nameChannelLabel[_nameChannelLabel.Count - 1].Width = 100;
                _nameChannelLabel[_nameChannelLabel.Count - 1].ToolTip = "Название канала";
                positonY += 20;
                _nameChannelLabel[_nameChannelLabel.Count - 1].Margin = new Thickness(-270, positonY, 0, 0);

                string str = "";
                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].Pane.CurveList[_nameChannelLabel.Count - 1].NPts; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].Pane.CurveList[_nameChannelLabel.Count - 1].Points[k].X >
                        MainWindow.GraphPanelList[numOsc].CursorDig.Location.X)
                    {
                        str = MainWindow.GraphPanelList[numOsc].Pane.CurveList[2].Points[k].Y.ToString("F3");
                        break;
                    }
                }

                _nameValue1Label.Add(new Label());
                _nameValue1Label[_nameChannelLabel.Count - 1].Content = str;
                _nameValue1Label[_nameChannelLabel.Count - 1].VerticalAlignment = VerticalAlignment.Top;
                _nameValue1Label[_nameChannelLabel.Count - 1].FontSize = 12;
                _nameValue1Label[_nameChannelLabel.Count - 1].Height = 25;
                _nameValue1Label[_nameChannelLabel.Count - 1].Width = 85;
                _nameValue1Label[_nameChannelLabel.Count - 1].Margin = new Thickness(-100, positonY, 0, 0);

            }
             for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count - 3; j++)
            {
                 _nameChannelLabel.Add(new Label());
                 _nameChannelLabel[j + 1].Content = j + 1;
                 _nameChannelLabel[j + 1].VerticalAlignment = VerticalAlignment.Top;
                 _nameChannelLabel[j + 1].FontSize = 12;
                 _nameChannelLabel[j + 1].Height = 25;
                 _nameChannelLabel[j + 1].Width = 100;
                 _nameChannelLabel[j + 1].ToolTip = "Название канала";
                 positonY += 20;
                 _nameChannelLabel[j + 1].Margin = new Thickness(-270, positonY, 0, 0);

                 string str = "";

                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].Pane.CurveList[j + 3].NPts; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].Pane.CurveList[j + 3].Points[k].X >
                        MainWindow.GraphPanelList[numOsc].CursorDig.Location.X)
                    {
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        str = (MainWindow.GraphPanelList[numOsc].Pane.CurveList[j + 3].Points[k].Y + 0.2) % 1 == 0 ? "1" : "0";
                        break;
                    }
                }

                 _nameValue1Label.Add(new Label());
                 _nameValue1Label[j + 1].Content = str;
                 _nameValue1Label[j + 1].VerticalAlignment = VerticalAlignment.Top;
                 _nameValue1Label[j + 1].FontSize = 12;
                 _nameValue1Label[j + 1].Height = 25;
                 _nameValue1Label[j + 1].Width = 85;
                 _nameValue1Label[j + 1].Margin = new Thickness(-100, positonY, 0, 0);
            }


            LayoutPanel[0].Children.Add(_nameLabel);
            LayoutPanel[0].Children.Add(_nameStatuslLabel[0]);
            LayoutPanel[0].Children.Add(_nameStatuslLabel[1]);
            LayoutPanel[0].Children.Add(_nameStatuslLabel[2]);

            for (int j = 0; j < _nameChannelLabel.Count; j++)
            {
                LayoutPanel[0].Children.Add(_nameChannelLabel[j]);
                LayoutPanel[0].Children.Add(_nameValue1Label[j]);
            }

            LayoutPanel[0].Children.Add(_panelBorder);
        }


        public void AnalysisCursorClear()
        {
            for (int j = _nameChannelLabel.Count - 1; j >= 0; j--)
            {
                LayoutPanel[0].Children.Remove(_nameChannelLabel[j]);
                LayoutPanel[0].Children.Remove(_nameValue1Label[j]);
                if(_nameValue2Label.Count != 0) {  LayoutPanel[0].Children.Remove(_nameValue2Label[j]);    }
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

            double x1 = MainWindow.GraphPanelList[numOsc].Cursor1.Location.X;
            double x2 = MainWindow.GraphPanelList[numOsc].Cursor2.Location.X;

            _nameStatuslLabel[1].Content = x1.ToString("F6") + " сек";
            _nameStatuslLabel[1].ToolTip = (int)x1 + " сек " + (int)((x1 - (int)x1) * 1000) + " мс " +
                    (int)((x1 * 1000 - (int)(x1 * 1000)) * 1000) + " мкс " + (int)((x1 * 1000000 - (int)(x1 * 1000000)) * 1000) + " нс";
            _nameStatuslLabel[2].Content = x2.ToString("F6") + " сек";
            _nameStatuslLabel[2].ToolTip = (int)x2 + " сек " + (int)((x2 - (int)x2) * 1000) + " мс " +
                    (int)((x2 * 1000 - (int)(x2 * 1000)) * 1000) + " мкс " + (int)((x2 * 1000000 - (int)(x2 * 1000000)) * 1000) + " нс";
        }

        public void UpdateCursorDig(int numOsc)
        {
            for (int j = 2; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
            {
                for (int k = 0; k < MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].NPts; k++)
                {
                    if (MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Points[k].X >
                        MainWindow.GraphPanelList[numOsc].CursorDig.Location.X)
                    {
                        if (j == 2)
                        {
                            var str1 = MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Points[k].Y.ToString("F3");
                            _nameValue1Label[j - 2].Content = str1;
                        }
                        else
                        {
                            // ReSharper disable once CompareOfFloatsByEqualityOperator
                            var str = (MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Points[k].Y + 0.2) % 1 == 0 ? "1" : "0";
                            _nameValue1Label[j - 2].Content = str;
                        }
                        break;
                    }
                }
            }

            double x = MainWindow.GraphPanelList[numOsc].CursorDig.Location.X;

            _nameStatuslLabel[1].Content = x.ToString("F6") + " сек";
            _nameStatuslLabel[1].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
                    (int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
        }

        private void click_LayoutPanelCursor(object sender, MouseButtonEventArgs e)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            LayoutPanel[0].Height = LayoutPanel[0].Height == 25 ? 25 + 20 * (_nameChannelLabel.Count + 1) * 2 : 25;
        }
        private void click_LayoutPanelCursorDig(object sender, MouseButtonEventArgs e)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            LayoutPanel[0].Height = LayoutPanel[0].Height == 25 ? 25 + 20 * (_nameChannelLabel.Count + 2) : 25;
        }
    }
}