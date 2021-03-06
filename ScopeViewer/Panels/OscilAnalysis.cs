﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Border = System.Windows.Controls.Border;
using Brushes = System.Windows.Media.Brushes;
using Label = System.Windows.Controls.Label;

namespace ScopeViewer
{
	public class OscilAnalysis
	{
		/// <summary>
		/// Interaction logic for Analysis.xaml
		/// </summary>
		public readonly List<DockPanel> LayoutPanel = new List<DockPanel>();

		private readonly Label _nameLabel = new Label();
		private readonly Border _panelBorder = new Border();

		private readonly List<Label> _nameChannelLabel = new List<Label>();
		private readonly List<Label> _nameStatuslLabel = new List<Label>();
		private readonly List<Label> _nameValue1Label = new List<Label>();
		private readonly List<Label> _nameValue2Label = new List<Label>();
		private readonly List<Label> _nameValueDeltLabel = new List<Label>();

		private readonly Label _nameLabelDig = new Label();
		private readonly Border _panelBorderDig = new Border();

		private readonly List<Label> _nameChannelLabelDig = new List<Label>();
		private readonly List<Label> _nameStatuslLabelDig = new List<Label>();
		private readonly List<Label> _nameValue1LabelDig = new List<Label>();
		private readonly List<Label> _nameValue2LabelDig = new List<Label>();
		private int _numerOscil;


		public void AnalysisCursorAdd(int numOsc, bool absOrRel, bool bind)
		{
			_numerOscil = numOsc;
			_panelBorder.BorderBrush = Brushes.DarkGray;
			_panelBorder.BorderThickness = new Thickness(1.0);
			_panelBorder.Margin = new Thickness(-225, 0, 0, 0);

			LayoutPanel.Add(new DockPanel());
			LayoutPanel[0].Width = 225;
			LayoutPanel[0].Height = 25;
			LayoutPanel[0].Margin = new Thickness(2, 5, 2, 0);
			LayoutPanel[0].MouseDown += click_LayoutPanelCursor;

			_nameLabel.Content = $"Курсоры: {MainWindow.OscilList[numOsc].OscilNames}";
			_nameLabel.VerticalAlignment = VerticalAlignment.Top;
			_nameLabel.FontSize = 12;
			_nameLabel.Height = 25;
			_nameLabel.Width = 225;
			_nameLabel.TabIndex = 0;
			_nameLabel.ToolTip = "Положения курсоров на осциллограмме:\n" + MainWindow.OscilList[numOsc].OscilNames;
			_nameLabel.Margin = new Thickness(0, 0, 0, 0);

			double positonY = 0;
			{
				_nameStatuslLabel.Add(new Label());
				_nameStatuslLabel[0].Content = "Красный         Синий              \u0394";
				// _nameStatuslLabel[0].Foreground = Brushes.Red;
				_nameStatuslLabel[0].VerticalAlignment = VerticalAlignment.Top;
				_nameStatuslLabel[0].FontSize = 12;
				_nameStatuslLabel[0].Height = 25;
				_nameStatuslLabel[0].Width = 225;
				_nameStatuslLabel[0].ToolTip = "Положние курсоров";
				positonY += 20;
				_nameStatuslLabel[0].Margin = new Thickness(-225, positonY, 0, 0);
			}

			{
				_nameStatuslLabel.Add(new Label());
				double x = MainWindow.GraphPanelList[numOsc].Cursor1.Location.X;
				_nameStatuslLabel[1].VerticalAlignment = VerticalAlignment.Top;
				_nameStatuslLabel[1].Foreground = Brushes.Red;
				_nameStatuslLabel[1].FontSize = 12;
				_nameStatuslLabel[1].Height = 25;
				_nameStatuslLabel[1].Width = 75;
				_nameStatuslLabel[1].Content = x.ToString("F6") + " сек";
				_nameStatuslLabel[1].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
				(int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
				positonY += 20;
				_nameStatuslLabel[1].Margin = new Thickness(-375, positonY, 0, 0);
			}

			{
				_nameStatuslLabel.Add(new Label());
				double x = MainWindow.GraphPanelList[numOsc].Cursor2.Location.X;
				_nameStatuslLabel[2].VerticalAlignment = VerticalAlignment.Top;
				_nameStatuslLabel[2].Foreground = Brushes.Blue;
				_nameStatuslLabel[2].FontSize = 12;
				_nameStatuslLabel[2].Height = 25;
				_nameStatuslLabel[2].Width = 75;
				_nameStatuslLabel[2].Content = x.ToString("F6") + " сек";
				_nameStatuslLabel[2].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
				(int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
				_nameStatuslLabel[2].Margin = new Thickness(-225, positonY, 0, 0);
			}

			for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
			{
				var visible = MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Label.IsVisible;

				_nameChannelLabel.Add(new Label());
				_nameChannelLabel[j].Content = MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Label.Text;
				_nameChannelLabel[j].VerticalAlignment = VerticalAlignment.Top;
				_nameChannelLabel[j].FontSize = 12;
				_nameChannelLabel[j].Height = 25;
				_nameChannelLabel[j].Width = 225;
				_nameChannelLabel[j].ToolTip = "Название канала";
				_nameChannelLabel[j].Visibility = visible ? Visibility.Visible:Visibility.Hidden;
				positonY += visible ? 20 : 0;
				double positonX = visible ? -225 : 0;
				_nameChannelLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);

				double str1 = 0;
				double str2 = 0;
				double str3;

				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
						MainWindow.GraphPanelList[numOsc].Cursor1.Location.X)
					{
						str1 = MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y;
						break;
					}
				}
				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
						MainWindow.GraphPanelList[numOsc].Cursor2.Location.X)
					{
						str2 = MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y;
						break;
					}
				}

				str3 = Math.Abs(str2 - str1);


				_nameValue1Label.Add(new Label());
				_nameValue1Label[j].Content = str1.ToString("F3");
				_nameValue1Label[j].VerticalAlignment = VerticalAlignment.Top;
				_nameValue1Label[j].Foreground = Brushes.Red;
				_nameValue1Label[j].FontSize = 12;
				_nameValue1Label[j].Height = 25;
				_nameValue1Label[j].Width = 75;
				_nameValue1Label[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonY += visible ? 20 : 0;
				positonX = visible ? -375 : 0;
				_nameValue1Label[j].Margin = new Thickness(positonX, positonY, 0, 0);

				_nameValue2Label.Add(new Label());
				_nameValue2Label[j].Content = str2.ToString("F3");
				_nameValue2Label[j].VerticalAlignment = VerticalAlignment.Top;
				_nameValue2Label[j].Foreground = Brushes.Blue;
				_nameValue2Label[j].FontSize = 12;
				_nameValue2Label[j].Height = 25;
				_nameValue2Label[j].Width = 75;
				_nameValue2Label[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonX = visible ? -225 : 0;
				_nameValue2Label[j].Margin = new Thickness(positonX, positonY, 0, 0);

				_nameValueDeltLabel.Add(new Label());
				_nameValueDeltLabel[j].Content = str3.ToString("F3");
				_nameValueDeltLabel[j].VerticalAlignment = VerticalAlignment.Top;
				_nameValueDeltLabel[j].Foreground = Brushes.Blue;
				_nameValueDeltLabel[j].FontSize = 12;
				_nameValueDeltLabel[j].Height = 25;
				_nameValueDeltLabel[j].Width = 75;
				_nameValueDeltLabel[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonX = visible ? -70 : 0;
				_nameValueDeltLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);
			}

			LayoutPanel[0].Children.Add(_nameLabel);
			LayoutPanel[0].Children.Add(_nameStatuslLabel[0]);
			LayoutPanel[0].Children.Add(_nameStatuslLabel[1]);
			LayoutPanel[0].Children.Add(_nameStatuslLabel[2]);

			for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
			{
				LayoutPanel[0].Children.Add(_nameChannelLabel[j]);
				LayoutPanel[0].Children.Add(_nameValue1Label[j]);
				LayoutPanel[0].Children.Add(_nameValue2Label[j]);
				LayoutPanel[0].Children.Add(_nameValueDeltLabel[j]);
			}

			LayoutPanel[0].Children.Add(_panelBorder);

			UpdateCursor(numOsc, absOrRel, bind);
		}

		//Добавляем панель анализа для цифрового канала 
		public void AnalysisCursorAddDig(int numOsc, bool absOrRel, bool bind)
		{
			_panelBorderDig.BorderBrush = Brushes.DarkGray;
			_panelBorderDig.BorderThickness = new Thickness(1.0);
			_panelBorderDig.Margin = new Thickness(-225, 0, 0, 0);

			LayoutPanel.Add(new DockPanel());
			LayoutPanel[1].Width = 225;
			LayoutPanel[1].Height = 25;
			LayoutPanel[1].Margin = new Thickness(2, 5, 2, 0);
			LayoutPanel[1].MouseDown += click_LayoutPanelCursorDig;

			_nameLabelDig.Content = $"Цифровые курсоры: {MainWindow.OscilList[numOsc].OscilNames}";
			_nameLabelDig.VerticalAlignment = VerticalAlignment.Top;
			_nameLabelDig.FontSize = 12;
			_nameLabelDig.Height = 25;
			_nameLabelDig.Width = 180;
			_nameLabelDig.TabIndex = 0;
			_nameLabelDig.ToolTip = "Положение курсоров в дискретном канале:\n" + MainWindow.OscilList[numOsc].OscilNames;
			_nameLabelDig.Margin = new Thickness(0, 0, 0, 0);

			double positonY = 0;
			{
				_nameStatuslLabelDig.Add(new Label());
				double x = MainWindow.GraphPanelList[numOsc].CursorDig1.Location.X;
				_nameStatuslLabelDig[0].Content = x.ToString("F1") + " сек";
				_nameStatuslLabelDig[0].VerticalAlignment = VerticalAlignment.Top;
				_nameStatuslLabelDig[0].Foreground = Brushes.Red;
				_nameStatuslLabelDig[0].FontSize = 12;
				_nameStatuslLabelDig[0].Height = 25;
				_nameStatuslLabelDig[0].Width = 75;
				_nameStatuslLabelDig[0].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
					(int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
				positonY += 20;
				_nameStatuslLabelDig[0].Margin = new Thickness(-125, positonY, 0, 0);
			}

			{
				_nameStatuslLabelDig.Add(new Label());
				double x = MainWindow.GraphPanelList[numOsc].CursorDig2.Location.X;
				_nameStatuslLabelDig[1].Content = x.ToString("F1") + " сек";
				_nameStatuslLabelDig[1].VerticalAlignment = VerticalAlignment.Top;
				_nameStatuslLabelDig[1].Foreground = Brushes.Blue;
				_nameStatuslLabelDig[1].FontSize = 12;
				_nameStatuslLabelDig[1].Height = 25;
				_nameStatuslLabelDig[1].Width = 75;
				_nameStatuslLabelDig[1].ToolTip = (int)x + " сек " + (int)((x - (int)x) * 1000) + " мс " +
					(int)((x * 1000 - (int)(x * 1000)) * 1000) + " мкс " + (int)((x * 1000000 - (int)(x * 1000000)) * 1000) + " нс";
				_nameStatuslLabelDig[1].Margin = new Thickness(-30, positonY, 0, 0);
			}

			{
				_nameStatuslLabelDig.Add(new Label());
				_nameStatuslLabelDig[2].Content = "Канал:              Значения:";
				_nameStatuslLabelDig[2].VerticalAlignment = VerticalAlignment.Top;
				_nameStatuslLabelDig[2].FontSize = 12;
				_nameStatuslLabelDig[2].Height = 25;
				_nameStatuslLabelDig[2].Width = 200;
				positonY += 20;
				_nameStatuslLabelDig[2].Margin = new Thickness(-250, positonY, 0, 0);
			}

			{
				_nameChannelLabelDig.Add(new Label());
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].Content = MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[2].Label.Text;
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].VerticalAlignment = VerticalAlignment.Top;
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].FontSize = 12;
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].Height = 25;
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].Width = 90;
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].ToolTip = "Название канала";
				positonY += 20;
				_nameChannelLabelDig[_nameChannelLabelDig.Count - 1].Margin = new Thickness(-350, positonY, 0, 0);

				string str = "";
				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[_nameChannelLabelDig.Count - 1].NPts; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[_nameChannelLabelDig.Count - 1].Points[k].X >
						MainWindow.GraphPanelList[numOsc].CursorDig1.Location.X)
					{
						str = MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[2].Points[k].Y.ToString("####");
						break;
					}
				}

				_nameValue1LabelDig.Add(new Label());
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].Content = str;
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].VerticalAlignment = VerticalAlignment.Top;
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].Foreground = Brushes.Red;
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].FontSize = 12;
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].Height = 25;
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].Width = 75;
				_nameValue1LabelDig[_nameChannelLabelDig.Count - 1].Margin = new Thickness(-160, positonY, 0, 0);

				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[_nameChannelLabelDig.Count - 1].NPts; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[_nameChannelLabelDig.Count - 1].Points[k].X >
						MainWindow.GraphPanelList[numOsc].CursorDig2.Location.X)
					{
						str = MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[2].Points[k].Y.ToString("####");
						break;
					}
				}

				_nameValue2LabelDig.Add(new Label());
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].Content = str;
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].VerticalAlignment = VerticalAlignment.Top;
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].Foreground = Brushes.Blue;
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].FontSize = 12;
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].Height = 25;
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].Width = 75;
				_nameValue2LabelDig[_nameChannelLabelDig.Count - 1].Margin = new Thickness(-50, positonY, 0, 0);

			}
			for (int j = 0; j < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList.Count - 3; j++)
			{
				_nameChannelLabelDig.Add(new Label());
				_nameChannelLabelDig[j + 1].Content = j;
				_nameChannelLabelDig[j + 1].VerticalAlignment = VerticalAlignment.Top;
				_nameChannelLabelDig[j + 1].FontSize = 12;
				_nameChannelLabelDig[j + 1].Height = 25;
				_nameChannelLabelDig[j + 1].Width = 100;
				_nameChannelLabelDig[j + 1].ToolTip = "Бит канала";
				positonY += 20;
				_nameChannelLabelDig[j + 1].Margin = new Thickness(-350, positonY, 0, 0);

				string str = "";

				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j + 3].NPts; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j + 3].Points[k].X >
						MainWindow.GraphPanelList[numOsc].CursorDig1.Location.X)
					{
						// ReSharper disable once CompareOfFloatsByEqualityOperator
						str = (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j + 3].Points[k].Y + 0.2) % 1 == 0 ? "1" : "0";
						break;
					}
				}

				_nameValue1LabelDig.Add(new Label());
				_nameValue1LabelDig[j + 1].Content = str;
				_nameValue1LabelDig[j + 1].VerticalAlignment = VerticalAlignment.Top;
				_nameValue1LabelDig[j + 1].Foreground = Brushes.Red;
				_nameValue1LabelDig[j + 1].FontSize = 12;
				_nameValue1LabelDig[j + 1].Height = 25;
				_nameValue1LabelDig[j + 1].Width = 85;
				_nameValue1LabelDig[j + 1].Margin = new Thickness(-150, positonY, 0, 0);

				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j + 3].NPts; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j + 3].Points[k].X >
						MainWindow.GraphPanelList[numOsc].CursorDig2.Location.X)
					{
						// ReSharper disable once CompareOfFloatsByEqualityOperator
						str = (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j + 3].Points[k].Y + 0.2) % 1 == 0 ? "1" : "0";
						break;
					}
				}

				_nameValue2LabelDig.Add(new Label());
				_nameValue2LabelDig[j + 1].Content = str;
				_nameValue2LabelDig[j + 1].VerticalAlignment = VerticalAlignment.Top;
				_nameValue2LabelDig[j + 1].Foreground = Brushes.Blue;
				_nameValue2LabelDig[j + 1].FontSize = 12;
				_nameValue2LabelDig[j + 1].Height = 25;
				_nameValue2LabelDig[j + 1].Width = 85;
				_nameValue2LabelDig[j + 1].Margin = new Thickness(-50, positonY, 0, 0);
			}

			LayoutPanel[1].Children.Add(_nameLabelDig);
			LayoutPanel[1].Children.Add(_nameStatuslLabelDig[0]);
			LayoutPanel[1].Children.Add(_nameStatuslLabelDig[1]);
			LayoutPanel[1].Children.Add(_nameStatuslLabelDig[2]);

			for (int j = 0; j < _nameChannelLabelDig.Count; j++)
			{
				LayoutPanel[1].Children.Add(_nameChannelLabelDig[j]);
				LayoutPanel[1].Children.Add(_nameValue1LabelDig[j]);
				LayoutPanel[1].Children.Add(_nameValue2LabelDig[j]);
			}

			LayoutPanel[1].Children.Add(_panelBorderDig);

			UpdateCursorDig(numOsc, absOrRel, bind);
		}


		public void AnalysisCursorClear()
		{
			for (int j = _nameChannelLabel.Count - 1; j >= 0; j--)
			{
				LayoutPanel[0].Children.Remove(_nameChannelLabel[j]);
				LayoutPanel[0].Children.Remove(_nameValue1Label[j]);

				if (_nameValue2Label.Count != 0)
				{
					LayoutPanel[0].Children.Remove(_nameValue2Label[j]);
				}

				if (_nameValueDeltLabel.Count != 0)
				{
					LayoutPanel[0].Children.Remove(_nameValueDeltLabel[j]);
				}
			}

			for (int i = 2; i >= 0; i--)
			{
				LayoutPanel[0].Children.Remove(_nameStatuslLabel[i]);
			}

			LayoutPanel[0].Children.Remove(_nameLabel);
			LayoutPanel[0].Children.Remove(_panelBorder);

			_nameStatuslLabel.Remove(_nameStatuslLabel[2]);
			_nameStatuslLabel.Remove(_nameStatuslLabel[1]);
			_nameStatuslLabel.Remove(_nameStatuslLabel[0]);

			_nameChannelLabel.Clear();
			_nameValue1Label.Clear();
			_nameValue2Label.Clear();
			_nameValueDeltLabel.Clear();

			LayoutPanel.Clear();
		}

		public void AnalysisCursorClearDig()
		{
			LayoutPanel[1].Children.Remove(_nameLabelDig);
			LayoutPanel[1].Children.Remove(_panelBorderDig);

			_nameStatuslLabelDig.Remove(_nameStatuslLabelDig[2]);
			_nameStatuslLabelDig.Remove(_nameStatuslLabelDig[1]);
			_nameStatuslLabelDig.Remove(_nameStatuslLabelDig[0]);

			_nameChannelLabelDig.Clear();
			_nameValue1LabelDig.Clear();
			_nameValue2LabelDig.Clear();
		}

		public void UpdateCursor(int numOsc, bool absOrRel, bool bind)
		{
			if (_numerOscil != numOsc) _numerOscil = numOsc;
			var positonY = 40;

			for (int j = 0; j < MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count; j++)
			{
				var visible = MainWindow.GraphPanelList[numOsc].Pane.CurveList[j].Label.IsVisible;

				double str1 = 0;
				double str2 = 0;

				_nameChannelLabel[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonY += visible ? 20 : 0;
				var positonX = visible ? -225 : 0;
				_nameChannelLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);

				_nameValue1Label[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonY += visible ? 20 : 0;
				positonX = visible ? -375 : 0;
				_nameValue1Label[j].Margin = new Thickness(positonX, positonY, 0, 0);

				_nameValue2Label[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonX = visible ? -225 : 0;
				_nameValue2Label[j].Margin = new Thickness(positonX, positonY, 0, 0);

				_nameValueDeltLabel[j].Visibility = visible ? Visibility.Visible : Visibility.Hidden;
				positonX = visible ? -70 : 0;
				_nameValueDeltLabel[j].Margin = new Thickness(positonX, positonY, 0, 0);

				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
						MainWindow.GraphPanelList[numOsc].Cursor1.Location.X)
					{
						var index = k != 0 ? k - 1 : 0;
					    str1 = MainWindow.GraphPanelList[numOsc].ListTemp[j][index].Y;
                        //if (bind)
						//else str1 = (MainWindow.GraphPanelList[numOsc].Cursor1.Location.X -
						//             MainWindow.GraphPanelList[numOsc].ListTemp[j][index].X) /
						//            (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X -
						//             MainWindow.GraphPanelList[numOsc].ListTemp[j][index].X) *
						//            (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y -
						//             MainWindow.GraphPanelList[numOsc].ListTemp[j][index].Y) +
						//            MainWindow.GraphPanelList[numOsc].ListTemp[j][index].Y;
						_nameValue1Label[j].Content = str1.ToString("F3");
						break;
					}
				}
				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].ListTemp[j].Count; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X >
						MainWindow.GraphPanelList[numOsc].Cursor2.Location.X)
					{
						var index = k != 0 ? k - 1 : 0;
					    str2 = MainWindow.GraphPanelList[numOsc].ListTemp[j][index].Y;
      //                  if (bind) 
						//else str2 = (MainWindow.GraphPanelList[numOsc].Cursor2.Location.X -
						//	        MainWindow.GraphPanelList[numOsc].ListTemp[j][index].X) /
						//	       (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].X -
						//	        MainWindow.GraphPanelList[numOsc].ListTemp[j][index].X) *
						//	       (MainWindow.GraphPanelList[numOsc].ListTemp[j][k].Y -
						//	        MainWindow.GraphPanelList[numOsc].ListTemp[j][index].Y) +
						//	        MainWindow.GraphPanelList[numOsc].ListTemp[j][index].Y;
						_nameValue2Label[j].Content = str2.ToString("F3");
						break;
					}
				}
				_nameValueDeltLabel[j].Content = Math.Abs(str2 - str1).ToString("F3");
			}

			double x1 = MainWindow.GraphPanelList[numOsc].Cursor1.Location.X;
			double x2 = MainWindow.GraphPanelList[numOsc].Cursor2.Location.X;

			if (absOrRel)
			{
				//Abs time
				_nameStatuslLabel[1].Content =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Millisecond.ToString("D3");
				_nameStatuslLabel[1].ToolTip =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Millisecond.ToString("D3");

				_nameStatuslLabel[2].Content =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Millisecond.ToString("D3");
				_nameStatuslLabel[2].ToolTip =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Millisecond.ToString("D3");
			}
			else
			{
				//Relativ time
				_nameStatuslLabel[1].Content = x1.ToString("F6") + " сек";
				_nameStatuslLabel[1].ToolTip = (int)x1 + " сек " + (int)((x1 - (int)x1) * 1000) + " мс " +
											   (int)((x1 * 1000 - (int)(x1 * 1000)) * 1000) + " мкс " +
											   (int)((x1 * 1000000 - (int)(x1 * 1000000)) * 1000) + " нс";
				_nameStatuslLabel[2].Content = x2.ToString("F6") + " сек";
				_nameStatuslLabel[2].ToolTip = (int)x2 + " сек " + (int)((x2 - (int)x2) * 1000) + " мс " +
											   (int)((x2 * 1000 - (int)(x2 * 1000)) * 1000) + " мкс " +
											   (int)((x2 * 1000000 - (int)(x2 * 1000000)) * 1000) + " нс";
			}

			LayoutPanel[0].Height = 25 + 20 * (MainWindow.GraphPanelList[numOsc].Pane.CurveList.Count(x => x.IsVisible) + 1) * 2;

		}

		public void UpdateCursorDig(int numOsc, bool absOrRel, bool bind)
		{
			for (int j = 2; j < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList.Count; j++)
			{
				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].NPts; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[k].X >
						MainWindow.GraphPanelList[numOsc].CursorDig1.Location.X)
					{
						if (j == 2)
						{
							var index = k != 0 ? k - 1 : 0;
							var str1 = MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[index].Y.ToString(CultureInfo.InvariantCulture);
							_nameValue1LabelDig[j - 2].Content = str1;
						}
						else
						{
							var index = k != 0 ? k - 1 : 0;
							var str = Math.Abs((MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[k].Y + 0.2) % 1) < 0.01 ? "1" : "0";
    						if (bind)
							{
								str = Math.Abs((MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[index].Y + 0.2) % 1) < 0.01 ? "1" : "0";
    						}
							_nameValue1LabelDig[j - 2].Content = str;
						}
						break;
					}
				}
			}

			double x1 = MainWindow.GraphPanelList[numOsc].CursorDig1.Location.X;

			if (absOrRel)
			{
				//Abs time
				_nameStatuslLabelDig[0].Content =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Millisecond.ToString("D3");
				_nameStatuslLabelDig[0].ToolTip =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x1 * 1000).Millisecond.ToString("D3");
			}
			else
			{
				//Relativ time
				_nameStatuslLabelDig[0].Content = x1.ToString("F1") + " сек";
				_nameStatuslLabelDig[0].ToolTip = (int)x1 + " сек " + (int)((x1 - (int)x1) * 1000) + " мс " +
												  (int)((x1 * 1000 - (int)(x1 * 1000)) * 1000) + " мкс " +
												  (int)((x1 * 1000000 - (int)(x1 * 1000000)) * 1000) + " нс";
			}

			for (int j = 2; j < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList.Count; j++)
			{
				for (int k = 0; k < MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].NPts; k++)
				{
					if (MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[k].X >
						MainWindow.GraphPanelList[numOsc].CursorDig2.Location.X)
					{
						if (j == 2)
						{
							var index = k != 0 ? k - 1 : 0;
						    var str1 = MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[index].Y.ToString(CultureInfo.InvariantCulture);
                            _nameValue2LabelDig[j - 2].Content = str1;
						}
						else
						{
							var index = k != 0 ? k - 1 : 0;

						    var str = Math.Abs((MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[k].Y + 0.2) % 1) < 0.01 ? "1" : "0";
							if (bind)
							{
							    str = Math.Abs((MainWindow.GraphPanelList[numOsc].PaneDig.CurveList[j].Points[index].Y + 0.2) % 1) < 0.01 ? "1" : "0";
							}
                            _nameValue2LabelDig[j - 2].Content = str;
						}
						break;
					}
				}
			}

			double x2 = MainWindow.GraphPanelList[numOsc].CursorDig2.Location.X;
			if (absOrRel)
			{
				//Abs time
				_nameStatuslLabelDig[1].Content =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Millisecond.ToString("D3");
				_nameStatuslLabelDig[1].ToolTip =
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Hour.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Minute.ToString("D2") + ":" +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Second.ToString("D2") + "." +
					MainWindow.OscilList[numOsc].OscilStampDateStart.AddMilliseconds(x2 * 1000).Millisecond.ToString("D3");
			}
			else
			{
				//Relativ time
				_nameStatuslLabelDig[1].Content = x2.ToString("F1") + " сек";
				_nameStatuslLabelDig[1].ToolTip = (int)x2 + " сек " + (int)((x2 - (int)x2) * 1000) + " мс " +
												  (int)((x2 * 1000 - (int)(x2 * 1000)) * 1000) + " мкс " +
												  (int)((x2 * 1000000 - (int)(x2 * 1000000)) * 1000) + " нс";
			}
		}

		private void click_LayoutPanelCursor(object sender, MouseButtonEventArgs e)
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			var countChannelVisible = MainWindow.GraphPanelList[_numerOscil].Pane.CurveList.Count(x=>x.IsVisible); //[j].Label.IsVisible;
			LayoutPanel[0].Height = Math.Abs(LayoutPanel[0].Height - 25) < 0.01 ? 25 + 20 * (countChannelVisible + 1) * 2 : 25;
		}

		private void click_LayoutPanelCursorDig(object sender, MouseButtonEventArgs e)
		{
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			LayoutPanel[1].Height = LayoutPanel[1].Height == 25 ? 25 + 20 * (_nameChannelLabelDig.Count + 2) : 25;
		}
	}
}