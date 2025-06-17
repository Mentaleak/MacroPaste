namespace MacroCopyPaste
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
            this.textBox_HotKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_update = new System.Windows.Forms.Button();
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
            this.button_update.Location = new System.Drawing.Point(322, 8);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(75, 23);
            this.button_update.TabIndex = 2;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 42);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_HotKey);
            this.Name = "Form1";
            this.Text = "MarcoPaste";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_HotKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_update;
    }
}

