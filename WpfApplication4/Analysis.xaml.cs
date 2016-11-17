using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Analysis.xaml
    /// </summary>
    public partial class Analysis : UserControl
    {
        List<DockPanel> LayoutPanel = new List<DockPanel>();
        List<Label> nameLabel = new List<Label>();
        List<Label> nameChannelLabel = new List<Label>();
        List<Label> nameStatuslLabel = new List<Label>();
        List<Label> nameValue1Label = new List<Label>();
        List<Label> nameValue2Label = new List<Label>();

        List<Border> panelBorder = new List<Border>();
        List<bool> OpenClose = new List<bool>();

        public Analysis()
        {
            InitializeComponent();
        }

        public void AnalysisCursorClear()
        {
            for (int i = LayoutPanel.Count - 1; i >= 0; i--)
            {
                if(nameLabel[i].Content == "Cursor")
                {
                    for (int j = GraphPanel.pane.CurveList.Count - 1; j >= 0; j--)
                    { 
                    
                            LayoutPanel[i].Children.Remove(nameChannelLabel[j]);
                            LayoutPanel[i].Children.Remove(nameValue1Label[j]);
                            LayoutPanel[i].Children.Remove(nameValue2Label[j]);
                    }

                    LayoutPanel[i].Children.Remove(nameStatuslLabel[1]);
                    LayoutPanel[i].Children.Remove(nameStatuslLabel[0]);
                    LayoutPanel[i].Children.Remove(nameLabel[i]);
                    LayoutPanel[i].Children.Remove(panelBorder[i]);

                    AnalysisStackPanel.Children.Remove(LayoutPanel[i]);

                    nameLabel.Remove(nameLabel[i]);
                    panelBorder.Remove(panelBorder[i]);
                    nameStatuslLabel.Remove(nameStatuslLabel[1]);
                    nameStatuslLabel.Remove(nameStatuslLabel[0]);

                    nameChannelLabel.Clear();
                    nameValue1Label.Clear();
                    nameValue2Label.Clear();

                    OpenClose.Remove(OpenClose[i]);

                    LayoutPanel.Remove(LayoutPanel[i]);
                }
            }
        }
    

        public void AnalysisCursorAdd()
        {
            int i;

            panelBorder.Add(new Border());
            i = panelBorder.Count - 1;
            panelBorder[i].BorderBrush = Brushes.DarkGray;
            panelBorder[i].BorderThickness = new Thickness(1.0);
            panelBorder[i].Margin = new Thickness(-215, 0, 0, 0);

            LayoutPanel.Add(new DockPanel());
            LayoutPanel[i].Width = 225;
            LayoutPanel[i].Height = 30;
            LayoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[i].Background = Brushes.White;
            LayoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanelCursor);

            nameLabel.Add(new Label());
            nameLabel[i].Content = "Cursor";
            nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            nameLabel[i].FontSize = 12;
            nameLabel[i].Height = 25;
            nameLabel[i].Width = 50;
            nameLabel[i].ToolTip = "Положения курсоров";
            nameLabel[i].Margin = new Thickness(0, 0, 0, 0);

            double positonY = 10;
            double positonX = 0;
            {
                nameStatuslLabel.Add(new Label());
                nameStatuslLabel[0].Content = "  \u0394t                   Cursor1          Cursor2";
                nameStatuslLabel[0].VerticalAlignment = VerticalAlignment.Top;
                nameStatuslLabel[0].FontSize = 11;
                nameStatuslLabel[0].Height = 20;
                nameStatuslLabel[0].Width = 185;
                nameStatuslLabel[0].ToolTip = "Положние курсоров";
                positonY += 20;
                nameStatuslLabel[0].Margin = new Thickness(-20, positonY, 0, 0);
            }
            positonX = -195;
            {
                nameStatuslLabel.Add(new Label());
                String str = (GraphPanel.Cursor2.Location.X - GraphPanel.Cursor1.Location.X).ToString("F4") + "                  "
                    + GraphPanel.Cursor1.Location.X.ToString("F4") + "               " + GraphPanel.Cursor2.Location.X.ToString("F4") ;
                nameStatuslLabel[1].Content = str;
                nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
                nameStatuslLabel[1].FontSize = 10;
                nameStatuslLabel[1].Height = 20;
                nameStatuslLabel[1].Width = 185;
                positonY += 20;
                nameStatuslLabel[1].Margin = new Thickness(positonX, positonY, 0, 0);
            }
            positonX = -330;
            for (int j = 0; j < GraphPanel.pane.CurveList.Count; j++)
            {
                nameChannelLabel.Add(new Label());
                nameChannelLabel[j].Content = GraphPanel.pane.CurveList[j].Label.Text;
                nameChannelLabel[j].VerticalAlignment = VerticalAlignment.Top;
                nameChannelLabel[j].FontSize = 11;
                nameChannelLabel[j].Height = 20;
                nameChannelLabel[j].Width = 105;
                nameChannelLabel[j].ToolTip = "Название канала";
                positonY += 20;
                nameChannelLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);

                string str1 = "";
                string str2 = "";

                for (int k = 0; k < GraphPanel.listTemp[j].Count; k++)
                {
                    if (GraphPanel.listTemp[j][k].X > GraphPanel.Cursor1.Location.X)
                    {
                        str1 = GraphPanel.listTemp[j][k].Y.ToString("F3");
                        break;
                    }
                }
                for (int k = 0; k < GraphPanel.listTemp[j].Count; k++)
                {
                    if (GraphPanel.listTemp[j][k].X > GraphPanel.Cursor2.Location.X)
                    {
                        str2 = GraphPanel.listTemp[j][k].Y.ToString("F3");
                        break;
                    }
                }

                nameValue1Label.Add(new Label());
                nameValue1Label[j].Content = str1;
                nameValue1Label[j].VerticalAlignment = VerticalAlignment.Top;
                nameValue1Label[j].FontSize = 11;
                nameValue1Label[j].Height = 20;
                nameValue1Label[j].Width = 60;
                nameValue1Label[j].Margin = new Thickness(-175, positonY, 0, 0);

                nameValue2Label.Add(new Label());
                nameValue2Label[j].Content = str2;
                nameValue2Label[j].VerticalAlignment = VerticalAlignment.Top;
                nameValue2Label[j].FontSize = 11;
                nameValue2Label[j].Height = 20;
                nameValue2Label[j].Width = 60;
                nameValue2Label[j].Margin = new Thickness(-60, positonY, 0, 0);
            }

            OpenClose.Add(new bool());
            OpenClose[i] = false;


            LayoutPanel[i].Children.Add(nameLabel[i]);
            LayoutPanel[i].Children.Add(nameStatuslLabel[0]);
            LayoutPanel[i].Children.Add(nameStatuslLabel[1]);
            for (int j = 0; j < GraphPanel.pane.CurveList.Count; j++)
            {
                LayoutPanel[i].Children.Add(nameChannelLabel[j]);
                LayoutPanel[i].Children.Add(nameValue1Label[j]);
                LayoutPanel[i].Children.Add(nameValue2Label[j]);
            }

            LayoutPanel[i].Children.Add(panelBorder[i]);

            AnalysisStackPanel.Children.Add(LayoutPanel[i]);
        }

        public void updateCursor()
        {
            string str1 = "";
            string str2 = "";
            for (int j = 0; j < GraphPanel.pane.CurveList.Count; j++)
            {
                for (int k = 0; k < GraphPanel.listTemp[j].Count; k++)
                {
                    if (GraphPanel.listTemp[j][k].X > GraphPanel.Cursor1.Location.X)
                    {
                        str1 = GraphPanel.listTemp[j][k].Y.ToString("F3");
                        nameValue1Label[j].Content = str1;
                        break;
                    }
                }
                for (int k = 0; k < GraphPanel.listTemp[j].Count; k++)
                {
                    if (GraphPanel.listTemp[j][k].X > GraphPanel.Cursor2.Location.X)
                    {
                        str2 = GraphPanel.listTemp[j][k].Y.ToString("F3");
                        nameValue2Label[j].Content = str2;
                        break;
                    }
                }
            }
            nameStatuslLabel[1].Content = (GraphPanel.Cursor2.Location.X - GraphPanel.Cursor1.Location.X).ToString("F4") + "                  "
                    + GraphPanel.Cursor1.Location.X.ToString("F4") + "               " + GraphPanel.Cursor2.Location.X.ToString("F4"); 
        }

        private void click_LayoutPanelCursor(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < OpenClose.Count; i++)
            {
                if (LayoutPanel[i].IsMouseOver == true && OpenClose[i] == false) OpenAnimation(i, GraphPanel.pane.CurveList.Count + 2);
                else if (LayoutPanel[i].IsMouseOver == true && OpenClose[i] == true) CloseAnimation(i, GraphPanel.pane.CurveList.Count + 2);
            }
        }

        private void OpenAnimation(int i, int j)
        {
            DoubleAnimation OpenAnimation = new DoubleAnimation();
            if (nameLabel[i].Content == "Cursor")
            {
                OpenAnimation.From = 30;
                OpenAnimation.To = 30 + 20 * j;
            }
            OpenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, OpenAnimation);
            OpenClose[i] = true;

            {

            }
        }
        private void CloseAnimation(int i, int j)
        {
            DoubleAnimation CloseAnimation = new DoubleAnimation();
            if (nameLabel[i].Content == "Cursor")
            {
                CloseAnimation.From = 30 + 25 * j;
                CloseAnimation.To = 30;
            }
            CloseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, CloseAnimation);
            OpenClose[i] = false;

            {

            }
        }
    }
}
