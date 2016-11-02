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
        ZedGraph.MasterPane masterPane;
        GraphPane pane;

        double maxXAxis = 1000;
        double minXAxis = -1000;
        double maxYAxis = 1000;
        double minYAxis = -1000;


        public GraphPanel()
        {
            InitializeComponent();
            zedGraph.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zedGraph_ZoomEvent);
            InitDrawGraph();
        }

        void zedGraph_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            pane = sender.GraphPane;

            // Для простоты примера будем ограничивать масштабирование 
            // только в сторону уменьшения размера графика

            // Проверим интервал для каждой оси и 
            // при необходимости скорректируем его

            if (pane.XAxis.Scale.Min <= minXAxis)
            {
                pane.XAxis.Scale.Min = minXAxis;
            }

            if (pane.XAxis.Scale.Max >= maxXAxis)
            {
                pane.XAxis.Scale.Max = maxXAxis;
            }

            if (pane.YAxis.Scale.Min <= minYAxis)
            {
                pane.YAxis.Scale.Min = minYAxis;
            }

            if (pane.YAxis.Scale.Max >= maxYAxis)
            {
                pane.YAxis.Scale.Max = maxYAxis;
            }
        }


        public void addGraph(int j)
        {
            // Создадим список точек
            PointPairList list = new PointPairList();

            // Заполняем список точек. Приращение по оси X 
            for (int i = 0; i < Oscil.NumCount; i++)
            {
                // добавим в список точку
                list.Add(i, Oscil.Data[i][j]);
            }

            // Выберем случайный цвет для графика
            Random rng = new Random();
            LineItem myCurve = pane.AddCurve("", list, Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255)), SymbolType.None);

            // Включим сглаживание
            myCurve.Line.IsSmooth = true;

            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;
            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();

            maxXAxis = zedGraph.GraphPane.XAxis.Scale.Max;
            minXAxis = zedGraph.GraphPane.XAxis.Scale.Min;
            maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max;
            minYAxis = zedGraph.GraphPane.YAxis.Scale.Min;
        }

        public void removeGraph()
        {
            pane.CurveList.Clear();
        }

        public void InitDrawGraph()
        {

            masterPane = zedGraph.MasterPane;
            masterPane.PaneList.Clear();
            pane = new GraphPane();

            pane.Legend.IsVisible = false;
            pane.XAxis.Title.IsVisible = false;
            pane.XAxis.Scale.FontSpec.Size = 10;
            pane.YAxis.Title.IsVisible = false;
            pane.YAxis.Scale.FontSpec.Size = 10;
            pane.Title.IsVisible = false;
            pane.XAxis.Scale.FontSpec.Size = 10;

            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            pane.IsBoundedRanges = true;

            masterPane.Add(pane);

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

