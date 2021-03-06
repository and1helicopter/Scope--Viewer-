﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ZedGraph;
using FillType = ZedGraph.FillType;

namespace ScopeViewer
{
	public sealed partial class GraphPanel : UserControl
	{
		MasterPane _masterPane;
		public GraphPane Pane;
		public GraphPane PaneDig;
		readonly List<LineItem> _myCurve = new List<LineItem>();

		double _maxXAxis, _minXAxis, _maxYAxis, _minYAxis, _smallX, _lastMinX, _lastMaxX;
		double _maxYAxisAuto;
		double _minYAxisAuto;
		bool _scaleY;
		bool _absOrRel = true;
		private bool _bind;
		private bool _init;
		private bool _autoScaling = true;

		public GraphPanel(bool bind)
		{
			ListTemp = new List<PointPairList>();
			InitializeComponent();
			_bind = bind;

			DoubleBuffered = true;

			zedGraph.ZoomEvent += zedGraph_ZoomEvent;
			zedGraph.ScrollEvent += zedGraph_ScrollEvent;
			zedGraph.PointValueEvent += ZedGraph_PointValueEvent;
			zedGraph.ContextMenuBuilder += zedGraph_ContextMenuBuilder;

			InitDrawGraph();
		}

		private void zedGraph_ContextMenuBuilder(ZedGraphControl sender,
			ContextMenuStrip menuStrip,
			Point mousePt,
			ZedGraphControl.ContextMenuObjectState objState)
		{
            menuStrip.Items.RemoveAt(1);
            menuStrip.Items.RemoveAt(4);
			menuStrip.Items.RemoveAt(4);
			menuStrip.Items.RemoveAt(4);

            ToolStripItem saveMenuItem = new ToolStripMenuItem("Сохранить рисунок как...");
            saveMenuItem.Click += MenuItemSaveImage;
            menuStrip.Items.Add(saveMenuItem);

            ToolStripItem scaleAllMenuItem = new ToolStripMenuItem("Отменить всё масштабирование");
			scaleAllMenuItem.Click += MenuItemScaleAllOnClick;
			menuStrip.Items.Add(scaleAllMenuItem);
		}

        private void MenuItemSaveImage(object sender, EventArgs e)
        {
            // ДИалог выбора имени файла создаем вручную
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.png|*.png|*.jpg; *.jpeg|*.jpg;*.jpeg|*.bmp|*.bmp|Все файлы|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                TextObj cursor1Text = null;
                TextObj cursor2Text = null;
                TextObj stampTimeText = null;
                TextObj diffCursorText = null;

                //old position legend
                var legendLocationH = Pane.Legend.Location.AlignH;
                var legendLocationV = Pane.Legend.Location.AlignV;
                var legendPosition = Pane.Legend.Position;
                var legendCoordinateFrame = Pane.Legend.Location.CoordinateFrame;
                var legendIsVisibale = Pane.Legend.IsVisible;
                var legendFontSpecSize = Pane.Legend.FontSpec.Size;


                Pane.Legend.IsVisible = true;
                Pane.Legend.Position = LegendPos.BottomCenter;
                Pane.Legend.Location.CoordinateFrame = CoordType.PaneFraction;
                Pane.Legend.FontSpec.Size = 12;

                //var y = zedGraph.ScrollMinY + 1.00 * (zedGraph.ScrollMaxY - zedGraph.ScrollMinY);

                if (Cursor1 != null)
                {
                    var x = Cursor1.Location.X;
                    cursor1Text = new TextObj(PositionText(Cursor1), x, zedGraph.ScrollMaxY);
                    Pane.GraphObjList.Add(cursor1Text);
                    zedGraph.Invalidate();
                }
                if (Cursor2 != null)
                {
                    var x = Cursor2.Location.X;
                    cursor2Text = new TextObj(PositionText(Cursor2), x, zedGraph.ScrollMaxY);
                    Pane.GraphObjList.Add(cursor2Text);
                    zedGraph.Invalidate();
                }
                if (_stampTrigger != null)
                {
                    var x = _stampTrigger.Location.X;
                    stampTimeText = new TextObj(PositionText(_stampTrigger), x, zedGraph.ScrollMinY);
                    Pane.GraphObjList.Add(stampTimeText);
                    zedGraph.Invalidate();
                }
                if (Cursor1 != null && Cursor2 != null)
                {
                    var x1 = Cursor1.Location.X;
                    var x2 = Cursor2.Location.X;
                    diffCursorText = new TextObj($"Δ:{Math.Abs(x2 - x1):F6}", x2, zedGraph.ScrollMinY);
                    Pane.GraphObjList.Add(diffCursorText);
                    zedGraph.Invalidate();
                }
                
                // Получаем картинку, соответствующую панели
                Bitmap bmp = zedGraph.MasterPane.GetImage();

                // Сохраняем картинку средствами класса Bitmap
                // Формат картинки выбирается исходя из имени выбранного файла
                
                if (dlg.FileName.EndsWith(".png"))
                {
                    bmp.Save(dlg.FileName, ImageFormat.Png);
                }
                else if (dlg.FileName.EndsWith(".jpg") || dlg.FileName.EndsWith(".jpeg"))
                {
                    bmp.Save(dlg.FileName, ImageFormat.Jpeg);
                }
                else if (dlg.FileName.EndsWith(".bmp"))
                {
                    bmp.Save(dlg.FileName, ImageFormat.Bmp);
                }
                else
                {
                    bmp.Save(dlg.FileName);
                }

                if (cursor1Text != null) Pane.GraphObjList.Remove(cursor1Text);
                if (cursor2Text != null) Pane.GraphObjList.Remove(cursor2Text);
                if (stampTimeText != null) Pane.GraphObjList.Remove(stampTimeText);
                if (diffCursorText != null) Pane.GraphObjList.Remove(diffCursorText);                

                Pane.Legend.Location.AlignH = legendLocationH;
                Pane.Legend.Location.AlignV = legendLocationV;
                Pane.Legend.Position = legendPosition;
                Pane.Legend.Location.CoordinateFrame = legendCoordinateFrame;
                Pane.Legend.IsVisible = legendIsVisibale;
            }

            string PositionText(LineObj obj)
            {
                double x = obj.Location.X;

                return Text(x);
            }

            string Text(double val)
            {
                if (_absOrRel)
                {
                    //Abs time
                    var value =
                        MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Hour.ToString("D2") + ":" +
                        MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Minute.ToString("D2") + ":" +
                        MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Second.ToString("D2") + "." +
                        MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Millisecond.ToString("D3");
                    return value;
                }
                else
                {
                    //Relativ time
                    var value = (int)val + " сек " + (int)((val - (int)val) * 1000) + " мс " +
                                (int)((val * 1000 - (int)(val * 1000)) * 1000) + " мкс ";
                    return value;
                }
            }
        }

        private void MenuItemScaleAllOnClick(object sender, EventArgs eventArgs)
		{
			Pane.XAxis.Scale.Min = _minXAxis;
			Pane.XAxis.Scale.Max = _maxXAxis;
			Pane.YAxis.Scale.Max = _maxYAxis;
			Pane.YAxis.Scale.Min = _minYAxis;

			if (PaneDig != null)
			{
				PaneDig.XAxis.Scale.Min = _minXAxis;
				PaneDig.XAxis.Scale.Max = _maxXAxis;
			}

			zedGraph.AxisChange();
			zedGraph.Invalidate();
			UpdateCursor();
			UpdateGraph();

		}

		//private void MenuItemScaleHorizontalOnClick(object sender, EventArgs eventArgs)
		//{
		//	Pane.XAxis.Scale.Min = _minXAxis;
		//	Pane.XAxis.Scale.Max = _maxXAxis;

		//	//UpdateCursor();
		//	//UpdateGraph();

		//	zedGraph.ScrollMaxY = _maxYAxis;
		//	zedGraph.ScrollMinY = _minYAxis;
		//	ScrollUpdate();

		//	//zedGraph.Invalidate();
		//	//zedGraph.AxisChange();

		//}

		//private void MenuItemScaleVerticalOnClick(object sender, EventArgs eventArgs)
		//{

		//	Pane.YAxis.Scale.Max = _maxYAxis;
		//	Pane.YAxis.Scale.Min = _minYAxis;

		//	//UpdateCursor();
		//	//UpdateGraph();

		//	zedGraph.ScrollMaxX = _maxXAxis;
		//	zedGraph.ScrollMinX = _minXAxis;

		//	ScrollUpdate();
		//	//zedGraph.Invalidate();
		//	//zedGraph.AxisChange();
		//}

		internal void SetCursorBinding(bool bind)
		{
			_bind = bind;
		}

		private void ScrollUpdate()
		{
			ScrollEvent();

			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
		}

		private void zedGraph_ScrollEvent(object sender, ScrollEventArgs e)
		{
			zedGraph.ScrollMaxX = _maxXAxis;
			zedGraph.ScrollMinX = _minXAxis;
			zedGraph.ScrollMaxY = _maxYAxis;
			zedGraph.ScrollMinY = _minYAxis;

			ScrollEvent();

			Pane.YAxis.Scale.MajorStepAuto = true;
			Pane.YAxis.Scale.MinorStepAuto = true;

			zedGraph.Invalidate();
		}

		private void zedGraph_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
		{

			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
		}

		private void ScrollEvent()
		{
			if (Pane.XAxis.Scale.Min <= _minXAxis)
			{
				Pane.XAxis.Scale.Min = _minXAxis;
				if (PaneDig != null)
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
				if (PaneDig != null)
				{
					PaneDig.YAxis.Scale.Min = _minYAxisAuto;
				}
			}

			if (Pane.YAxis.Scale.Max >= _maxYAxis)
			{
				Pane.YAxis.Scale.Max = _maxYAxis;
				if (PaneDig != null)
				{
					PaneDig.YAxis.Scale.Max = _maxYAxisAuto;
				}
			}

			if (_scaleY == false)
			{
				Pane.YAxis.Scale.Max = _maxYAxis;
				Pane.YAxis.Scale.Min = _minYAxis;
			}

			if (PaneDig != null)
			{
				PaneDig.YAxis.Scale.Max = _maxYAxisAuto;
				PaneDig.YAxis.Scale.Min = _minYAxisAuto;
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

			if (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min < _smallX * 10)
			{
				Pane.XAxis.Scale.Min = _lastMinX;
				Pane.XAxis.Scale.Max = _lastMaxX;
				if (PaneDig != null)
				{
					PaneDig.XAxis.Scale.Min = _lastMinX;
					PaneDig.XAxis.Scale.Max = _lastMaxX;
				}
			}

			_lastMinX = Pane.XAxis.Scale.Min;
			_lastMaxX = Pane.XAxis.Scale.Max;

			//Refresh();
			UpdateCursor();
			UpdateGraph();

			Pane.YAxis.Scale.MajorStepAuto = true;
			Pane.YAxis.Scale.MinorStepAuto = true;

			//zedGraph.AxisChange();
			//zedGraph_ScrollEvent(null, null);
			//zedGraph.Invalidate();

			//zedGraph.AxisChange();
		}

		private void ChangeScale()
		{
			_scaleY = !_scaleY;
		}

		// Создадим список точек   
		private readonly List<double> _scaleLine = new List<double>();
		private readonly List<double> _shiftLine = new List<double>();



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
			//zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
		}


		public void ChangeDigitalList(int i, int j, int status)
		{
			_digitalList[i] = status != 0;
			MainWindow.OscilList[j].ChannelType[i] = status != 0;
			UpdateGraph();
			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
		}

		private void UpdateGraph()
		{
			if (ListTemp.Count == 0) return;
			try
			{
				//Очищаю точки кривых в кэше
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
						//Pane.CurveList[i].AddPoint(ListTemp[i][j]);
						Pane.CurveList[i].AddPoint(ListTemp[i][j].X, ListTemp[i][j].Y * _scaleLine[i] + _shiftLine[i]);
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
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Second:D2}";
				}
				if (axis.Scale.Max - axis.Scale.Min > 0.005)
				{
					return
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Second:D2}." +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Millisecond:D3}";
				}
				if (axis.Scale.Max - axis.Scale.Min > 0.000005)
				{
					return
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Second:D2}." +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Millisecond:D3}" +
						$"'{(int)(val * 1000000) % 1000:D3}";
				}
				else
				{
					return
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Hour:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Minute:D2}:" +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Second:D2}." +
						$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(val * 1000).Millisecond:D3}" +
						$"'{(int)(val * 1000000) % 1000:D3}" +
						$"\"{(uint)((val * 1000000000) % 1000):D3}";
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
					return $"{(int)(val * 1000) / 1000}.{(int)(val * 1000) % 1000:D3} ms";
				}
				if (axis.Scale.Max - axis.Scale.Min > 0.000005)
				{
					return $"{(int)(val * 1000) / 1000}.{(int)(val * 1000) % 1000:D3}'{(int)(val * 1000000) % 1000:D3} \u03BCs";
				}
				else
				{
					return $"{(int)(val * 1000) / 1000}.{(int)(val * 1000) % 1000:D3}'{(int)(val * 1000000) % 1000:D3}\"{(uint)((val * 1000000000) % 1000):D3} ns";
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
			StampTime_label.Text = MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger + @"." +
								   MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger.Millisecond.ToString("000");

			if (!dig) AddAnalogChannel(j, color);
			_init = true;
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
			_digitalList.Add(MainWindow.OscilList[MainWindow.OscilList.Count - 1].ChannelType[j]);

			string nameCh = "";

			// Заполняем список точек. Приращение по оси X 
			// ReSharper disable once PossibleLossOfFraction
			int sum = Convert.ToInt32((double)(MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilEndSample / _pointInLine));
			if (sum == 0) sum = 1;
			// DateTime tempTime;

			for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilEndSample; i += sum)
			{
				//tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilStampDateStart;
				// добавим в список точку
				list.Add((i) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilSampleRate, MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilData[i][j]);
				nameCh = MainWindow.OscilList[MainWindow.OscilList.Count - 1].ChannelNames[j];
			}

			for (int i = 0; i < MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilEndSample; i++)
			{
				//tempTime = MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilStampDateStart;
				// добавим в список точку
				ListTemp[ListTemp.Count - 1].Add((i) / MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilSampleRate, MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilData[i][j]);
			}

			_smallX = Math.Abs(ListTemp[0][1].X -
			                   ListTemp[0][0].X);

			// Выберем случайный цвет для графика
			LineItem newCurve = Pane.AddCurve(nameCh, list, color, SymbolType.None);
			newCurve.Line.IsSmooth = false;
			_myCurve.Add(newCurve);
			_scaleLine.Add(1.00);
			_shiftLine.Add(0.00);

			_stampTriggerCreate = false;
			StampTriggerEvent();

			ResizeAxis();

			if (MainWindow.OscilList[MainWindow.OscilList.Count - 1].OscilChannelCount - 1 == j)
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (_minYAxis == 0)
				{
					double minYAxisTemp = Pane.CurveList[0].Points[0].Y;

					foreach (var item in Pane.CurveList)
					{

						for (int i = 0; i < item.NPts; i++)
						{
							if (item.Points[i].Y <= minYAxisTemp)
							{
								minYAxisTemp = item.Points[i].Y;
							}
						}
					}

					_minYAxis = minYAxisTemp - (_maxYAxis - minYAxisTemp) / 10;
				}


				ScrollUpdate();
				//ScrollEvent();
			}
		}


		private string ZedGraph_PointValueEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
		{
			PointPair point = curve[iPt];
			// Color clr = curve.Color;
			string nameChannel = curve.Label.Text;


			if (_absOrRel)
			{
				return
					$"{nameChannel}" + "\n" +
					$"X: {MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(point.X * 1000).Hour:D2}:" +
					$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(point.X * 1000).Minute:D2}:" +
					$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(point.X * 1000).Second:D2}." +
					$"{MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(point.X * 1000).Millisecond:D3}" +
					"\n" + $"Y: {point.Y}";
			}
			return $"{nameChannel}" + "\n" + $"X: {point.X}" + "\n" + $"Y: {point.Y}";
		}

		public void AddDigitalChannel(int numCh, int numOsc, Color color)
		{
			if (PaneDig != null)         //Проверка на откыртый канал
			{
				MessageBox.Show(@"Дискретный канал уже открыт!");
				return;
			}

			Pane.YAxis.Scale.MajorStepAuto = true;
			Pane.YAxis.Scale.MinorStepAuto = true;

			if (_cursorsCreate)          //Удалим Курсоры, если оние есть
			{
				AddCoursorVerticalEvent();
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

			for (int i = 1; i < MainWindow.OscilList[numOsc].OscilEndSample; i++)
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (MainWindow.OscilList[numOsc].OscilData[i][numCh] !=
					MainWindow.OscilList[numOsc].OscilData[i - 1][numCh])
				{
					list1.Add((i - 1) / MainWindow.OscilList[numOsc].OscilSampleRate, line1);
					list0.Add((i - 1) / MainWindow.OscilList[numOsc].OscilSampleRate, line0);

					double temp0 = line0;
					double temp1 = line1;
					line1 = temp0;
					line0 = temp1;

					list1.Add(i / MainWindow.OscilList[numOsc].OscilSampleRate, line1);
					list0.Add(i / MainWindow.OscilList[numOsc].OscilSampleRate, line0);
				}
				if (i == MainWindow.OscilList[numOsc].OscilEndSample - 1)
				{
					list1.Add((i) / MainWindow.OscilList[numOsc].OscilSampleRate, line1);
					list0.Add((i) / MainWindow.OscilList[numOsc].OscilSampleRate, line0);
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

			for (int i = 0; i < MainWindow.OscilList[numOsc].OscilEndSample; i++)
			{
				// добавим в список точку
				list.Add(i / MainWindow.OscilList[numOsc].OscilSampleRate, MainWindow.OscilList[numOsc].OscilData[i][numCh]);
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
			else if (binary == 1)
			{
				maxMaskCount = 32;
			}

			for (int l = 0; l < maxMaskCount; l++)
			{
				list = new PointPairList();
				double line;

				if ((Convert.ToInt32(MainWindow.OscilList[numOsc].OscilData[0][numCh]) & 1 << l) == 1 << l)
				{
					line = -0.2 - 1 - l;
				}
				else
				{
					line = -0.8 - 1 - l;
				}

				list.Add(0 / MainWindow.OscilList[numOsc].OscilSampleRate, line);

				for (int i = 1; i < MainWindow.OscilList[numOsc].OscilEndSample; i++)
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					if (MainWindow.OscilList[numOsc].OscilData[i][numCh] !=
					   MainWindow.OscilList[numOsc].OscilData[i - 1][numCh])
					{
						list.Add( i/ MainWindow.OscilList[numOsc].OscilSampleRate, line);
						if ((Convert.ToInt32(MainWindow.OscilList[numOsc].OscilData[i][numCh]) & 1 << l) == 1 << l)
						{
							line = -0.2 - 1 - l;
						}
						else
						{
							line = -0.8 - 1 - l;
						}
						list.Add( i / MainWindow.OscilList[numOsc].OscilSampleRate, line);
					}
					if (i == MainWindow.OscilList[numOsc].OscilEndSample - 1)
					{
						list.Add( i / MainWindow.OscilList[numOsc].OscilSampleRate, line);
					}
				}

				LineItem newCurve = PaneDig.AddCurve(nameCh1, list, color, SymbolType.None);
				newCurve.Line.IsSmooth = false;
				_myCurve.Add(newCurve);
				_myCurve[_myCurve.Count - 1].Line.Width = 2;
			}

			PaneDig.Border.Color = Color.White;

			PaneDig.YAxis.IsVisible = true;

			PaneDig.X2Axis.IsVisible = false;
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

			_posTabHoriz = true;
			//posTab_StripButton.Visible = true;
			delateDig_toolStripButton.Visible = true;
			toolStripSeparator2.Visible = false;
			Mask1_label.Visible = true;
			Mask2_label.Visible = true;
			MaskMin_textBox.Visible = true;
			MaskMax_textBox.Visible = true;
			toolStripSeparator2.Visible = false;

			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();

			ChangedPos();
		}

		private void ChangedPos()
		{
			if (PaneDig != null)
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
						Pane.Chart.Rect.X + Pane.Chart.Rect.Width + 75 + 15,
						Pane.Chart.Rect.Y,
						Pane.Chart.Rect.Width,
						Pane.Chart.Rect.Height);
				}
			}
			ScrollUpdate();
		}

		private void ResizeAxis()
		{

			zedGraph.GraphPane.YAxis.Scale.MinAuto = true;
			zedGraph.GraphPane.YAxis.Scale.MaxAuto = true;
			zedGraph.GraphPane.XAxis.Scale.MinAuto = true;
			zedGraph.GraphPane.XAxis.Scale.MaxAuto = true;

			zedGraph.AxisChange();
			zedGraph.Invalidate();

			zedGraph.IsShowHScrollBar = true;
			zedGraph.IsShowVScrollBar = true;
			zedGraph.IsAutoScrollRange = true;

			int numPoint = Pane.CurveList.Last().Points.Count - 1;
			_maxXAxis = Pane.CurveList.Last().Points[numPoint].X;
			_minXAxis = zedGraph.GraphPane.XAxis.Scale.Min;

			if (_maxYAxis < zedGraph.GraphPane.YAxis.Scale.Max)
			{
				_maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max;

				toolStripTextBox_Top.Text = _maxYAxis.ToString("F");
			}

			if (_minYAxis > zedGraph.GraphPane.YAxis.Scale.Min)
			{
				_minYAxis = zedGraph.GraphPane.YAxis.Scale.Min;

				toolStripTextBox_Bottom.Text = _minYAxis.ToString("F");
			}

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (zedGraph.GraphPane.YAxis.Scale.Max == 0)
			{
				_maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max +
							(zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 10;

				toolStripTextBox_Top.Text = _maxYAxis.ToString("F");
			}

			// ReSharper disable once CompareOfFloatsByEqualityOperator
			if (zedGraph.GraphPane.YAxis.Scale.Min == 0)
			{
				_minYAxis = zedGraph.GraphPane.YAxis.Scale.Min -
							(zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 10;

				toolStripTextBox_Bottom.Text = _minYAxis.ToString("F");
			}

			// Обновим график
			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
			UpdateGraph();
			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
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

		/// <summary>
		/// Change visual style line
		/// </summary>
		/// <param name="numChannel">Number line</param>
		/// <param name="typeLine">Type line</param>
		/// <param name="typeStep">Step</param>
		/// <param name="width">Width</param>
		/// <param name="show">Visability</param>
		/// <param name="smooth">Smooth</param>
		/// <param name="colorLine">Color line</param>
		/// <param name="scale">Scale</param>
		/// <param name="shift">Shift</param>
		public void ChangeVisualLine(int numChannel, int typeLine, int typeStep, bool width, bool show, bool smooth, Color colorLine, double scale, double shift)
		{
			Pane.CurveList[numChannel].Color = colorLine;
			Pane.CurveList[numChannel].IsVisible = show;
			Pane.CurveList[numChannel].Label.IsVisible = show;
			_myCurve[numChannel].Line.IsSmooth = smooth;
			_myCurve[numChannel].Line.SmoothTension = 0.5F;
			_myCurve[numChannel].Line.Width = width ? 3 : 1;

			//Курсоры
			if (_cursorsCreate && Cursor1 != null && Cursor2 != null)
			{
				_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
				if (PaneDig != null)
				{
					_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
				}
			}
			
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

			//Изменение масштаба
			_scaleLine[numChannel] = scale;

			//Изменение сдвига
			_shiftLine[numChannel] = shift;

			//Обновляем график
			UpdateGraph();

			if (Pane.CurveList.Count(x=>x.IsVisible) == 0)
			{
				Pane.YAxis.Scale.MinAuto = true;
				Pane.YAxis.Scale.MaxAuto = true;
				Pane.IsBoundedRanges = false;
				zedGraph.AxisChange();
				zedGraph.Invalidate();
				UpdateCursor();
				UpdateGraph();
				return;
			}

			if (_autoScaling)
			{
				//_myCurve[numChannel].GetRange(out double xMin, out double xMax, out double yMin, out double yMax, true, true, Pane);

				//Устанавливаем новую верхнюю и нижнюю границу для оси OY 
				Pane.YAxis.Scale.MinAuto = true;
				Pane.YAxis.Scale.MaxAuto = true;
				Pane.YAxis.Scale.MajorStepAuto = true;
				Pane.YAxis.Scale.MinorStepAuto = true;


				Pane.IsBoundedRanges = false;

				zedGraph.AxisChange();
				zedGraph.Invalidate();

				UpdateCursor();
				UpdateGraph();


				var xMinRange = zedGraph.GraphPane.XAxis.Scale.Min;
				var xMaxRange = zedGraph.GraphPane.XAxis.Scale.Max;
				var yMinRange = zedGraph.GraphPane.YAxis.Scale.Min;
				var yMaxRange = zedGraph.GraphPane.YAxis.Scale.Max;





				zedGraph.GraphPane.XAxis.Scale.Min = _minXAxis;
				zedGraph.GraphPane.XAxis.Scale.Max = _maxXAxis;
				zedGraph.GraphPane.YAxis.Scale.Min = _minYAxis;
				zedGraph.GraphPane.YAxis.Scale.Max = _maxYAxis;

				//_myCurve[numChannel].GetRange(out double xMin, out double xMax, out double yMin, out double yMax, true, true, Pane);


				//zedGraph.AxisChange();
				//zedGraph.Invalidate();

				UpdateCursor();
				UpdateGraph();

				Pane.YAxis.Scale.MinAuto = true;
				Pane.YAxis.Scale.MaxAuto = true;
				Pane.YAxis.Scale.MajorStepAuto = true;
				Pane.YAxis.Scale.MinorStepAuto = true;

				Pane.IsBoundedRanges = false;



				zedGraph.AxisChange();
				zedGraph.Invalidate();

				_maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max;
				_minYAxis = zedGraph.GraphPane.YAxis.Scale.Min;

				//if (_maxYAxis < yMax) _maxYAxis = yMax;
				//if (_minXAxis > yMin) _minYAxis = yMin;

				if (Math.Abs(Pane.YAxis.Scale.Max) < 0.01)
				{
					_maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max +
					            (zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 15;
				}

				if (Math.Abs(Pane.YAxis.Scale.Min) < 0.01)
				{
					_minYAxis = zedGraph.GraphPane.YAxis.Scale.Min -
					            (zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 15;
				}

				Pane.YAxis.Scale.Max = _maxYAxis;
				Pane.YAxis.Scale.Min = _minYAxis;


				zedGraph.GraphPane.XAxis.Scale.Min = xMinRange;
				zedGraph.GraphPane.XAxis.Scale.Max = xMaxRange;
				zedGraph.GraphPane.YAxis.Scale.Min = yMinRange;
				zedGraph.GraphPane.YAxis.Scale.Max = yMaxRange;
			}


			UpdateCursor();
			UpdateGraph();
			zedGraph.AxisChange();
			zedGraph.Invalidate();
			zedGraph_ScrollEvent(null, null);
			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
			zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
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
				if (v == 1)
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

			Pane.Legend.Fill.Color = Color.WhiteSmoke;
			Pane.Legend.Fill.Type = FillType.Solid;

			// Вызываем метод AxisChange (), чтобы обновить данные об осях.
			zedGraph.AxisChange();
			zedGraph.Invalidate();
			
			zedGraph.Update();
		}

		private LineObj _stampTrigger;
		private LineObj _stampTriggerDig;

		private void LineStampTrigger()
		{
			try
			{
				double timeStamp = MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount / MainWindow.OscilList[NumGraphPanel()].OscilSampleRate;
				_stampTrigger = new LineObj(timeStamp, Pane.YAxis.Scale.Min, timeStamp, Pane.YAxis.Scale.Max)
				{
					Line =
					{
						Style = System.Drawing.Drawing2D.DashStyle.DashDot,
						Color = Color.DarkGreen,
						Width = 2
					},
					Link = { Title = "StampTrigger" },
					ZOrder = ZOrder.B_BehindLegend
				};

				Pane.GraphObjList.Add(_stampTrigger);
				if (_stampTrigger.Location.X < Pane.XAxis.Scale.Min || _stampTrigger.Location.X > Pane.XAxis.Scale.Max)
				{
					_stampTrigger.IsVisible = false;
				}

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
						Link = { Title = "StampTriggerDig" },
						ZOrder = ZOrder.B_BehindLegend
					};

					PaneDig.GraphObjList.Add(_stampTriggerDig);
					if (_stampTriggerDig.Location.X < PaneDig.XAxis.Scale.Min || _stampTriggerDig.Location.X > PaneDig.XAxis.Scale.Max)
					{
						_stampTriggerDig.IsVisible = false;
					}
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
		private bool _cursorsCreateHorizontal;
		public LineObj Cursor1;
		public LineObj Cursor2;
		public LineObj CursorHorizontal1;
		public LineObj CursorHorizontal2;
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

	    private bool cursorInit = false;

		private void CursorAdd()
		{
            //var countLeft = (int)(Cursor1.Location.X / _smallX);
            //Cursor1.Location.X = countLeft * _smallX;
            Cursor1 = new LineObj(
				((int)(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 4 + Pane.XAxis.Scale.Min) / _smallX)) * _smallX,
				Pane.YAxis.Scale.Min,
				((int)(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) / 4 + Pane.XAxis.Scale.Min) / _smallX)) * _smallX,
				Pane.YAxis.Scale.Max)
			{
				Line =
				{
					Style = System.Drawing.Drawing2D.DashStyle.Solid,
					Color = Color.Red,
					Width = 2
				},
				Link = { Title = "Cursor1" },
				ZOrder = ZOrder.B_BehindLegend,
			};

			Cursor2 = new LineObj(
				((int)(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) * 3 / 4 + Pane.XAxis.Scale.Min) / _smallX)) *_smallX,
				Pane.YAxis.Scale.Min,
				((int)(((Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min) * 3 / 4 + Pane.XAxis.Scale.Min) / _smallX)) *_smallX,
				Pane.YAxis.Scale.Max)
			{
				Line =
				{
					Style = System.Drawing.Drawing2D.DashStyle.Solid,
					Color = Color.Blue,
					Width = 2
				},
				Link = { Title = "Cursor2" },
				ZOrder = ZOrder.B_BehindLegend
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
					Link = {Title = "CursorDig1"},
					ZOrder = ZOrder.B_BehindLegend,
					//Location =
					//{
					//	X1 = Cursor1.Location.X1,
					//	X = Cursor1.Location.X
					//}
				};

				//CursorDig1.Location.X = Cursor1.Location.X;


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
					Link = {Title = "CursorDig2"},
					ZOrder = ZOrder.B_BehindLegend,
					//Location =
					//{
					//	X1 = Cursor2.Location.X1,
					//	X = Cursor2.Location.X
					//}
				};

				//CursorDig2.Location.X = Cursor2.Location.X;


				// Добавим линию в список отображаемых объектов
				PaneDig.GraphObjList.Add(CursorDig1);
				PaneDig.GraphObjList.Add(CursorDig2);
			}

			_cursorsCreate = true;

            //            UpdateCursor();

            // Обновляем график
            //zedGraph.AxisChange();
            //zedGraph.Invalidate();
		}

		private void CursorHorizontalAdd()
		{
			CursorHorizontal1 = new LineObj(
				Pane.XAxis.Scale.Min,
				(Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min) * 2 / 3 + Pane.YAxis.Scale.Min,
				Pane.XAxis.Scale.Max,
				(Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min) * 2 / 3 + Pane.YAxis.Scale.Min
				)
			{
				Line =
				{
					Style = System.Drawing.Drawing2D.DashStyle.Solid,
					Color = Color.DarkGreen,
					Width = 2
				},
				Link = { Title = "CursorHorizontal1" },
				ZOrder = ZOrder.B_BehindLegend
			};

			Pane.GraphObjList.Add(CursorHorizontal1);

			CursorHorizontal2 = new LineObj(
				Pane.XAxis.Scale.Min,
				(Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min) / 3 + Pane.YAxis.Scale.Min,
				Pane.XAxis.Scale.Max,
				(Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min) / 3 + Pane.YAxis.Scale.Min)
			{
				Line =
				{
					Style = System.Drawing.Drawing2D.DashStyle.Solid,
					Color = Color.Green,
					Width = 2
				},
				Link = { Title = "CursorHorizontal2" },
				ZOrder = ZOrder.B_BehindLegend
			};

			Pane.GraphObjList.Add(CursorHorizontal2);

			_cursorsCreateHorizontal = true;

			// Обновляем график
			zedGraph.AxisChange();
			zedGraph.Invalidate();
		}

		private void CursorClear()
		{
			for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
			{
				if (Pane.GraphObjList[i].Link.Title == "Cursor1" || Pane.GraphObjList[i].Link.Title == "Cursor2")
				{
					Pane.GraphObjList.Remove(Pane.GraphObjList[i]);
				}
			}

			Cursor1 = null;
			Cursor2 = null;

			if (PaneDig != null)
			{
				for (int i = PaneDig.GraphObjList.Count - 1; i >= 0; i--)
				{
					if (PaneDig.GraphObjList[i].Link.Title == "CursorDig1" || PaneDig.GraphObjList[i].Link.Title == "CursorDig2") { PaneDig.GraphObjList.Remove(PaneDig.GraphObjList[i]); }
				}
			}

			CursorDig1 = null;
			CursorDig2 = null;

			_cursorsCreate = false;

			zedGraph.AxisChange();
			zedGraph.Invalidate();
		}

		private void CursorClearHorizontal()
		{
			for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
			{
				if (Pane.GraphObjList[i].Link.Title == "CursorHorizontal1" || Pane.GraphObjList[i].Link.Title == "CursorHorizontal2")
				{
					Pane.GraphObjList.Remove(Pane.GraphObjList[i]);
				}
			}

			_cursorsCreateHorizontal = false;

			zedGraph.AxisChange();
			zedGraph.Invalidate();
		}

		private void UpdateCursor()
		{
			for (int i = Pane.GraphObjList.Count - 1; i >= 0; i--)
			{
				if (Pane.GraphObjList[i].Link.Title == "Cursor1")
				{
					if (Cursor1.Location.X <= Pane.XAxis.Scale.Min ||
					    Cursor1.Location.X >= Pane.XAxis.Scale.Max) Cursor1.IsVisible = false;
					else Cursor1.IsVisible = true;

					Cursor1.Location.Y1 = Pane.YAxis.Scale.Min;
					Cursor1.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);
					
					if (PaneDig != null)
					{
						if (CursorDig1.Location.X <= PaneDig.XAxis.Scale.Min ||
						    CursorDig1.Location.X >= PaneDig.XAxis.Scale.Max) CursorDig1.IsVisible = false;
						else CursorDig1.IsVisible = true;

						CursorDig1.Location.Y1 = PaneDig.YAxis.Scale.Min;
						CursorDig1.Location.Height = PaneDig.YAxis.Scale.Max - PaneDig.YAxis.Scale.Min;
					}
					//Обновляю значение положение курсора в label
					var x1 = Cursor1.Location.X;
					tool_EnterLeft_label.Text = TextPosition(x1, NumGraphPanel(), _absOrRel);
					//Разница между курсорами
					CursorDif(x1, Cursor2.Location.X);
					_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);

					continue;
				}
				if (Pane.GraphObjList[i].Link.Title == "Cursor2")
				{
					if (Cursor2.Location.X <= Pane.XAxis.Scale.Min ||
					    Cursor2.Location.X >= Pane.XAxis.Scale.Max) Cursor2.IsVisible = false;
					else Cursor2.IsVisible = true;

					Cursor2.Location.Y1 = Pane.YAxis.Scale.Min;
					Cursor2.Location.Height = (Pane.YAxis.Scale.Max - Pane.YAxis.Scale.Min);

					if (PaneDig != null)
					{
						if (CursorDig2.Location.X <= PaneDig.XAxis.Scale.Min ||
						    CursorDig2.Location.X >= PaneDig.XAxis.Scale.Max) CursorDig2.IsVisible = false;
						else CursorDig2.IsVisible = true;

						CursorDig2.Location.Y1 = PaneDig.YAxis.Scale.Min;
						CursorDig2.Location.Height = PaneDig.YAxis.Scale.Max - PaneDig.YAxis.Scale.Min;
					}
					//Обновляю значение положение курсора в label
					var x2 = Cursor2.Location.X;
					tool_EnterRight_label.Text = TextPosition(x2, NumGraphPanel(), _absOrRel);
					//Разница между курсорами
					CursorDif(Cursor1.Location.X, x2);
					_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);

					continue;
				}

				if (Pane.GraphObjList[i].Link.Title == "CursorHorizontal1")
				{
					CursorHorizontal1.Location.X1 = Pane.XAxis.Scale.Min;
					CursorHorizontal1.Location.Width = (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min);

					if (CursorHorizontal1.Location.Y <= Pane.YAxis.Scale.Min ||
						CursorHorizontal1.Location.Y >= Pane.YAxis.Scale.Max) CursorHorizontal1.IsVisible = false;
					else CursorHorizontal1.IsVisible = true;

					////Обновляю значение положение курсора в label
					var y = CursorHorizontal1.Location.Y;
					tool_Horizont1Enter.Text = TextPosition(y, NumGraphPanel(), false);
					CursorHorizontalDif(y, CursorHorizontal2.Location.Y);

					continue;
				}

				if (Pane.GraphObjList[i].Link.Title == "CursorHorizontal2")
				{
					CursorHorizontal2.Location.X1 = Pane.XAxis.Scale.Min;
					CursorHorizontal2.Location.Width = (Pane.XAxis.Scale.Max - Pane.XAxis.Scale.Min);

					if (CursorHorizontal2.Location.Y <= Pane.YAxis.Scale.Min ||
					    CursorHorizontal2.Location.Y >= Pane.YAxis.Scale.Max) CursorHorizontal2.IsVisible = false;
					else CursorHorizontal2.IsVisible = true;

					////Обновляю значение положение курсора в label
					var y = CursorHorizontal2.Location.Y;
					tool_Horizont2Enter.Text = TextPosition(y, NumGraphPanel(), false);
					CursorHorizontalDif(CursorHorizontal1.Location.Y, y);

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

					if (_leftLineCut.Location.X > _rightLineCut.Location.X)
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

		private void CursorDif(double x1, double x2)
		{
			var dif = Math.Abs(x2 - x1);
			tool_CursorsDif.Text = $@"Δ: {dif:F6}";
		}

		private void CursorHorizontalDif(double y1, double y2)
		{
			var dif = Math.Abs(y2 - y1);
			tool_CursorsHorizontalDif.Text = $@"Δ: {dif:F6}";
		}

		private void zedGraph_MouseMove(object sender, MouseEventArgs e)
		{
			Pane.ReverseTransform(new PointF(e.X, e.Y), out var graphX, out var graphY);

			if (Cursor1 != null || Cursor2 != null || _leftLineCut != null || _rightLineCut != null || CursorHorizontal1 != null)
			{
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				// ReSharper disable once PossibleNullReferenceException
				if (Cursor1 != null && Cursor1.Line.Width == 3)
				{
					Cursor1.Location.X1 = graphX;
					if (_bind)
					{
						var count = (int)(Cursor1.Location.X / _smallX);
						Cursor1.Location.X = count * _smallX;
					}

					UpdateCursor();
					//zedGraph.AxisChange();
					zedGraph.Invalidate();
					Cursor = Cursors.VSplit;
					if (PaneDig != null)
					{
						if (_bind)
						{
							var count = (int)(Cursor1.Location.X / _smallX);
							CursorDig1.Location.X1 = count * _smallX;
						}
						else CursorDig1.Location.X1 = graphX;
						UpdateCursor();
						//zedGraph.AxisChange();
						zedGraph.Invalidate();
					}
				}

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (Cursor2 != null && Cursor2.Line.Width == 3)
				{
					Cursor2.Location.X1 = graphX;
					if (_bind)
					{
						var count = (int)(Cursor2.Location.X / _smallX);
						Cursor2.Location.X = count * _smallX;
					}
					
					UpdateCursor();
					//zedGraph.AxisChange();
					zedGraph.Invalidate();
					Cursor = Cursors.VSplit;
					if (PaneDig != null)
					{
						if (_bind)
						{
							var count = (int)(Cursor2.Location.X / _smallX);
							CursorDig2.Location.X1 = count * _smallX;
						}
						else CursorDig2.Location.X1 = graphX;
						UpdateCursor();
						//zedGraph.AxisChange();
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
						//zedGraph.AxisChange();
						zedGraph.Invalidate();
						Cursor = Cursors.VSplit;
					}

					if (_leftLineCut != null && Math.Abs(_leftLineCut.Line.Width - 3) < 1)
					{
						_leftLineCut.Location.X1 = graphX;
						UpdateCursor();
						//zedGraph.AxisChange();
						zedGraph.Invalidate();
						Cursor = Cursors.VSplit;
					}
				}

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (CursorHorizontal1 != null && CursorHorizontal1.Line.Width == 3)
				{
					CursorHorizontal1.Location.Y1 = graphY;
					UpdateCursor();
					//zedGraph.AxisChange();
					zedGraph.Invalidate();
					Cursor = Cursors.HSplit;
				}

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (CursorHorizontal2 != null && CursorHorizontal2.Line.Width == 3)
				{
					CursorHorizontal2.Location.Y1 = graphY;
					UpdateCursor();
					//zedGraph.AxisChange();
					zedGraph.Invalidate();
					Cursor = Cursors.HSplit;
				}

				zedGraph_ScrollEvent(null, null);
				zedGraph.Invalidate();
			}
		}

		private void zedGraph_MouseClick(object sender, MouseEventArgs e)
		{
			Pane.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out var nearestObject, out _);




			if (nearestObject == null)
			{
				if (PaneDig != null)
				{
					PaneDig.FindNearestObject(new PointF(e.X, e.Y), CreateGraphics(), out nearestObject, out _);

				}

				if (Cursor1 != null && Cursor2 != null)
				{

					if (Math.Abs(Cursor1.Line.Width - 3) < 0.01)
					{
						Cursor1.Line.Width = 2;
						if (PaneDig != null)
						{
							CursorDig1.Line.Width = 2;
							_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
						}

						_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
						zedGraph.Invalidate();

						return;
					}

					if (Math.Abs(Cursor2.Line.Width - 3) < 0.01)
					{
						Cursor2.Line.Width = 2;
						if (PaneDig != null)
						{
							CursorDig2.Line.Width = 2;
							_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
						}

						_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
						zedGraph.Invalidate();

						return;
					}
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
					_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
					if (PaneDig != null)
					{
						_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
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
					_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
					if (PaneDig != null)
					{
						_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
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

				if (lineObject.Link.Title == "CursorHorizontal1")
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					CursorHorizontal1.Line.Width = CursorHorizontal1.Line.Width == 3 ? 2 : 3;
				}

				if (lineObject.Link.Title == "CursorHorizontal2")
				{
					// ReSharper disable once CompareOfFloatsByEqualityOperator
					CursorHorizontal2.Line.Width = CursorHorizontal2.Line.Width == 3 ? 2 : 3;
				}
			}

			//zedGraph.AxisChange();
			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
		}

		private void AddCoursorVertical_MouseDown(object sender, MouseEventArgs e)
		{
            if(cursorInit) AddCoursorVerticalEvent();
            else
            {
                AddCoursorVerticalEvent();
                AddCoursorVerticalEvent();
                AddCoursorVerticalEvent();
                cursorInit = true;
            }
		}

        private void AddCoursorVerticalEvent()
		{
		   

            if (_cursorsCreate == false)
			{
                CursorClear();
				CursorAdd();
				_oscilCursor.AnalysisCursorAdd(NumGraphPanel(), _absOrRel, _bind);
				MainWindow.AnalysisObj.AnalysisStackPanel.Children.Add(_oscilCursor.LayoutPanel[0]);
				if (PaneDig != null)
				{
					_oscilCursor.AnalysisCursorAddDig(NumGraphPanel(), _absOrRel, _bind);
					MainWindow.AnalysisObj.AnalysisStackPanel.Children.Add(_oscilCursor.LayoutPanel[1]);
				}
				AddCursor.Image = Properties.Resources.CursorRemoveV;
				tool_Cursors_label.Visible = true;
				tool_CursorsLeft_label.Visible = true;
				tool_CursorsRight_label.Visible = true;
				tool_EnterLeft_label.Visible = true;
				tool_EnterRight_label.Visible = true;
				tool_CursorsDif.Visible = true;
				tool_separator.Visible = true;
				tool_separator2.Visible = true;


                _oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);

			    var x1 = Cursor1.Location.X;
			    tool_EnterLeft_label.Text = TextPosition(x1, NumGraphPanel(), _absOrRel);
			    var x2 = Cursor2.Location.X;
			    tool_EnterRight_label.Text = TextPosition(x2, NumGraphPanel(), _absOrRel);

                if (PaneDig != null)
				{
					CursorDig1.Location.X = Cursor1.Location.X;
					CursorDig2.Location.X = Cursor2.Location.X;
					_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
				}

                tool_CursorsDif.Text = $@"Δ:{Math.Abs(x2 - x1):F6}";
				tool_CursorsDif.ToolTipText = @"Приращение времени";

                UpdateCursor();
			    _oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
                if (PaneDig != null) _oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
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
					if (MainWindow.AnalysisObj.AnalysisStackPanel.Children.Count != 0)
						MainWindow.AnalysisObj.AnalysisStackPanel.Children.Remove(_oscilCursor.LayoutPanel[0]);
					_oscilCursor.AnalysisCursorClear();
				}

				AddCursor.Image = Properties.Resources.CursorAddV;
				tool_CursorsLeft_label.Visible = false;
				tool_CursorsRight_label.Visible = false;
				tool_EnterLeft_label.Visible = false;
				tool_EnterRight_label.Visible = false;
				tool_CursorsDif.Visible = false;
				if (!_cursorsCreateHorizontal)
				{
					tool_Cursors_label.Visible = false;
					tool_separator.Visible = false;
				}
				tool_separator2.Visible = false;
			}

			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();

		    
        }

		private string TextPosition(double x, int numOsc, bool absOrRel)
		{
			if (absOrRel)
			{
				var hour = MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x * 1000).Hour.ToString(CultureInfo.CurrentCulture);
				var min = MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x * 1000).Minute.ToString(CultureInfo.CurrentCulture);
				var sec = MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x * 1000).Second.ToString(CultureInfo.CurrentCulture);
				var milisec = MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x * 1000).Millisecond.ToString(CultureInfo.CurrentCulture);
				
				return milisec == String.Empty || milisec == "0" ? hour + ":" + min + ":" + sec : hour + ":" + min + ":" + sec + "." + milisec;
			}

			return x.ToString(CultureInfo.CurrentCulture).Length <= 10 ? x.ToString(CultureInfo.CurrentCulture) : x.ToString("F6");
		}

		private void AddCoursorHorizontal_MouseDown(object sender, MouseEventArgs e)
		{
			AddCoursorHorizontalEvent();
		}

		private void AddCoursorHorizontalEvent()
		{
			if (!_cursorsCreateHorizontal)
			{
				CursorClearHorizontal();
				CursorHorizontalAdd();
				AddCursorH.Image = Properties.Resources.CursorRemoveH;

				tool_Cursors_label.Visible = true;
				tool_separator.Visible = true;
				tool_Horizont_label.Visible = true;
				tool_Horizont1Enter.Visible = true;
				tool_Horizont2Enter.Visible = true;
				tool_CursorsHorizontalDif.Visible = true;
				tool_separator3.Visible = true;
				tool_separator4.Visible = true;
				var y1 = CursorHorizontal1.Location.Y;
				tool_Horizont1Enter.Text = TextPosition(y1, NumGraphPanel(), false);
				var y2 = CursorHorizontal2.Location.Y;
				tool_Horizont2Enter.Text = TextPosition(y2, NumGraphPanel(), false);
				tool_CursorsHorizontalDif.Text = $@"Δ:{Math.Abs(y2 - y1):F6}";
				tool_CursorsHorizontalDif.ToolTipText = @"Приращение";
			}
			else
			{
				CursorClearHorizontal();
				AddCursorH.Image = Properties.Resources.CursorAddH;
				if (!_cursorsCreate)
				{
					tool_Cursors_label.Visible = false;
					tool_separator.Visible = false;
				}
				tool_CursorsHorizontalDif.Visible = false;
				tool_Horizont_label.Visible = false;
				tool_Horizont1Enter.Visible = false;
				tool_Horizont2Enter.Visible = false;
				tool_separator3.Visible = false;
				tool_separator4.Visible = false;
			}

			zedGraph_ScrollEvent(null, null);
			zedGraph.Invalidate();
		}

		public void DelCursor()
		{
			if (_cursorsCreate)
			{

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
			    try
			    {
			        if (Convert.ToInt32(MaskMin_textBox.Text) >= 0 && Convert.ToInt32(MaskMin_textBox.Text) <= 31)
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
			            zedGraph_ScrollEvent(null, null);
			            zedGraph.Invalidate();
			        }
			    }
			    catch
			    {
			        MaskMin_textBox.Text = "";
			    }

			}
		}

		private void MaskMax_textBox_TextChanged(object sender, EventArgs e)
		{
		    try
		    {
		        if (MaskMax_textBox.Text != "")
		        {
		            if (Convert.ToInt32(MaskMin_textBox.Text) >= 0 && Convert.ToInt32(MaskMin_textBox.Text) <= 31)
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
		                zedGraph_ScrollEvent(null, null);
		                zedGraph.Invalidate();
		            }
		        }
            }
		    catch
		    {
		        MaskMax_textBox.Text = "";

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
				AddCoursorVerticalEvent();
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
			absOrRelTime_toolStripButton.ToolTipText = _absOrRel ? "Относительное время" : "Абсолютное время";
			if (_cursorsCreate)
			{
				_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
				if (PaneDig != null)
				{
					_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
				}

				var x1 = Cursor1.Location.X;
				tool_EnterLeft_label.Text = TextPosition(x1, NumGraphPanel(), _absOrRel);
				var x2 = Cursor2.Location.X;
				tool_EnterRight_label.Text = TextPosition(x2, NumGraphPanel(), _absOrRel);
			}

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
				Link = { Title = "LeftLine" },
				ZOrder = ZOrder.B_BehindLegend
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
					Width = 2,
				},
				Link = { Title = "RightLine" },
				ZOrder = ZOrder.B_BehindLegend
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
			double hisCount = MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount;

			for (int j = ListTemp[0].Count - 1; j >= 0; j--)
			{
				if (ListTemp[0][j].X > _rightLineCut.Location.X)   //Правая сторона
				{
					MainWindow.OscilList[NumGraphPanel()].OscilData.RemoveAt(j);
				}
				else if (ListTemp[0][j].X < _leftLineCut.Location.X)   //Левая сторона
				{
					MainWindow.OscilList[NumGraphPanel()].OscilData.RemoveAt(j);
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

			//Определение положения штампа времени 
			if (hisCount > numRight)
			{
				hisCount = numRight;
			}
			else if (hisCount < numLeft)
			{
				hisCount = 0;
			}
			else
			{
				hisCount = hisCount - numLeft;
			}

			foreach (PointPairList listTemp in ListTemp)
			{
				for (int j = 0; j < listTemp.Count; j++)
				{
					listTemp[j].X = j / MainWindow.OscilList[NumGraphPanel()].OscilSampleRate;
				}
			}



			MainWindow.OscilList[NumGraphPanel()].OscilEndSample = Convert.ToUInt32(ListTemp[0].Count);
			MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount = hisCount > 0 ? hisCount : 0;
			MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger =
				MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart.AddMilliseconds(1000 * (numLeft + MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount) / MainWindow.OscilList[NumGraphPanel()].OscilSampleRate);
			MainWindow.OscilList[NumGraphPanel()].OscilStampDateStart =
				MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger.AddMilliseconds(-(1000 * MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount / MainWindow.OscilList[NumGraphPanel()].OscilSampleRate));
			MainWindow.OscilList[NumGraphPanel()].OscilStampDateEnd =
				MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger.AddMilliseconds(1000 * (MainWindow.OscilList[NumGraphPanel()].OscilEndSample - MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount) / MainWindow.OscilList[NumGraphPanel()].OscilSampleRate);

			StampTime_label.Text = MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger + @"." +
					   MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger.Millisecond.ToString("000");

			if (_stampTriggerCreate)
			{
				StampTriggerClear();
				LineStampTrigger();
			}

			ResizeAxis();

			foreach (var listTemp in ListTemp)
			{
				_maxXAxis = listTemp[listTemp.Count - 1].X;
				break;
			}

			//_minXAxis

			ApplyCut_StripButton.Visible = false;
			Cut_StripButton.Image = Properties.Resources.Cutting_Add;
			_createCutBox = false;

			DelCutBox();

			ScrollUpdate();
			//ScrollEvent();
		}

		//При наведении курсора на панаель инструментов, отображаем его как стандартный
		private void toolStrip1_MouseEnter(object sender, EventArgs e)
		{
			Cursor = Cursors.Default;
		}

		private void zedGraph_Resize_1(object sender, EventArgs e)
		{
			zedGraph.ScrollMaxX = _maxXAxis;
			zedGraph.ScrollMinX = _minXAxis;
			zedGraph.ScrollMaxY = _maxYAxis;
			zedGraph.ScrollMinY = _minYAxis;

			if (_init)
				ScrollUpdate();
		}

		private void zedGraph_SizeChanged(object sender, EventArgs e)
		{
			zedGraph.ScrollMaxX = _maxXAxis;
			zedGraph.ScrollMinX = _minXAxis;
			zedGraph.ScrollMaxY = _maxYAxis;
			zedGraph.ScrollMinY = _minYAxis;

			if (_init)
				ScrollUpdate();
		}

		private void SaveScope_toolStripButton_MouseDown(object sender, MouseEventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog()
			{
				DefaultExt = @".txt",
				Filter = @"Text Files (*.txt)|*.txt|COMTRADE rev. 1999 (*.cfg)|*.cfg|COMTRADE rev. 2013 (*.cfg)|*.cfg"
			};

			if (sfd.ShowDialog() != DialogResult.OK) { return; }

			// Save to .txt
			if (sfd.FilterIndex == 1)
			{
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
					DateTime dateTemp = MainWindow.OscilList[NumGraphPanel()].OscilStampDateTrigger;
					sw.WriteLine(dateTemp.ToString("dd'/'MM'/'yyyy HH:mm:ss.fff000"));                  //Штамп времени
					sw.WriteLine(MainWindow.OscilList[NumGraphPanel()].OscilSampleRate);                     //Частота выборки (частота запуска осциллогрофа/ делитель)
					sw.WriteLine(MainWindow.OscilList[NumGraphPanel()].OscilHistotyCount);                   //Предыстория 
					sw.WriteLine(FileHeaderLine());                                                     //Формирование заголовка (подписи названия каналов)
					sw.WriteLine(FileColorsLine());
					for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].OscilEndSample; i++)            //Формирование строк всех загруженных данных (отсортированых с предысторией)
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
			else
			{
				StreamWriter sw;
				try
				{
					sw = File.CreateText(sfd.FileName);
					var index = NumGraphPanel();

					var window = sfd.FilterIndex == 2 ? new SaveToComtrade(MainWindow.OscilList[index], false) : new SaveToComtrade(MainWindow.OscilList[NumGraphPanel()], true);
					if (window.ShowDialog() == true)
					{
						var tempOscil = MainWindow.OscilList[index];

						string namefile;
						string pathfile;
						try
						{
							namefile = Path.GetFileNameWithoutExtension(sfd.FileName);
							pathfile = Path.GetDirectoryName(sfd.FileName);
						}
						catch
						{
							MessageBox.Show(@"Ошибка при создании файла!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						try
						{
							sw.WriteLine(Line1(sfd.FilterIndex, tempOscil));
							sw.WriteLine(Line2(tempOscil));

							for (int i = 0, j = 0; i < tempOscil.OscilChannelCount; i++)
							{
								if (!tempOscil.ChannelType[i]) { sw.WriteLine(Line3(i, j + 1, tempOscil)); j++; }
							}
							for (int i = 0, j = 0; i < tempOscil.OscilChannelCount; i++)
							{
								if (tempOscil.ChannelType[i]) { sw.WriteLine(Line4(i, j + 1, tempOscil)); j++; }
							}

							sw.WriteLine(Line5(tempOscil));
							sw.WriteLine(Line6(tempOscil));
							sw.WriteLine(Line7(tempOscil));
							sw.WriteLine(Line8(tempOscil));
							sw.WriteLine(Line9(tempOscil));
							sw.WriteLine(Line10(tempOscil));
							sw.WriteLine(Line11(tempOscil));
							if (sfd.FilterIndex == 3)
							{
								sw.WriteLine(Line12(tempOscil));
								sw.WriteLine(Line13(tempOscil));
							}
							sw.Close();

							string pathDateFile = pathfile + "\\" + namefile + @".dat";
							try
							{
								sw = File.CreateText(pathDateFile);
							}
							catch
							{
								MessageBox.Show(@"Ошибка при создании файла!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}

							try
							{

								for (int i = 0; i < tempOscil.OscilData.Count; i++)
								{
									string str = (i + 1) + ",";

									for (int j = 0; j < tempOscil.OscilChannelCount; j++)
									{
										if (!tempOscil.ChannelType[j])
										{
											str = str + "," + tempOscil.OscilData[i][j];
										}
									}

									sw.WriteLine(str);
								}
							}
							catch
							{
								MessageBox.Show(@"Ошибка при записи в файл!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}
						catch
						{
							MessageBox.Show(@"Ошибка при записи в файл!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}
				}
				catch
				{
					MessageBox.Show(@"Ошибка при создании файла!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				sw.Close();
			}
		}

		private string FileHeaderLine()
		{
			string str = " " + "\t";
			for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].OscilChannelCount; i++)
			{
				str = str + MainWindow.OscilList[NumGraphPanel()].ChannelNames[i] + "\t";
			}
			return str;
		}

		private string FileColorsLine()
		{
			string str = " " + "\t";
			for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].OscilChannelCount; i++)
			{
				str = str + MainWindow.OscilList[NumGraphPanel()].ChannelColor[i] + "\t";
			}
			return str;
		}

		private string FileParamLine(int numLine)
		{
			string str = numLine + "\t";
			for (int i = 0; i < MainWindow.OscilList[NumGraphPanel()].OscilData[numLine].Count; i++)
			{
				str = str + MainWindow.OscilList[NumGraphPanel()].OscilData[numLine][i] + "\t";
			}
			return str;
		}

		private string Line1(int filterIndex, Oscil oscil)
		{
			string stationName = oscil.OscilStationName;
			string recDevId = oscil.OscilRecordingDevice;
			string revYear = "";
			if (filterIndex == 2) revYear = "1999";
			if (filterIndex == 3) revYear = "2013";
			string str = stationName + "," + recDevId + "," + revYear;
			return str;
		}

		private string Line2(Oscil oscil)
		{
			int nA = 0, nD = 0;
			for (int i = 0; i < oscil.OscilChannelCount; i++)
			{
				//Если параметр в списке известных, то пишем его в файл
				if (!oscil.ChannelType[i]) nA += 1;
				if (oscil.ChannelType[i]) nD += 1;
			}
			int tt = nA + nD;
			string str = tt + "," + nA + "A," + nD + "D";
			return str;
		}

		private string Line3(int num, int nA, Oscil oscil)
		{
			string chId = oscil.ChannelNames[num];
			string ph = oscil.ChannelPhase[num];
			string ccbm = oscil.ChannelCcbm[num];
			string uu = oscil.ChannelDimension[num];
			string a = "1";
			string b = "0";
			int skew = 0;
			string min;
			string max;
			try
			{
				min = oscil.ChannelMin[num];
				max = oscil.ChannelMax[num];
			}
			catch
			{
				min = "0";
				max = "0";
			}

			int primary = 1;
			int secondary = 1;
			string ps = "P";

			string str = nA + "," + chId + "," + ph + "," + ccbm + "," + uu + "," + a + "," + b + "," + skew + "," +
						 min + "," + max + "," + primary + "," + secondary + "," + ps;

			return str;
		}

		private string Line4(int num, int nD, Oscil oscil)
		{
			string chId = oscil.ChannelNames[num];
			string ph = oscil.ChannelPhase[num];
			string ccbm = oscil.ChannelCcbm[num];
			int y = 0;

			string str = nD + "," + chId + "," + ph + "," + ccbm + "," + y;

			return str;
		}

		private string Line5(Oscil oscil)
		{
			return oscil.OscilNominalFrequency;
		}

		private string Line6(Oscil oscil)
		{
			return oscil.OscilNRates;
		}

		private string Line7(Oscil oscil)
		{
			string samp = oscil.OscilSampleRate.ToString("######");
			string endsamp = oscil.OscilEndSample.ToString();
			string str = samp + "," + endsamp;
			return str;
		}

		private string Line8(Oscil oscil)
		{
			//Время начала осциллограммы 
			DateTime dateTemp = oscil.OscilStampDateStart;
			return dateTemp.ToString("dd'/'MM'/'yyyy,HH:mm:ss.fff000");
		}

		private string Line9(Oscil oscil)
		{
			//Время срабатывания триггера
			DateTime dateTemp = oscil.OscilStampDateTrigger;
			return dateTemp.ToString("dd'/'MM'/'yyyy,HH:mm:ss.fff000");
		}

		private string Line10(Oscil oscil)
		{
			string ft = oscil.OscilFT;
			return ft;
		}

		private void tool_EnterLeft_label_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != ':') && (e.KeyChar != '.'))
			{
				e.Handled = true;
			}
		}

		private void tool_EnterRight_label_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != ':') && (e.KeyChar != '.'))
			{
				e.Handled = true;
			}
		}

		private void tool_HorizontEnter_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
			{
				e.Handled = true;
			}
		}

		private void tool_EnterLeft_label_TextChanged(object sender, EventArgs e)
		{
			PositionCursors(Cursor1, tool_EnterLeft_label.Text);
		}

		private void tool_EnterRight_label_TextChanged(object sender, EventArgs e)
		{
			PositionCursors(Cursor2, tool_EnterRight_label.Text);
		}

		private void toolStripButtonAutosizing_Click(object sender, EventArgs e)
		{
			toolStripButtonAutosizing.Checked = !toolStripButtonAutosizing.Checked;
			_autoScaling = toolStripButtonAutosizing.Checked;

			toolStripLabel_Top.Visible = !_autoScaling;
			toolStripLabel_Bottom.Visible = !_autoScaling;
			toolStripSeparator_Bottom1.Visible = !_autoScaling;
			toolStripSeparator_Bottom2.Visible = !_autoScaling;
			toolStripTextBox_Top.Visible = !_autoScaling;
			toolStripTextBox_Bottom.Visible = !_autoScaling;

			if (_autoScaling)
			{
				//Устанавливаем новую верхнюю и нижнюю границу для оси OY 
				Pane.YAxis.Scale.MinAuto = true;
				Pane.YAxis.Scale.MaxAuto = true;

				zedGraph.AxisChange();
				zedGraph.Invalidate();

				_maxYAxis = Pane.YAxis.Scale.Max;
				_minYAxis = Pane.YAxis.Scale.Min;

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (Pane.YAxis.Scale.Max == 0)
				{
					_maxYAxis = zedGraph.GraphPane.YAxis.Scale.Max +
					            (zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 10;
				}

				// ReSharper disable once CompareOfFloatsByEqualityOperator
				if (Pane.YAxis.Scale.Min == 0)
				{
					_minYAxis = zedGraph.GraphPane.YAxis.Scale.Min -
					            (zedGraph.GraphPane.YAxis.Scale.Max - zedGraph.GraphPane.YAxis.Scale.Min) / 10;
				}

				Pane.YAxis.Scale.Max = _maxYAxis;
				Pane.YAxis.Scale.Min = _minYAxis;
			}
			else
			{
				toolStripTextBox_Top.Text = _maxYAxis.ToString("F");
				toolStripTextBox_Bottom.Text = _minYAxis.ToString("F");
			}

			UpdateCursor();
			UpdateGraph();
			zedGraph.AxisChange();
			zedGraph.Invalidate();
		}

		private void toolStripTextBox_Top_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-'))
			{
				e.Handled = true;
			}
		}

		private void toolStripTextBox_Top_TextChanged(object sender, EventArgs e)
		{
			var position = toolStripTextBox_Top.Text;
			Regex regex = new Regex(@"^[-+]?([0-9]+)\,?([0-9]+)?$");
			var result = regex.IsMatch(position);
			if (result)
			{
				var val = Convert.ToDouble(position);
				if (val > _minYAxis)
				{
					zedGraph.AxisChange();
					zedGraph.Invalidate();

					Pane.YAxis.Scale.Max = val;

					_maxYAxis = val;

					UpdateCursor();
					UpdateGraph();
					zedGraph.AxisChange();
					zedGraph.Invalidate();
				} 
			}
		}



		private void toolStripTextBox_Bottom_TextChanged(object sender, EventArgs e)
		{
			var position = toolStripTextBox_Bottom.Text;
			Regex regex = new Regex(@"^[-+]?([0-9]+)\,?([0-9]+)?$");
			var result = regex.IsMatch(position);
			if (result)
			{
				var val = Convert.ToDouble(position);
				if (val < _maxYAxis)
				{
					zedGraph.AxisChange();
					zedGraph.Invalidate();

					Pane.YAxis.Scale.Min = val;

					_minYAxis = val;

					UpdateCursor();
					UpdateGraph();
					zedGraph.AxisChange();
					zedGraph.Invalidate();
				}
			}
		}

		private void toolStripTextBox_Bottom_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '-'))
			{
				e.Handled = true;
			}
		}

		private void tool_Horizont1Enter_TextChanged(object sender, EventArgs e)
		{
			var position = tool_Horizont1Enter.Text;
			Regex regex = new Regex(@"\d*\,?\d*");
			var result = regex.IsMatch(position);
			if (result)
			{
				if (new Regex(@"^\d*[^\,]$").IsMatch(position) || new Regex(@"^\d*\,\d+$").IsMatch(position))
				{
					CursorHorizontal1.Location.Y = Convert.ToDouble(position);
					UpdateCursor();
					zedGraph.AxisChange();
					zedGraph.Invalidate();
				}
			}
		}

		private void tool_Horizont2Enter_TextChanged(object sender, EventArgs e)
		{
			var position = tool_Horizont2Enter.Text;
			Regex regex = new Regex(@"\d*\,?\d*");
			var result = regex.IsMatch(position);
			if (result)
			{
				if (new Regex(@"^\d*[^\,]$").IsMatch(position) || new Regex(@"^\d*\,\d+$").IsMatch(position))
				{
					CursorHorizontal2.Location.Y = Convert.ToDouble(position);
					UpdateCursor();
					zedGraph.AxisChange();
					zedGraph.Invalidate();
				}
			}
		}

		private void PositionCursors(LineObj cursObj, string position)
		{
			Regex regex = _absOrRel ? new Regex(@"\d{1,2}:\d{1,2}:\d{1,2}\.?\d?") : new Regex(@"\d*\,?\d*");
			var result = regex.IsMatch(position);
			//Запуск метода изменения положения курсора
			if (result)
			{
				try
				{
					if (_absOrRel)
					{
						if (new Regex(@"^\d{0,2}:\d{0,2}:\d{0,2}\.?\d+$").IsMatch(position))
						{
							var numOsc = NumGraphPanel();
							var startTime = MainWindow.OscilList[numOsc].OscilStampDateStart;
							var currentHour = Convert.ToInt32(new Regex(@"^\d{1,2}").Match(position).ToString());
							var temp = new Regex(@"\:\d{1,2}\:").Match(position).ToString();
							var currentMin = Convert.ToInt32(new Regex(@"\d{1,2}").Match(temp).ToString());
							temp = new Regex(@"\:\d{0,2}\.?\d+$").Match(position).ToString();
							temp = new Regex(@"^[\:?]\d{0,2}([^\.])?").Match(temp).ToString();
							var currentSecond = Convert.ToInt32(new Regex(@"\d{1,2}").Match(temp).ToString());
							//Милесекунды 
							temp = new Regex(@"\.\d+$").Match(position).ToString();
							var currentMilisecond = Convert.ToInt32(temp != String.Empty ? new Regex(@"\d+").Match(temp).ToString() : "0");

							var currentDay = startTime.Hour > currentHour ? startTime.Day + 1 : startTime.Day;
							var currentMonth = startTime.Day > currentDay ? startTime.Month + 1 : startTime.Month;
							var currentYear = startTime.Month > currentMonth ? startTime.Year + 1 : startTime.Year;						
							var currentTime = new DateTime(currentYear, currentMonth, currentDay , currentHour, currentMin, currentSecond, currentMilisecond);
							var sec = (currentTime - startTime).TotalSeconds;
		
							cursObj.Location.X = Convert.ToDouble(sec);
							UpdateCursor();
							zedGraph.AxisChange();
							zedGraph.Invalidate();
						}
					}
					else
					{
						if (new Regex(@"^\d*[^\,]$").IsMatch(position) || new Regex(@"^\d*\,\d+$").IsMatch(position))
						{
							cursObj.Location.X = Convert.ToDouble(position);
							UpdateCursor();
							zedGraph.AxisChange();
							zedGraph.Invalidate();
						}
					}

					_oscilCursor.UpdateCursor(NumGraphPanel(), _absOrRel, _bind);
					if (PaneDig != null)
					{
						_oscilCursor.UpdateCursorDig(NumGraphPanel(), _absOrRel, _bind);
					}
				}
				catch 
				{
					//ignored
				}
			}
		}

		private string Line11(Oscil oscil)
		{
			string timemult = oscil.OscilTimemult;
			return timemult;
		}

		private string Line12(Oscil oscil)
		{
			string timecode = oscil.OscilTimeCode;
			string localcode = oscil.OscilLocalCode;
			return timecode + "," + localcode;
		}

		private string Line13(Oscil oscil)
		{
			string tmqCode = oscil.OscilTmqCode;
			string leapsec = oscil.OscilLeapsec;
			return tmqCode + "," + leapsec;
		}
	}
}

