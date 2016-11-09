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
        public static GraphPane pane;
        List <LineItem> myCurve = new List<LineItem>();
        Random rngColor = new Random();



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

        public  Color GenerateColor(Random rng) {

            System.Drawing.Color color = Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            return color;
        }

        public void addGraph(int j)
        {
            // Создадим список точек
            PointPairList list = new PointPairList();
            string nameCh = "";

            // Заполняем список точек. Приращение по оси X 
            for (int i = 0; i < Oscil.NumCount; i++)
            {
                XDate time = 0;

                time.AddMilliseconds((i*100)/Oscil.SampleRate);
            
                // Для построения графика даты нужно преобразовать к Double
                //double[] xvalues = new double[dates.Length];

                // добавим в список точку
                list.Add((time), Oscil.Data[i][j]);
                nameCh = Oscil.ChannelNames[j];
            }

            // Выберем случайный цвет для графика
            LineItem newCurve;
            newCurve = pane.AddCurve(nameCh, list, GenerateColor(rngColor), SymbolType.None);
            newCurve.Line.IsOptimizedDraw = true;
            newCurve.Line.IsSmooth = false;
            myCurve.Add(newCurve);

            // Включим сглаживание

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
            //zedGraph.IsShowHScrollBar = true;

            pane = new GraphPane();
            masterPane.Add(pane);


            pane.Legend.IsVisible = false;
            pane.XAxis.Title.IsVisible = false;
            pane.XAxis.Scale.FontSpec.Size = 10;
            pane.YAxis.Title.IsVisible = false;
            pane.YAxis.Scale.FontSpec.Size = 10;
            pane.Title.IsVisible = false;
            pane.XAxis.Scale.FontSpec.Size = 10;
            pane.XAxis.Type = AxisType.Date;
            pane.XAxis.Scale.Format = "HH:mm:ss.ms";

            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            pane.IsBoundedRanges = true;

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
            if (statusChecked == true) { pane.YAxis.MinorGrid.IsVisible = true;  }
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

        public void AxisDash(int style, int XY)
        {
            if(style == 0)
            {
                if (XY == 0) { pane.YAxis.MajorGrid.DashOff = 5; pane.YAxis.MajorGrid.DashOn = 1; }
                if (XY == 1) { pane.YAxis.MinorGrid.DashOff = 10; pane.YAxis.MinorGrid.DashOn = 1; }
                if (XY == 2) { pane.XAxis.MajorGrid.DashOff = 5; pane.XAxis.MajorGrid.DashOn = 1; }
                if (XY == 3) { pane.XAxis.MinorGrid.DashOff = 10; pane.XAxis.MinorGrid.DashOn = 1; }
            }

            if (style == 1)
            {
                if (XY == 0) { pane.YAxis.MajorGrid.DashOff = 5; pane.YAxis.MajorGrid.DashOn = 10; }
                if (XY == 1) { pane.YAxis.MinorGrid.DashOff = 10; pane.YAxis.MinorGrid.DashOn = 10; }
                if (XY == 2) { pane.XAxis.MajorGrid.DashOff = 5; pane.XAxis.MajorGrid.DashOn = 10; }
                if (XY == 3) { pane.XAxis.MinorGrid.DashOff = 10; pane.XAxis.MinorGrid.DashOn = 10; }
            }

            if (style == 2)
            {
                if (XY == 0) { pane.YAxis.MajorGrid.DashOff = 0; pane.YAxis.MajorGrid.DashOn = 10; }
                if (XY == 2) { pane.XAxis.MajorGrid.DashOff = 0; pane.XAxis.MajorGrid.DashOn = 10; }

            }
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }


        public void ChangeLine(int numChannel, int typeLine, int typeStep, bool width, bool show, bool smooth, Color colorLine)
        {
            pane.CurveList[numChannel].Color = colorLine;
            pane.CurveList[numChannel].IsVisible = show;
            myCurve[numChannel].Line.IsSmooth = smooth;
            myCurve[numChannel].Line.SmoothTension = 0.5F;

            if (width == true) myCurve[numChannel].Line.Width = 2;
            else myCurve[numChannel].Line.Width = 1;

            if (typeLine == 0) myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            if (typeLine == 1) myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            if (typeLine == 2) myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot;
            if (typeLine == 3) myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            if (typeLine == 4) myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;

            if (typeStep == 0) myCurve[numChannel].Line.StepType = StepType.NonStep;
            if (typeStep == 1) myCurve[numChannel].Line.StepType = StepType.ForwardSegment;
            if (typeStep == 2) myCurve[numChannel].Line.StepType = StepType.ForwardStep;
            if (typeStep == 3) myCurve[numChannel].Line.StepType = StepType.RearwardSegment;
            if (typeStep == 4) myCurve[numChannel].Line.StepType = StepType.RearwardStep;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
    }
}

