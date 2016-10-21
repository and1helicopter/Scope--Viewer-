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
        GraphPane pane;

        public GraphPanel()
        {
            InitializeComponent();
            InitDrawGraph();
        }

        public void InitDrawGraph()
        {
            pane = zedGraph.GraphPane;

            pane.Legend.IsVisible = false;
            pane.XAxis.Title.IsVisible = false;
            pane.YAxis.Title.IsVisible = false;
            pane.Title.IsVisible = false;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void XMinorGRIDAxisChange(bool statusChecked)
        {
            pane = zedGraph.GraphPane;
            if (statusChecked == true) { pane.XAxis.MinorGrid.IsVisible = true; }
            else pane.XAxis.MinorGrid.IsVisible = false;
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void XMajorGRIDAxisChange(bool statusChecked)
        {
            pane = zedGraph.GraphPane;
            if (statusChecked == true) { pane.XAxis.MajorGrid.IsVisible = true; }
            else pane.XAxis.MajorGrid.IsVisible = false;
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void YMinorGRIDAxisChange(bool statusChecked)
        {
            pane = zedGraph.GraphPane;
            if (statusChecked == true) { pane.YAxis.MinorGrid.IsVisible = true; }
            else pane.YAxis.MinorGrid.IsVisible = false;
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void YMajorGRIDAxisChange(bool statusChecked)
        {
            pane = zedGraph.GraphPane;
            if (statusChecked == true) { pane.YAxis.MajorGrid.IsVisible = true; }
            else pane.YAxis.MajorGrid.IsVisible = false;
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
    }
}

