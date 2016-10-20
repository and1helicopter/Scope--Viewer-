using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace WpfApplication4
{
    public partial class GraphPanel : UserControl
    {


        public GraphPanel()
        {
            InitializeComponent();
            CreateGraph(zedGraph);
            SetSize();
        }

        public new int Width
        {
            get { return zedGraph.Width; }
            set { zedGraph.Width = value; }
        }

        public new int Height
        {
            get { return zedGraph.Height; }
            set { zedGraph.Height = value; }
        }

        private bool enableZoom = true;
        public bool EnableZoom
        {
            get { return enableZoom; }
            set
            {
                if (value != enableZoom)
                {
                    enableZoom = value;
                    zedGraph.IsEnableHZoom = enableZoom;
                    zedGraph.IsEnableVZoom = enableZoom;
                }
            }
        }


        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane

            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Legend.IsVisible = false;
            myPane.XAxis.Title.IsVisible = false;
            myPane.XAxis.MajorGrid.IsVisible = true;
            //myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.YAxis.Title.IsVisible = false;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.Title.IsVisible = false;
            // Make up some data arrays based on the Sine function

            double x, y1, y2;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < 36; i++)
            {
                x = (double)i + 5;
                y1 = 1.5 + Math.Sin((double)i * 0.2);
                y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                list1.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a red curve with diamond

            // symbols, and "Porsche" in the legend

            LineItem myCurve = myPane.AddCurve("Porsche",
                  list1, System.Drawing.Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle

            // symbols, and "Piper" in the legend

            LineItem myCurve2 = myPane.AddCurve("Piper",
                  list2, System.Drawing.Color.Blue, SymbolType.Circle);

            // Tell ZedGraph to refigure the

            // axes since the data have changed

            zgc.AxisChange();
        }

        private void SetSize()
        {
            zedGraph.Location = new System.Drawing.Point(0, 0);
            // Leave a small margin around the outside of the control
            zedGraph.Size = new System.Drawing.Size((int)this.Width, (int)this.Height);
        }

        public void XMinorGRIDAxisChange(ZedGraphControl zgc, bool statusChecked)
        {   
            GraphPane myPane = zgc.GraphPane;

            if (statusChecked == true) { myPane.XAxis.MinorGrid.IsVisible = true; }
            else myPane.XAxis.MinorGrid.IsVisible = false;
            
            zgc.AxisChange();
        }
    }

}

