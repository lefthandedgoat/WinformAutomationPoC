namespace Winforms
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
            this.button1 = new System.Windows.Forms.Button();
            this.testLabel = new System.Windows.Forms.Label();
            this.buttonClickLabel = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(44, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Click Me!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(41, 9);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(54, 13);
            this.testLabel.TabIndex = 1;
            this.testLabel.Text = "Read Me!";
            // 
            // buttonClickLabel
            // 
            this.buttonClickLabel.AutoSize = true;
            this.buttonClickLabel.Location = new System.Drawing.Point(41, 87);
            this.buttonClickLabel.Name = "buttonClickLabel";
            this.buttonClickLabel.Size = new System.Drawing.Size(96, 13);
            this.buttonClickLabel.TabIndex = 2;
            this.buttonClickLabel.Text = "Button Not Clicked";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(44, 130);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(100, 20);
            this.textBox.TabIndex = 3;
            this.textBox.Text = "Default Text";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 328);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.buttonClickLabel);
            this.Controls.Add(this.testLabel);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Test Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label testLabel;
        private System.Windows.Forms.Label buttonClickLabel;
        private System.Windows.Forms.TextBox textBox;
    }
}

