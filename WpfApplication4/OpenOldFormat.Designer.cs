namespace ScopeViewer
{
    partial class OpenOldFormat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenOldFormat));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.KHz1_radioButton = new System.Windows.Forms.RadioButton();
            this.KHz2_radioButton = new System.Windows.Forms.RadioButton();
            this.KHz4_radioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.percent25_radioButton = new System.Windows.Forms.RadioButton();
            this.percent50_radioButton = new System.Windows.Forms.RadioButton();
            this.percent75_radioButton = new System.Windows.Forms.RadioButton();
            this.OK_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.KHz1_radioButton);
            this.groupBox1.Controls.Add(this.KHz2_radioButton);
            this.groupBox1.Controls.Add(this.KHz4_radioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(115, 93);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Дискретность";
            // 
            // KHz1_radioButton
            // 
            this.KHz1_radioButton.AutoSize = true;
            this.KHz1_radioButton.Location = new System.Drawing.Point(6, 66);
            this.KHz1_radioButton.Name = "KHz1_radioButton";
            this.KHz1_radioButton.Size = new System.Drawing.Size(53, 17);
            this.KHz1_radioButton.TabIndex = 2;
            this.KHz1_radioButton.Text = "1 КГц";
            this.KHz1_radioButton.UseVisualStyleBackColor = true;
            // 
            // KHz2_radioButton
            // 
            this.KHz2_radioButton.AutoSize = true;
            this.KHz2_radioButton.Location = new System.Drawing.Point(7, 43);
            this.KHz2_radioButton.Name = "KHz2_radioButton";
            this.KHz2_radioButton.Size = new System.Drawing.Size(53, 17);
            this.KHz2_radioButton.TabIndex = 1;
            this.KHz2_radioButton.Text = "2 КГц";
            this.KHz2_radioButton.UseVisualStyleBackColor = true;
            // 
            // KHz4_radioButton
            // 
            this.KHz4_radioButton.AutoSize = true;
            this.KHz4_radioButton.Checked = true;
            this.KHz4_radioButton.Location = new System.Drawing.Point(7, 20);
            this.KHz4_radioButton.Name = "KHz4_radioButton";
            this.KHz4_radioButton.Size = new System.Drawing.Size(53, 17);
            this.KHz4_radioButton.TabIndex = 0;
            this.KHz4_radioButton.TabStop = true;
            this.KHz4_radioButton.Text = "4 КГц";
            this.KHz4_radioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.percent25_radioButton);
            this.groupBox2.Controls.Add(this.percent50_radioButton);
            this.groupBox2.Controls.Add(this.percent75_radioButton);
            this.groupBox2.Location = new System.Drawing.Point(134, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(115, 93);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Предыстория";
            // 
            // percent25_radioButton
            // 
            this.percent25_radioButton.AutoSize = true;
            this.percent25_radioButton.Location = new System.Drawing.Point(6, 66);
            this.percent25_radioButton.Name = "percent25_radioButton";
            this.percent25_radioButton.Size = new System.Drawing.Size(45, 17);
            this.percent25_radioButton.TabIndex = 5;
            this.percent25_radioButton.Text = "25%";
            this.percent25_radioButton.UseVisualStyleBackColor = true;
            // 
            // percent50_radioButton
            // 
            this.percent50_radioButton.AutoSize = true;
            this.percent50_radioButton.Location = new System.Drawing.Point(6, 43);
            this.percent50_radioButton.Name = "percent50_radioButton";
            this.percent50_radioButton.Size = new System.Drawing.Size(45, 17);
            this.percent50_radioButton.TabIndex = 4;
            this.percent50_radioButton.Text = "50%";
            this.percent50_radioButton.UseVisualStyleBackColor = true;
            // 
            // percent75_radioButton
            // 
            this.percent75_radioButton.AutoSize = true;
            this.percent75_radioButton.Checked = true;
            this.percent75_radioButton.Location = new System.Drawing.Point(6, 20);
            this.percent75_radioButton.Name = "percent75_radioButton";
            this.percent75_radioButton.Size = new System.Drawing.Size(45, 17);
            this.percent75_radioButton.TabIndex = 3;
            this.percent75_radioButton.TabStop = true;
            this.percent75_radioButton.Text = "75%";
            this.percent75_radioButton.UseVisualStyleBackColor = true;
            // 
            // OK_button
            // 
            this.OK_button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK_button.Location = new System.Drawing.Point(174, 111);
            this.OK_button.Name = "OK_button";
            this.OK_button.Size = new System.Drawing.Size(75, 23);
            this.OK_button.TabIndex = 2;
            this.OK_button.Text = "OK";
            this.OK_button.UseVisualStyleBackColor = true;
            this.OK_button.Click += new System.EventHandler(this.OK_button_Click);
            // 
            // OpenOldFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 137);
            this.ControlBox = false;
            this.Controls.Add(this.OK_button);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenOldFormat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Выберете параметры осциллограммы";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton KHz1_radioButton;
        private System.Windows.Forms.RadioButton KHz2_radioButton;
        private System.Windows.Forms.RadioButton KHz4_radioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton percent25_radioButton;
        private System.Windows.Forms.RadioButton percent50_radioButton;
        private System.Windows.Forms.RadioButton percent75_radioButton;
        private System.Windows.Forms.Button OK_button;
    }
}