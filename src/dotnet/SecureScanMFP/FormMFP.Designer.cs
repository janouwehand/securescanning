namespace SecureScanMFP
{
  partial class FormMFP
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
      this.buttonPlaceDocument = new System.Windows.Forms.Button();
      this.labelDocumentFileName = new System.Windows.Forms.Label();
      this.buttonReset = new System.Windows.Forms.Button();
      this.buttonInitiateSecureScanProcess = new System.Windows.Forms.Button();
      this.labelBluetoothStatus = new System.Windows.Forms.Label();
      this.edtlog = new System.Windows.Forms.RichTextBox();
      this.SuspendLayout();
      // 
      // buttonPlaceDocument
      // 
      this.buttonPlaceDocument.Location = new System.Drawing.Point(33, 29);
      this.buttonPlaceDocument.Name = "buttonPlaceDocument";
      this.buttonPlaceDocument.Size = new System.Drawing.Size(169, 70);
      this.buttonPlaceDocument.TabIndex = 0;
      this.buttonPlaceDocument.Text = "1. Place document on flatbed";
      this.buttonPlaceDocument.UseVisualStyleBackColor = true;
      this.buttonPlaceDocument.Click += new System.EventHandler(this.buttonPlaceDocument_Click);
      // 
      // labelDocumentFileName
      // 
      this.labelDocumentFileName.AutoSize = true;
      this.labelDocumentFileName.Location = new System.Drawing.Point(217, 58);
      this.labelDocumentFileName.Name = "labelDocumentFileName";
      this.labelDocumentFileName.Size = new System.Drawing.Size(10, 13);
      this.labelDocumentFileName.TabIndex = 1;
      this.labelDocumentFileName.Text = " ";
      // 
      // buttonReset
      // 
      this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonReset.Location = new System.Drawing.Point(33, 356);
      this.buttonReset.Name = "buttonReset";
      this.buttonReset.Size = new System.Drawing.Size(169, 70);
      this.buttonReset.TabIndex = 2;
      this.buttonReset.Text = "Reset input";
      this.buttonReset.UseVisualStyleBackColor = true;
      this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
      // 
      // buttonInitiateSecureScanProcess
      // 
      this.buttonInitiateSecureScanProcess.Enabled = false;
      this.buttonInitiateSecureScanProcess.Location = new System.Drawing.Point(33, 105);
      this.buttonInitiateSecureScanProcess.Name = "buttonInitiateSecureScanProcess";
      this.buttonInitiateSecureScanProcess.Size = new System.Drawing.Size(169, 70);
      this.buttonInitiateSecureScanProcess.TabIndex = 3;
      this.buttonInitiateSecureScanProcess.Text = "2. Initiate Secure Scanning";
      this.buttonInitiateSecureScanProcess.UseVisualStyleBackColor = true;
      this.buttonInitiateSecureScanProcess.Click += new System.EventHandler(this.buttonInitiateSecureScanProcess_Click);
      // 
      // labelBluetoothStatus
      // 
      this.labelBluetoothStatus.AutoSize = true;
      this.labelBluetoothStatus.Location = new System.Drawing.Point(217, 134);
      this.labelBluetoothStatus.Name = "labelBluetoothStatus";
      this.labelBluetoothStatus.Size = new System.Drawing.Size(10, 13);
      this.labelBluetoothStatus.TabIndex = 4;
      this.labelBluetoothStatus.Text = " ";
      // 
      // edtlog
      // 
      this.edtlog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.edtlog.Location = new System.Drawing.Point(33, 193);
      this.edtlog.Name = "edtlog";
      this.edtlog.Size = new System.Drawing.Size(596, 144);
      this.edtlog.TabIndex = 5;
      this.edtlog.Text = "";
      // 
      // FormMFP
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(665, 438);
      this.Controls.Add(this.edtlog);
      this.Controls.Add(this.labelBluetoothStatus);
      this.Controls.Add(this.buttonInitiateSecureScanProcess);
      this.Controls.Add(this.buttonReset);
      this.Controls.Add(this.labelDocumentFileName);
      this.Controls.Add(this.buttonPlaceDocument);
      this.Name = "FormMFP";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Secure Scanning MFP";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Button buttonPlaceDocument;
        private System.Windows.Forms.Label labelDocumentFileName;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonInitiateSecureScanProcess;
        private System.Windows.Forms.Label labelBluetoothStatus;
        private System.Windows.Forms.RichTextBox edtlog;
    }
}