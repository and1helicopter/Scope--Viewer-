using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ZedGraph;
using Border = System.Windows.Controls.Border;
using Brushes = System.Windows.Media.Brushes;
using Label = System.Windows.Controls.Label;

namespace ScopeViewer
{
    /// <summary>
    /// Interaction logic for Analysis.xaml
    /// </summary>
    public partial class Analysis 
    {
        readonly List<DockPanel> _layoutPanel = new List<DockPanel>();
        readonly List<Label> _nameLabel = new List<Label>();
        readonly List<Label> _nameChannelLabel = new List<Label>();
        readonly List<Label> _nameStatuslLabel = new List<Label>();
        readonly List<Label> _nameValue1Label = new List<Label>();
        readonly List<Label> _nameValue2Label = new List<Label>();

        readonly List<Border> _panelBorder = new List<Border>();
        readonly List<bool> _openClose = new List<bool>();

        public Analysis()
        {
            InitializeComponent();
        }

        public void AnalysisCursorClear()
        {
            for (int i = _layoutPanel.Count - 1; i >= 0; i--)
            {
                if(_nameLabel[i].TabIndex == 0)
                {
                    for (int j = GraphPanel.Pane.CurveList.Count - 1; j >= 0; j--)
                    { 
                    
                            _layoutPanel[i].Children.Remove(_nameChannelLabel[j]);
                            _layoutPanel[i].Children.Remove(_nameValue1Label[j]);
                            _layoutPanel[i].Children.Remove(_nameValue2Label[j]);
                    }

                    _layoutPanel[i].Children.Remove(_nameStatuslLabel[2]);
                    _layoutPanel[i].Children.Remove(_nameStatuslLabel[1]);
                    _layoutPanel[i].Children.Remove(_nameStatuslLabel[0]);
                    

                    _layoutPanel[i].Children.Remove(_nameLabel[i]);
                    _layoutPanel[i].Children.Remove(_panelBorder[i]);

                    AnalysisStackPanel.Children.Remove(_layoutPanel[i]);

                    _nameLabel.Remove(_nameLabel[i]);
                    _panelBorder.Remove(_panelBorder[i]);
                    _nameStatuslLabel.Remove(_nameStatuslLabel[2]);
                    _nameStatuslLabel.Remove(_nameStatuslLabel[1]);
                    _nameStatuslLabel.Remove(_nameStatuslLabel[0]);

                    _nameChannelLabel.Clear();
                    _nameValue1Label.Clear();
                    _nameValue2Label.Clear();

                    _openClose.Remove(_openClose[i]);

                    _layoutPanel.Remove(_layoutPanel[i]);
                }
            }
      }
    

        public void AnalysisCursorAdd()
        {
            _panelBorder.Add(new Border());
            int i = _panelBorder.Count - 1;
            _panelBorder[i].BorderBrush = Brushes.DarkGray;
            _panelBorder[i].BorderThickness = new Thickness(1.0);
            _panelBorder[i].Margin = new Thickness(-195, 0, 0, 0);

            _layoutPanel.Add(new DockPanel());
            _layoutPanel[i].Width = 225;
            _layoutPanel[i].Height = 25;
            _layoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            _layoutPanel[i].MouseDown += click_LayoutPanelCursor;

            _nameLabel.Add(new Label());
            _nameLabel[i].Content = "Курсоры";
            _nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            _nameLabel[i].FontSize = 12;
            _nameLabel[i].Height = 25;
            _nameLabel[i].Width = 180;
            _nameLabel[i].TabIndex = 0;
            _nameLabel[i].ToolTip = "Положения курсоров";
            _nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

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
                XDate x = new XDate(GraphPanel.Cursor1.Location.X);
                _nameStatuslLabel[1].Content = x.DateTime.Second + "." + x.DateTime.Millisecond.ToString("000") + " сек";
                _nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[1].FontSize = 12;
                _nameStatuslLabel[1].Height = 25;
                _nameStatuslLabel[1].Width = 85;
                _nameStatuslLabel[1].ToolTip = x + ":" + x.DateTime.Second + "." + x.DateTime.Millisecond.ToString("000");
                positonY += 20;
                _nameStatuslLabel[1].Margin = new Thickness(-275, positonY, 0, 0);
            }

            {
                //(GraphPanel.Cursor2.Location.X - GraphPanel.Cursor1.Location.X).ToString("F4") + "                  "
                _nameStatuslLabel.Add(new Label());
                XDate x = new XDate(GraphPanel.Cursor2.Location.X);
                _nameStatuslLabel[2].Content = x.DateTime.Second + "." + x.DateTime.Millisecond.ToString("000") + " сек";
                _nameStatuslLabel[2].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[2].FontSize = 12;
                _nameStatuslLabel[2].Height = 25;
                _nameStatuslLabel[2].Width = 85;
                _nameStatuslLabel[2].ToolTip = x + ":" + x.DateTime.Second + "." + x.DateTime.Millisecond.ToString("000");
                _nameStatuslLabel[2].Margin = new Thickness(-80, positonY, 0, 0);
            }

            double positonX = -200;
            for (int j = 0; j < GraphPanel.Pane.CurveList.Count; j++)
            {
                _nameChannelLabel.Add(new Label());
                _nameChannelLabel[j].Content = GraphPanel.Pane.CurveList[j].Label.Text;
                _nameChannelLabel[j].VerticalAlignment = VerticalAlignment.Top;
                _nameChannelLabel[j].FontSize = 12;
                _nameChannelLabel[j].Height = 25;
                _nameChannelLabel[j].Width = 180;
                _nameChannelLabel[j].ToolTip = "Название канала";
                positonY += 20;
                _nameChannelLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);

                string str1 = "";
                string str2 = "";

                for (int k = 0; k < GraphPanel.ListTemp[j].Count; k++)
                {
                    if (GraphPanel.ListTemp[j][k].X > GraphPanel.Cursor1.Location.X)
                    {
                        str1 = GraphPanel.ListTemp[j][k].Y.ToString("F3");
                        break;
                    }
                }
                for (int k = 0; k < GraphPanel.ListTemp[j].Count; k++)
                {
                    if (GraphPanel.ListTemp[j][k].X > GraphPanel.Cursor2.Location.X)
                    {
                        str2 = GraphPanel.ListTemp[j][k].Y.ToString("F3");
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

            _openClose.Add(new bool());
            _openClose[i] = false;

            _layoutPanel[i].Children.Add(_nameLabel[i]);
            _layoutPanel[i].Children.Add(_nameStatuslLabel[0]);
            _layoutPanel[i].Children.Add(_nameStatuslLabel[1]);
            _layoutPanel[i].Children.Add(_nameStatuslLabel[2]);

            for (int j = 0; j < GraphPanel.Pane.CurveList.Count; j++)
            {
                _layoutPanel[i].Children.Add(_nameChannelLabel[j]);
                _layoutPanel[i].Children.Add(_nameValue1Label[j]);
                _layoutPanel[i].Children.Add(_nameValue2Label[j]);
            }

            _layoutPanel[i].Children.Add(_panelBorder[i]);

            AnalysisStackPanel.Children.Add(_layoutPanel[i]);
        }

        public void UpdateCursor()
        {
            for (int j = 0; j < GraphPanel.Pane.CurveList.Count; j++)
            {
                for (int k = 0; k < GraphPanel.ListTemp[j].Count; k++)
                {
                    if (GraphPanel.ListTemp[j][k].X > GraphPanel.Cursor1.Location.X)
                    {
                        var str1 = GraphPanel.ListTemp[j][k].Y.ToString("F3");
                        _nameValue1Label[j].Content = str1;
                        break;
                    }
                }
                for (int k = 0; k < GraphPanel.ListTemp[j].Count; k++)
                {
                    if (GraphPanel.ListTemp[j][k].X > GraphPanel.Cursor2.Location.X)
                    {
                        var str2 = GraphPanel.ListTemp[j][k].Y.ToString("F3");
                        _nameValue2Label[j].Content = str2;
                        break;
                    }
                }
            }

            XDate x1 = new XDate(GraphPanel.Cursor1.Location.X);
            XDate x2 = new XDate(GraphPanel.Cursor2.Location.X);

            _nameStatuslLabel[1].Content = x1.DateTime.Second + "." + x1.DateTime.Millisecond.ToString("000") + " сек";
            _nameStatuslLabel[1].ToolTip = x1 + ":" + x1.DateTime.Second + "." + x1.DateTime.Millisecond.ToString("000");
            _nameStatuslLabel[2].Content = x2.DateTime.Second + "." + x2.DateTime.Millisecond.ToString("000") + " сек";
            _nameStatuslLabel[2].ToolTip = x2 + ":" + x2.DateTime.Second + "." + x2.DateTime.Millisecond.ToString("000");
        }

        private void click_LayoutPanelCursor(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _openClose.Count; i++)
            {
                if (_layoutPanel[i].IsMouseOver && _openClose[i] == false) OpenAnimation(i, GraphPanel.Pane.CurveList.Count + 2);
                else if (_layoutPanel[i].IsMouseOver && _openClose[i]) CloseAnimation(i, GraphPanel.Pane.CurveList.Count + 2);
            }
        }

        private void OpenAnimation(int i, int j)
        {
            DoubleAnimation openAnimation = new DoubleAnimation();
            if ( _nameLabel[i].TabIndex == 0)
            {
                openAnimation.From = 25;
                openAnimation.To = 20 * j * 2 ;
            }
            openAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            _layoutPanel[i].BeginAnimation(MinHeightProperty, openAnimation);
            _openClose[i] = true;

            {

            }
        }
        private void CloseAnimation(int i, int j)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation();
            if ( _nameLabel[i].TabIndex == 0)
            {
                closeAnimation.From = 25 * j * 2;
                closeAnimation.To = 25;
            }
            closeAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            _layoutPanel[i].BeginAnimation(MinHeightProperty, closeAnimation);
            _openClose[i] = false;

            {

            }
        }
    }
}
