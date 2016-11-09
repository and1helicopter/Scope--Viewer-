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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication4
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        List<DockPanel> LayoutPanel = new List<DockPanel>();
        List<Label> nameLabel = new List<Label>();
        List<CheckBox> visibleCheckBox = new List<CheckBox>();
        List<Ellipse> colorEllipse = new List<Ellipse>();
        List<ComboBox> typeComboBox = new List<ComboBox>();
        List<CheckBox> smoothCheckBox = new List<CheckBox>();
        List<Border> panelBorder = new List<Border>();
        List<bool> OpenClose = new List<bool>();
        List<ComboBox> stepTypeComboBox = new List<ComboBox>();
        List<CheckBox> widthCheckBox = new List<CheckBox>();

        string[] StyleType = new string[] {
            "Solid",
            "Dash",
            "DashDot",
            "DashDotDot",
            "Dot"
        };

        string[] StepType = new string[] {
            "NonStep",
            "ForwardSegment",
            "ForwardStep",
            "RearwardSegment",
            "RearwardStep"
        };

        public Graph()
        {
            InitializeComponent();
        }

        public void GraphConfigClear()
        {
            for(int i = LayoutPanel.Count - 1; i >= 0 ; i--)
            {
                LayoutPanel[i].Children.Remove(nameLabel[i]);
                LayoutPanel[i].Children.Remove(visibleCheckBox[i]);
                LayoutPanel[i].Children.Remove(colorEllipse[i]);
                LayoutPanel[i].Children.Remove(typeComboBox[i]);
                LayoutPanel[i].Children.Remove(smoothCheckBox[i]);
                LayoutPanel[i].Children.Remove(panelBorder[i]);
                LayoutPanel[i].Children.Remove(stepTypeComboBox[i]);
                LayoutPanel[i].Children.Remove(widthCheckBox[i]);
                GraphStackPanel.Children.Remove(LayoutPanel[i]);

                nameLabel.Remove(nameLabel[i]);
                visibleCheckBox.Remove(visibleCheckBox[i]);
                colorEllipse.Remove(colorEllipse[i]);
                typeComboBox.Remove(typeComboBox[i]);
                smoothCheckBox.Remove(smoothCheckBox[i]);
                panelBorder.Remove(panelBorder[i]);
                stepTypeComboBox.Remove(stepTypeComboBox[i]);
                widthCheckBox.Remove(widthCheckBox[i]);
            }

            nameLabel.Clear();
            visibleCheckBox.Clear();
            colorEllipse.Clear();
            typeComboBox.Clear();
            smoothCheckBox.Clear();
            panelBorder.Clear();
            OpenClose.Clear();
            stepTypeComboBox.Clear();
            widthCheckBox.Clear();

            LayoutPanel.Clear();
        }

        public void GraphConfigAdd(string nameChannel, string dimensionChannel)
        {
            int i;

            panelBorder.Add(new Border());
            i = panelBorder.Count - 1;
            panelBorder[i].BorderBrush = Brushes.DarkGray;
            panelBorder[i].BorderThickness = new Thickness(1.0);
            panelBorder[i].Margin = new Thickness(-175, 0, 0, 0);

            LayoutPanel.Add(new DockPanel());
            LayoutPanel[i].Width = 175;
            LayoutPanel[i].Height = 30;
            LayoutPanel[i].Margin = new Thickness(2, 5, 2, 0);
            LayoutPanel[i].Background = Brushes.White;
            LayoutPanel[i].MouseDown += new MouseButtonEventHandler(click_LayoutPanel);

            nameLabel.Add(new Label());
            nameLabel[i].Content = nameChannel + ", " + dimensionChannel;
            nameLabel[i].VerticalAlignment = VerticalAlignment.Top;
            nameLabel[i].FontSize = 12;
            nameLabel[i].Height = 25;
            nameLabel[i].Width = 155;
            nameLabel[i].ToolTip = "Название канала";
            nameLabel[i].Margin = new Thickness(0,0,0,0);

            visibleCheckBox.Add(new CheckBox());
            visibleCheckBox[i].IsChecked = true;
            visibleCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            visibleCheckBox[i].Height = visibleCheckBox[i].Width = 16;
            visibleCheckBox[i].Margin = new Thickness(0, 5, 0, 0);
            visibleCheckBox[i].ToolTip = "Отображать";
            visibleCheckBox[i].Click += new RoutedEventHandler(click_checkedButton);

            colorEllipse.Add(new Ellipse());
            colorEllipse[i].Width = colorEllipse[i].Height = 25;
            colorEllipse[i].VerticalAlignment = VerticalAlignment.Top;
            colorEllipse[i].Margin = new Thickness(-275, 25, 0, 0);
            colorEllipse[i].Fill = new SolidColorBrush(Color.FromArgb(GraphPanel.pane.CurveList[i].Color.A, GraphPanel.pane.CurveList[i].Color.R, GraphPanel.pane.CurveList[i].Color.G, GraphPanel.pane.CurveList[i].Color.B));
            colorEllipse[i].Visibility = Visibility.Hidden;
            colorEllipse[i].ToolTip = "Цвет";
            colorEllipse[i].MouseDown += new MouseButtonEventHandler(click_ColorEllipse);

            typeComboBox.Add(new ComboBox());
            typeComboBox[i].Width = 100;
            typeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            typeComboBox[i].Margin = new Thickness(-170, 20, 0, 0);
            typeComboBox[i].ToolTip = "Тип линии";
            typeComboBox[i].ItemsSource = StyleType;
            typeComboBox[i].SelectedIndex = 0;
            typeComboBox[i].Visibility = Visibility.Hidden;
            typeComboBox[i].SelectionChanged += new SelectionChangedEventHandler (change_index);


            smoothCheckBox.Add(new CheckBox());
            smoothCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            smoothCheckBox[i].Height = visibleCheckBox[i].Width = 16;
            smoothCheckBox[i].Margin = new Thickness(-16, 30, 0, 0);
            smoothCheckBox[i].ToolTip = "Сглаживание";
            smoothCheckBox[i].Visibility = Visibility.Hidden;
            smoothCheckBox[i].Click += new RoutedEventHandler(click_checkedButton);

            stepTypeComboBox.Add(new ComboBox());
            stepTypeComboBox[i].Width = 100;
            stepTypeComboBox[i].VerticalAlignment = VerticalAlignment.Top;
            stepTypeComboBox[i].Margin = new Thickness(-175, 50, 0, 0);
            stepTypeComboBox[i].ToolTip = "Type step";
            stepTypeComboBox[i].ItemsSource = StepType;
            stepTypeComboBox[i].SelectedIndex = 0;
            stepTypeComboBox[i].Visibility = Visibility.Hidden;
            stepTypeComboBox[i].SelectionChanged += new SelectionChangedEventHandler(change_index);

            widthCheckBox.Add(new CheckBox());
            widthCheckBox[i].VerticalAlignment = VerticalAlignment.Top;
            widthCheckBox[i].Height = visibleCheckBox[i].Width = 16;
            widthCheckBox[i].Margin = new Thickness(-18, 60, 0, 0);
            widthCheckBox[i].ToolTip = "Толщина";
            widthCheckBox[i].Visibility = Visibility.Hidden;
            widthCheckBox[i].Click += new RoutedEventHandler(click_checkedButton);

            OpenClose.Add(new bool());
            OpenClose[i] = false;

            LayoutPanel[i].Children.Add(nameLabel[i]);
            LayoutPanel[i].Children.Add(colorEllipse[i]);
            LayoutPanel[i].Children.Add(visibleCheckBox[i]);
            LayoutPanel[i].Children.Add(typeComboBox[i]);
            LayoutPanel[i].Children.Add(smoothCheckBox[i]);
            LayoutPanel[i].Children.Add(stepTypeComboBox[i]);
            LayoutPanel[i].Children.Add(widthCheckBox[i]);
            LayoutPanel[i].Children.Add(panelBorder[i]);


            GraphStackPanel.Children.Add(LayoutPanel[i]);
        }

        private void change_index(object sender, SelectionChangedEventArgs e)
        {
            int j = 0;
            for (int i = 0; i < colorEllipse.Count; i++)
            {
                if (typeComboBox[i].IsMouseOver == true || stepTypeComboBox[i].IsMouseOver == true) { j = i; break; }
            }
            ChangeSomething(j, typeComboBox[j].SelectedIndex, stepTypeComboBox[j].SelectedIndex, GraphPanel.pane.CurveList[j].Color);
        }
        
        private void click_checkedButton(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i < colorEllipse.Count; i++)
            {
                if (visibleCheckBox[i].IsMouseOver == true || smoothCheckBox[i].IsMouseOver == true || widthCheckBox[i].IsMouseOver == true) { j = i; break; }
            }
            ChangeSomething(j, typeComboBox[j].SelectedIndex, stepTypeComboBox[j].SelectedIndex, GraphPanel.pane.CurveList[j].Color);
        }

        private void click_ColorEllipse(object sender, EventArgs e)
        {
            int j = 0;
            for (int i = 0; i < colorEllipse.Count; i++)
            {
                if (colorEllipse[i].IsMouseOver == true) { j = i; break; } 
            }
            var dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var color_colorEllipse = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                var brush_colorEllipse = new SolidColorBrush(color_colorEllipse);
                colorEllipse[j].Fill = brush_colorEllipse;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(brush_colorEllipse.Color.A, brush_colorEllipse.Color.R, brush_colorEllipse.Color.G, brush_colorEllipse.Color.B);
                ChangeSomething(j, typeComboBox[j].SelectedIndex, stepTypeComboBox[j].SelectedIndex, color);
            }
        }

        private void click_LayoutPanel(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < OpenClose.Count; i++)
            {
                if (LayoutPanel[i].IsMouseOver == true && OpenClose[i] == false) OpenAnimation(i);
                else if (LayoutPanel[i].IsMouseOver == true && OpenClose[i] == true) CloseAnimation(i);
            }
        }

        private void OpenAnimation(int i)
        {
            DoubleAnimation OpenAnimation = new DoubleAnimation();
            OpenAnimation.From = 30;
            OpenAnimation.To = 90;

            OpenAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, OpenAnimation);
            OpenClose[i] = true;

            {
                colorEllipse[i].Visibility = Visibility.Visible;
                typeComboBox[i].Visibility = Visibility.Visible;
                smoothCheckBox[i].Visibility = Visibility.Visible;
                stepTypeComboBox[i].Visibility = Visibility.Visible;
                widthCheckBox[i].Visibility = Visibility.Visible;
            }
        }
        private void CloseAnimation(int i)
        {
            DoubleAnimation CloseAnimation = new DoubleAnimation();
            CloseAnimation.From = 90;
            CloseAnimation.To = 30;
            CloseAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            LayoutPanel[i].BeginAnimation(DockPanel.MinHeightProperty, CloseAnimation);
            OpenClose[i] = false;

            {
                colorEllipse[i].Visibility = Visibility.Hidden;
                typeComboBox[i].Visibility = Visibility.Hidden;
                smoothCheckBox[i].Visibility = Visibility.Hidden;
                stepTypeComboBox[i].Visibility = Visibility.Hidden;
                widthCheckBox[i].Visibility = Visibility.Hidden;
            }
        }

        private void ChangeSomething(int num, int line, int typeStep, System.Drawing.Color color) {
            bool Show = true, Smooth = false, width = false;
            if (visibleCheckBox[num].IsChecked == false) Show = false;
            if (smoothCheckBox[num].IsChecked == true) Smooth = true;
            if (widthCheckBox[num].IsChecked == true) width = true;
            MainWindow.graph.ChangeLine(num, line, typeStep, width, Show, Smooth, color);
        }
    }
}
