namespace SecureScanTool
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
      this.buttonScan = new System.Windows.Forms.Button();
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.buttonScan2 = new System.Windows.Forms.Button();
      this.buttonFindPaired = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonScan
      // 
      this.buttonScan.Location = new System.Drawing.Point(12, 12);
      this.buttonScan.Name = "buttonScan";
      this.buttonScan.Size = new System.Drawing.Size(111, 31);
      this.buttonScan.TabIndex = 0;
      this.buttonScan.Text = "Scan devices";
      this.buttonScan.UseVisualStyleBackColor = true;     
      // 
      // richTextBox1
      // 
      this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.richTextBox1.Location = new System.Drawing.Point(12, 60);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.Size = new System.Drawing.Size(776, 378);
      this.richTextBox1.TabIndex = 1;
      this.richTextBox1.Text = "";
      // 
      // buttonScan2
      // 
      this.buttonScan2.Location = new System.Drawing.Point(129, 12);
      this.buttonScan2.Name = "buttonScan2";
      this.buttonScan2.Size = new System.Drawing.Size(111, 31);
      this.buttonScan2.TabIndex = 2;
      this.buttonScan2.Text = "Scan devices 2";
      this.buttonScan2.UseVisualStyleBackColor = true;
      this.buttonScan2.Click += new System.EventHandler(this.buttonScan2_Click);
      // 
      // buttonFindPaired
      // 
      this.buttonFindPaired.Location = new System.Drawing.Point(246, 12);
      this.buttonFindPaired.Name = "buttonFindPaired";
      this.buttonFindPaired.Size = new System.Drawing.Size(111, 31);
      this.buttonFindPaired.TabIndex = 3;
      this.buttonFindPaired.Text = "Find paired devices";
      this.buttonFindPaired.UseVisualStyleBackColor = true;
      this.buttonFindPaired.Click += new System.EventHandler(this.buttonFindPaired_Click);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(572, 12);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(111, 31);
      this.button1.TabIndex = 4;
      this.button1.Text = "Find paired devices";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.buttonFindPaired);
      this.Controls.Add(this.buttonScan2);
      this.Controls.Add(this.richTextBox1);
      this.Controls.Add(this.buttonScan);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Secure Scan Tool";
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonScan2;
        private System.Windows.Forms.Button buttonFindPaired;
        private System.Windows.Forms.Button button1;
    }
}

