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
            System.Windows.Forms.ListViewItem listViewItem133 = new System.Windows.Forms.ListViewItem("q2e");
            System.Windows.Forms.ListViewItem listViewItem134 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "1233234"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204))));
            System.Windows.Forms.ListViewItem listViewItem135 = new System.Windows.Forms.ListViewItem("2");
            System.Windows.Forms.ListViewItem listViewItem136 = new System.Windows.Forms.ListViewItem("3");
            System.Windows.Forms.ListViewItem listViewItem137 = new System.Windows.Forms.ListViewItem("4");
            System.Windows.Forms.ListViewItem listViewItem138 = new System.Windows.Forms.ListViewItem("5");
            System.Windows.Forms.ListViewItem listViewItem139 = new System.Windows.Forms.ListViewItem("6");
            System.Windows.Forms.ListViewItem listViewItem140 = new System.Windows.Forms.ListViewItem("7");
            System.Windows.Forms.ListViewItem listViewItem141 = new System.Windows.Forms.ListViewItem("8");
            System.Windows.Forms.ListViewItem listViewItem142 = new System.Windows.Forms.ListViewItem("9");
            System.Windows.Forms.ListViewItem listViewItem143 = new System.Windows.Forms.ListViewItem("10");
            System.Windows.Forms.ListViewItem listViewItem144 = new System.Windows.Forms.ListViewItem("11");
            System.Windows.Forms.ListViewItem listViewItem145 = new System.Windows.Forms.ListViewItem("12");
            System.Windows.Forms.ListViewItem listViewItem146 = new System.Windows.Forms.ListViewItem("13");
            System.Windows.Forms.ListViewItem listViewItem147 = new System.Windows.Forms.ListViewItem("14");
            System.Windows.Forms.ListViewItem listViewItem148 = new System.Windows.Forms.ListViewItem("15");
            System.Windows.Forms.ListViewItem listViewItem149 = new System.Windows.Forms.ListViewItem("16");
            System.Windows.Forms.ListViewItem listViewItem150 = new System.Windows.Forms.ListViewItem("17");
            System.Windows.Forms.ListViewItem listViewItem151 = new System.Windows.Forms.ListViewItem("18");
            System.Windows.Forms.ListViewItem listViewItem152 = new System.Windows.Forms.ListViewItem("19");
            System.Windows.Forms.ListViewItem listViewItem153 = new System.Windows.Forms.ListViewItem("20");
            System.Windows.Forms.ListViewItem listViewItem154 = new System.Windows.Forms.ListViewItem("21");
            System.Windows.Forms.ListViewItem listViewItem155 = new System.Windows.Forms.ListViewItem("22");
            System.Windows.Forms.ListViewItem listViewItem156 = new System.Windows.Forms.ListViewItem("23");
            System.Windows.Forms.ListViewItem listViewItem157 = new System.Windows.Forms.ListViewItem("24");
            System.Windows.Forms.ListViewItem listViewItem158 = new System.Windows.Forms.ListViewItem("25");
            System.Windows.Forms.ListViewItem listViewItem159 = new System.Windows.Forms.ListViewItem("26");
            System.Windows.Forms.ListViewItem listViewItem160 = new System.Windows.Forms.ListViewItem("27");
            System.Windows.Forms.ListViewItem listViewItem161 = new System.Windows.Forms.ListViewItem("28");
            System.Windows.Forms.ListViewItem listViewItem162 = new System.Windows.Forms.ListViewItem("29");
            System.Windows.Forms.ListViewItem listViewItem163 = new System.Windows.Forms.ListViewItem("30");
            System.Windows.Forms.ListViewItem listViewItem164 = new System.Windows.Forms.ListViewItem("31");
            System.Windows.Forms.ListViewItem listViewItem165 = new System.Windows.Forms.ListViewItem("32");
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddCursor = new System.Windows.Forms.ToolStripButton();
            this.StampTrigger = new System.Windows.Forms.ToolStripButton();
            this.ScaleButton = new System.Windows.Forms.ToolStripButton();
            this.panel = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Names = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MaskMin_textBox = new System.Windows.Forms.TextBox();
            this.MaskMax_textBox = new System.Windows.Forms.TextBox();
            this.MaskY_panel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel.SuspendLayout();
            this.MaskY_panel.SuspendLayout();
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
            this.zedGraph.Location = new System.Drawing.Point(190, 33);
            this.zedGraph.Margin = new System.Windows.Forms.Padding(0);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(619, 438);
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
            this.ScaleButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(809, 33);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
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
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.BackColor = System.Drawing.SystemColors.Window;
            this.panel.Controls.Add(this.listView1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel.Location = new System.Drawing.Point(0, 33);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(5, 5, 5, 40);
            this.panel.Size = new System.Drawing.Size(190, 438);
            this.panel.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Names,
            this.Value1});
            this.listView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Linux Libertine G", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listView1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            listViewItem133.StateImageIndex = 0;
            listViewItem134.StateImageIndex = 0;
            listViewItem135.StateImageIndex = 0;
            listViewItem136.StateImageIndex = 0;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem133,
            listViewItem134,
            listViewItem135,
            listViewItem136,
            listViewItem137,
            listViewItem138,
            listViewItem139,
            listViewItem140,
            listViewItem141,
            listViewItem142,
            listViewItem143,
            listViewItem144,
            listViewItem145,
            listViewItem146,
            listViewItem147,
            listViewItem148,
            listViewItem149,
            listViewItem150,
            listViewItem151,
            listViewItem152,
            listViewItem153,
            listViewItem154,
            listViewItem155,
            listViewItem156,
            listViewItem157,
            listViewItem158,
            listViewItem159,
            listViewItem160,
            listViewItem161,
            listViewItem162,
            listViewItem163,
            listViewItem164,
            listViewItem165});
            this.listView1.Location = new System.Drawing.Point(5, 5);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(180, 393);
            this.listView1.TabIndex = 0;
            this.listView1.TileSize = new System.Drawing.Size(168, 60);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Names
            // 
            this.Names.Text = "Name";
            this.Names.Width = 73;
            // 
            // Value1
            // 
            this.Value1.Text = "Курсор";
            this.Value1.Width = 75;
            // 
            // MaskMin_textBox
            // 
            this.MaskMin_textBox.Location = new System.Drawing.Point(111, 6);
            this.MaskMin_textBox.Name = "MaskMin_textBox";
            this.MaskMin_textBox.Size = new System.Drawing.Size(50, 20);
            this.MaskMin_textBox.TabIndex = 3;
            this.MaskMin_textBox.TextChanged += new System.EventHandler(this.MaskMin_textBox_TextChanged);
            this.MaskMin_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mask_KeyPress);
            // 
            // MaskMax_textBox
            // 
            this.MaskMax_textBox.Location = new System.Drawing.Point(30, 6);
            this.MaskMax_textBox.Name = "MaskMax_textBox";
            this.MaskMax_textBox.Size = new System.Drawing.Size(50, 20);
            this.MaskMax_textBox.TabIndex = 4;
            this.MaskMax_textBox.TextChanged += new System.EventHandler(this.MaskMax_textBox_TextChanged);
            this.MaskMax_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mask_KeyPress);
            // 
            // MaskY_panel
            // 
            this.MaskY_panel.Controls.Add(this.label2);
            this.MaskY_panel.Controls.Add(this.label1);
            this.MaskY_panel.Controls.Add(this.MaskMax_textBox);
            this.MaskY_panel.Controls.Add(this.MaskMin_textBox);
            this.MaskY_panel.Location = new System.Drawing.Point(190, 0);
            this.MaskY_panel.Name = "MaskY_panel";
            this.MaskY_panel.Size = new System.Drawing.Size(169, 30);
            this.MaskY_panel.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "От";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "до";
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.MaskY_panel);
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(809, 471);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel.ResumeLayout(false);
            this.MaskY_panel.ResumeLayout(false);
            this.MaskY_panel.PerformLayout();
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
        private ListView listView1;
        private TextBox MaskMin_textBox;
        private TextBox MaskMax_textBox;
        private Panel MaskY_panel;
        private Label label2;
        private Label label1;
    }
}

