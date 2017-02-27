using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZedGraph;

namespace ScopeViewer
{
    public sealed partial class GraphPanel : UserControl
    {
        MasterPane _masterPane;
        public GraphPane Pane;
        public GraphPane PaneDig;
        readonly List<LineItem> _myCurve = new List<LineItem>();

        double _maxXAxis, _minXAxis, _maxYAxis, _minYAxis;
        double _maxYAxisAuto;
        double _minYAxisAuto;
        bool _scaleY;
        bool _absOrRel;

        public GraphPanel()
        {
            ListTemp = new List<PointPairList>();
            InitializeComponent();

            DoubleBuffered = true;

            zedGraph.ZoomEvent += zedGraph_ZoomEvent;
            zedGraph.ScrollEvent += zedGraph_ScrollEvent;
            zedGraph.PointValueEvent += ZedGraph_PointValueEvent;

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
                if(PaneDig != null)
                {
                    PaneDig.XAxis.Scale.Min = _minXAxis;
                    PaneDig.X2Axis.Scale.Min = _minXAxis;
                }
            }

            if (Pane.XAxis.Scale.Max >= _maxXAxis)
            {
                Pane.XAxis.Scale.Max = _maxXAxis;
                if (PaneDig != null)
               {
                    PaneDig.XAxis.Scale.Max = _maxXAxis;
                    PaneDig.X2Axis.Scale.Max = _maxXAxis;
               }
            }

            if (Pane.YAxis.Scale.Min <= _minYAxis)
            {
                Pane.YAxis.Scale.Min = _minYAxis;
                if (PaneDig != null) { PaneDig.YAxis.Scale.Min = _minYAxisAuto; }
            }

            if (Pane.YAxis.Scale.Max >= _maxYAxis)
            {
                Pane.YAxis.Scale.Max = _maxYAxis;
               if (PaneDig != null) { PaneDig.YAxis.Scale.Max = _maxYAxisAuto; }
            }

            if (_scaleY == false)
            {
                Pane.YAxis.Scale.Max = _maxYAxis;
                Pane.YAxis.Scale.Min = _minYAxis;
                if (PaneDig != null)
                {
                    PaneDig.YAxis.Scale.Max = _maxYAxisAuto;
                    PaneDig.YAxis.Scale.Min = _minYAxisAuto;
                }
            }
            
            Pane.YAxis.Scale.Mag = 0;
            Pane.XAxis.Scale.Mag = 0;
            if (PaneDig != null)
            {
                PaneDig.XAxis.Scale.Mag = 0;
                PaneDig.X2Axis.Scale.Mag = 0;
                PaneDig.X2Axis.Scale.Max = PaneDig.XAxis.Scale.Max;
                PaneDig.X2Axis.Scale.Min = PaneDig.XAxis.Scale.Min;
            }

            UpdateCursor();
            UpdateGraph();
        }

        private void ChangeScale()
        {
            _scaleY = !_scaleY;
        }
        
        // Создадим список точек   
        public readonly List<PointPairList> ListTemp;
        private static int _pointInLine = 250;
        private readonly List<bool> _digitalList = new List<bool>();
        private static bool _showDigital;
 
        public void PointInLineChange(int point, bool showDigital)
        {
            _pointInLine = point;
            _showDigital = showDigital;
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
                    int sum = Convert.ToInt32((double)((stopIndex - startIndex) / _pointInLine));
                    if (sum == 0 || (_showDigital && _digitalList[i])) sum = 1;

                    for (int j = startIndex; j < stopIndex; j += sum)
                    {
                        Pane.CurveList[i].AddPoint(ListTemp[i][j]);
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        private string XAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            if (_absOrRel)
            {
                //Абсолютное время
                if (axis.Scale.Max - axis.Scale.Min > 5)
                {
                    return
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Second:D2}";
                }
                if (axis.Scale.Max - axis.Scale.Min > 0.005)
                {
                    return
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Second:D2}." +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Millisecond:D3}";
                }
                if (axis.Scale.Max - axis.Scale.Min > 0.000005)
                {
                    return
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Second:D2}." +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Millisecond:D3}" +
                        $"'{(int) (val * 1000000) % 1000:D3}";
                }
                else
                {
                    return
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Second:D2}." +
                        $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(val * 1000).Millisecond:D3}" +
                        $"'{(int)(val * 1000000) % 1000:D3}"+
                        $"\"{(uint)((val * 1000000000)%1000):D3}";
                }
            }
            else
            {
                //Относительное время
                if (axis.Scale.Max - axis.Scale.Min > 5)
                {
                    return $"{val} s";
                }
                if (axis.Scale.Max - axis.Scale.Min > 0.005)
                {
                    return $"{(int)(val*1000)/1000}.{(int)(val*1000)%1000:D3} ms";
                }
                if (axis.Scale.Max - axis.Scale.Min > 0.000005)
                {
                    return $"{(int)(val * 1000) / 1000}.{(int)(val * 1000) % 1000:D3}'{(int)(val * 1000000)%1000:D3} \u03BCs";
                }
                else
                {
                    return $"{(int)(val * 1000) / 1000}.{(int)(val * 1000) % 1000:D3}'{(int)(val * 1000000) % 1000:D3}\"{(uint)((val * 1000000000)%1000):D3} ns";
                }
            }
        }

        private string YAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            PaneDig.YAxis.MinorGrid.IsVisible = false;
            PaneDig.YAxis.Scale.MinorStep = 1.0;
            PaneDig.YAxis.MajorGrid.IsVisible = true;
            PaneDig.YAxis.Scale.MajorStep = 1.0;

            if (Math.Abs(val) < 1)
            {
                return "";
            }
            return val * -1 - 2 < 0 ? "main" : $"{val * -1 - 2}";
        }

        public void AddGraph(int j, Color color, bool dig)
        {
            zedGraph.IsEnableVZoom = false;

            Pane.Border.Color = Color.White;
            StampTime_label.Text = MainWindow.OscilList[NumGraphPanel()].StampDateTrigger + @"." +
                                   MainWindow.OscilList[NumGraphPanel()].StampDateTrigger.Millisecond.ToString("000");

            if (!dig) AddAnalogChannel(j, color);
        }

        private void AddAnalogChannel(int j, Color color)
        {
            //Скроем инструменты, которые позволяют пользоватся представлением дискретного канала 
            toolStripSeparator1.Visible = false;
            Mask1_label.Visible = false;
            Mask2_label.Visible = false;
            MaskMin_textBox.Visible = false;
            MaskMax_textBox.Visible = false;
            toolStripSeparator2.Visible = false;
            posTab_StripButton.Visible = false;
            delateDig_toolStripButton.Visible = false;

            PointPairList list = new PointPairList();
            ListTemp.Add(new PointPairList());
            _digitalList.Add(MainWindow.OscilList[MainWindow.OscilList.Count - 1].TypeChannel[j]);

            string nameCh = "";

            // Заполняем список точек. Приращение по оси X 
            // ReSharper disable once PossibleLossOfFraction
            int sum = Convert.ToInt32((double)(MainWindow.OscilList[MainWindow.OscilList.Count - 1].NumCount / _pointInLine));
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

        private string ZedGraph_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair point = curve[iPt];
           // Color clr = curve.Color;
            string nameChannel = curve.Label.Text;


            if(_absOrRel)
            {
                return
                    $"{nameChannel}" + "\n" +
                    $"X: {MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(point.X * 1000).Hour:D2}:" +
                    $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(point.X * 1000).Minute:D2}:" +
                    $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(point.X * 1000).Second:D2}." +
                    $"{MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(point.X * 1000).Millisecond:D3}" + 
                    "\n" + $"Y: {point.Y}";
            }
            return $"{nameChannel}" + "\n" + $"X: {point.X}" + "\n" + $"Y: {point.Y}";
        }

        public void AddDigitalChannel(int numCh, int numOsc  ,Color color)
        {
            if (PaneDig != null)         //Проверка на откыртый канал
            {
                MessageBox.Show(@"Дискретный канал уже открыт!");
                return;
            }

            if (_cursorsCreate)          //Удалим Курсоры, если оние есть
            {
                AddCoursorEvent();
            }

            if (_stampTriggerCreate)     //Удалим штамп времени, если есть
            {
                StampTriggerEvent();
            }

            if (_createCutBox)           //Закрываем меню: "Вырезать"
            {
                CutEvent();
            }               

            BinaryMask binObj = new BinaryMask();  //Вызов окна выбора маски 
            int binary = 0;

            DialogResult dlgr = binObj.ShowDialog();

            if (dlgr == DialogResult.OK)
            {
                binary = BinaryMask.BinnaryMask;
            }

            PaneDig = new GraphPane();   

            _masterPane.Add(PaneDig);
            using (Graphics g = CreateGraphics())  //Расположение графиков в столбец
            {
                _masterPane.SetLayout(g, true, new[] { 1, 1 });
            }

            zedGraph.IsShowPointValues = false;   //Отключил отображение точек 

            PointPairList list1 = new PointPairList();
            PointPairList list0 = new PointPairList();

            list1.Add(0, -0.2);
            list0.Add(0, -0.8);

            string nameCh1 = MainWindow.OscilList[numOsc].ChannelNames[numCh];
            double line1 = -0.2, line0 = -0.8;

            for (int i = 1; i < MainWindow.OscilList[numOsc].NumCount; i++)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (MainWindow.OscilList[numOsc].Data[i][numCh] !=
                    MainWindow.OscilList[numOsc].Data[i - 1][numCh])
                {
                    list1.Add((i - 1) / MainWindow.OscilList[numOsc].SampleRate, line1);
                    list0.Add((i - 1 )/ MainWindow.OscilList[numOsc].SampleRate, line0);

                    double temp0 = line0;
                    double temp1 = line1;
                    line1 = temp0;
                    line0 = temp1;

                    list1.Add(i / MainWindow.OscilList[numOsc].SampleRate, line1);
                    list0.Add(i / MainWindow.OscilList[numOsc].SampleRate, line0);
                }
                if (i == MainWindow.OscilList[numOsc].NumCount - 1)
                {
                    list1.Add((i - 1) / MainWindow.OscilList[numOsc].SampleRate, line1);
                    list0.Add((i - 1) / MainWindow.OscilList[numOsc].SampleRate, line0);
                }
            }

            LineItem newCurve1 = PaneDig.AddCurve(nameCh1, list1, Color.DarkBlue, SymbolType.None);
            LineItem newCurve0 = PaneDig.AddCurve(nameCh1, list0, Color.DarkBlue, SymbolType.None);
            newCurve0.Line.IsSmooth = false;
            newCurve1.Line.IsSmooth = false;
            _myCurve.Add(newCurve1);
            _myCurve[_myCurve.Count - 1].Line.Width = 3;
            _myCurve.Add(newCurve0);
            _myCurve[_myCurve.Count - 1].Line.Width = 3;

            var list = new PointPairList();

            for (int i = 0; i < MainWindow.OscilList[numOsc].NumCount; i++)
            {
                // добавим в список точку
                list.Add(i / MainWindow.OscilList[numOsc].SampleRate, MainWindow.OscilList[numOsc].Data[i][numCh]);
            }

            LineItem newCurveBase = PaneDig.AddCurve(nameCh1, list, color, SymbolType.None);
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

                if ((Convert.ToInt32(MainWindow.OscilList[numOsc].Data[0][numCh]) & 1 << l) == 1 << l)
                {
                    line = -0.2 - 1 - l;
                }
                else
                {
                    line = -0.8 - 1 - l;
                }

                list.Add(0 / MainWindow.OscilList[numOsc].SampleRate, line);

                for (int i = 1; i < MainWindow.OscilList[numOsc].NumCount; i++)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if(MainWindow.OscilList[numOsc].Data[i][numCh] !=
                       MainWindow.OscilList[numOsc].Data[i - 1][numCh])
                    {
                        list.Add((i - 1)/ MainWindow.OscilList[numOsc].SampleRate, line);
                        if ((Convert.ToInt32(MainWindow.OscilList[numOsc].Data[i][numCh]) & 1 << l) == 1 << l)
                        {
                            line = -0.2 - 1 - l;
                        }
                        else
                        {
                            line = -0.8 - 1 - l;
                        }
                        list.Add(i / MainWindow.OscilList[numOsc].SampleRate, line);
                    }
                    if (i == MainWindow.OscilList[numOsc].NumCount - 1)
                    {
                        list.Add((i - 1) / MainWindow.OscilList[numOsc].SampleRate, line);
                    }
                }

                LineItem newCurve = PaneDig.AddCurve(nameCh1, list, color, SymbolType.None);
                newCurve.Line.IsSmooth = false;
                _myCurve.Add(newCurve);
                _myCurve[_myCurve.Count - 1].Line.Width = 2;
            }

            PaneDig.Border.Color = Color.White;

            PaneDig.YAxis.IsVisible = true;

            PaneDig.X2Axis.IsVisible = true;
            PaneDig.X2Axis.Scale.FontSpec.Size = 11;
            PaneDig.YAxis.ScaleFormatEvent += YAxis_ScaleFormatEvent;

            PaneDig.YAxis.MinorGrid.IsVisible = false;
            PaneDig.YAxis.Scale.MinorStep = 1.0;
            PaneDig.YAxis.MajorGrid.IsVisible = true;
            PaneDig.YAxis.Scale.MajorStep = 1.0;
            PaneDig.YAxis.MajorGrid.DashOn = 1;
            PaneDig.YAxis.MajorGrid.DashOff = 0;

            PaneDig.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Center;
            PaneDig.YAxis.Scale.FontSpec.Angle = 80;

            PaneDig.XAxis.ScaleFormatEvent += XAxis_ScaleFormatEvent;
            PaneDig.X2Axis.ScaleFormatEvent += XAxis_ScaleFormatEvent;

            PaneDig.IsFontsScaled = false;

            PaneDig.Legend.IsVisible = false;
            PaneDig.XAxis.Title.IsVisible = false;
            PaneDig.XAxis.Scale.FontSpec.Size = 11;
            PaneDig.YAxis.Title.IsVisible = false;
            PaneDig.YAxis.Scale.FontSpec.Size = 11;
            PaneDig.Title.IsVisible = false;

            PaneDig.YAxis.MajorGrid.IsZeroLine = false;
            PaneDig.XAxis.MajorGrid.IsVisible = true;
            PaneDig.YAxis.MajorGrid.IsVisible = true;

            PaneDig.YAxis.Scale.Mag = 0;
            PaneDig.XAxis.Scale.Mag = 0;
            PaneDig.X2Axis.Scale.Mag = 0;

            PaneDig.YAxis.Scale.MinAuto = true;
            PaneDig.YAxis.Scale.MaxAuto = true;

            PaneDig.IsBoundedRanges = true;
            
            _minYAxisAuto = -maxMaskCount - 1;
            _maxYAxisAuto = 0;

            PaneDig.XAxis.Scale.Max = Pane.XAxis.Scale.Max;
            PaneDig.XAxis.Scale.Min = Pane.XAxis.Scale.Min;

            PaneDig.X2Axis.Scale.Max = PaneDig.XAxis.Scale.Max;
            PaneDig.X2Axis.Scale.Min = PaneDig.XAxis.Scale.Min;

            MaskMin_textBox.Text = Convert.ToInt32(-2 + -1 * _minYAxisAuto).ToString();
            MaskMax_textBox.Text = Convert.ToInt32(-1 * _maxYAxisAuto).ToString();

            PaneDig.Chart.Rect = new RectangleF(
                Pane.Chart.Rect.X,
                Pane.Chart.Rect.Y + Pane.Chart.Rect.Height + 75,
                Pane.Chart.Rect.Width, 
                Pane.Chart.Rect.Height - 15);

            zedGraph.Resize += ZedGraph_Resize;
            zedGraph.SizeChanged += ZedGraphOnSizeChanged;

            _posTabHoriz = true;
            posTab_StripButton.Visible = true;
            delateDig_toolStripButton.Visible = true;
            toolStripSeparator2.Visible = false;
            Mask1_label.Visible = true;
            Mask2_label.Visible = true;
            MaskMin_textBox.Visible = true;
            MaskMax_textBox.Visible = true;
            toolStripSeparator2.Visible = false;

            zedGraph.AxisChange();
            zedGraph.Invalidate();

            ChangedPos();
        }

        private void ZedGraphOnSizeChanged(object sender, EventArgs eventArgs)
        {
            ChangedPos();
        }

        private void ZedGraph_Resize(object sender, EventArgs e)
        {
            ChangedPos();
        }

        private void ChangedPos()
        {
            if(PaneDig != null)
            {
                if (_posTabHoriz)
                {
                    PaneDig.Chart.Rect = new RectangleF(
                        Pane.Chart.Rect.X,
                        Pane.Chart.Rect.Y + Pane.Chart.Rect.Height + 75,
                        Pane.Chart.Rect.Width,
                        Pane.Chart.Rect.Height - 15);
                }
                else
                {
                    PaneDig.Chart.Rect = new RectangleF(
                        Pane.Chart.Rect.X + Pane.Chart.Rect.Width + 75, 
                        Pane.Chart.Rect.Y,
                        Pane.Chart.Rect.Width, 
                        Pane.Chart.Rect.Height);
                }
            }
            ScrollEvent();
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void ResizeAxis()
        {
            Pane.YAxis.Scale.MinAuto = true;
            Pane.YAxis.Scale.MaxAuto = true;
            Pane.XAxis.Scale.MinAuto = true;
            Pane.XAxis.Scale.MaxAuto = true;

            zedGraph.IsShowHScrollBar = true;
            zedGraph.IsShowVScrollBar = true;
            zedGraph.IsAutoScrollRange = true;

            _maxXAxis = zedGraph.GraphPane.XAxis.Scale.Max;
            _minXAxis = zedGraph.GraphPane.XAxis.Scale.Min;

            if (_maxYAxis < zedGraph.GraphPane.YAxis.Scale.Max)
            {
                _maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max;
            }

            if (_minYAxis > zedGraph.GraphPane.YAxis.Scale.Min)
            {
                _minYAxis = zedGraph.GraphPane.YAxis.Scale.Min;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (zedGraph.GraphPane.YAxis.Scale.Max == 0)
            {
                _maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max +
                            (zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 10;
            }

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (zedGraph.GraphPane.YAxis.Scale.Min == 0)
            {
                _minYAxis = zedGraph.GraphPane.YAxis.Scale.Min -
                            (zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 10;
            }

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
            UpdateGraph();
            zedGraph.AxisChange();
        }

        private void InitDrawGraph()
        {
            Pane = zedGraph.GraphPane;
            
            Pane.XAxis.ScaleFormatEvent += XAxis_ScaleFormatEvent;

            Pane.IsFontsScaled = false;

            Pane.Legend.IsVisible = false;
            Pane.XAxis.Title.IsVisible = false;
            Pane.XAxis.Scale.FontSpec.Size = 11;
            Pane.YAxis.Title.IsVisible = false;
            Pane.YAxis.Scale.FontSpec.Size = 11;
            Pane.Title.IsVisible = false;
  
            Pane.YAxis.MajorGrid.IsZeroLine = false;
            Pane.XAxis.MajorGrid.IsVisible = true;
            Pane.YAxis.MajorGrid.IsVisible = true;

            Pane.YAxis.Scale.Mag = 0;
            Pane.XAxis.Scale.Mag = 0;

            Pane.YAxis.Scale.MinAuto = true;
            Pane.YAxis.Scale.MaxAuto = true;

            Pane.IsBoundedRanges = true;

            _masterPane = zedGraph.MasterPane;
            _masterPane.PaneList.Clear();
            _masterPane.Add(Pane);
            // Будем размещать добавленные графики в MasterPane

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

            _myCurve[numChannel].Line.Width = width ? 3 : 1;

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
            if (fontSize < 5 || fontSize > 12)
            {
                fontSize = 9;
            }

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

            Pane.Legend.FontSpec.Size = fontSize;
            Pane.Legend.IsVisible = show;

            // Вызываем метод AxisChange (), чтобы обновить данные об осях.
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private LineObj _stampTrigger;
        private LineObj _stampTriggerDig;

        private void LineStampTrigger()
        {
            try
            {
                double timeStamp = MainWindow.OscilList[NumGraphPanel()].HistotyCount / MainWindow.OscilList[NumGraphPanel()].SampleRate;
                _stampTrigger = new LineObj(timeStamp, Pane.YAxis.Scale.Min, timeStamp, Pane.YAxis.Scale.Max)
                {
                    Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.DashDot,
                    Color = Color.DarkGreen,
                    Width = 2
                },
                    Link = { Title = "StampTrigger" }
                };

                Pane.GraphObjList.Add(_stampTrigger);

                if (PaneDig != null)
                {
                    _stampTriggerDig = new LineObj(timeStamp, PaneDig.YAxis.Scale.Min, timeStamp, PaneDig.YAxis.Scale.Max)
                    {
                        Line =
                    {
                        Style = System.Drawing.Drawing2D.DashStyle.DashDot,
                        Color = Color.DarkGreen,
                        Width = 2
                    },
                        Link = { Title = "StampTriggerDig" }
                    };

                    PaneDig.GraphObjList.Add(_stampTriggerDig);
                }

            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            { }
        }

        private void StampTriggerClear()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "StampTrigger")
                {
                    Pane.GraphObjList.Remove(Pane.GraphObjList[i]);
                }
            }

            if (PaneDig != null)
            {
                for (int i = PaneDig.GraphObjList.Count - 1; i >= 0; i--)
                {
                    if (PaneDig.GraphObjList[i].Link.Title == "StampTriggerDig")
                    {
                        PaneDig.GraphObjList.Remove(PaneDig.GraphObjList[i]);
                    }
                }
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private readonly OscilAnalysis _oscilCursor = new OscilAnalysis();
        private bool _cursorsCreate;
        public LineObj Cursor1;
        public LineObj Cursor2;
        public LineObj CursorDig1;
        public LineObj CursorDig2;
        
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

        private void CursorAdd()
        {
            Cursor1 = new LineObj(
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/4 + Pane.XAxis.Scale.Min,
                Pane.YAxis.Scale.Min,
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)/4 + Pane.XAxis.Scale.Min,
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

            Cursor2 = new LineObj(
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)*3/4 + Pane.XAxis.Scale.Min,
                Pane.YAxis.Scale.Min,
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min)*3/4 + Pane.XAxis.Scale.Min,
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

            // Добавим линию в список отображаемых объектов
            Pane.GraphObjList.Add(Cursor1);
            Pane.GraphObjList.Add(Cursor2);

            if (PaneDig != null)
            {
                CursorDig1 = new LineObj(
                    (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) / 4 + PaneDig.XAxis.Scale.Min,
                    PaneDig.YAxis.Scale.Min, 
                    (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) / 4 + PaneDig.XAxis.Scale.Min,
                    PaneDig.YAxis.Scale.Max)
                {
                    Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.Red,
                    Width = 2
                },
                    Link = { Title = "CursorDig1" }
                };

                CursorDig2 = new LineObj(
                    (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) * 3 / 4 + PaneDig.XAxis.Scale.Min,
                    PaneDig.YAxis.Scale.Min, 
                    (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) * 3 / 4 + PaneDig.XAxis.Scale.Min,
                    PaneDig.YAxis.Scale.Max)
                {
                    Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.Blue,
                    Width = 2
                },
                    Link = { Title = "CursorDig2" }
                };

                // Добавим линию в список отображаемых объектов
                PaneDig.GraphObjList.Add(CursorDig1);
                PaneDig.GraphObjList.Add(CursorDig2);
            }

            _cursorsCreate = true;

            // Обновляем график
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void CursorClear()
        {
            for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
            {
                if (Pane.GraphObjList[i].Link.Title == "Cursor1" || Pane.GraphObjList[i].Link.Title == "Cursor2") { Pane.GraphObjList.Remove(Pane.GraphObjList[i]);}
            }
            if (PaneDig != null)
            {
                for (int i = PaneDig.GraphObjList.Count - 1; i >= 0; i--)
                {
                    if (PaneDig.GraphObjList[i].Link.Title == "CursorDig1" || PaneDig.GraphObjList[i].Link.Title == "CursorDig2") { PaneDig.GraphObjList.Remove(PaneDig.GraphObjList[i]); }
                }
            }
            _cursorsCreate = false;

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

                    if (PaneDig != null)
                    {
                        CursorDig1.Location.Y1 = PaneDig.YAxis.Scale.Min;
                        CursorDig1.Location.Height = PaneDig.YAxis.Scale.Max - PaneDig.YAxis.Scale.Min;

                        if (CursorDig1.Location.X <= PaneDig.XAxis.Scale.Min) CursorDig1.Location.X = PaneDig.XAxis.Scale.Min + (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) / 100;
                        if (CursorDig1.Location.X >= PaneDig.XAxis.Scale.Max) CursorDig1.Location.X = PaneDig.XAxis.Scale.Max - (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) / 100;
                    }
                    continue;
                }
                if (Pane.GraphObjList[i].Link.Title == "Cursor2")
                {
                    Cursor2.Location.Y1 = Pane.YAxis.Scale.Min;
                    Cursor2.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

                    if (Cursor2.Location.X <= Pane.XAxis.Scale.Min) Cursor2.Location.X = Pane.XAxis.Scale.Min + (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;
                    if (Cursor2.Location.X >= Pane.XAxis.Scale.Max) Cursor2.Location.X = Pane.XAxis.Scale.Max - (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 100;

                    if (PaneDig != null)
                    {
                        CursorDig2.Location.Y1 = PaneDig.YAxis.Scale.Min;
                        CursorDig2.Location.Height = PaneDig.YAxis.Scale.Max - PaneDig.YAxis.Scale.Min;

                        if (CursorDig2.Location.X <= PaneDig.XAxis.Scale.Min) CursorDig2.Location.X = PaneDig.XAxis.Scale.Min + (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) / 100;
                        if (CursorDig2.Location.X >= PaneDig.XAxis.Scale.Max) CursorDig2.Location.X = PaneDig.XAxis.Scale.Max - (PaneDig.XAxis.Scale.Max - PaneDig.XAxis.Scale.Min) / 100;
                    }
                    continue;
                }
                if (Pane.GraphObjList[i].Link.Title == "StampTrigger")
                {
                    _stampTrigger.Location.Y1 = Pane.YAxis.Scale.Min;
                    _stampTrigger.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

                    if (_stampTrigger.Location.X <= Pane.XAxis.Scale.Min)
                    {
                        _stampTrigger.IsVisible = false;
                    }
                    if (_stampTrigger.Location.X >= Pane.XAxis.Scale.Max)
                    {
                        _stampTrigger.IsVisible = false;
                    }
                    if (_stampTrigger.Location.X >= Pane.XAxis.Scale.Min &&
                        _stampTrigger.Location.X <= Pane.XAxis.Scale.Max)
                    {
                        _stampTrigger.IsVisible = true;
                    }
                    if (PaneDig != null)
                    {
                        _stampTriggerDig.Location.Y1 = PaneDig.YAxis.Scale.Min;
                        _stampTriggerDig.Location.Height = PaneDig.YAxis.Scale.Max - PaneDig.YAxis.Scale.Min;

                        if (_stampTriggerDig.Location.X <= PaneDig.XAxis.Scale.Min)
                        {
                            _stampTriggerDig.IsVisible = false;
                        }
                        if (_stampTriggerDig.Location.X >= PaneDig.XAxis.Scale.Max)
                        {
                            _stampTriggerDig.IsVisible = false;
                        }
                        if (_stampTriggerDig.Location.X >= PaneDig.XAxis.Scale.Min &&
                            _stampTriggerDig.Location.X <= PaneDig.XAxis.Scale.Max)
                        {
                            _stampTriggerDig.IsVisible = true;
                        }
                    }
                    continue;
                }

                if (Pane.GraphObjList[i].Link.Title == "LeftLine" || Pane.GraphObjList[i].Link.Title == "RightLine")
                {
                    _leftLineCut.Location.Y1 = Pane.YAxis.Scale.Min;
                    _leftLineCut.Location.Height = Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min;
                    _rightLineCut.Location.Y1 = Pane.YAxis.Scale.Min;
                    _rightLineCut.Location.Height = Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min;

                    _boxCut.Location.Y1 = Pane.YAxis.Scale.Max;
                    _boxCut.Location.Height = Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min;
                    _boxCut.Location.X = _leftLineCut.Location.X;
                    _boxCut.Location.Width = _rightLineCut.Location.X - _leftLineCut.Location.X;

                    if ( _leftLineCut.Location.X > _rightLineCut.Location.X )
                    {
                        _leftLineCut.Location.X = _rightLineCut.Location.X;
                    }

                    if (_leftLineCut.Location.X > Pane.XAxis.Scale.Min &&
                        _rightLineCut.Location.X < Pane.XAxis.Scale.Max)
                    { 
                        _boxCut.IsVisible = true;
                        _rightLineCut.IsVisible = true;
                        _leftLineCut.IsVisible = true;
                    }

                    if (_leftLineCut.Location.X <= Pane.XAxis.Scale.Min &&
                        _rightLineCut.Location.X < Pane.XAxis.Scale.Max)
                    {
                        _boxCut.IsVisible = true;
                        _rightLineCut.IsVisible = true;
                        _leftLineCut.IsVisible = false;
                        _boxCut.Location.X = Pane.XAxis.Scale.Min;
                        _boxCut.Location.Width = _rightLineCut.Location.X - Pane.XAxis.Scale.Min;
                    }

                    if (_leftLineCut.Location.X > Pane.XAxis.Scale.Min &&
                        _rightLineCut.Location.X >= Pane.XAxis.Scale.Max)
                    {
                        _boxCut.IsVisible = true;
                        _rightLineCut.IsVisible = false;
                        _leftLineCut.IsVisible = true;
                        _boxCut.Location.X = _leftLineCut.Location.X;
                        _boxCut.Location.Width = Pane.XAxis.Scale.Max - _leftLineCut.Location.X;
                    }

                    if (_leftLineCut.Location.X <= Pane.XAxis.Scale.Min && 
                        _rightLineCut.Location.X >= Pane.XAxis.Scale.Max)
                    {
                        _boxCut.IsVisible = true;
                        _rightLineCut.IsVisible = false;
                        _leftLineCut.IsVisible = false;
                        _boxCut.Location.X = Pane.XAxis.Scale.Min;
                        _boxCut.Location.Width = Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min;
                    }

                    if (_leftLineCut.Location.X <= Pane.XAxis.Scale.Min &&
                        _rightLineCut.Location.X <= Pane.XAxis.Scale.Min 
                        ||
                        _leftLineCut.Location.X >= Pane.XAxis.Scale.Max &&
                        _rightLineCut.Location.X >= Pane.XAxis.Scale.Max)
                    {
                        _boxCut.IsVisible = false;
                        _rightLineCut.IsVisible = false;
                        _leftLineCut.IsVisible = false;
                    }
                }
            }           
        }

        private void zedGraph_MouseMove(object sender, MouseEventArgs e)
        {
            double graphX, graphY;
            Pane.ReverseTransform(new PointF(e.X, e.Y), out graphX, out graphY);

            if (Cursor1 != null || Cursor2 != null || _leftLineCut != null || _rightLineCut != null)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                // ReSharper disable once PossibleNullReferenceException
                if (Cursor1 != null && Cursor1.Line.Width == 3)
                {
                    Cursor1.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                    Cursor = Cursors.VSplit;
                    if (PaneDig != null)
                    {
                        CursorDig1.Location.X1 = graphX;
                        UpdateCursor();
                        zedGraph.Invalidate();
                    }
                }

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Cursor2 != null && Cursor2.Line.Width == 3)
                {
                    Cursor2.Location.X1 = graphX;
                    UpdateCursor();
                    zedGraph.Invalidate();
                    Cursor = Cursors.VSplit;
                    if (PaneDig != null)
                    {
                        CursorDig2.Location.X1 = graphX;
                        UpdateCursor();
                        zedGraph.Invalidate();
                    }
                }

                if (_leftLineCut != null || _rightLineCut != null)
                {

                    if (_rightLineCut != null && Math.Abs(_rightLineCut.Line.Width - 3) < 1)
                    {
                        if (_leftLineCut != null && _leftLineCut.Location.X1 > graphX)
                        {
                            _rightLineCut.Location.X1 = _leftLineCut.Location.X1;
                        }
                        else
                        {
                            _rightLineCut.Location.X1 = graphX;
                        }
                        UpdateCursor();
                        zedGraph.Invalidate();
                        Cursor = Cursors.VSplit;
                    }

                    if (_leftLineCut != null && Math.Abs(_leftLineCut.Line.Width - 3) < 1)
                    {
                        _leftLineCut.Location.X1 = graphX;
                        UpdateCursor();
                        zedGraph.Invalidate();
                        Cursor = Cursors.VSplit;
                    }
                }
            }

            

        }

        private void zedGraph_MouseClick(object sender, MouseEventArgs e)
        {
            object nearestObject;
            int index;
            Pane.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out nearestObject, out index);

            if (nearestObject == null)
            {
                if (PaneDig != null)
                {
                    PaneDig.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out nearestObject, out index);
                }
            }

            if (nearestObject != null && nearestObject.GetType() == typeof(LineObj))
            {
                LineObj lineObject = (LineObj)nearestObject;

                if (lineObject.Link.Title == "Cursor1" || lineObject.Link.Title == "CursorDig1")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Cursor1.Line.Width == 3)
                    {
                        Cursor1.Line.Width = 2;
                        if (PaneDig != null)
                        {
                            CursorDig1.Line.Width = 2;
                        }
                    }
                    else
                    {
                        Cursor1.Line.Width = 3;
                        Cursor2.Line.Width = 2;
                        if (PaneDig != null)
                        {
                            CursorDig1.Line.Width = 3;
                            CursorDig2.Line.Width = 2;
                        }
                    }
                    _oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel);
                    if (PaneDig != null)
                    {
                        _oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel);
                    }
                }

                if (lineObject.Link.Title == "Cursor2" || lineObject.Link.Title == "CursorDig2")
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (Cursor2.Line.Width == 3)
                    {
                        Cursor2.Line.Width = 2;
                        if (PaneDig != null)
                        {
                            CursorDig2.Line.Width = 2;
                        }
                    }
                    else
                    {
                        Cursor1.Line.Width = 2;
                        Cursor2.Line.Width = 3;
                        if (PaneDig != null)
                        {
                            CursorDig1.Line.Width = 2;
                            CursorDig2.Line.Width = 3;
                        }
                    }
                    _oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel);
                    if (PaneDig != null)
                    {
                        _oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel);
                    }
                }

                if (lineObject.Link.Title == "LeftLine")
                {
                    if (Math.Abs(_leftLineCut.Line.Width - 2) > 0)
                    {
                        _leftLineCut.Line.Width = 2;
                    }
                    else
                    {
                        _rightLineCut.Line.Width = 2;
                        _leftLineCut.Line.Width = 3;
                    }
                }

                if (lineObject.Link.Title == "RightLine")
                {
                    if (Math.Abs(_rightLineCut.Line.Width - 2) > 0)
                    {
                        _rightLineCut.Line.Width = 2;
                    }
                    else
                    {
                        _leftLineCut.Line.Width = 2;
                        _rightLineCut.Line.Width = 3;
                    }
                }
            }

            zedGraph.Invalidate();
        }

        private void AddCoursor_MouseDown(object sender, MouseEventArgs e)
        {
            AddCoursorEvent();
        }

        private void AddCoursorEvent()
        {
            if (_cursorsCreate == false)
            {
                CursorClear();
                CursorAdd();
                _oscilCursor.AnalysisCursorAdd(NumGraphPanel());
                MainWindow.AnalysisObj.AnalysisStackPanel.Children.Add(_oscilCursor.LayoutPanel[0]);
                if (PaneDig != null)
                {
                    _oscilCursor.AnalysisCursorAddDig(NumGraphPanel());
                    MainWindow.AnalysisObj.AnalysisStackPanel.Children.Add(_oscilCursor.LayoutPanel[1]);
                }
                AddCursor.Image = Properties.Resources.Stocks_Rem;
            }
            else
            {
                CursorClear();
                if (PaneDig != null)
                {
                    MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(_oscilCursor.LayoutPanel[1]);
                    MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(_oscilCursor.LayoutPanel[0]);
                    _oscilCursor.AnalysisCursorClearDig();
                    _oscilCursor.AnalysisCursorClear();
                }
                else
                {
                    MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(_oscilCursor.LayoutPanel[0]);
                    _oscilCursor.AnalysisCursorClear();
                }
                AddCursor.Image = Properties.Resources.Stocks_Add;
            }
        }

        public void DelCursor()
        {
            if (_cursorsCreate)
            {
                MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(_oscilCursor.LayoutPanel[0]);
                _oscilCursor.AnalysisCursorClear();
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
                StampTrigger.Image = Properties.Resources.Watch_Rem;
            }
            else
            {
                StampTriggerClear();
                _stampTriggerCreate = false;
                StampTrigger.Image = Properties.Resources.Watch_Add;
            }
        }

        private int _changeScale = 1;

        private void ScaleButton_MouseDown(object sender, MouseEventArgs e)
        {
            if (_changeScale == 0)
            {
                ChangeScale();
                _changeScale = 1;
                zedGraph.IsEnableVZoom = false;
                zedGraph.IsEnableHZoom = true;
                ScaleButton.Image = Properties.Resources.Width_48;
                return;
            }
            if (_changeScale == 1)
            {
                ChangeScale();
                _changeScale = 2;
                zedGraph.IsEnableVZoom = true;
                zedGraph.IsEnableHZoom = true;
                ScaleButton.Image = Properties.Resources.Resize_48;
                return;
            }
            if (_changeScale == 2)
            {
                _changeScale = 0;
                zedGraph.IsEnableHZoom = false;
                zedGraph.IsEnableVZoom = true;
                ScaleButton.Image = Properties.Resources.Height_48;
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
                if (Convert.ToInt32(MaskMin_textBox.Text) == 0)
                {
                    _minYAxisAuto = -1 * Convert.ToInt32(MaskMin_textBox.Text);
                }
                else
                {
                    _minYAxisAuto = -2 + -1 * Convert.ToInt32(MaskMin_textBox.Text);
                }

                ScrollEvent();

                zedGraph.AxisChange();
                zedGraph.Invalidate();
            }
        }

        private void MaskMax_textBox_TextChanged(object sender, EventArgs e)
        {
            if (MaskMax_textBox.Text != "")
            {
                if (Convert.ToInt32(MaskMax_textBox.Text) == 0)
                {
                    _maxYAxisAuto = -1 * Convert.ToInt32(MaskMax_textBox.Text);
                }
                else
                {
                    _maxYAxisAuto = -1 + -1 * Convert.ToInt32(MaskMax_textBox.Text);
                }

                ScrollEvent();

                zedGraph.AxisChange();
                zedGraph.Invalidate();
            }
        }

        bool _posTabHoriz = true; 

        private void posTab_StripButton_MouseDown(object sender, MouseEventArgs e)
        {
            _posTabHoriz = !_posTabHoriz;
            posTab_StripButton.Image = _posTabHoriz ? Properties.Resources.Flip_Vertical_48 : Properties.Resources.Flip_Horizontal_48;

            using (Graphics g = CreateGraphics())
            {
                _masterPane.SetLayout(g, _posTabHoriz, new[] { 1, 1 });
            }

            // Обновим график
            zedGraph.AxisChange();
            zedGraph.Invalidate();

            ChangedPos();
        }

        private void delateDig_toolStripButton_MouseDown(object sender, MouseEventArgs e)
        {
            CloseDigChannel();
        }

        private void CloseDigChannel()
        {
            if (_cursorsCreate)          //Удалим Курсоры если оние есть
            {
                AddCoursorEvent();
            }
            if (_stampTriggerCreate)
            {
                StampTriggerEvent();
            }

            _masterPane.PaneList.Remove(PaneDig);
            using (Graphics g = CreateGraphics())
            {
                _masterPane.SetLayout(g, true, new[] { 1 });
            }
            PaneDig = null;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
            
            delateDig_toolStripButton.Visible = false;
            posTab_StripButton.Visible = false;
            toolStripSeparator1.Visible = false;
            Mask1_label.Visible = false;
            Mask2_label.Visible = false;
            MaskMin_textBox.Visible = false;
            MaskMax_textBox.Visible = false;
            toolStripSeparator2.Visible = false;
        }

        private void absOrRelTime_toolStripButton_MouseDown(object sender, MouseEventArgs e)
        {
            _absOrRel = !_absOrRel;
            absOrRelTime_toolStripButton.Image = _absOrRel ? Properties.Resources.Time_abs : Properties.Resources.Time_rel;
            absOrRelTime_toolStripButton.ToolTipText = _absOrRel ? "Абсолютное время" : "Относительное время" ;

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        BoxObj _boxCut;
        LineObj _leftLineCut;
        LineObj _rightLineCut;
        bool _createCutBox;

        private void Cut_StripButton_MouseDown(object sender, MouseEventArgs e)
        {
            CutEvent();
        }

        private void CutEvent()
        {
            if (!_createCutBox)
            {
                AddCutBox();
            }
            else
            {

                DelCutBox();
            }

            ApplyCut_StripButton.Visible = !_createCutBox;
            Cut_StripButton.Image = !_createCutBox ? Properties.Resources.Cutting_Remove : Properties.Resources.Cutting_Add;
            Cut_StripButton.ToolTipText = !_createCutBox ? "Закрыть область" : "Добавить область";

            _createCutBox = !_createCutBox;
        }

        private void AddCutBox()
        {
            if (PaneDig != null)
            {
                CloseDigChannel();
            }

            _leftLineCut = new LineObj(
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 4 + Pane.XAxis.Scale.Min,
                Pane.YAxis.Scale.Min,
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 4 + Pane.XAxis.Scale.Min,
                Pane.YAxis.Scale.Max)
            {
                Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.DarkSlateBlue,
                    Width = 2
                },
                Link = { Title = "LeftLine" }
            };

            _rightLineCut = new LineObj(
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) * 3 / 4 + Pane.XAxis.Scale.Min,
                Pane.YAxis.Scale.Min,
                (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) * 3 / 4 + Pane.XAxis.Scale.Min,
                Pane.YAxis.Scale.Max)
            {
                Line =
                {
                    Style = System.Drawing.Drawing2D.DashStyle.Solid,
                    Color = Color.DarkSlateBlue,
                    Width = 2
                },
                Link = { Title = "RightLine" }
            };

            // Добавим линию в список отображаемых объектов
            Pane.GraphObjList.Add(_leftLineCut);
            Pane.GraphObjList.Add(_rightLineCut);


            _boxCut = new BoxObj(
                _leftLineCut.Location.X,
                Pane.YAxis.Scale.Max,
                _rightLineCut.Location.X - _leftLineCut.Location.X,
                Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min,
                Color.Empty,
                Color.AliceBlue)
            {
                Location =
                {
                    CoordinateFrame = CoordType.AxisXYScale,
                    AlignH = AlignH.Left,
                    AlignV = AlignV.Top,
                },
                
                ZOrder = ZOrder.F_BehindGrid  //В каком слое расположена область 
            };

            // place the _boxCut behind the axis items, so the grid is drawn on top of it
            Pane.GraphObjList.Add(_boxCut);

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void DelCutBox()
        {
            Pane.GraphObjList.Remove(_leftLineCut);
            Pane.GraphObjList.Remove(_rightLineCut);
            Pane.GraphObjList.Remove(_boxCut);

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void ApplyCut_StripButton_MouseDown(object sender, MouseEventArgs e)
        {
            //Формируем новую осциллограмму 
            int numLeft = 0;
            int numRight = ListTemp[0].Count;
            double hisCount = MainWindow.OscilList[NumGraphPanel()].HistotyCount;

            for (int j = ListTemp[0].Count - 1; j >= 0; j--)
            {
                if (ListTemp[0][j].X > _rightLineCut.Location.X)   //Правая сторона
                {
                    MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data.RemoveAt(j);
                }
                else if (ListTemp[0][j].X < _leftLineCut.Location.X)   //Левая сторона
                {
                    MainWindow.OscilList[MainWindow.OscilList.Count - 1].Data.RemoveAt(j);
                }
            }

            foreach (PointPairList listTemp in ListTemp)
            {
                for (int j = listTemp.Count - 1; j >= 0; j--)
                {
                    if (listTemp[j].X >= _rightLineCut.Location.X)
                    {
                        numRight = j;
                    }
                    if (listTemp[j].X >= _leftLineCut.Location.X)
                    {
                        numLeft = j;
                    }
                    if (listTemp[j].X > _rightLineCut.Location.X)   //Правая сторона
                    {
                        listTemp.RemoveAt(j);
                    }  
                    else if (listTemp[j].X < _leftLineCut.Location.X)   //Левая сторона
                    {
                        listTemp.RemoveAt(j);
                    }
                }
            }

            if (hisCount > numRight)
            {
                hisCount = 0;
            }
            else
            {
                hisCount = hisCount - numLeft;
            }

            foreach (PointPairList listTemp in ListTemp)
            {
                for (int j =  0; j < listTemp.Count; j++)
                {
                    listTemp[j].X = j / MainWindow.OscilList[NumGraphPanel()].SampleRate;
                }
            }

            MainWindow.OscilList[NumGraphPanel()].NumCount = Convert.ToUInt32(ListTemp[0].Count);
            MainWindow.OscilList[NumGraphPanel()].HistotyCount = hisCount > 0 ? hisCount : 0;
            MainWindow.OscilList[NumGraphPanel()].StampDateTrigger =
                MainWindow.OscilList[NumGraphPanel()].StampDateStart.AddMilliseconds(1000 * (numLeft + MainWindow.OscilList[NumGraphPanel()].HistotyCount) / MainWindow.OscilList[NumGraphPanel()].SampleRate);
            MainWindow.OscilList[NumGraphPanel()].StampDateStart =
                MainWindow.OscilList[NumGraphPanel()].StampDateTrigger.AddMilliseconds(-(1000 * MainWindow.OscilList[NumGraphPanel()].HistotyCount / MainWindow.OscilList[NumGraphPanel()].SampleRate));
            MainWindow.OscilList[NumGraphPanel()].StampDateEnd =
                MainWindow.OscilList[NumGraphPanel()].StampDateTrigger.AddMilliseconds(1000 * (MainWindow.OscilList[NumGraphPanel()].NumCount - MainWindow.OscilList[NumGraphPanel()].HistotyCount) / MainWindow.OscilList[NumGraphPanel()].SampleRate);

            StampTime_label.Text = MainWindow.OscilList[NumGraphPanel()].StampDateTrigger + @"." +
                       MainWindow.OscilList[NumGraphPanel()].StampDateTrigger.Millisecond.ToString("000");

            if (_stampTriggerCreate)
            {
                StampTriggerClear();
                LineStampTrigger();
            }

            ResizeAxis();

            ApplyCut_StripButton.Visible = false;
            Cut_StripButton.Image = Properties.Resources.Cutting_Add;
            _createCutBox = false;

            DelCutBox();

        }

        //При наведении курсора на панаель инструментов, отображаем его как стандартный
        private void toolStrip1_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        private void zedGraph_Resize_1(object sender, EventArgs e)
        {
            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

        private void SaveScope_toolStripButton_MouseDown(object sender, MouseEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = @".txt",
                Filter = @"Text Files (*.txt)|*.txt"
            };

            if (sfd.ShowDialog() != DialogResult.OK) { return; }

            // Save to .txt

            StreamWriter sw;

            try
            {
                sw = File.CreateText(sfd.FileName);
            }
            catch
            {
                MessageBox.Show(@"Ошибка при создании файла!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DateTime dateTemp = MainWindow.OscilList[NumGraphPanel()].StampDateTrigger;
                sw.WriteLine(dateTemp.ToString("dd'/'MM'/'yyyy HH:mm:ss.fff000"));                  //Штамп времени
                sw.WriteLine(MainWindow.OscilList[NumGraphPanel()].SampleRate);                     //Частота выборки (частота запуска осциллогрофа/ делитель)
                sw.WriteLine(MainWindow.OscilList[NumGraphPanel()].HistotyCount);                   //Предыстория 
                sw.WriteLine(FileHeaderLine());                                                     //Формирование заголовка (подписи названия каналов)
                for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].NumCount; i++)            //Формирование строк всех загруженных данных (отсортированых с предысторией)
                {
                    sw.WriteLine(FileParamLine(i));
                }

            }
            catch
            {
                MessageBox.Show(@"Ошибка при записи в файл!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sw.Close();
        }

        private string FileHeaderLine()
        {
            string str = " " + "\t";
            for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].ChannelCount; i++)
            {
                str = str + MainWindow.OscilList[NumGraphPanel()].ChannelNames[i] + "\t";
            }
            return str;
        }

        private string FileParamLine(int numLine)
        {
            string str = numLine + "\t";
            for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].Data[numLine].Count; i++)
            {
                str = str + MainWindow.OscilList[NumGraphPanel()].Data[numLine][i] + "\t";
            }
            return str;
        }
    }
}

