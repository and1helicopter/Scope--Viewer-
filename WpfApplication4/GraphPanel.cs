using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace ScopeViewer
{
    public partial class GraphPanel : UserControl
    {
       // MasterPane _masterPane;
        public GraphPane Pane;
        readonly List<LineItem> _myCurve = new List<LineItem>();
       // List<LineItem> myCurveTemp = new List<LineItem>();

        double _maxXAxis = 1000;
        double _minXAxis = -1000;
        double _maxYAxis = 1000;
        double _minYAxis = -1000;
        bool _scaleY;

        public GraphPanel()
        {
            InitializeComponent();

            zedGraph.ZoomEvent += zedGraph_ZoomEvent;
            zedGraph.ScrollEvent += zedGraph_ScrollEvent;

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


        // Создадим список точек   
        private readonly List<PointPairList> _listTemp = new List<PointPairList>();
        public static int PointInLine = 250;
        private readonly List<bool> _digitalList = new List<bool>();
        public static bool ShowDigital;
 
        public void PointInLineChange(int point, bool showDigital)
        {
            PointInLine = point;
            ShowDigital = showDigital;
            UpdateGraph();
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }


        public void ChangeDigitalList(int i, int status)
        {
            _digitalList[i] = status != 0;
            UpdateGraph();
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void ClearListTemp()
        {
            _listTemp.Clear();
        }
        
       
        private void UpdateGraph()
        {
            if(_listTemp.Count == 0) return;
            try {
                //Очишаю точки кривых в кэше
                foreach (var i in Pane.CurveList)
                {
                    for (int j = i.NPts - 1; j >= 0; j--) i.RemovePoint(j);
                }

                for (int i = 0; i < Pane.CurveList.Count; i++)
                {
                    int startIndex = 0;
                    int stopIndex = _listTemp[i].Count - 1;

                    for (int k = 0; k < _listTemp[i].Count; k++)
                    {
                        if (_listTemp[i][k].X >= Pane.XAxis.Scale.Min)
                        {
                            startIndex = k != 0 ? k - 1 : k;
                            break;
                        }
                    }

                    for (int k = 0; k < _listTemp[i].Count; k++)
                    {
                        if (_listTemp[i][k].X >= Pane.XAxis.Scale.Max)
                        {
                            stopIndex = k == _listTemp[i].Count - 1 ? k : k + 1;
                            break;
                        }
                    }
                    // ReSharper disable once PossibleLossOfFraction
                    int sum = Convert.ToInt32((double)((stopIndex - startIndex) / PointInLine));
                    if (sum == 0 || (ShowDigital && _digitalList[i])) sum = 1;

                    for (int j = startIndex; j < stopIndex; j += sum)
                    {
                        Pane.CurveList[i].AddPoint(_listTemp[i][j]);
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }
 
    
        public void AddGraph(int j, Color color)
        {
            PointPairList list = new PointPairList();
            _listTemp.Add(new PointPairList());
            _digitalList.Add(MainWindow.OscilList[MainWindow.OscilList.Count - 1].TypeChannel[j]);

            string nameCh = "";

            // Заполняем список точек. Приращение по оси X 

            // ReSharper disable once PossibleLossOfFraction
            int sum = Convert.ToInt32((double)(MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount / PointInLine));
            if (sum == 0) sum = 1;
            DateTime tempTime;

            for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i+= sum)
            {
                tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].StampDateStart;
                // добавим в список точку
                list.Add(new XDate (tempTime.AddMilliseconds((i * 100) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate)), MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]);
                nameCh = MainWindow.OscilList[MainWindow.OscilList.Count - 1].ChannelNames[j];
            }

            for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i++)   
            {
                tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].StampDateStart;
                // добавим в список точку
                _listTemp[_listTemp.Count - 1].Add(new XDate(tempTime.AddMilliseconds((i * 100) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate)), MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]);
            }

            // Выберем случайный цвет для графика
            LineItem newCurve = Pane.AddCurve(nameCh, list, color, SymbolType.None);
            newCurve.Line.IsSmooth = false;
            _myCurve.Add(newCurve);

            ResizeAxis();
        }

        public void ResizeAxis()
        {
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
            _listTemp.Remove(_listTemp[i]);
            _digitalList.Remove(_digitalList[i]);

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void InitDrawGraph()
        {
            Pane = zedGraph.GraphPane; 

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
                Pane.XAxis.MinorGrid.IsVisible = statusChecked;
            }

            if (xy == 1)
            {
                if (style == 0) { Pane.XAxis.MajorGrid.DashOff = 5; Pane.XAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { Pane.XAxis.MajorGrid.DashOff = 5; Pane.XAxis.MajorGrid.DashOn = 10; }
                if (style == 2) { Pane.XAxis.MajorGrid.DashOff = 0; Pane.XAxis.MajorGrid.DashOn = 10; }
                Pane.XAxis.MajorGrid.IsVisible = statusChecked;
            }

            if (xy == 2)
            {
                if (style == 0) { Pane.YAxis.MajorGrid.DashOff = 5; Pane.YAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { Pane.YAxis.MinorGrid.DashOff = 10; Pane.YAxis.MinorGrid.DashOn = 10; }
                Pane.YAxis.MinorGrid.IsVisible = statusChecked;
            }

            if (xy == 3)
            {
                if (style == 0) { Pane.YAxis.MajorGrid.DashOff = 5; Pane.YAxis.MajorGrid.DashOn = 1; }
                if (style == 1) { Pane.YAxis.MajorGrid.DashOff = 5; Pane.YAxis.MajorGrid.DashOn = 10; }
                if (style == 2) { Pane.YAxis.MajorGrid.DashOff = 0; Pane.YAxis.MajorGrid.DashOn = 10; }
                Pane.YAxis.MajorGrid.IsVisible = statusChecked;
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }


        public void ChangeLine(int numChannel, int typeLine, int typeStep, bool width, bool show, bool smooth, Color colorLine)
        {
           // Pane = MainWindow.GraphPanelList[numGraphPane];

            Pane.CurveList[numChannel].Color = colorLine;
            Pane.CurveList[numChannel].IsVisible = show;
            _myCurve[numChannel].Line.IsSmooth = smooth;
            _myCurve[numChannel].Line.SmoothTension = 0.5F;

            _myCurve[numChannel].Line.Width = width ? 2 : 1;

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
            Pane.Legend.IsVisible = show;


            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private LineObj _stampTrigger;
        private TextObj _stampTriggTextObj;

        public void LineStampTrigger()
        {  try
            {
                XDate timeStamp = MainWindow.OscilList[MainWindow.OscilList.Count - 1].StampDateTrigger;
                _stampTrigger = new LineObj(timeStamp, Pane.YAxis.Scale.Min, timeStamp, Pane.YAxis.Scale.Max)
                {
                    Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.DashDot,
                    Color = Color.Chocolate,
                    Width = 2
                },
                    Link = { Title = "StampTrigger" }
                };

                _stampTriggTextObj = new TextObj(timeStamp.DateTime.Second + "." + timeStamp.DateTime.Millisecond.ToString("000"), timeStamp, 9*Pane.YAxis.Scale.Max/10)
                {
                    FontSpec =
                    {
                        Border = {IsVisible = false}
                    },
                    Link = {Title = "StampTrigger"}
                };


                Pane.GraphObjList.Add(_stampTriggTextObj);
                Pane.GraphObjList.Add(_stampTrigger);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            { }

        }

        public void StampTriggerClear()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "StampTrigger")
                {
                    Pane.GraphObjList.Remove(Pane.GraphObjList[i]);
                }
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }


        public static LineObj Cursor1;
        public static LineObj Cursor2;

        public void CursorAdd()
        {
            Cursor1 = new LineObj(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/4 + Pane.XAxis.Scale.Min),
                Pane.YAxis.Scale.Min, ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/4 + Pane.XAxis.Scale.Min),
                Pane.YAxis.Scale.Max)
            {
                Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.Red,
                    Width = 2
                },
                Link = {Title = "Cursor1"}
            };
            // Стиль линии - пунктирная

            Cursor2 = new LineObj(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)*3/4 + Pane.XAxis.Scale.Min),
                Pane.YAxis.Scale.Min, ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)*3/4 + Pane.XAxis.Scale.Min),
                Pane.YAxis.Scale.Max)
            {
                Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.Blue,
                    Width = 2
                },
                Link = {Title = "Cursor2"}
            };
            // Стиль линии - пунктирная

            // Добавим линию в список отображаемых объектов
            Pane.GraphObjList.Add(Cursor1);
            Pane.GraphObjList.Add(Cursor2);

            // Обновляем график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void CursorClear()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "Cursor1" || Pane.GraphObjList[i].Link.Title == "Cursor2") Pane.GraphObjList.Remove(Pane.GraphObjList[i]);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void UpdateCursor()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "Cursor1")
                {
                    Cursor1.Location.Y1 = Pane.YAxis.Scale.Min;
                    Cursor1.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

                    if (Cursor1.Location.X <= Pane.XAxis.Scale.Min) Cursor1.Location.X = Pane.XAxis.Scale.Min + (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    if (Cursor1.Location.X >= Pane.XAxis.Scale.Max) Cursor1.Location.X = Pane.XAxis.Scale.Max - (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    continue;
                }
                if (Pane.GraphObjList[i].Link.Title == "Cursor2")
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
                    _stampTriggTextObj.IsVisible = !(Pane.YAxis.Scale.Max >= _stampTriggTextObj.Location.Y1) &&
                                                   !(Pane.YAxis.Scale.Min <= _stampTriggTextObj.Location.Y1);

                    if (_stampTrigger.Location.X <= Pane.XAxis.Scale.Min)
                    {
                        _stampTrigger.IsVisible = false;
                        _stampTriggTextObj.IsVisible = false;
                    }
                    if (_stampTrigger.Location.X >= Pane.XAxis.Scale.Max)
                    {
                        _stampTrigger.IsVisible = false;
                        _stampTriggTextObj.IsVisible = false;
                    }
                    if (_stampTrigger.Location.X >= Pane.XAxis.Scale.Min &&
                        _stampTrigger.Location.X <= Pane.XAxis.Scale.Max)
                    {
                        _stampTrigger.IsVisible = true;
                        _stampTriggTextObj.IsVisible = true;

                    }
                }
            }           
        }

        private void zedGraph_MouseMove(object sender, MouseEventArgs e)
        {
            double graphX, graphY;
            Pane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY);

            if (Cursor1 != null || Cursor2 != null)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                // ReSharper disable once PossibleNullReferenceException
                if (Cursor1.Line.Width == 3)
                {
                    Cursor1.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                }

                // ReSharper disable once CompareOfFloatsByEqualityOperator
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
            object nearestObject;
            int index;
            Pane.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out nearestObject, out index);
            if (nearestObject != null && nearestObject.GetType() == typeof(LineObj))
            {
                LineObj lineObject = (LineObj)nearestObject;

                if (lineObject.Link.Title == "Cursor1")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Cursor1.Line.Width == 3) { Cursor1.Line.Width = 2; zedGraph.Cursor = Cursors.HSplit; }
                    else { Cursor1.Line.Width = 3; Cursor2.Line.Width = 2; }
                    MainWindow.AnalysisObj.UpdateCursor();

                }
                if (lineObject.Link.Title == "Cursor2")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Cursor2.Line.Width == 3) { Cursor2.Line.Width = 2; zedGraph.Cursor = Cursors.HSplit; }
                    else { Cursor1.Line.Width = 2; Cursor2.Line.Width = 3; }
                    MainWindow.AnalysisObj.UpdateCursor();
                }

                zedGraph.Invalidate();
            }
        }
    }
}

