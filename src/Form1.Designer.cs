﻿namespace MacroCopyPaste
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox_HotKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_update = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_Delay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Delay)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_HotKey
            // 
            this.textBox_HotKey.Location = new System.Drawing.Point(104, 10);
            this.textBox_HotKey.Name = "textBox_HotKey";
            this.textBox_HotKey.ReadOnly = true;
            this.textBox_HotKey.Size = new System.Drawing.Size(212, 20);
            this.textBox_HotKey.TabIndex = 0;
            this.textBox_HotKey.TabStop = false;
            this.textBox_HotKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_HotKey_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Set Paste Hot Key";
            // 
            // button_update
            // 
            this.button_update.Location = new System.Drawing.Point(241, 45);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(75, 23);
            this.button_update.TabIndex = 2;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Delay";
            // 
            // numericUpDown_Delay
            // 
            this.numericUpDown_Delay.DecimalPlaces = 1;
            this.numericUpDown_Delay.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_Delay.Location = new System.Drawing.Point(105, 45);
            this.numericUpDown_Delay.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDown_Delay.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_Delay.Name = "numericUpDown_Delay";
            this.numericUpDown_Delay.Size = new System.Drawing.Size(38, 20);
            this.numericUpDown_Delay.TabIndex = 5;
            this.numericUpDown_Delay.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 73);
            this.Controls.Add(this.numericUpDown_Delay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_HotKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MarcoPaste";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Delay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_HotKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Label label3;
        public  System.Windows.Forms.NumericUpDown numericUpDown_Delay;
    }
}

