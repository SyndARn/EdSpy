namespace ARnEdSpy
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
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tbRss = new System.Windows.Forms.TextBox();
            this.lblEURUSD = new System.Windows.Forms.Label();
            this.lblEURAUD = new System.Windows.Forms.Label();
            this.lblEURGBP = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(27, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "EDDN Network";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(142, 26);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(186, 212);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(334, 26);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(970, 212);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Msg Qtt";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(39, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Build JSON";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(39, 351);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Yahoo Feed";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbRss
            // 
            this.tbRss.Location = new System.Drawing.Point(153, 288);
            this.tbRss.Multiline = true;
            this.tbRss.Name = "tbRss";
            this.tbRss.Size = new System.Drawing.Size(394, 290);
            this.tbRss.TabIndex = 6;
            // 
            // lblEURUSD
            // 
            this.lblEURUSD.AutoSize = true;
            this.lblEURUSD.Location = new System.Drawing.Point(620, 309);
            this.lblEURUSD.Name = "lblEURUSD";
            this.lblEURUSD.Size = new System.Drawing.Size(63, 13);
            this.lblEURUSD.TabIndex = 7;
            this.lblEURUSD.Text = "lblEURUSD";
            // 
            // lblEURAUD
            // 
            this.lblEURAUD.AutoSize = true;
            this.lblEURAUD.Location = new System.Drawing.Point(620, 391);
            this.lblEURAUD.Name = "lblEURAUD";
            this.lblEURAUD.Size = new System.Drawing.Size(63, 13);
            this.lblEURAUD.TabIndex = 8;
            this.lblEURAUD.Text = "lblEURAUD";
            // 
            // lblEURGBP
            // 
            this.lblEURGBP.AutoSize = true;
            this.lblEURGBP.Location = new System.Drawing.Point(620, 459);
            this.lblEURGBP.Name = "lblEURGBP";
            this.lblEURGBP.Size = new System.Drawing.Size(62, 13);
            this.lblEURGBP.TabIndex = 9;
            this.lblEURGBP.Text = "lblEURGBP";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(39, 166);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "Actor Observable";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1316, 590);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.lblEURGBP);
            this.Controls.Add(this.lblEURAUD);
            this.Controls.Add(this.lblEURUSD);
            this.Controls.Add(this.tbRss);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbRss;
        private System.Windows.Forms.Label lblEURUSD;
        private System.Windows.Forms.Label lblEURAUD;
        private System.Windows.Forms.Label lblEURGBP;
        private System.Windows.Forms.Button button4;
    }
}

