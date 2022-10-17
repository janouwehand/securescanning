namespace SecureScan.Base.WaitForm
{
  partial class FormWaitModal
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWaitModal));
      this.panel1 = new System.Windows.Forms.Panel();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.labelText = new System.Windows.Forms.Label();
      this.buttonAbort = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.buttonAbort);
      this.panel1.Controls.Add(this.labelText);
      this.panel1.Controls.Add(this.pictureBox1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(8, 8);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(615, 110);
      this.panel1.TabIndex = 0;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(16, 21);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(92, 70);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // labelText
      // 
      this.labelText.AutoSize = true;
      this.labelText.Location = new System.Drawing.Point(133, 53);
      this.labelText.Name = "labelText";
      this.labelText.Size = new System.Drawing.Size(41, 13);
      this.labelText.TabIndex = 1;
      this.labelText.Text = "Wait ...";
      // 
      // buttonAbort
      // 
      this.buttonAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAbort.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonAbort.Location = new System.Drawing.Point(491, 39);
      this.buttonAbort.Name = "buttonAbort";
      this.buttonAbort.Size = new System.Drawing.Size(108, 41);
      this.buttonAbort.TabIndex = 2;
      this.buttonAbort.Text = "Abort";
      this.buttonAbort.UseVisualStyleBackColor = true;
      this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
      // 
      // FormWaitModal
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.SkyBlue;
      this.CancelButton = this.buttonAbort;
      this.ClientSize = new System.Drawing.Size(631, 126);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FormWaitModal";
      this.Padding = new System.Windows.Forms.Padding(8);
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "FormWaitModal";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label labelText;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Button buttonAbort;
  }
}