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
        List<LineItem> myCurve = new List<LineItem>();
       // List<LineItem> myCurveTemp = new List<LineItem>();
        Random rngColor = new Random();

        double maxXAxis = 1000;
        double minXAxis = -1000;
        double maxYAxis = 1000;
        double minYAxis = -1000;
        bool scaleY = false;

        public GraphPanel()
        {
            InitializeComponent();
            zedGraph.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zedGraph_ZoomEvent);
            zedGraph.ScrollEvent += new ScrollEventHandler(zedGraph_ScrollEvent);
            InitDrawGraph();
        }

        private void zedGraph_ScrollEvent(object sender, ScrollEventArgs e)
        {
            scrollEvent();
        }

        private void zedGraph_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            pane = sender.GraphPane;
            scrollEvent();
        }

        private void scrollEvent()
        {            
         //   

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

            if (scaleY == false)
            {
                pane.YAxis.Scale.Max = maxYAxis;
                pane.YAxis.Scale.Min = minYAxis;
            }

            updateCursor();
            updateGraph();
        }

        public void changeScale()
        {
            scaleY = !scaleY;
        }

        public  Color GenerateColor(Random rng) {

            System.Drawing.Color color = Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            return color;
        }

        // Создадим список точек   
        public static List<PointPairList> listTemp = new List<PointPairList>();
        int pointCashCount = 250;
        public void clearListTemp()
        {
            listTemp.Clear();
        }

       
        private void updateGraph()
        {
            try {
                int startIndex = 0;
                int stopIndex = listTemp[0].Count - 1;

                for (int i = 0; i < pane.CurveList.Count; i++)
                {
                    for (int j = pane.CurveList[i].NPts - 1; j >= 0; j--) pane.CurveList[i].RemovePoint(j);
                }

                for (int k = 0; k < listTemp[0].Count; k++)
                {
                    if (listTemp[0][k].X >= pane.XAxis.Scale.Min)
                    { startIndex = k; break; }
                }
                for (int k = 0; k < listTemp[0].Count; k++)
                {
                    if (listTemp[0][k].X >= pane.XAxis.Scale.Max)
                    {
                        stopIndex = k; break;
                    }
                }
                int sum = Convert.ToInt32((double)((stopIndex - startIndex) / pointCashCount));
                if (sum == 0) sum = 1;

                for (int i = 0; i < pane.CurveList.Count; i++)
                {
                    for (int j = startIndex; j < stopIndex; j += sum)
                    {
                        pane.CurveList[i].AddPoint(listTemp[i][j]);
                    }
                }
            }
            catch { }
        }

    
        public void addGraph(int j)
        {
            PointPairList list = new PointPairList(); ;
            listTemp.Add(new PointPairList());

            string nameCh = "";

            // Заполняем список точек. Приращение по оси X 
            int sum = Convert.ToInt32((double)(Oscil.NumCount / pointCashCount));
            if (sum == 0) sum = 1;
            DateTime TempTime;

            for (int i = 0; i < Oscil.NumCount; i+= sum)
            {
                TempTime = Oscil.StampDateStart;
                // добавим в список точку
                list.Add(new XDate (TempTime.AddMilliseconds((i * 100) / Oscil.SampleRate)), Oscil.Data[i][j]);
                nameCh = Oscil.ChannelNames[j];
            }

            for (int i = 0; i < Oscil.NumCount; i++)   
            {
                TempTime = Oscil.StampDateStart;
                // добавим в список точку
                listTemp[j].Add(new XDate(TempTime.AddMilliseconds((i * 100) / Oscil.SampleRate)), Oscil.Data[i][j]);
            }

            // Выберем случайный цвет для графика
            LineItem newCurve;
            newCurve = pane.AddCurve(nameCh, list, GenerateColor(rngColor), SymbolType.None);
            newCurve.Line.IsSmooth = false;
            myCurve.Add(newCurve);

            // Включим сглаживание
            pane.YAxis.Scale.MinGrace = 0.01;
            pane.YAxis.Scale.MaxGrace = 0.01;
            pane.XAxis.Scale.MinGrace = 0.01;
            pane.XAxis.Scale.MaxGrace = 0.01;
            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;
            pane.XAxis.Scale.MinAuto = true;
            pane.XAxis.Scale.MaxAuto = true;
            zedGraph.IsShowHScrollBar = true;
            zedGraph.IsAutoScrollRange = true;

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
            myCurve.Clear();
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
            pane.XAxis.Scale.Format = "HH:mm:ss.fff";
            pane.YAxis.MajorGrid.IsZeroLine = false;

            pane.YAxis.Scale.MinAuto = true;
            pane.YAxis.Scale.MaxAuto = true;

            pane.IsBoundedRanges = true;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void GRIDAxisChange(bool statusChecked, int style, int XY)
        {
            if (XY == 0)
            {
                if (style == 0) { pane.XAxis.MinorGrid.DashOff = 10; pane.XAxis.MinorGrid.DashOn = 1; }
                if (style == 1) { pane.XAxis.MinorGrid.DashOff = 10; pane.XAxis.MinorGrid.DashOn = 10; }
                if (statusChecked == true) pane.XAxis.MinorGrid.IsVisible = true;
                else pane.XAxis.MinorGrid.IsVisible = false;
            }

            if (XY == 1)
            {
                if (style == 0) { pane.XAxis.MajorGrid.DashOff = 5; pane.XAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { pane.XAxis.MajorGrid.DashOff = 5; pane.XAxis.MajorGrid.DashOn = 10; }
                if (style == 2) { pane.XAxis.MajorGrid.DashOff = 0; pane.XAxis.MajorGrid.DashOn = 10; }
                if (statusChecked == true) pane.XAxis.MajorGrid.IsVisible = true;
                else pane.XAxis.MajorGrid.IsVisible = false;
            }

            if (XY == 2)
            {
                if (style == 0) { pane.YAxis.MajorGrid.DashOff = 5; pane.YAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { pane.YAxis.MinorGrid.DashOff = 10; pane.YAxis.MinorGrid.DashOn = 10; }
                if (statusChecked == true) pane.YAxis.MinorGrid.IsVisible = true;
                else pane.YAxis.MinorGrid.IsVisible = false;
            }

            if (XY == 3)
            {
                if (style == 0) { pane.YAxis.MajorGrid.DashOff = 5; pane.YAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { pane.YAxis.MajorGrid.DashOff = 5; pane.YAxis.MajorGrid.DashOn = 10; }
                if (style == 2) { pane.YAxis.MajorGrid.DashOff = 0; pane.YAxis.MajorGrid.DashOn = 10; }
                if (statusChecked == true) pane.YAxis.MajorGrid.IsVisible = true;
                else pane.YAxis.MajorGrid.IsVisible = false;
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

        public void LegendShow(bool show, int fontSize, int H, int V)
        {
            // Указываем, что расположение легенды мы будет задавать
            // в виде координат левого верхнего угла
            pane.Legend.Position = LegendPos.Float;

            // Координаты будут отсчитываться в системе координат окна графика
            pane.Legend.Location.CoordinateFrame = CoordType.ChartFraction;
            if (H == 1)
            {
                if(V == 1)
                {
                    pane.Legend.Location.AlignH = AlignH.Right;
                    pane.Legend.Location.AlignV = AlignV.Bottom;

                    // Задаем координаты легенды
                    // Вычитаем 0.02f, чтобы был небольшой зазор между осями и легендой
                    pane.Legend.Location.TopLeft = new PointF(1.0f - 0.02f, 1.0f - 0.02f);
                }
                if (V == 0)
                {
                    pane.Legend.Location.AlignH = AlignH.Right;
                    pane.Legend.Location.AlignV = AlignV.Top;

                    // Задаем координаты легенды
                    // Вычитаем 0.02f, чтобы был небольшой зазор между осями и легендой
                    pane.Legend.Location.TopLeft = new PointF(1.0f - 0.02f, 0.02f);
                }
            }

            if (H == 0)
            {
                if (V == 1)
                {
                    pane.Legend.Location.AlignH = AlignH.Left;
                    pane.Legend.Location.AlignV = AlignV.Bottom;

                    pane.Legend.Location.TopLeft = new PointF(0.02f, 1.0f - 0.02f);

                }
                if (V == 0)
                {
                    pane.Legend.Location.AlignH = AlignH.Left;
                    pane.Legend.Location.AlignV = AlignV.Top;

                    pane.Legend.Location.TopLeft = new PointF(0.02f, 0.02f);

                }
            }

            // Задаем выравнивание, относительно которого мы будем задавать координаты
            // В данном случае мы будем располагать легенду справа внизу

            // pane.Legend.FontSpec = 10;


            pane.Legend.FontSpec.Size = fontSize;


            if (show == true) pane.Legend.IsVisible = true;
            else pane.Legend.IsVisible = false;


            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        LineObj StampTrigger;
        public void lineStampTrigger()
        {
            XDate timeStamp = Oscil.StampDateTrigger;
            StampTrigger = new LineObj(timeStamp, pane.YAxis.Scale.Min, timeStamp, pane.YAxis.Scale.Max);
            // Стиль линии - пунктирная
            StampTrigger.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot;
            StampTrigger.Line.Color = Color.Chocolate;
            StampTrigger.Line.Width = 2;
            StampTrigger.Link.Title = "StampTrigger";

            pane.GraphObjList.Add(StampTrigger);
        }

        public void StampTriggerClear()
        {
            for (int i = GraphPanel.pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (GraphPanel.pane.GraphObjList[i].Link.Title == "StampTrigger") GraphPanel.pane.GraphObjList.Remove(GraphPanel.pane.GraphObjList[i]);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public static LineObj Cursor1;
        public static LineObj Cursor2;

        public void CursorAdd()
        {
            Cursor1 = new LineObj(((pane.XAxis.Scale.Max - pane.XAxis.Scale.Min)/4 + pane.XAxis.Scale.Min), pane.YAxis.Scale.Min, ((pane.XAxis.Scale.Max - pane.XAxis.Scale.Min)/4 + pane.XAxis.Scale.Min), pane.YAxis.Scale.Max);
            // Стиль линии - пунктирная
            Cursor1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            Cursor1.Line.Color = Color.Red;
            Cursor1.Line.Width = 2;
            Cursor1.Link.Title = "Cursor1";

            Cursor2 = new LineObj(((pane.XAxis.Scale.Max - pane.XAxis.Scale.Min)*3 / 4 + pane.XAxis.Scale.Min), pane.YAxis.Scale.Min, ((pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) * 3 / 4 + pane.XAxis.Scale.Min), pane.YAxis.Scale.Max);
            // Стиль линии - пунктирная
            Cursor2.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            Cursor2.Line.Color = Color.Blue;
            Cursor2.Line.Width = 2;
            Cursor2.Link.Title = "Cursor2";

            // Добавим линию в список отображаемых объектов
            pane.GraphObjList.Add(Cursor1);
            pane.GraphObjList.Add(Cursor2);

            // Обновляем график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void CursorClear()
        {
            for (int i = GraphPanel.pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (GraphPanel.pane.GraphObjList[i].Link.Title == "Cursor1" || GraphPanel.pane.GraphObjList[i].Link.Title == "Cursor2") GraphPanel.pane.GraphObjList.Remove(GraphPanel.pane.GraphObjList[i]);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void updateCursor()
        {
            for (int i = GraphPanel.pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (GraphPanel.pane.GraphObjList[i].Link.Title == "Cursor1")
                {
                    Cursor1.Location.Y1 = pane.YAxis.Scale.Min;
                    Cursor1.Location.Height = (pane.YAxis.Scale.Max - pane.YAxis.Scale.Min);

                    if (Cursor1.Location.X <= pane.XAxis.Scale.Min) Cursor1.Location.X = pane.XAxis.Scale.Min + (pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) / 100;
                    if (Cursor1.Location.X >= pane.XAxis.Scale.Max) Cursor1.Location.X = pane.XAxis.Scale.Max - (pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) / 100;
                    continue;
                }
                if (GraphPanel.pane.GraphObjList[i].Link.Title == "Cursor2")
                {
                    Cursor2.Location.Y1 = pane.YAxis.Scale.Min;
                    Cursor2.Location.Height = (pane.YAxis.Scale.Max - pane.YAxis.Scale.Min);

                    if (Cursor2.Location.X <= pane.XAxis.Scale.Min) Cursor2.Location.X = pane.XAxis.Scale.Min + (pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) / 100;
                    if (Cursor2.Location.X >= pane.XAxis.Scale.Max) Cursor2.Location.X = pane.XAxis.Scale.Max - (pane.XAxis.Scale.Max - pane.XAxis.Scale.Min) / 100;
                    continue;
                }
                if (StampTrigger.Link.Title == "StampTrigger")
                {
                    StampTrigger.Location.Y1 = pane.YAxis.Scale.Min;
                    StampTrigger.Location.Height = (pane.YAxis.Scale.Max - pane.YAxis.Scale.Min);
                    continue;
                }
            }           
        }

        private void zedGraph_MouseMove(object sender, MouseEventArgs e)
        {
            double graphX, graphY;
            pane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY);

            if (Cursor1 != null || Cursor2 != null)
            {
                if (Cursor1.Line.Width == 3)
                {
                    Cursor1.Location.X1 = graphX;
                    updateCursor();
                    zedGraph.Invalidate();
                }

                if (Cursor2.Line.Width == 3)
                {
                    Cursor2.Location.X1 = graphX;
                    updateCursor();
                    zedGraph.Invalidate();
                }
            }
        }

        private void zedGraph_MouseClick(object sender, MouseEventArgs e)
        {
            LineObj lineObject;
            object nearestObject;
            int index;
            pane.FindNearestObject(new PointF(e.X, e.Y), this.CreateGraphics(), out nearestObject, out index);
            if (nearestObject != null && nearestObject.GetType() == typeof(LineObj))
            {
                lineObject = (LineObj)nearestObject;

                if (lineObject.Link.Title == "Cursor1")
                {
                    if (Cursor1.Line.Width == 3) { Cursor1.Line.Width = 2; zedGraph.Cursor = Cursors.HSplit; }
                    else { Cursor1.Line.Width = 3; Cursor2.Line.Width = 2; }
                }
                if (lineObject.Link.Title == "Cursor2")
                {
                    if (Cursor2.Line.Width == 3) { Cursor2.Line.Width = 2; zedGraph.Cursor = Cursors.HSplit; }
                    else { Cursor1.Line.Width = 2; Cursor2.Line.Width = 3; }
                }
                MainWindow.analysisObj.updateCursor();

                zedGraph.Invalidate();
            }
        }
    }
}

