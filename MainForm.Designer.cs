namespace SportsBetting
{
    partial class MainForm
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
            this.MainDisplay = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Output1 = new System.Windows.Forms.TextBox();
            this.Output2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // MainDisplay
            // 
            this.MainDisplay.Dock = System.Windows.Forms.DockStyle.Left;
            this.MainDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainDisplay.Location = new System.Drawing.Point(0, 0);
            this.MainDisplay.Multiline = true;
            this.MainDisplay.Name = "MainDisplay";
            this.MainDisplay.ReadOnly = true;
            this.MainDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MainDisplay.Size = new System.Drawing.Size(869, 654);
            this.MainDisplay.TabIndex = 3;
            this.MainDisplay.TextChanged += new System.EventHandler(this.MainDisplay_TextChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SpringGreen;
            this.button1.Location = new System.Drawing.Point(1018, 336);
            this.button1.MaximumSize = new System.Drawing.Size(75, 23);
            this.button1.MinimumSize = new System.Drawing.Size(75, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Output1
            // 
            this.Output1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output1.Location = new System.Drawing.Point(1059, 28);
            this.Output1.Multiline = true;
            this.Output1.Name = "Output1";
            this.Output1.ReadOnly = true;
            this.Output1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output1.Size = new System.Drawing.Size(60, 48);
            this.Output1.TabIndex = 4;
            // 
            // Output2
            // 
            this.Output2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Output2.Location = new System.Drawing.Point(1059, 82);
            this.Output2.Multiline = true;
            this.Output2.Name = "Output2";
            this.Output2.ReadOnly = true;
            this.Output2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output2.Size = new System.Drawing.Size(59, 44);
            this.Output2.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Crimson;
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(1018, 598);
            this.button2.MaximumSize = new System.Drawing.Size(75, 23);
            this.button2.MinimumSize = new System.Drawing.Size(75, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Delete Logs";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1179, 654);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Output1);
            this.Controls.Add(this.Output2);
            this.Controls.Add(this.MainDisplay);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "Arbiter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox MainDisplay;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Output1;
        private System.Windows.Forms.TextBox Output2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
    }
}