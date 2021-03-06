﻿namespace Population
{
    partial class ControlPanel
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
            this.components = new System.ComponentModel.Container();
            this.chkSolid = new System.Windows.Forms.CheckBox();
            this.HelpTips = new System.Windows.Forms.ToolTip(this.components);
            this.chkSolidAll = new System.Windows.Forms.CheckBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.nudSeconds = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSolid
            // 
            this.chkSolid.AutoSize = true;
            this.chkSolid.Location = new System.Drawing.Point(32, 26);
            this.chkSolid.Name = "chkSolid";
            this.chkSolid.Size = new System.Drawing.Size(114, 17);
            this.chkSolid.TabIndex = 0;
            this.chkSolid.Text = "Make objects solid";
            this.HelpTips.SetToolTip(this.chkSolid, "Solid objects will collide with each other and lose health as a result.");
            this.chkSolid.UseVisualStyleBackColor = true;
            this.chkSolid.CheckedChanged += new System.EventHandler(this.chkSolid_CheckedChanged);
            // 
            // chkSolidAll
            // 
            this.chkSolidAll.AutoSize = true;
            this.chkSolidAll.Enabled = false;
            this.chkSolidAll.Location = new System.Drawing.Point(157, 26);
            this.chkSolidAll.Name = "chkSolidAll";
            this.chkSolidAll.Size = new System.Drawing.Size(74, 17);
            this.chkSolidAll.TabIndex = 1;
            this.chkSolidAll.Text = "All objects";
            this.HelpTips.SetToolTip(this.chkSolidAll, "Make existing objects solid as well as new objects.");
            this.chkSolidAll.UseVisualStyleBackColor = true;
            this.chkSolidAll.CheckedChanged += new System.EventHandler(this.chkSolidAll_CheckedChanged);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(156, 104);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.HelpTips.SetToolTip(this.btnStop, "Pause animation.");
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(36, 104);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.HelpTips.SetToolTip(this.btnStart, "Start animation.");
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(36, 144);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.HelpTips.SetToolTip(this.btnClear, "Clear objects from field.");
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(156, 144);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.HelpTips.SetToolTip(this.btnExit, "Exit the program.");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // nudSeconds
            // 
            this.nudSeconds.DecimalPlaces = 1;
            this.nudSeconds.Location = new System.Drawing.Point(132, 58);
            this.nudSeconds.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.nudSeconds.Name = "nudSeconds";
            this.nudSeconds.Size = new System.Drawing.Size(50, 20);
            this.nudSeconds.TabIndex = 2;
            this.nudSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudSeconds.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudSeconds.ValueChanged += new System.EventHandler(this.nudSeconds_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "New member every";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "seconds.";
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 200);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudSeconds);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.chkSolidAll);
            this.Controls.Add(this.chkSolid);
            this.Name = "ControlPanel";
            this.Opacity = 0.25D;
            this.Text = "Settings";
            this.Deactivate += new System.EventHandler(this.ControlPanel_Deactivate);
            this.MouseEnter += new System.EventHandler(this.ControlPanel_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSolid;
        private System.Windows.Forms.ToolTip HelpTips;
        private System.Windows.Forms.CheckBox chkSolidAll;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.NumericUpDown nudSeconds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}