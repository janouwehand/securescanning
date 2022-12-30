namespace SecureScan.Bluetooth.UI
{
  partial class UcAdvertisement
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
      this.labelName = new System.Windows.Forms.Label();
      this.labelAddress = new System.Windows.Forms.Label();
      this.buttonPair = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(18, 12);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(57, 13);
      this.labelName.TabIndex = 0;
      this.labelName.Text = "labelName";
      // 
      // labelAddress
      // 
      this.labelAddress.AutoSize = true;
      this.labelAddress.Location = new System.Drawing.Point(18, 34);
      this.labelAddress.Name = "labelAddress";
      this.labelAddress.Size = new System.Drawing.Size(67, 13);
      this.labelAddress.TabIndex = 1;
      this.labelAddress.Text = "labelAddress";
      // 
      // buttonPair
      // 
      this.buttonPair.Location = new System.Drawing.Point(21, 59);
      this.buttonPair.Name = "buttonPair";
      this.buttonPair.Size = new System.Drawing.Size(162, 23);
      this.buttonPair.TabIndex = 2;
      this.buttonPair.Text = "Pair device";
      this.buttonPair.UseVisualStyleBackColor = true;
      this.buttonPair.Click += new System.EventHandler(this.buttonPair_Click);
      // 
      // UcAdvertisement
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.buttonPair);
      this.Controls.Add(this.labelAddress);
      this.Controls.Add(this.labelName);
      this.Cursor = System.Windows.Forms.Cursors.Default;
      this.Name = "UcAdvertisement";
      this.Size = new System.Drawing.Size(204, 94);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelAddress;
    private System.Windows.Forms.Button buttonPair;
  }
}
