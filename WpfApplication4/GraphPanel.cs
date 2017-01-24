using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace ScopeViewer
{
    public sealed partial class GraphPanel : UserControl
    {
       // MasterPane _masterPane;
        public GraphPane Pane;
        public GraphPane PaneDig;
        readonly List<LineItem> _myCurve = new List<LineItem>();
       // List<LineItem> myCurveTemp = new List<LineItem>();

        double _maxXAxis = 1000;
        double _minXAxis = -1000;
        double _maxYAxis = 1000;
        double _minYAxis = -1000;
        double _maxYAxisAuto;
        double _minYAxisAuto;
        bool _scaleY;

        public GraphPanel()
        {
            ListTemp = new List<PointPairList>();
            InitializeComponent();

            DoubleBuffered = true;

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
            ScrollEvent();
        }

        private void ScrollEvent()
        {            
            if (Pane.XAxis.Scale.Min <= _minXAxis)
            {
                Pane.XAxis.Scale.Min = _minXAxis;
                Pane.X2Axis.Scale.Min = _minXAxis;
            }

            if (Pane.XAxis.Scale.Max >= _maxXAxis)
            {
                Pane.XAxis.Scale.Max = _maxXAxis;
                Pane.X2Axis.Scale.Max = _maxXAxis;
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

            Pane.X2Axis.Scale.Max = Pane.XAxis.Scale.Max;
            Pane.X2Axis.Scale.Min = Pane.XAxis.Scale.Min;

            Pane.YAxis.Scale.Mag = 0;
            Pane.XAxis.Scale.Mag = 0;
            Pane.X2Axis.Scale.Mag = 0;

            UpdateCursor();
            UpdateGraph();
        }

        private void ChangeScale()
        {
            _scaleY = !_scaleY;
        }


        // Создадим список точек   
        public readonly List<PointPairList> ListTemp;
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


        public void ChangeDigitalList(int i, int j, int status)
        {
            _digitalList[i] = status != 0;
            MainWindow.OscilList[j].TypeChannel[i] = status != 0;
            UpdateGraph();
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
      
        private void UpdateGraph()
        {
            if(ListTemp.Count == 0) return;
            try {
                //Очишаю точки кривых в кэше
                foreach (var i in Pane.CurveList)
                {
                    for (int j = i.NPts - 1; j >= 0; j--) i.RemovePoint(j);
                }

                for (int i = 0; i < Pane.CurveList.Count; i++)
                {
                    int startIndex = 0;
                    int stopIndex = ListTemp[i].Count - 1;

                    for (int k = 0; k < ListTemp[i].Count; k++)
                    {
                        if (ListTemp[i][k].X >= Pane.XAxis.Scale.Min)
                        {
                            startIndex = k != 0 ? k - 1 : k;
                            break;
                        }
                    }

                    for (int k = 0; k < ListTemp[i].Count; k++)
                    {
                        if (ListTemp[i][k].X >= Pane.XAxis.Scale.Max)
                        {
                            stopIndex = k == ListTemp[i].Count - 1 ? k : k + 1;
                            break;
                        }
                    }
                    // ReSharper disable once PossibleLossOfFraction
                    int sum = Convert.ToInt32((double)((stopIndex - startIndex) / PointInLine));
                    if (sum == 0 || (ShowDigital && _digitalList[i])) sum = 1;

                    for (int j = startIndex; j < stopIndex; j += sum)
                    {
                        Pane.CurveList[i].AddPoint(ListTemp[i][j]);
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        string XAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            /*
            if ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) > 86400)
            {
                return string.Format("{0} H", val/3600);
            }
            if ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) > 3600)
            {
                return string.Format("{0} m", val/60);
            }*/
           
            if ((axis.Scale.Max - axis.Scale.Min) > 1)
            {

                return string.Format("{0} s", val);
            }
            if ((axis.Scale.Max - axis.Scale.Min) > 0.001)
            {
                return string.Format("{0} ms", val*1000);
            }
            if ((axis.Scale.Max - axis.Scale.Min) > 0.000001)
            {
                return string.Format("{0} \u03BCs", val*1000000);
            }
            else
            {
                // Остальные числа просто преобразуем в строку
                return string.Format("{0} ns", val*1000000000);
            }
        }

        string YAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            Pane.YAxis.MinorGrid.IsVisible = false;
            Pane.YAxis.Scale.MinorStep = 1.0;
            Pane.YAxis.MajorGrid.IsVisible = true;
            Pane.YAxis.Scale.MajorStep = 1.0;


            return string.Format("{0}", val * -1);
        }

        public void AddGraph(int j, Color color, bool dig)
        {
            zedGraph.IsEnableVZoom = false;

            Pane.Border.Color = Color.White;
            StampTime_label.Text = MainWindow.OscilList[NumGraphPanel()].StampDateTrigger + @"." +
                                   MainWindow.OscilList[NumGraphPanel()].StampDateTrigger.Millisecond.ToString("000");

            if (!dig) AddAnalogChannel(j, color);
            if (dig)  AddDigitalChannel(j, color);
        }

        private void AddAnalogChannel(int j, Color color)
        {
            AddCursorDig_button.Visible = false;
            toolStripSeparator1.Visible = false;
            Mask1_label.Visible = false;
            Mask2_label.Visible = false;
            MaskMin_textBox.Visible = false;
            MaskMax_textBox.Visible = false;
            AutoRange_Button.Visible = false;
            toolStripSeparator2.Visible = false;

            PointPairList list = new PointPairList();
            ListTemp.Add(new PointPairList());
            _digitalList.Add(MainWindow.OscilList[MainWindow.OscilList.Count - 1].TypeChannel[j]);

            string nameCh = "";

            // Заполняем список точек. Приращение по оси X 
            // ReSharper disable once PossibleLossOfFraction
            int sum = Convert.ToInt32((double)(MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount / PointInLine));
            if (sum == 0) sum = 1;
            // DateTime tempTime;

            for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i += sum)
            {
                //tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].StampDateStart;
                // добавим в список точку
                list.Add((i) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]);
                nameCh = MainWindow.OscilList[MainWindow.OscilList.Count - 1].ChannelNames[j];
            }

            for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i++)
            {
                //tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].StampDateStart;
                // добавим в список точку
                ListTemp[ListTemp.Count - 1].Add((i) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]);
            }

            // Выберем случайный цвет для графика
            LineItem newCurve = Pane.AddCurve(nameCh, list, color, SymbolType.None);
            newCurve.Line.IsSmooth = false;
            _myCurve.Add(newCurve);

            ResizeAxis();
        }

        private void AddDigitalChannel(int j, Color color)
        {
            zedGraph.IsShowPointValues = false;   //Отключил отображение точек 

            BinaryMask binObj = new BinaryMask();  //Вызов окна выбора маски 
            int binary = 0;

            DialogResult dlgr = binObj.ShowDialog();

            if (dlgr == DialogResult.OK)
            {
                binary = BinaryMask.BinnaryMask;
            }

            PointPairList list1 = new PointPairList();
            PointPairList list0 = new PointPairList();

            list1.Add(0, -0.2);
            list0.Add(0, -0.8);

            string nameCh1 = MainWindow.OscilList[MainWindow.OscilList.Count - 1].ChannelNames[j];
            double line1 = -0.2, line0 = -0.8;

            for (int i = 1; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i++)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j] !=
                    MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i - 1][j])
                {
                    list1.Add((i - 1) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line1);
                    list0.Add((i - 1 )/ MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line0);

                    double temp0 = line0;
                    double temp1 = line1;
                    line1 = temp0;
                    line0 = temp1;

                    list1.Add(i / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line1);
                    list0.Add(i / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line0);
                }
                if (i == MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount - 1)
                {
                    list1.Add((i - 1) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line1);
                    list0.Add((i - 1) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line0);
                }
            }

            LineItem newCurve1 = Pane.AddCurve(nameCh1, list1, color, SymbolType.None);
            LineItem newCurve0 = Pane.AddCurve(nameCh1, list0, color, SymbolType.None);
            newCurve0.Line.IsSmooth = false;
            newCurve1.Line.IsSmooth = false;
            _myCurve.Add(newCurve1);
            _myCurve[_myCurve.Count - 1].Line.Width = 2;
            _myCurve.Add(newCurve0);
            _myCurve[_myCurve.Count - 1].Line.Width = 2;

           // ListTemp.Add(new PointPairList());
            var list = new PointPairList();

            for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i++)
            {
                //tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].StampDateStart;
                // добавим в список точку
                list.Add((i) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]);
               // ListTemp[ListTemp.Count - 1].Add((i) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]);
            }

            LineItem newCurveBase = Pane.AddCurve(nameCh1, list, color, SymbolType.None);
            newCurveBase.Line.IsSmooth = false;
            newCurveBase.IsVisible = false;
            _myCurve.Add(newCurveBase);

            int maxMaskCount = 0;

            if (binary == 0)
            {
                maxMaskCount = 16;
            }
            else if(binary == 1)
            {
                maxMaskCount = 32;
            }

            for (int l = 0; l < maxMaskCount; l++)
            {
                list = new PointPairList();
                double line;

                if ((Convert.ToInt32(MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[0][j]) & 1 << l) == 1 << l)
                {
                    line = -0.2 - 1 - l;
                }
                else
                {
                    line = -0.8 - 1 - l;
                }

                list.Add(0 / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line);

                for (int i = 1; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount; i++)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if(MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j] !=
                       MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i - 1][j])
                    {
                        list.Add((i - 1)/ MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line);
                        if ((Convert.ToInt32(MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data[i][j]) & 1 << l) == 1 << l)
                        {
                            line = -0.2 - 1 - l;
                        }
                        else
                        {
                            line = -0.8 - 1 - l;
                        }
                        list.Add(i / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line);
                    }
                    if (i == MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount - 1)
                    {
                        list.Add((i - 1) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].SampleRate, line);
                    }
                }

                LineItem newCurve = Pane.AddCurve(nameCh1, list, color, SymbolType.None);
                newCurve.Line.IsSmooth = false;
                _myCurve.Add(newCurve);
                _myCurve[_myCurve.Count - 1].Line.Width = 2;
            }


            zedGraph.Resize += ZedGraph_Resize;

            AddCursor.Visible = false;

            Pane.YAxis.IsVisible = true;

            Pane.X2Axis.IsVisible = true;
            Pane.X2Axis.Scale.FontSpec.Size = 11;
            Pane.YAxis.ScaleFormatEvent += YAxis_ScaleFormatEvent;

            Pane.YAxis.MinorGrid.IsVisible = false;
            Pane.YAxis.Scale.MinorStep = 1.0;
            Pane.YAxis.MajorGrid.IsVisible = true;
            Pane.YAxis.Scale.MajorStep = 1.0;
            Pane.YAxis.MajorGrid.DashOn = 1;
            Pane.YAxis.MajorGrid.DashOff = 0;


            Pane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Center;
            Pane.YAxis.Scale.FontSpec.Angle = 80;

            ResizeAxis();

            _maxXAxis = zedGraph.GraphPane.XAxis.Scale.Max;
            _minXAxis = zedGraph.GraphPane.XAxis.Scale.Min;
            _maxYAxis = 0;
            _minYAxis = -maxMaskCount - 1;
            _maxYAxisAuto = _maxYAxis;
            _minYAxisAuto = _minYAxis;

            Pane.X2Axis.Scale.Max = Pane.XAxis.Scale.Max;
            Pane.X2Axis.Scale.Min = Pane.XAxis.Scale.Min;

            MaskMin_textBox.Text = Convert.ToInt32(-1 * _minYAxis).ToString();
            MaskMax_textBox.Text = Convert.ToInt32(-1 * _maxYAxis).ToString();

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void ZedGraph_Resize(object sender, EventArgs e)
        {
            zedGraph.GraphPane.YAxis.Scale.Min = _minYAxis;
        }

        private void ResizeAxis()
        {
            // Включим сглаживание
            Pane.YAxis.Scale.MinAuto = true;
            Pane.YAxis.Scale.MaxAuto = true;
            Pane.XAxis.Scale.MinAuto = true;
            Pane.XAxis.Scale.MaxAuto = true;
            Pane.X2Axis.Scale.MinAuto = true;
            Pane.X2Axis.Scale.MaxAuto = true;

            zedGraph.IsShowHScrollBar = true;
            zedGraph.IsShowVScrollBar = true;
            zedGraph.IsAutoScrollRange = true;

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
            UpdateGraph();
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
            ListTemp.Remove(ListTemp[i]);
            _digitalList.Remove(_digitalList[i]);

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void InitDrawGraph()
        {
            Pane = zedGraph.GraphPane;

            Pane.XAxis.ScaleFormatEvent += XAxis_ScaleFormatEvent;
            Pane.X2Axis.ScaleFormatEvent += XAxis_ScaleFormatEvent;

            Pane.IsFontsScaled = false;


           // Pane.PaneList.IsFontScaled = false;

            Pane.Legend.IsVisible = false;
            Pane.XAxis.Title.IsVisible = false;
            Pane.XAxis.Scale.FontSpec.Size = 11;
            Pane.YAxis.Title.IsVisible = false;
            Pane.YAxis.Scale.FontSpec.Size = 11;
            Pane.Title.IsVisible = false;
           // Pane.XAxis.Type = AxisType.Date;
           // Pane.XAxis.Scale.Format = "HH:mm:ss.fff";
  
            Pane.YAxis.MajorGrid.IsZeroLine = false;
            Pane.XAxis.MajorGrid.IsVisible = true;
            Pane.YAxis.MajorGrid.IsVisible = true;

            Pane.YAxis.Scale.Mag = 0;
            Pane.XAxis.Scale.Mag = 0;
            Pane.X2Axis.Scale.Mag = 0;

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
      //  private TextObj _stampTriggTextObj;

        public void LineStampTrigger()
        {  try
            {
                double timeStamp = MainWindow.OscilList[NumGraphPanel()].HistotyCount / MainWindow.OscilList[NumGraphPanel()].SampleRate;
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

             /*   _stampTriggTextObj = new TextObj(MainWindow.OscilList[NumGraphPanel()].StampDateTrigger + "." + MainWindow.OscilList[NumGraphPanel()].StampDateTrigger.Millisecond.ToString("000"), timeStamp, 9*Pane.YAxis.Scale.Max/10)
                {
                    FontSpec =
                    {
                        Border = {IsVisible = false}
                    },
                    Link = {Title = "StampTrigger"}
                };*/


              //  Pane.GraphObjList.Add(_stampTriggTextObj);
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

        public OscilAnalysis OscilCursor = new OscilAnalysis();
        public bool CursorsCreate;
        public int NumCursors;
        public LineObj Cursor1;
        public LineObj Cursor2;
        public LineObj CursorDig;


        private int NumGraphPanel()
        {
            //Определяем номер рабочей GraphPanel 
            int j = 0;
            for (int i = 0; i < MainWindow.GraphPanelList.Count; i++)
            {
                if (Pane == MainWindow.GraphPanelList[i].Pane)
                {
                    j = i;
                    break;
                }
            }
            return j;
        }

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

            CursorsCreate = true;

            // Обновляем график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        public void CursorAddDig()
        {
            CursorDig = new LineObj(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 4 + Pane.XAxis.Scale.Min),
                Pane.YAxis.Scale.Min, ((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 4 + Pane.XAxis.Scale.Min),
                Pane.YAxis.Scale.Max)
            {
                Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.DarkGreen,
                    Width = 2
                },
                Link = { Title = "CursorDigital" }
            };


            // Добавим линию в список отображаемых объектов
            Pane.GraphObjList.Add(CursorDig);

            CursorsCreate = true;

            // Обновляем график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void CursorClear()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "Cursor1" || Pane.GraphObjList[i].Link.Title == "Cursor2" || Pane.GraphObjList[i].Link.Title == "CursorDigital") { Pane.GraphObjList.Remove(Pane.GraphObjList[i]);}
            }
            CursorsCreate = false;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void UpdateCursor()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "CursorDigital")
                {
                    CursorDig.Location.Y1 = Pane.YAxis.Scale.Min;
                    CursorDig.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

                    if (CursorDig.Location.X <= Pane.XAxis.Scale.Min)
                        CursorDig.Location.X = Pane.XAxis.Scale.Min + (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/100;
                    if (CursorDig.Location.X >= Pane.XAxis.Scale.Max)
                        CursorDig.Location.X = Pane.XAxis.Scale.Max - (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/100;
                }
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
                if (Pane.GraphObjList[i].Link.Title == "StampTrigger")
                {
                    _stampTrigger.Location.Y1 = Pane.YAxis.Scale.Min;
                    _stampTrigger.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);
               //     _stampTriggTextObj.IsVisible = !(Pane.YAxis.Scale.Max >= _stampTriggTextObj.Location.Y1) &&
                //                                   !(Pane.YAxis.Scale.Min <= _stampTriggTextObj.Location.Y1);

                    if (_stampTrigger.Location.X <= Pane.XAxis.Scale.Min)
                    {
                        _stampTrigger.IsVisible = false;
                   //     _stampTriggTextObj.IsVisible = false;
                    }
                    if (_stampTrigger.Location.X >= Pane.XAxis.Scale.Max)
                    {
                        _stampTrigger.IsVisible = false;
                    //    _stampTriggTextObj.IsVisible = false;
                    }
                    if (_stampTrigger.Location.X >= Pane.XAxis.Scale.Min &&
                        _stampTrigger.Location.X <= Pane.XAxis.Scale.Max)
                    {
                        _stampTrigger.IsVisible = true;
                    //    _stampTriggTextObj.IsVisible = true;

                    }
                }
            }           
        }

        private void zedGraph_MouseMove(object sender, MouseEventArgs e)
        {
            double graphX, graphY;
            Pane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY);

            if (Cursor1 != null || Cursor2 != null || CursorDig != null)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (CursorDig != null && CursorDig.Line.Width == 3)
                {
                    CursorDig.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                    Cursor = Cursors.VSplit;
                }
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                // ReSharper disable once PossibleNullReferenceException
                if (Cursor1 != null && Cursor1.Line.Width == 3)
                {
                    Cursor1.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                    Cursor = Cursors.VSplit;
                }

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Cursor2 != null && Cursor2.Line.Width == 3)
                {
                    Cursor2.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                    Cursor = Cursors.VSplit;
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

                if (lineObject.Link.Title == "CursorDigital")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (CursorDig.Line.Width == 3)
                    {
                        CursorDig.Line.Width = 2;
                    }
                    else
                    {
                        CursorDig.Line.Width = 3;
                    }
                    OscilCursor.UpdateCursorDig(NumGraphPanel());
                }
                if (lineObject.Link.Title == "Cursor1")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Cursor1.Line.Width == 3)
                    {
                        Cursor1.Line.Width = 2;
                    }
                    else
                    {
                        Cursor1.Line.Width = 3;
                        Cursor2.Line.Width = 2;
                    }
                    OscilCursor.UpdateCursor(NumGraphPanel());
                }
                if (lineObject.Link.Title == "Cursor2")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Cursor2.Line.Width == 3)
                    {
                        Cursor2.Line.Width = 2;
                    }
                    else
                    {
                        Cursor1.Line.Width = 2;
                        Cursor2.Line.Width = 3;
                    }
                    OscilCursor.UpdateCursor(NumGraphPanel());
                }

                zedGraph.Invalidate();
            }
        }

        private void AddCoursor_MouseDown(object sender, MouseEventArgs e)
        {
            AddCoursorEvent();
        }

        private void AddCoursorEvent()
        {
            if (CursorsCreate == false)
            {
                CursorClear();
                CursorAdd();
                OscilCursor.AnalysisCursorAdd(NumGraphPanel());
                MainWindow.AnalysisObj.AnalysisStackPanel.Children.Add(OscilCursor.LayoutPanel[0]);
                AddCursor.Image = Properties.Resources.Line_48_1_;
            }
            else
            {
                CursorClear();
                MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(OscilCursor.LayoutPanel[0]);
                OscilCursor.AnalysisCursorClear();
                AddCursor.Image = Properties.Resources.Line_48_2_;
            }
        }

        private void AddCursorDig_button_MouseDown(object sender, MouseEventArgs e)
        {
            AddCoursorDigEvent();
        }
        private void AddCoursorDigEvent()
        {
            if (CursorsCreate == false)
            {
                CursorClear();
                CursorAddDig();
                OscilCursor.AnalysisCursorAddDig(NumGraphPanel());
                MainWindow.AnalysisObj.AnalysisStackPanel.Children.Add(OscilCursor.LayoutPanel[0]);
                AddCursorDig_button.Image = Properties.Resources.Long_Position_48;
            }
            else
            {
               CursorClear();
               MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(OscilCursor.LayoutPanel[0]);
               OscilCursor.AnalysisCursorClear();
                AddCursorDig_button.Image = Properties.Resources.Long_Position_48_;
            }
        }


        public void DelCursor()
        {
            if (CursorsCreate)
            {
                MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(OscilCursor.LayoutPanel[0]);
                OscilCursor.AnalysisCursorClear();
            }
        }

        private bool _stampTriggerCreate;

        private void StampTrigger_MouseDown(object sender, MouseEventArgs e)
        {
            StampTriggerEvent();
        }
        private void StampTriggerEvent()
        {
            if (_stampTriggerCreate == false)
            {
                StampTriggerClear();
                LineStampTrigger();
                _stampTriggerCreate = true;
                StampTrigger.Image = Properties.Resources.Horizontal_Line_48;
            }
            else
            {
                StampTriggerClear();
                _stampTriggerCreate = false;
                StampTrigger.Image = Properties.Resources.Horizontal_Line_48_2_;
            }
        }

        private bool _changeScale;

        private void ScaleButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (_changeScale == false)
            {
                ChangeScale();
                _changeScale = true;
                zedGraph.IsEnableVZoom = true;
                ScaleButton.Image = Properties.Resources.Resize_48;
            }
            else
            {
                ChangeScale();
                zedGraph.IsEnableVZoom = false;
                _changeScale = false;
                ScaleButton.Image = Properties.Resources.Width_48;
            }
        }
        private void Mask_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8))
            {
                e.Handled = true;
            }
        }

        private void MaskMin_textBox_TextChanged(object sender, EventArgs e)
        {
            if (MaskMin_textBox.Text != "")
            {
                _minYAxis = -1 * Convert.ToInt32(MaskMin_textBox.Text);

                ScrollEvent();

                zedGraph.AxisChange();
                zedGraph.Invalidate();
            }
        }

        private void MaskMax_textBox_TextChanged(object sender, EventArgs e)
        {
            if (MaskMax_textBox.Text != "")
            {
                _maxYAxis = -1 * Convert.ToInt32(MaskMax_textBox.Text);

                ScrollEvent();

                zedGraph.AxisChange();
                zedGraph.Invalidate();
            }
        }

        private void AutoRange_Button_Click(object sender, EventArgs e)
        {
            _minYAxis = _minYAxisAuto;
            _maxYAxis = _maxYAxisAuto;

            MaskMin_textBox.Text = Convert.ToInt32(-1 * _minYAxis).ToString();
            MaskMax_textBox.Text = Convert.ToInt32(-1 * _maxYAxis).ToString();

            ScrollEvent();

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }
    }
}

