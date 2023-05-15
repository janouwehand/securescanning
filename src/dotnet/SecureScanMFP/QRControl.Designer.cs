namespace SecureScanMFP
{
  partial class QRControl
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.pictureBoxQR = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBoxQR
      // 
      this.pictureBoxQR.BackColor = System.Drawing.Color.White;
      this.pictureBoxQR.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBoxQR.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxQR.Name = "pictureBoxQR";
      this.pictureBoxQR.Size = new System.Drawing.Size(308, 299);
      this.pictureBoxQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.pictureBoxQR.TabIndex = 1;
      this.pictureBoxQR.TabStop = false;
      // 
      // QRControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.pictureBoxQR);
      this.Name = "QRControl";
      this.Size = new System.Drawing.Size(308, 299);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBoxQR;
  }
}
