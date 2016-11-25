using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Analysis.xaml
    /// </summary>
    public partial class Analysis : UserControl
    {
        List<DockPanel> _layoutPanel = new List<DockPanel>();
        List<Label> _nameLabel = new List<Label>();
        List<Label> _nameChannelLabel = new List<Label>();
        List<Label> _nameStatuslLabel = new List<Label>();
        List<Label> _nameValue1Label = new List<Label>();
        List<Label> _nameValue2Label = new List<Label>();

        List<Border> _panelBorder = new List<Border>();
        List<bool> _openClose = new List<bool>();

        public Analysis()
        {
            InitializeComponent();
        }

        public void AnalysisCursorClear()
        {
            for (int i = _layoutPanel.Count - 1; i >= 0; i--)
            {
                if(_nameLabel[i].Content == "Cursor")
                {
                    for (int j = GraphPanel.Pane.CurveList.Count - 1; j >= 0; j--)
                    { 
                    
                            _layoutPanel[i].Children.Remove(_nameChannelLabel[j]);
                            _layoutPanel[i].Children.Remove(_nameValue1Label[j]);
                            _layoutPanel[i].Children.Remove(_nameValue2Label[j]);
                    }

                    _layoutPanel[i].Children.Remove(_nameStatuslLabel[1]);
                    _layoutPanel[i].Children.Remove(_nameStatuslLabel[0]);
                    _layoutPanel[i].Children.Remove(_nameLabel[i]);
                    _layoutPanel[i].Children.Remove(_panelBorder[i]);

                    AnalysisStackPanel.Children.Remove(_layoutPanel[i]);

                    _nameLabel.Remove(_nameLabel[i]);
                    _panelBorder.Remove(_panelBorder[i]);
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
            int i;

            _panelBorder.Add(new Border());
            i = _panelBorder.Count - 1;
            _panelBorder[i].BorderBrush = Brushes.DarkGray;
            _panelBorder[i].BorderThickness = new Thickness(1.0);
            _panelBorder[i].Margin = new Thickness(-215, 0, 0, 0);

            _layoutPanel.Add(new DockPanel());
            _layoutPanel[i].Width = 225;
            _layoutPanel[i].Height = 30;
            _layoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            _layoutPanel[i].Background = Brushes.White;
            _layoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanelCursor);

            _nameLabel.Add(new Label());
            _nameLabel[i].Content = "Cursor";
            _nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            _nameLabel[i].FontSize = 12;
            _nameLabel[i].Height = 25;
            _nameLabel[i].Width = 50;
            _nameLabel[i].ToolTip = "Положения курсоров";
            _nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

            double positonY = 10;
            double positonX = 0;
            {
                _nameStatuslLabel.Add(new Label());
                _nameStatuslLabel[0].Content = "  \u0394t                   Cursor1          Cursor2";
                _nameStatuslLabel[0].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[0].FontSize = 11;
                _nameStatuslLabel[0].Height = 20;
                _nameStatuslLabel[0].Width = 185;
                _nameStatuslLabel[0].ToolTip = "Положние курсоров";
                positonY += 20;
                _nameStatuslLabel[0].Margin = new Thickness(-20, positonY, 0, 0);
            }
            positonX = -195;
            {
                _nameStatuslLabel.Add(new Label());
                String str = (GraphPanel.Cursor2.Location.X - GraphPanel.Cursor1.Location.X).ToString("F4") + "                  "
                    + GraphPanel.Cursor1.Location.X.ToString("F4") + "               " + GraphPanel.Cursor2.Location.X.ToString("F4") ;
                _nameStatuslLabel[1].Content = str;
                _nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
                _nameStatuslLabel[1].FontSize = 10;
                _nameStatuslLabel[1].Height = 20;
                _nameStatuslLabel[1].Width = 185;
                positonY += 20;
                _nameStatuslLabel[1].Margin = new Thickness(positonX, positonY, 0, 0);
            }
            positonX = -330;
            for (int j = 0; j < GraphPanel.Pane.CurveList.Count; j++)
            {
                _nameChannelLabel.Add(new Label());
                _nameChannelLabel[j].Content = GraphPanel.Pane.CurveList[j].Label.Text;
                _nameChannelLabel[j].VerticalAlignment = VerticalAlignment.Top;
                _nameChannelLabel[j].FontSize = 11;
                _nameChannelLabel[j].Height = 20;
                _nameChannelLabel[j].Width = 105;
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

                _nameValue1Label.Add(new Label());
                _nameValue1Label[j].Content = str1;
                _nameValue1Label[j].VerticalAlignment = VerticalAlignment.Top;
                _nameValue1Label[j].FontSize = 11;
                _nameValue1Label[j].Height = 20;
                _nameValue1Label[j].Width = 60;
                _nameValue1Label[j].Margin = new Thickness(-175, positonY, 0, 0);

                _nameValue2Label.Add(new Label());
                _nameValue2Label[j].Content = str2;
                _nameValue2Label[j].VerticalAlignment = VerticalAlignment.Top;
                _nameValue2Label[j].FontSize = 11;
                _nameValue2Label[j].Height = 20;
                _nameValue2Label[j].Width = 60;
                _nameValue2Label[j].Margin = new Thickness(-60, positonY, 0, 0);
            }

            _openClose.Add(new bool());
            _openClose[i] = false;


            _layoutPanel[i].Children.Add(_nameLabel[i]);
            _layoutPanel[i].Children.Add(_nameStatuslLabel[0]);
            _layoutPanel[i].Children.Add(_nameStatuslLabel[1]);
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
            string str1 = "";
            string str2 = "";
            for (int j = 0; j < GraphPanel.Pane.CurveList.Count; j++)
            {
                for (int k = 0; k < GraphPanel.ListTemp[j].Count; k++)
                {
                    if (GraphPanel.ListTemp[j][k].X > GraphPanel.Cursor1.Location.X)
                    {
                        str1 = GraphPanel.ListTemp[j][k].Y.ToString("F3");
                        _nameValue1Label[j].Content = str1;
                        break;
                    }
                }
                for (int k = 0; k < GraphPanel.ListTemp[j].Count; k++)
                {
                    if (GraphPanel.ListTemp[j][k].X > GraphPanel.Cursor2.Location.X)
                    {
                        str2 = GraphPanel.ListTemp[j][k].Y.ToString("F3");
                        _nameValue2Label[j].Content = str2;
                        break;
                    }
                }
            }
            _nameStatuslLabel[1].Content = (GraphPanel.Cursor2.Location.X - GraphPanel.Cursor1.Location.X).ToString("F4") + "                  "
                    + GraphPanel.Cursor1.Location.X.ToString("F4") + "               " + GraphPanel.Cursor2.Location.X.ToString("F4"); 
        }

        private void click_LayoutPanelCursor(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < _openClose.Count; i++)
            {
                if (_layoutPanel[i].IsMouseOver == true && _openClose[i] == false) OpenAnimation(i, GraphPanel.Pane.CurveList.Count + 2);
                else if (_layoutPanel[i].IsMouseOver == true && _openClose[i] == true) CloseAnimation(i, GraphPanel.Pane.CurveList.Count + 2);
            }
        }

        private void OpenAnimation(int i, int j)
        {
            DoubleAnimation openAnimation = new DoubleAnimation();
            if (_nameLabel[i].Content == "Cursor")
            {
                openAnimation.From = 30;
                openAnimation.To = 30 + 20 * j;
            }
            openAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            _layoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, openAnimation);
            _openClose[i] = true;

            {

            }
        }
        private void CloseAnimation(int i, int j)
        {
            DoubleAnimation closeAnimation = new DoubleAnimation();
            if (_nameLabel[i].Content == "Cursor")
            {
                closeAnimation.From = 30 + 25 * j;
                closeAnimation.To = 30;
            }
            closeAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            _layoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, closeAnimation);
            _openClose[i] = false;

            {

            }
        }
    }
}
