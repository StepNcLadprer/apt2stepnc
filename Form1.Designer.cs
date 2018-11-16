namespace apt2stepnc
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblBrowser = new System.Windows.Forms.Label();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnMakeStepnc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblReady = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblBrowser);
            this.groupBox1.Controls.Add(this.btnBrowser);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 57);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Entry Data";
            // 
            // lblBrowser
            // 
            this.lblBrowser.AutoSize = true;
            this.lblBrowser.Location = new System.Drawing.Point(6, 25);
            this.lblBrowser.MinimumSize = new System.Drawing.Size(250, 0);
            this.lblBrowser.Name = "lblBrowser";
            this.lblBrowser.Size = new System.Drawing.Size(250, 13);
            this.lblBrowser.TabIndex = 1;
            this.lblBrowser.Text = "Choose a file!";
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(308, 19);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 25);
            this.btnBrowser.TabIndex = 0;
            this.btnBrowser.Text = "Browser";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Mastercam APT (*.apt)|*.apt|All files (*.*)|*.*";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(280, 20);
            this.textBox1.TabIndex = 2;
            // 
            // btnMakeStepnc
            // 
            this.btnMakeStepnc.Enabled = false;
            this.btnMakeStepnc.Location = new System.Drawing.Point(309, 19);
            this.btnMakeStepnc.Name = "btnMakeStepnc";
            this.btnMakeStepnc.Size = new System.Drawing.Size(75, 23);
            this.btnMakeStepnc.TabIndex = 1;
            this.btnMakeStepnc.Text = "Make";
            this.btnMakeStepnc.UseVisualStyleBackColor = true;
            this.btnMakeStepnc.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.MinimumSize = new System.Drawing.Size(250, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblReady);
            this.groupBox2.Controls.Add(this.btnMakeStepnc);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(11, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(392, 79);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "STEP-NC File";
            // 
            // lblReady
            // 
            this.lblReady.AutoSize = true;
            this.lblReady.Location = new System.Drawing.Point(317, 54);
            this.lblReady.MinimumSize = new System.Drawing.Size(60, 0);
            this.lblReady.Name = "lblReady";
            this.lblReady.Size = new System.Drawing.Size(60, 13);
            this.lblReady.TabIndex = 3;
            this.lblReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 174);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "apt2stepnc";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblBrowser;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnMakeStepnc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblReady;
    }
}

