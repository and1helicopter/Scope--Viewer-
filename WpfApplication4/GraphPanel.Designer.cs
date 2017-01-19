using System.Windows.Forms;

namespace ScopeViewer
{
    public sealed partial class GraphPanel
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Mask1_label = new System.Windows.Forms.ToolStripLabel();
            this.MaskMax_textBox = new System.Windows.Forms.ToolStripTextBox();
            this.Mask2_label = new System.Windows.Forms.ToolStripLabel();
            this.MaskMin_textBox = new System.Windows.Forms.ToolStripTextBox();
            this.panel = new System.Windows.Forms.Panel();
            this.Mask_listView = new System.Windows.Forms.ListView();
            this.Names = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StampTime_label = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.AddCursor = new System.Windows.Forms.ToolStripButton();
            this.StampTrigger = new System.Windows.Forms.ToolStripButton();
            this.ScaleButton = new System.Windows.Forms.ToolStripButton();
            this.AutoRange_Button = new System.Windows.Forms.ToolStripButton();
            this.HidePanel_button = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraph
            // 
            this.zedGraph.AutoSize = true;
            this.zedGraph.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.zedGraph.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zedGraph.Cursor = System.Windows.Forms.Cursors.Default;
            this.zedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zedGraph.IsShowPointValues = true;
            this.zedGraph.IsSynchronizeXAxes = true;
            this.zedGraph.Location = new System.Drawing.Point(175, 33);
            this.zedGraph.Margin = new System.Windows.Forms.Padding(0);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(634, 438);
            this.zedGraph.TabIndex = 0;
            this.zedGraph.UseExtendedPrintDialog = true;
            this.zedGraph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseClick);
            this.zedGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseMove);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddCursor,
            this.StampTrigger,
            this.ScaleButton,
            this.toolStripSeparator1,
            this.Mask1_label,
            this.MaskMax_textBox,
            this.Mask2_label,
            this.MaskMin_textBox,
            this.AutoRange_Button,
            this.StampTime_label,
            this.toolStripSeparator2,
            this.HidePanel_button});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(809, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // Mask1_label
            // 
            this.Mask1_label.Name = "Mask1_label";
            this.Mask1_label.Size = new System.Drawing.Size(21, 30);
            this.Mask1_label.Text = "От";
            // 
            // MaskMax_textBox
            // 
            this.MaskMax_textBox.Name = "MaskMax_textBox";
            this.MaskMax_textBox.Size = new System.Drawing.Size(20, 33);
            this.MaskMax_textBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MaskMax_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mask_KeyPress);
            this.MaskMax_textBox.TextChanged += new System.EventHandler(this.MaskMax_textBox_TextChanged);
            // 
            // Mask2_label
            // 
            this.Mask2_label.Name = "Mask2_label";
            this.Mask2_label.Size = new System.Drawing.Size(20, 30);
            this.Mask2_label.Text = "до";
            // 
            // MaskMin_textBox
            // 
            this.MaskMin_textBox.Name = "MaskMin_textBox";
            this.MaskMin_textBox.Size = new System.Drawing.Size(20, 33);
            this.MaskMin_textBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MaskMin_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mask_KeyPress);
            this.MaskMin_textBox.TextChanged += new System.EventHandler(this.MaskMin_textBox_TextChanged);
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.BackColor = System.Drawing.SystemColors.Window;
            this.panel.Controls.Add(this.Mask_listView);
            this.panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel.Location = new System.Drawing.Point(0, 33);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(5, 10, 5, 40);
            this.panel.Size = new System.Drawing.Size(175, 438);
            this.panel.TabIndex = 2;
            // 
            // Mask_listView
            // 
            this.Mask_listView.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Mask_listView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Mask_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Names,
            this.Value1});
            this.Mask_listView.Cursor = System.Windows.Forms.Cursors.Default;
            this.Mask_listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Mask_listView.Font = new System.Drawing.Font("Linux Libertine G", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Mask_listView.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Mask_listView.FullRowSelect = true;
            this.Mask_listView.GridLines = true;
            this.Mask_listView.Location = new System.Drawing.Point(5, 10);
            this.Mask_listView.MultiSelect = false;
            this.Mask_listView.Name = "Mask_listView";
            this.Mask_listView.Size = new System.Drawing.Size(165, 388);
            this.Mask_listView.TabIndex = 0;
            this.Mask_listView.TileSize = new System.Drawing.Size(168, 60);
            this.Mask_listView.UseCompatibleStateImageBehavior = false;
            this.Mask_listView.View = System.Windows.Forms.View.Details;
            // 
            // Names
            // 
            this.Names.Text = "Name";
            this.Names.Width = 75;
            // 
            // Value1
            // 
            this.Value1.Text = "Курсор";
            this.Value1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Value1.Width = 75;
            // 
            // StampTime_label
            // 
            this.StampTime_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.StampTime_label.Name = "StampTime_label";
            this.StampTime_label.Size = new System.Drawing.Size(68, 30);
            this.StampTime_label.Text = "StampTime";
            this.StampTime_label.ToolTipText = "Штамп времени";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // AddCursor
            // 
            this.AddCursor.AutoSize = false;
            this.AddCursor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AddCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddCursor.Image = global::ScopeViewer.Properties.Resources.Line_48_2_;
            this.AddCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddCursor.Margin = new System.Windows.Forms.Padding(1);
            this.AddCursor.Name = "AddCursor";
            this.AddCursor.Size = new System.Drawing.Size(30, 30);
            this.AddCursor.Text = "Добавить курсоры";
            this.AddCursor.ToolTipText = "Добавить курсоры ";
            this.AddCursor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AddCoursor_MouseDown);
            // 
            // StampTrigger
            // 
            this.StampTrigger.AutoSize = false;
            this.StampTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StampTrigger.Image = global::ScopeViewer.Properties.Resources.Horizontal_Line_48_2_;
            this.StampTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StampTrigger.Name = "StampTrigger";
            this.StampTrigger.Size = new System.Drawing.Size(30, 30);
            this.StampTrigger.Text = "Штамп времени ";
            this.StampTrigger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StampTrigger_MouseDown);
            // 
            // ScaleButton
            // 
            this.ScaleButton.AutoSize = false;
            this.ScaleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ScaleButton.Image = global::ScopeViewer.Properties.Resources.Width_48;
            this.ScaleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScaleButton.Margin = new System.Windows.Forms.Padding(0);
            this.ScaleButton.Name = "ScaleButton";
            this.ScaleButton.Size = new System.Drawing.Size(30, 30);
            this.ScaleButton.Text = "Изменить размер";
            this.ScaleButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.ScaleButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScaleButton_MouseDown);
            // 
            // AutoRange_Button
            // 
            this.AutoRange_Button.AutoSize = false;
            this.AutoRange_Button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AutoRange_Button.Image = global::ScopeViewer.Properties.Resources.Available_Updates_48;
            this.AutoRange_Button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AutoRange_Button.Name = "AutoRange_Button";
            this.AutoRange_Button.Size = new System.Drawing.Size(30, 30);
            this.AutoRange_Button.Text = "Сбросить";
            this.AutoRange_Button.Click += new System.EventHandler(this.AutoRange_Button_Click);
            // 
            // HidePanel_button
            // 
            this.HidePanel_button.AutoSize = false;
            this.HidePanel_button.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HidePanel_button.Image = global::ScopeViewer.Properties.Resources.Show_Property_48_1_;
            this.HidePanel_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HidePanel_button.Name = "HidePanel_button";
            this.HidePanel_button.Size = new System.Drawing.Size(30, 30);
            this.HidePanel_button.Text = "Спрятать боковую панель";
            this.HidePanel_button.ToolTipText = "Спрятать боковую панель";
            this.HidePanel_button.Click += new System.EventHandler(this.HidePanel_button_Click);
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(809, 471);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ZedGraph.ZedGraphControl zedGraph;
        private ToolStrip toolStrip1;
        private Panel panel;
        private ToolStripButton AddCursor;
        private ToolStripButton StampTrigger;
        private ToolStripButton ScaleButton;
        private ColumnHeader Names;
        private ColumnHeader Value1;
        private ListView Mask_listView;
        private ToolStripTextBox MaskMin_textBox;
        private ToolStripButton AutoRange_Button;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel Mask1_label;
        private ToolStripTextBox MaskMax_textBox;
        private ToolStripLabel Mask2_label;
        private ToolStripLabel StampTime_label;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton HidePanel_button;
    }
}

