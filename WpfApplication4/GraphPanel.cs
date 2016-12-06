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
        ZedGraph.MasterPane _masterPane;
        public static GraphPane Pane;
        List<LineItem> _myCurve = new List<LineItem>();
       // List<LineItem> myCurveTemp = new List<LineItem>();
        Random _rngColor = new Random();

        double _maxXAxis = 1000;
        double _minXAxis = -1000;
        double _maxYAxis = 1000;
        double _minYAxis = -1000;
        bool _scaleY = false;

        public GraphPanel()
        {
            InitializeComponent();
            zedGraph.ZoomEvent += new ZedGraphControl.ZoomEventHandler(zedGraph_ZoomEvent);
            zedGraph.ScrollEvent += new ScrollEventHandler(zedGraph_ScrollEvent);
            InitDrawGraph();
        }

        private void zedGraph_ScrollEvent(object sender, ScrollEventArgs e)
        {
            ScrollEvent();
        }

        private void zedGraph_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            Pane = sender.GraphPane;
            ScrollEvent();
        }

        private void ScrollEvent()
        {            
            if (Pane.XAxis.Scale.Min <= _minXAxis)
            {
                Pane.XAxis.Scale.Min = _minXAxis;
            }

            if (Pane.XAxis.Scale.Max >= _maxXAxis)
            {
                Pane.XAxis.Scale.Max = _maxXAxis;
            }

            if (Pane.YAxis.Scale.Min <= _minYAxis)
            {
                Pane.YAxis.Scale.Min = _minYAxis;
            }

            if (Pane.YAxis.Scale.Max >= _maxYAxis)
            {
                Pane.YAxis.Scale.Max = _maxYAxis;
            }

            if (_scaleY == false)
            {
                Pane.YAxis.Scale.Max = _maxYAxis;
                Pane.YAxis.Scale.Min = _minYAxis;
            }

            Pane.YAxis.Scale.Mag = 0;

            UpdateCursor();
            UpdateGraph();
        }

        public void ChangeScale()
        {
            _scaleY = !_scaleY;
        }

        public  Color GenerateColor(Random rng) {

            System.Drawing.Color color = Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            return color;
        }

        // Создадим список точек   
        public static List<PointPairList> ListTemp = new List<PointPairList>();
        public static int PointCashCount = 250;

        public void PointPerChannel(int point)
        {
            PointCashCount = point;
        }

        public void ClearListTemp()
        {
            ListTemp.Clear();
        }

       
        private void UpdateGraph()
        {
            try {
                int startIndex = 0;
                int stopIndex = ListTemp[0].Count - 1;

                for (int i = 0; i < Pane.CurveList.Count; i++)
                {
                    for (int j = Pane.CurveList[i].NPts - 1; j >= 0; j--) Pane.CurveList[i].RemovePoint(j);
                }

                for (int k = 0; k < ListTemp[0].Count; k++)
                {
                    if (ListTemp[0][k].X >= Pane.XAxis.Scale.Min)
                    { startIndex = k; break; }
                }
                for (int k = 0; k < ListTemp[0].Count; k++)
                {
                    if (ListTemp[0][k].X >= Pane.XAxis.Scale.Max)
                    {
                        stopIndex = k; break;
                    }
                }
                int sum = Convert.ToInt32((double)((stopIndex - startIndex) / PointCashCount));
                if (sum == 0) sum = 1;

                for (int i = 0; i < Pane.CurveList.Count; i++)
                {
                    for (int j = startIndex; j < stopIndex; j += sum)
                    {
                        Pane.CurveList[i].AddPoint(ListTemp[i][j]);
                    }
                }
            }
            catch { }
        }

    
        public void AddGraph(int j)
        {
            PointPairList list = new PointPairList(); ;
            ListTemp.Add(new PointPairList());

            string nameCh = "";

            // Заполняем список точек. Приращение по оси X 
            int sum = Convert.ToInt32((double)(MainWindow._oscilList[MainWindow._oscilList.Count - 1].NumCount / PointCashCount));
            if (sum == 0) sum = 1;
            DateTime tempTime;

            for (int i = 0; i < MainWindow._oscilList[MainWindow._oscilList.Count - 1].NumCount; i+= sum)
            {
                tempTime = MainWindow._oscilList[MainWindow._oscilList.Count - 1].StampDateStart;
                // добавим в список точку
                list.Add(new XDate (tempTime.AddMilliseconds((i * 100) / MainWindow._oscilList[MainWindow._oscilList.Count - 1].SampleRate)), MainWindow._oscilList[MainWindow._oscilList.Count - 1].Data[i][j]);
                nameCh = MainWindow._oscilList[MainWindow._oscilList.Count - 1].ChannelNames[j];
            }

            for (int i = 0; i < MainWindow._oscilList[MainWindow._oscilList.Count - 1].NumCount; i++)   
            {
                tempTime = MainWindow._oscilList[MainWindow._oscilList.Count - 1].StampDateStart;
                // добавим в список точку
                ListTemp[j].Add(new XDate(tempTime.AddMilliseconds((i * 100) / MainWindow._oscilList[MainWindow._oscilList.Count - 1].SampleRate)), MainWindow._oscilList[MainWindow._oscilList.Count - 1].Data[i][j]);
            }

            // Выберем случайный цвет для графика
            LineItem newCurve;
            newCurve = Pane.AddCurve(nameCh, list, GenerateColor(_rngColor), SymbolType.None);
            newCurve.Line.IsSmooth = false;
            _myCurve.Add(newCurve);

            // Включим сглаживание
            Pane.YAxis.Scale.MinGrace = 0.01;
            Pane.YAxis.Scale.MaxGrace = 0.01;
            Pane.XAxis.Scale.MinGrace = 0.01;
            Pane.XAxis.Scale.MaxGrace = 0.01;
            Pane.YAxis.Scale.MinAuto = true;
            Pane.YAxis.Scale.MaxAuto = true;
            Pane.XAxis.Scale.MinAuto = true;
            Pane.XAxis.Scale.MaxAuto = true;
            zedGraph.IsShowHScrollBar = true;
            zedGraph.IsShowVScrollBar = true;
            zedGraph.IsAutoScrollRange = true;

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();

            _maxXAxis = zedGraph.GraphPane.XAxis.Scale.Max;
            _minXAxis = zedGraph.GraphPane.XAxis.Scale.Min;
            _maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max;
            _minYAxis = zedGraph.GraphPane.YAxis.Scale.Min;
        }


        public void RemoveGraph(int i)
        {
            _myCurve.Remove(_myCurve[i]);
            Pane.CurveList.Remove(Pane.CurveList[i]);

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void InitDrawGraph()
        {

            _masterPane = zedGraph.MasterPane;
            _masterPane.PaneList.Clear();
            //zedGraph.IsShowHScrollBar = true;

            Pane = new GraphPane();
            _masterPane.Add(Pane);

            /*
            GraphPane pane1 = new GraphPane();
            masterPane.Add(pane1);

            GraphPane pane2 = new GraphPane();
            masterPane.Add(pane2);
            */

            Pane.Legend.IsVisible = false;
            Pane.XAxis.Title.IsVisible = false;
            Pane.XAxis.Scale.FontSpec.Size = 10;
            Pane.YAxis.Title.IsVisible = false;
            Pane.YAxis.Scale.FontSpec.Size = 10;
            Pane.Title.IsVisible = false;
            Pane.XAxis.Scale.FontSpec.Size = 10;
            Pane.XAxis.Type = AxisType.Date;
            Pane.XAxis.Scale.Format = "HH:mm:ss.fff";
            Pane.YAxis.MajorGrid.IsZeroLine = false;
            Pane.XAxis.MajorGrid.IsVisible = true;
            Pane.YAxis.MajorGrid.IsVisible = true;

            Pane.YAxis.Scale.Mag = 0;

            Pane.YAxis.Scale.MinAuto = true;
            Pane.YAxis.Scale.MaxAuto = true;

            Pane.IsBoundedRanges = true;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void GridAxisChange(bool statusChecked, int style, int xy)
        {
            if (xy == 0)
            {
                if (style == 0) { Pane.XAxis.MinorGrid.DashOff = 10; Pane.XAxis.MinorGrid.DashOn = 1; }
                if (style == 1) { Pane.XAxis.MinorGrid.DashOff = 10; Pane.XAxis.MinorGrid.DashOn = 10; }
                if (statusChecked == true) Pane.XAxis.MinorGrid.IsVisible = true;
                else Pane.XAxis.MinorGrid.IsVisible = false;
            }

            if (xy == 1)
            {
                if (style == 0) { Pane.XAxis.MajorGrid.DashOff = 5; Pane.XAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { Pane.XAxis.MajorGrid.DashOff = 5; Pane.XAxis.MajorGrid.DashOn = 10; }
                if (style == 2) { Pane.XAxis.MajorGrid.DashOff = 0; Pane.XAxis.MajorGrid.DashOn = 10; }
                if (statusChecked == true) Pane.XAxis.MajorGrid.IsVisible = true;
                else Pane.XAxis.MajorGrid.IsVisible = false;
            }

            if (xy == 2)
            {
                if (style == 0) { Pane.YAxis.MajorGrid.DashOff = 5; Pane.YAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { Pane.YAxis.MinorGrid.DashOff = 10; Pane.YAxis.MinorGrid.DashOn = 10; }
                if (statusChecked == true) Pane.YAxis.MinorGrid.IsVisible = true;
                else Pane.YAxis.MinorGrid.IsVisible = false;
            }

            if (xy == 3)
            {
                if (style == 0) { Pane.YAxis.MajorGrid.DashOff = 5; Pane.YAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { Pane.YAxis.MajorGrid.DashOff = 5; Pane.YAxis.MajorGrid.DashOn = 10; }
                if (style == 2) { Pane.YAxis.MajorGrid.DashOff = 0; Pane.YAxis.MajorGrid.DashOn = 10; }
                if (statusChecked == true) Pane.YAxis.MajorGrid.IsVisible = true;
                else Pane.YAxis.MajorGrid.IsVisible = false;
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }


        public void ChangeLine(int numChannel, int typeLine, int typeStep, bool width, bool show, bool smooth, Color colorLine)
        {
            Pane.CurveList[numChannel].Color = colorLine;
            Pane.CurveList[numChannel].IsVisible = show;
            _myCurve[numChannel].Line.IsSmooth = smooth;
            _myCurve[numChannel].Line.SmoothTension = 0.5F;

            if (width == true) _myCurve[numChannel].Line.Width = 2;
            else _myCurve[numChannel].Line.Width = 1;

            if (typeLine == 0) _myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            if (typeLine == 1) _myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            if (typeLine == 2) _myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot;
            if (typeLine == 3) _myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            if (typeLine == 4) _myCurve[numChannel].Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;

            if (typeStep == 0) _myCurve[numChannel].Line.StepType = StepType.NonStep;
            if (typeStep == 1) _myCurve[numChannel].Line.StepType = StepType.ForwardSegment;
            if (typeStep == 2) _myCurve[numChannel].Line.StepType = StepType.ForwardStep;
            if (typeStep == 3) _myCurve[numChannel].Line.StepType = StepType.RearwardSegment;
            if (typeStep == 4) _myCurve[numChannel].Line.StepType = StepType.RearwardStep;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void LegendShow(bool show, int fontSize, int h, int v)
        {
            // Указываем, что расположение легенды мы будет задавать
            // в виде координат левого верхнего угла
            Pane.Legend.Position = LegendPos.Float;

            // Координаты будут отсчитываться в системе координат окна графика
            Pane.Legend.Location.CoordinateFrame = CoordType.ChartFraction;
            if (h == 1)
            {
                if(v == 1)
                {
                    Pane.Legend.Location.AlignH = AlignH.Right;
                    Pane.Legend.Location.AlignV = AlignV.Bottom;

                    // Задаем координаты легенды
                    // Вычитаем 0.02f, чтобы был небольшой зазор между осями и легендой
                    Pane.Legend.Location.TopLeft = new PointF(1.0f - 0.02f, 1.0f - 0.02f);
                }
                if (v == 0)
                {
                    Pane.Legend.Location.AlignH = AlignH.Right;
                    Pane.Legend.Location.AlignV = AlignV.Top;

                    // Задаем координаты легенды
                    // Вычитаем 0.02f, чтобы был небольшой зазор между осями и легендой
                    Pane.Legend.Location.TopLeft = new PointF(1.0f - 0.02f, 0.02f);
                }
            }

            if (h == 0)
            {
                if (v == 1)
                {
                    Pane.Legend.Location.AlignH = AlignH.Left;
                    Pane.Legend.Location.AlignV = AlignV.Bottom;

                    Pane.Legend.Location.TopLeft = new PointF(0.02f, 1.0f - 0.02f);

                }
                if (v == 0)
                {
                    Pane.Legend.Location.AlignH = AlignH.Left;
                    Pane.Legend.Location.AlignV = AlignV.Top;

                    Pane.Legend.Location.TopLeft = new PointF(0.02f, 0.02f);

                }
            }

            // Задаем выравнивание, относительно которого мы будем задавать координаты
            // В данном случае мы будем располагать легенду справа внизу

            // pane.Legend.FontSpec = 10;


            Pane.Legend.FontSpec.Size = fontSize;


            if (show == true) Pane.Legend.IsVisible = true;
            else Pane.Legend.IsVisible = false;


            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        LineObj _stampTrigger;
        public void LineStampTrigger()
        {
            XDate timeStamp = MainWindow._oscilList[MainWindow._oscilList.Count - 1].StampDateTrigger;
            _stampTrigger = new LineObj(timeStamp, Pane.YAxis.Scale.Min, timeStamp, Pane.YAxis.Scale.Max);
            // Стиль линии - пунктирная
            _stampTrigger.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDot;
            _stampTrigger.Line.Color = Color.Chocolate;
            _stampTrigger.Line.Width = 2;
            _stampTrigger.Link.Title = "StampTrigger";

            Pane.GraphObjList.Add(_stampTrigger);
        }

        public void StampTriggerClear()
        {
            for (int i = GraphPanel.Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (GraphPanel.Pane.GraphObjList[i].Link.Title == "StampTrigger") GraphPanel.Pane.GraphObjList.Remove(GraphPanel.Pane.GraphObjList[i]);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public static LineObj Cursor1;
        public static LineObj Cursor2;

        public void CursorAdd()
        {
            Cursor1 = new LineObj(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/4 + Pane.XAxis.Scale.Min), Pane.YAxis.Scale.Min, ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/4 + Pane.XAxis.Scale.Min), Pane.YAxis.Scale.Max);
            // Стиль линии - пунктирная
            Cursor1.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            Cursor1.Line.Color = Color.Red;
            Cursor1.Line.Width = 2;
            Cursor1.Link.Title = "Cursor1";

            Cursor2 = new LineObj(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)*3 / 4 + Pane.XAxis.Scale.Min), Pane.YAxis.Scale.Min, ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) * 3 / 4 + Pane.XAxis.Scale.Min), Pane.YAxis.Scale.Max);
            // Стиль линии - пунктирная
            Cursor2.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            Cursor2.Line.Color = Color.Blue;
            Cursor2.Line.Width = 2;
            Cursor2.Link.Title = "Cursor2";

            // Добавим линию в список отображаемых объектов
            Pane.GraphObjList.Add(Cursor1);
            Pane.GraphObjList.Add(Cursor2);

            // Обновляем график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void CursorClear()
        {
            for (int i = GraphPanel.Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (GraphPanel.Pane.GraphObjList[i].Link.Title == "Cursor1" || GraphPanel.Pane.GraphObjList[i].Link.Title == "Cursor2") GraphPanel.Pane.GraphObjList.Remove(GraphPanel.Pane.GraphObjList[i]);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void UpdateCursor()
        {
            for (int i = GraphPanel.Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (GraphPanel.Pane.GraphObjList[i].Link.Title == "Cursor1")
                {
                    Cursor1.Location.Y1 = Pane.YAxis.Scale.Min;
                    Cursor1.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

                    if (Cursor1.Location.X <= Pane.XAxis.Scale.Min) Cursor1.Location.X = Pane.XAxis.Scale.Min + (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    if (Cursor1.Location.X >= Pane.XAxis.Scale.Max) Cursor1.Location.X = Pane.XAxis.Scale.Max - (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    continue;
                }
                if (GraphPanel.Pane.GraphObjList[i].Link.Title == "Cursor2")
                {
                    Cursor2.Location.Y1 = Pane.YAxis.Scale.Min;
                    Cursor2.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

                    if (Cursor2.Location.X <= Pane.XAxis.Scale.Min) Cursor2.Location.X = Pane.XAxis.Scale.Min + (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    if (Cursor2.Location.X >= Pane.XAxis.Scale.Max) Cursor2.Location.X = Pane.XAxis.Scale.Max - (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    continue;
                }
                if (_stampTrigger.Link.Title == "StampTrigger")
                {
                    _stampTrigger.Location.Y1 = Pane.YAxis.Scale.Min;
                    _stampTrigger.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);
                    continue;
                }
            }           
        }

        private void zedGraph_MouseMove(object sender, MouseEventArgs e)
        {
            double graphX, graphY;
            Pane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY);

            if (Cursor1 != null || Cursor2 != null)
            {
                if (Cursor1.Line.Width == 3)
                {
                    Cursor1.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                }

                if (Cursor2.Line.Width == 3)
                {
                    Cursor2.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                }
            }
        }

        private void zedGraph_MouseClick(object sender, MouseEventArgs e)
        {
            LineObj lineObject;
            object nearestObject;
            int index;
            Pane.FindNearestObject(new PointF(e.X, e.Y), this.CreateGraphics(), out nearestObject, out index);
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
                MainWindow.AnalysisObj.UpdateCursor();

                zedGraph.Invalidate();
            }
        }
    }
}

