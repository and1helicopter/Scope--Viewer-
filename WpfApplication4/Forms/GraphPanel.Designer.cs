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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.AddCursor = new System.Windows.Forms.ToolStripButton();
			this.AddCursorH = new System.Windows.Forms.ToolStripButton();
			this.StampTrigger = new System.Windows.Forms.ToolStripButton();
			this.ScaleButton = new System.Windows.Forms.ToolStripButton();
			this.StampTime_label = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.Cut_StripButton = new System.Windows.Forms.ToolStripButton();
			this.ApplyCut_StripButton = new System.Windows.Forms.ToolStripButton();
			this.SaveScope_toolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.absOrRelTime_toolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.posTab_StripButton = new System.Windows.Forms.ToolStripButton();
			this.delateDig_toolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.Mask1_label = new System.Windows.Forms.ToolStripLabel();
			this.MaskMax_textBox = new System.Windows.Forms.ToolStripTextBox();
			this.Mask2_label = new System.Windows.Forms.ToolStripLabel();
			this.MaskMin_textBox = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.tool_separator = new System.Windows.Forms.ToolStripSeparator();
			this.tool_CursorsHorizontalDif = new System.Windows.Forms.ToolStripLabel();
			this.tool_separator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tool_Horizont2Enter = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tool_Horizont1Enter = new System.Windows.Forms.ToolStripTextBox();
			this.tool_Horizont_label = new System.Windows.Forms.ToolStripLabel();
			this.tool_separator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tool_CursorsDif = new System.Windows.Forms.ToolStripLabel();
			this.tool_separator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tool_EnterRight_label = new System.Windows.Forms.ToolStripTextBox();
			this.tool_CursorsRight_label = new System.Windows.Forms.ToolStripLabel();
			this.tool_EnterLeft_label = new System.Windows.Forms.ToolStripTextBox();
			this.tool_CursorsLeft_label = new System.Windows.Forms.ToolStripLabel();
			this.tool_Cursors_label = new System.Windows.Forms.ToolStripLabel();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonAutosizing = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator_Bottom1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel_Top = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBox_Top = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel_Bottom = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBox_Bottom = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator_Bottom2 = new System.Windows.Forms.ToolStripSeparator();
			this.zedGraph = new ZedGraph.ZedGraphControl();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.BackColor = System.Drawing.SystemColors.Window;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddCursor,
            this.AddCursorH,
            this.StampTrigger,
            this.ScaleButton,
            this.StampTime_label,
            this.toolStripSeparator4,
            this.Cut_StripButton,
            this.ApplyCut_StripButton,
            this.SaveScope_toolStripButton,
            this.toolStripSeparator3,
            this.absOrRelTime_toolStripButton,
            this.toolStripSeparator2,
            this.posTab_StripButton,
            this.delateDig_toolStripButton,
            this.toolStripSeparator1,
            this.Mask1_label,
            this.MaskMax_textBox,
            this.Mask2_label,
            this.MaskMin_textBox,
            this.toolStripSeparator5,
            this.tool_separator,
            this.tool_CursorsHorizontalDif,
            this.tool_separator4,
            this.tool_Horizont2Enter,
            this.toolStripLabel1,
            this.tool_Horizont1Enter,
            this.tool_Horizont_label,
            this.tool_separator3,
            this.tool_CursorsDif,
            this.tool_separator2,
            this.tool_EnterRight_label,
            this.tool_CursorsRight_label,
            this.tool_EnterLeft_label,
            this.tool_CursorsLeft_label,
            this.tool_Cursors_label});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1165, 33);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			this.toolStrip1.MouseEnter += new System.EventHandler(this.toolStrip1_MouseEnter);
			// 
			// AddCursor
			// 
			this.AddCursor.AutoSize = false;
			this.AddCursor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.AddCursor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AddCursor.Image = global::ScopeViewer.Properties.Resources.CursorAddV;
			this.AddCursor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddCursor.Margin = new System.Windows.Forms.Padding(1);
			this.AddCursor.Name = "AddCursor";
			this.AddCursor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.AddCursor.Size = new System.Drawing.Size(30, 30);
			this.AddCursor.Text = "Добавить курсоры";
			this.AddCursor.ToolTipText = "Вертикальные курсоры ";
			this.AddCursor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AddCoursorVertical_MouseDown);
			// 
			// AddCursorH
			// 
			this.AddCursorH.AutoSize = false;
			this.AddCursorH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.AddCursorH.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.AddCursorH.Image = global::ScopeViewer.Properties.Resources.CursorAddH;
			this.AddCursorH.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.AddCursorH.Margin = new System.Windows.Forms.Padding(1);
			this.AddCursorH.Name = "AddCursorH";
			this.AddCursorH.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.AddCursorH.Size = new System.Drawing.Size(30, 30);
			this.AddCursorH.Text = "toolStripButton1";
			this.AddCursorH.ToolTipText = "Горизонтальный курсор";
			this.AddCursorH.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AddCoursorHorizontal_MouseDown);
			// 
			// StampTrigger
			// 
			this.StampTrigger.AutoSize = false;
			this.StampTrigger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.StampTrigger.Image = global::ScopeViewer.Properties.Resources.Watch_Add;
			this.StampTrigger.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.StampTrigger.Name = "StampTrigger";
			this.StampTrigger.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
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
			this.ScaleButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.ScaleButton.Size = new System.Drawing.Size(30, 30);
			this.ScaleButton.Text = "Изменить размер";
			this.ScaleButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.ScaleButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScaleButton_MouseDown);
			// 
			// StampTime_label
			// 
			this.StampTime_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.StampTime_label.Name = "StampTime_label";
			this.StampTime_label.Size = new System.Drawing.Size(68, 30);
			this.StampTime_label.Text = "StampTime";
			this.StampTime_label.ToolTipText = "Штамп времени";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 33);
			// 
			// Cut_StripButton
			// 
			this.Cut_StripButton.AutoSize = false;
			this.Cut_StripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.Cut_StripButton.Image = global::ScopeViewer.Properties.Resources.Cutting_Add;
			this.Cut_StripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.Cut_StripButton.Name = "Cut_StripButton";
			this.Cut_StripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.Cut_StripButton.Size = new System.Drawing.Size(30, 30);
			this.Cut_StripButton.Text = "Обрезать участок осциллограммы";
			this.Cut_StripButton.ToolTipText = "Обрезать участок осциллограммы";
			this.Cut_StripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Cut_StripButton_MouseDown);
			// 
			// ApplyCut_StripButton
			// 
			this.ApplyCut_StripButton.AutoSize = false;
			this.ApplyCut_StripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ApplyCut_StripButton.Image = global::ScopeViewer.Properties.Resources.Cut_Apply;
			this.ApplyCut_StripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ApplyCut_StripButton.Name = "ApplyCut_StripButton";
			this.ApplyCut_StripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.ApplyCut_StripButton.Size = new System.Drawing.Size(30, 30);
			this.ApplyCut_StripButton.Text = "Применить ";
			this.ApplyCut_StripButton.ToolTipText = "Обрезать участок осциллограммы";
			this.ApplyCut_StripButton.Visible = false;
			this.ApplyCut_StripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ApplyCut_StripButton_MouseDown);
			// 
			// SaveScope_toolStripButton
			// 
			this.SaveScope_toolStripButton.AutoSize = false;
			this.SaveScope_toolStripButton.CheckOnClick = true;
			this.SaveScope_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.SaveScope_toolStripButton.Image = global::ScopeViewer.Properties.Resources.Save_as_48_1_;
			this.SaveScope_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.SaveScope_toolStripButton.Name = "SaveScope_toolStripButton";
			this.SaveScope_toolStripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.SaveScope_toolStripButton.Size = new System.Drawing.Size(30, 30);
			this.SaveScope_toolStripButton.Text = "Сохранить осциллограмму";
			this.SaveScope_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SaveScope_toolStripButton_MouseDown);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 33);
			// 
			// absOrRelTime_toolStripButton
			// 
			this.absOrRelTime_toolStripButton.AutoSize = false;
			this.absOrRelTime_toolStripButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.absOrRelTime_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.absOrRelTime_toolStripButton.Image = global::ScopeViewer.Properties.Resources.Time_abs;
			this.absOrRelTime_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.absOrRelTime_toolStripButton.Name = "absOrRelTime_toolStripButton";
			this.absOrRelTime_toolStripButton.Size = new System.Drawing.Size(30, 30);
			this.absOrRelTime_toolStripButton.Text = "Относительное время";
			this.absOrRelTime_toolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
			this.absOrRelTime_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.absOrRelTime_toolStripButton_MouseDown);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
			// 
			// posTab_StripButton
			// 
			this.posTab_StripButton.AutoSize = false;
			this.posTab_StripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.posTab_StripButton.Image = global::ScopeViewer.Properties.Resources.Flip_Horizontal_48;
			this.posTab_StripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.posTab_StripButton.Name = "posTab_StripButton";
			this.posTab_StripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.posTab_StripButton.Size = new System.Drawing.Size(30, 30);
			this.posTab_StripButton.Text = "Расположение ";
			this.posTab_StripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.posTab_StripButton_MouseDown);
			// 
			// delateDig_toolStripButton
			// 
			this.delateDig_toolStripButton.AutoSize = false;
			this.delateDig_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.delateDig_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delateDig_toolStripButton.Image")));
			this.delateDig_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.delateDig_toolStripButton.Name = "delateDig_toolStripButton";
			this.delateDig_toolStripButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.delateDig_toolStripButton.Size = new System.Drawing.Size(30, 30);
			this.delateDig_toolStripButton.Text = "Убрать цифровой канал";
			this.delateDig_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.delateDig_toolStripButton_MouseDown);
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
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 33);
			// 
			// tool_separator
			// 
			this.tool_separator.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_separator.Name = "tool_separator";
			this.tool_separator.Size = new System.Drawing.Size(6, 33);
			this.tool_separator.Visible = false;
			// 
			// tool_CursorsHorizontalDif
			// 
			this.tool_CursorsHorizontalDif.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_CursorsHorizontalDif.Name = "tool_CursorsHorizontalDif";
			this.tool_CursorsHorizontalDif.Size = new System.Drawing.Size(18, 30);
			this.tool_CursorsHorizontalDif.Text = "Δ:";
			this.tool_CursorsHorizontalDif.Visible = false;
			// 
			// tool_separator4
			// 
			this.tool_separator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_separator4.Name = "tool_separator4";
			this.tool_separator4.Size = new System.Drawing.Size(6, 33);
			this.tool_separator4.Visible = false;
			// 
			// tool_Horizont2Enter
			// 
			this.tool_Horizont2Enter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_Horizont2Enter.Name = "tool_Horizont2Enter";
			this.tool_Horizont2Enter.Size = new System.Drawing.Size(80, 33);
			this.tool_Horizont2Enter.Visible = false;
			this.tool_Horizont2Enter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tool_HorizontEnter_KeyPress);
			this.tool_Horizont2Enter.TextChanged += new System.EventHandler(this.tool_Horizont2Enter_TextChanged);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripLabel1.Image = global::ScopeViewer.Properties.Resources.green_line;
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(30, 30);
			this.toolStripLabel1.Text = "toolStripLabel1";
			this.toolStripLabel1.Visible = false;
			// 
			// tool_Horizont1Enter
			// 
			this.tool_Horizont1Enter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_Horizont1Enter.Name = "tool_Horizont1Enter";
			this.tool_Horizont1Enter.Size = new System.Drawing.Size(80, 33);
			this.tool_Horizont1Enter.Visible = false;
			this.tool_Horizont1Enter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tool_HorizontEnter_KeyPress);
			this.tool_Horizont1Enter.TextChanged += new System.EventHandler(this.tool_Horizont1Enter_TextChanged);
			// 
			// tool_Horizont_label
			// 
			this.tool_Horizont_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_Horizont_label.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tool_Horizont_label.Image = global::ScopeViewer.Properties.Resources.green_line;
			this.tool_Horizont_label.Name = "tool_Horizont_label";
			this.tool_Horizont_label.Size = new System.Drawing.Size(30, 30);
			this.tool_Horizont_label.Text = "toolStripLabel1";
			this.tool_Horizont_label.Visible = false;
			// 
			// tool_separator3
			// 
			this.tool_separator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_separator3.Name = "tool_separator3";
			this.tool_separator3.Size = new System.Drawing.Size(6, 33);
			this.tool_separator3.Visible = false;
			// 
			// tool_CursorsDif
			// 
			this.tool_CursorsDif.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_CursorsDif.Name = "tool_CursorsDif";
			this.tool_CursorsDif.Size = new System.Drawing.Size(18, 30);
			this.tool_CursorsDif.Text = "Δ:";
			this.tool_CursorsDif.Visible = false;
			// 
			// tool_separator2
			// 
			this.tool_separator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_separator2.Name = "tool_separator2";
			this.tool_separator2.Size = new System.Drawing.Size(6, 33);
			this.tool_separator2.Visible = false;
			// 
			// tool_EnterRight_label
			// 
			this.tool_EnterRight_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_EnterRight_label.Name = "tool_EnterRight_label";
			this.tool_EnterRight_label.Size = new System.Drawing.Size(80, 33);
			this.tool_EnterRight_label.Visible = false;
			this.tool_EnterRight_label.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tool_EnterRight_label_KeyPress);
			this.tool_EnterRight_label.TextChanged += new System.EventHandler(this.tool_EnterRight_label_TextChanged);
			// 
			// tool_CursorsRight_label
			// 
			this.tool_CursorsRight_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_CursorsRight_label.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tool_CursorsRight_label.Image = global::ScopeViewer.Properties.Resources.blue_line;
			this.tool_CursorsRight_label.Name = "tool_CursorsRight_label";
			this.tool_CursorsRight_label.Size = new System.Drawing.Size(30, 30);
			this.tool_CursorsRight_label.Text = "toolStripLabel2";
			this.tool_CursorsRight_label.Visible = false;
			// 
			// tool_EnterLeft_label
			// 
			this.tool_EnterLeft_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_EnterLeft_label.Name = "tool_EnterLeft_label";
			this.tool_EnterLeft_label.Size = new System.Drawing.Size(80, 33);
			this.tool_EnterLeft_label.Visible = false;
			this.tool_EnterLeft_label.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tool_EnterLeft_label_KeyPress);
			this.tool_EnterLeft_label.TextChanged += new System.EventHandler(this.tool_EnterLeft_label_TextChanged);
			// 
			// tool_CursorsLeft_label
			// 
			this.tool_CursorsLeft_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_CursorsLeft_label.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.tool_CursorsLeft_label.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tool_CursorsLeft_label.Image = global::ScopeViewer.Properties.Resources.red_line;
			this.tool_CursorsLeft_label.Name = "tool_CursorsLeft_label";
			this.tool_CursorsLeft_label.Size = new System.Drawing.Size(30, 30);
			this.tool_CursorsLeft_label.Text = "toolStripLabel1";
			this.tool_CursorsLeft_label.Visible = false;
			// 
			// tool_Cursors_label
			// 
			this.tool_Cursors_label.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tool_Cursors_label.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.tool_Cursors_label.Name = "tool_Cursors_label";
			this.tool_Cursors_label.Size = new System.Drawing.Size(133, 30);
			this.tool_Cursors_label.Text = "Положение курсоров: ";
			this.tool_Cursors_label.Visible = false;
			// 
			// toolStrip2
			// 
			this.toolStrip2.AutoSize = false;
			this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(0);
			this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(30, 30);
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAutosizing,
            this.toolStripSeparator_Bottom1,
            this.toolStripLabel_Top,
            this.toolStripTextBox_Top,
            this.toolStripLabel_Bottom,
            this.toolStripTextBox_Bottom,
            this.toolStripSeparator_Bottom2});
			this.toolStrip2.Location = new System.Drawing.Point(0, 439);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Padding = new System.Windows.Forms.Padding(0);
			this.toolStrip2.Size = new System.Drawing.Size(1165, 32);
			this.toolStrip2.TabIndex = 2;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripButtonAutosizing
			// 
			this.toolStripButtonAutosizing.AutoSize = false;
			this.toolStripButtonAutosizing.Checked = true;
			this.toolStripButtonAutosizing.CheckState = System.Windows.Forms.CheckState.Checked;
			this.toolStripButtonAutosizing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAutosizing.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutosizing.Image")));
			this.toolStripButtonAutosizing.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAutosizing.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.toolStripButtonAutosizing.Name = "toolStripButtonAutosizing";
			this.toolStripButtonAutosizing.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.toolStripButtonAutosizing.Size = new System.Drawing.Size(30, 30);
			this.toolStripButtonAutosizing.Text = "Автомаштабирование";
			this.toolStripButtonAutosizing.Click += new System.EventHandler(this.toolStripButtonAutosizing_Click);
			// 
			// toolStripSeparator_Bottom1
			// 
			this.toolStripSeparator_Bottom1.Name = "toolStripSeparator_Bottom1";
			this.toolStripSeparator_Bottom1.Size = new System.Drawing.Size(6, 32);
			this.toolStripSeparator_Bottom1.Visible = false;
			// 
			// toolStripLabel_Top
			// 
			this.toolStripLabel_Top.Name = "toolStripLabel_Top";
			this.toolStripLabel_Top.Size = new System.Drawing.Size(42, 29);
			this.toolStripLabel_Top.Text = "Y max:";
			this.toolStripLabel_Top.Visible = false;
			// 
			// toolStripTextBox_Top
			// 
			this.toolStripTextBox_Top.Name = "toolStripTextBox_Top";
			this.toolStripTextBox_Top.Size = new System.Drawing.Size(80, 32);
			this.toolStripTextBox_Top.Visible = false;
			this.toolStripTextBox_Top.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox_Top_KeyPress);
			this.toolStripTextBox_Top.TextChanged += new System.EventHandler(this.toolStripTextBox_Top_TextChanged);
			// 
			// toolStripLabel_Bottom
			// 
			this.toolStripLabel_Bottom.Name = "toolStripLabel_Bottom";
			this.toolStripLabel_Bottom.Size = new System.Drawing.Size(41, 29);
			this.toolStripLabel_Bottom.Text = "Y min:";
			this.toolStripLabel_Bottom.Visible = false;
			// 
			// toolStripTextBox_Bottom
			// 
			this.toolStripTextBox_Bottom.Name = "toolStripTextBox_Bottom";
			this.toolStripTextBox_Bottom.Size = new System.Drawing.Size(80, 32);
			this.toolStripTextBox_Bottom.Visible = false;
			this.toolStripTextBox_Bottom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox_Bottom_KeyPress);
			this.toolStripTextBox_Bottom.TextChanged += new System.EventHandler(this.toolStripTextBox_Bottom_TextChanged);
			// 
			// toolStripSeparator_Bottom2
			// 
			this.toolStripSeparator_Bottom2.Name = "toolStripSeparator_Bottom2";
			this.toolStripSeparator_Bottom2.Size = new System.Drawing.Size(6, 32);
			this.toolStripSeparator_Bottom2.Visible = false;
			// 
			// zedGraph
			// 
			this.zedGraph.AutoSize = true;
			this.zedGraph.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.zedGraph.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.zedGraph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.zedGraph.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.zedGraph.IsAntiAlias = true;
			this.zedGraph.IsShowPointValues = true;
			this.zedGraph.IsSynchronizeXAxes = true;
			this.zedGraph.IsSynchronizeYAxes = true;
			this.zedGraph.Location = new System.Drawing.Point(0, 33);
			this.zedGraph.Margin = new System.Windows.Forms.Padding(0);
			this.zedGraph.Name = "zedGraph";
			this.zedGraph.ScrollGrace = 0D;
			this.zedGraph.ScrollMaxX = 0D;
			this.zedGraph.ScrollMaxY = 0D;
			this.zedGraph.ScrollMaxY2 = 0D;
			this.zedGraph.ScrollMinX = 0D;
			this.zedGraph.ScrollMinY = 0D;
			this.zedGraph.ScrollMinY2 = 0D;
			this.zedGraph.Size = new System.Drawing.Size(1165, 406);
			this.zedGraph.TabIndex = 0;
			this.zedGraph.UseExtendedPrintDialog = true;
			this.zedGraph.SizeChanged += new System.EventHandler(this.zedGraph_SizeChanged);
			this.zedGraph.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseClick);
			this.zedGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zedGraph_MouseMove);
			this.zedGraph.Resize += new System.EventHandler(this.zedGraph_Resize_1);
			// 
			// GraphPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.Controls.Add(this.zedGraph);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.toolStrip2);
			this.DoubleBuffered = true;
			this.Name = "GraphPanel";
			this.Size = new System.Drawing.Size(1165, 471);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripButton AddCursor;
        private ToolStripButton StampTrigger;
        private ToolStripButton ScaleButton;
        private ToolStripTextBox MaskMin_textBox;
        private ToolStripLabel Mask1_label;
        private ToolStripTextBox MaskMax_textBox;
        private ToolStripLabel Mask2_label;
        private ToolStripLabel StampTime_label;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton Cut_StripButton;
        private ToolStripButton ApplyCut_StripButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripButton posTab_StripButton;
        private ToolStripButton delateDig_toolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton absOrRelTime_toolStripButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton SaveScope_toolStripButton;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripButton AddCursorH;
		private ToolStripLabel tool_CursorsLeft_label;
		private ToolStripTextBox tool_EnterLeft_label;
		private ToolStripLabel tool_CursorsRight_label;
		private ToolStripTextBox tool_EnterRight_label;
		private ToolStripLabel tool_Cursors_label;
		private ToolStripLabel tool_CursorsDif;
		private ToolStripSeparator tool_separator;
		private ToolStripSeparator tool_separator2;
		private ToolStripLabel tool_Horizont_label;
		private ToolStripTextBox tool_Horizont1Enter;
		private ToolStripTextBox tool_Horizont2Enter;
		private ToolStripLabel toolStripLabel1;
		private ToolStripLabel tool_CursorsHorizontalDif;
		private ToolStripSeparator tool_separator4;
		private ToolStripSeparator tool_separator3;
		private ToolStrip toolStrip2;
		private ToolStripButton toolStripButtonAutosizing;
		public ZedGraph.ZedGraphControl zedGraph;
		private ToolStripSeparator toolStripSeparator_Bottom1;
		private ToolStripLabel toolStripLabel_Top;
		private ToolStripTextBox toolStripTextBox_Top;
		private ToolStripLabel toolStripLabel_Bottom;
		private ToolStripTextBox toolStripTextBox_Bottom;
		private ToolStripSeparator toolStripSeparator_Bottom2;
	}
}

