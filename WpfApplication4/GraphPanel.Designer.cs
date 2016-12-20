namespace ScopeViewer
{
    public partial class GraphPanel
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
            this.AddCursor = new System.Windows.Forms.ToolStripButton();
            this.StampTrigger = new System.Windows.Forms.ToolStripButton();
            this.ScaleButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraph
            // 
            this.zedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph.Location = new System.Drawing.Point(33, 0);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(559, 471);
            this.zedGraph.TabIndex = 0;
            this.zedGraph.UseExtendedPrintDialog = true;
            this.zedGraph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseClick);
            this.zedGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseMove);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddCursor,
            this.StampTrigger,
            this.ScaleButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(33, 471);
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
            this.ScaleButton.Text = "toolStripButton1";
            this.ScaleButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.ScaleButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScaleButton_MouseDown);
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.toolStrip1);
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(592, 471);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ZedGraph.ZedGraphControl zedGraph;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddCursor;
        private System.Windows.Forms.ToolStripButton StampTrigger;
        private System.Windows.Forms.ToolStripButton ScaleButton;
    }
}

