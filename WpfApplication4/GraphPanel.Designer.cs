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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphPanel));
            this.zedGraph = new ZedGraph.ZedGraphControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddCursor = new System.Windows.Forms.ToolStripButton();
            this.StampTrigger = new System.Windows.Forms.ToolStripButton();
            this.ScaleButton = new System.Windows.Forms.ToolStripButton();
            this.StampTime_label = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Cut_StripButton = new System.Windows.Forms.ToolStripButton();
            this.Save_StripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.posTab_StripButton = new System.Windows.Forms.ToolStripButton();
            this.delateDig_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Mask1_label = new System.Windows.Forms.ToolStripLabel();
            this.MaskMax_textBox = new System.Windows.Forms.ToolStripTextBox();
            this.Mask2_label = new System.Windows.Forms.ToolStripLabel();
            this.MaskMin_textBox = new System.Windows.Forms.ToolStripTextBox();
            this.AutoRange_Button = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.absOrRelTime_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraph
            // 
            this.zedGraph.AutoSize = true;
            this.zedGraph.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.zedGraph.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraph.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zedGraph.IsShowPointValues = true;
            this.zedGraph.IsSynchronizeXAxes = true;
            this.zedGraph.Location = new System.Drawing.Point(0, 36);
            this.zedGraph.Margin = new System.Windows.Forms.Padding(0);
            this.zedGraph.Name = "zedGraph";
            this.zedGraph.ScrollGrace = 0D;
            this.zedGraph.ScrollMaxX = 0D;
            this.zedGraph.ScrollMaxY = 0D;
            this.zedGraph.ScrollMaxY2 = 0D;
            this.zedGraph.ScrollMinX = 0D;
            this.zedGraph.ScrollMinY = 0D;
            this.zedGraph.ScrollMinY2 = 0D;
            this.zedGraph.Size = new System.Drawing.Size(809, 435);
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
            this.StampTime_label,
            this.toolStripSeparator4,
            this.Cut_StripButton,
            this.Save_StripButton,
            this.toolStripSeparator3,
            this.posTab_StripButton,
            this.delateDig_toolStripButton,
            this.toolStripSeparator1,
            this.Mask1_label,
            this.MaskMax_textBox,
            this.Mask2_label,
            this.MaskMin_textBox,
            this.AutoRange_Button,
            this.toolStripSeparator2,
            this.absOrRelTime_toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(809, 36);
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
            this.AddCursor.Size = new System.Drawing.Size(34, 34);
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
            // StampTime_label
            // 
            this.StampTime_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.StampTime_label.Name = "StampTime_label";
            this.StampTime_label.Size = new System.Drawing.Size(68, 33);
            this.StampTime_label.Text = "StampTime";
            this.StampTime_label.ToolTipText = "Штамп времени";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 36);
            // 
            // Cut_StripButton
            // 
            this.Cut_StripButton.AutoSize = false;
            this.Cut_StripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Cut_StripButton.Image = global::ScopeViewer.Properties.Resources.Cutting_Coupon_48;
            this.Cut_StripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Cut_StripButton.Name = "Cut_StripButton";
            this.Cut_StripButton.Size = new System.Drawing.Size(30, 30);
            this.Cut_StripButton.Text = "toolStripButton1";
            // 
            // Save_StripButton
            // 
            this.Save_StripButton.AutoSize = false;
            this.Save_StripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Save_StripButton.Image = ((System.Drawing.Image)(resources.GetObject("Save_StripButton.Image")));
            this.Save_StripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Save_StripButton.Name = "Save_StripButton";
            this.Save_StripButton.Size = new System.Drawing.Size(30, 30);
            this.Save_StripButton.Text = "toolStripButton2";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 36);
            // 
            // posTab_StripButton
            // 
            this.posTab_StripButton.AutoSize = false;
            this.posTab_StripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.posTab_StripButton.Image = global::ScopeViewer.Properties.Resources.Flip_Horizontal_48;
            this.posTab_StripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.posTab_StripButton.Name = "posTab_StripButton";
            this.posTab_StripButton.Size = new System.Drawing.Size(30, 30);
            this.posTab_StripButton.Text = "Расположение ";
            this.posTab_StripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.posTab_StripButton_MouseDown);
            // 
            // delateDig_toolStripButton
            // 
            this.delateDig_toolStripButton.AutoSize = false;
            this.delateDig_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delateDig_toolStripButton.Image = global::ScopeViewer.Properties.Resources.Cancel_4_Digits_48__3_;
            this.delateDig_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delateDig_toolStripButton.Name = "delateDig_toolStripButton";
            this.delateDig_toolStripButton.Size = new System.Drawing.Size(30, 30);
            this.delateDig_toolStripButton.Text = "toolStripButton1";
            this.delateDig_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.delateDig_toolStripButton_MouseDown);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 36);
            // 
            // Mask1_label
            // 
            this.Mask1_label.Name = "Mask1_label";
            this.Mask1_label.Size = new System.Drawing.Size(21, 33);
            this.Mask1_label.Text = "От";
            // 
            // MaskMax_textBox
            // 
            this.MaskMax_textBox.Name = "MaskMax_textBox";
            this.MaskMax_textBox.Size = new System.Drawing.Size(20, 36);
            this.MaskMax_textBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MaskMax_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mask_KeyPress);
            this.MaskMax_textBox.TextChanged += new System.EventHandler(this.MaskMax_textBox_TextChanged);
            // 
            // Mask2_label
            // 
            this.Mask2_label.Name = "Mask2_label";
            this.Mask2_label.Size = new System.Drawing.Size(20, 33);
            this.Mask2_label.Text = "до";
            // 
            // MaskMin_textBox
            // 
            this.MaskMin_textBox.Name = "MaskMin_textBox";
            this.MaskMin_textBox.Size = new System.Drawing.Size(20, 36);
            this.MaskMin_textBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MaskMin_textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Mask_KeyPress);
            this.MaskMin_textBox.TextChanged += new System.EventHandler(this.MaskMin_textBox_TextChanged);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 36);
            // 
            // absOrRelTime_toolStripButton
            // 
            this.absOrRelTime_toolStripButton.AutoSize = false;
            this.absOrRelTime_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.absOrRelTime_toolStripButton.Image = global::ScopeViewer.Properties.Resources.Cosine_50_4_;
            this.absOrRelTime_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.absOrRelTime_toolStripButton.Name = "absOrRelTime_toolStripButton";
            this.absOrRelTime_toolStripButton.Size = new System.Drawing.Size(30, 30);
            this.absOrRelTime_toolStripButton.Text = "Относительное время";
            this.absOrRelTime_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.absOrRelTime_toolStripButton_MouseDown);
            // 
            // GraphPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.zedGraph);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "GraphPanel";
            this.Size = new System.Drawing.Size(809, 471);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ZedGraph.ZedGraphControl zedGraph;
        private ToolStrip toolStrip1;
        private ToolStripButton AddCursor;
        private ToolStripButton StampTrigger;
        private ToolStripButton ScaleButton;
        private ToolStripTextBox MaskMin_textBox;
        private ToolStripButton AutoRange_Button;
        private ToolStripLabel Mask1_label;
        private ToolStripTextBox MaskMax_textBox;
        private ToolStripLabel Mask2_label;
        private ToolStripLabel StampTime_label;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton Cut_StripButton;
        private ToolStripButton Save_StripButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton posTab_StripButton;
        private ToolStripButton delateDig_toolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton absOrRelTime_toolStripButton;
        private ToolStripSeparator toolStripSeparator4;
    }
}

