namespace SecureScanOutlookAddIn
{
  partial class FormSettings
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
      this.panel1 = new System.Windows.Forms.Panel();
      this.buttonOK = new System.Windows.Forms.Button();
      this.panel2 = new System.Windows.Forms.Panel();
      this.labelCertificaat = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.listBoxDevices = new System.Windows.Forms.ListBox();
      this.buttonPairNew = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 449);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(777, 48);
      this.panel1.TabIndex = 0;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonOK.Location = new System.Drawing.Point(664, 6);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(105, 34);
      this.buttonOK.TabIndex = 0;
      this.buttonOK.Text = "Close";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.buttonPairNew);
      this.panel2.Controls.Add(this.listBoxDevices);
      this.panel2.Controls.Add(this.label2);
      this.panel2.Controls.Add(this.labelCertificaat);
      this.panel2.Controls.Add(this.label1);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(777, 449);
      this.panel2.TabIndex = 1;
      // 
      // labelCertificaat
      // 
      this.labelCertificaat.Location = new System.Drawing.Point(27, 32);
      this.labelCertificaat.Name = "labelCertificaat";
      this.labelCertificaat.Size = new System.Drawing.Size(576, 233);
      this.labelCertificaat.TabIndex = 1;
      this.labelCertificaat.Text = "none";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(65, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Certificaat";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(12, 283);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(118, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Paired BLE devices";
      // 
      // listBoxDevices
      // 
      this.listBoxDevices.FormattingEnabled = true;
      this.listBoxDevices.Location = new System.Drawing.Point(30, 311);
      this.listBoxDevices.Name = "listBoxDevices";
      this.listBoxDevices.Size = new System.Drawing.Size(358, 108);
      this.listBoxDevices.TabIndex = 3;
      // 
      // buttonPairNew
      // 
      this.buttonPairNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonPairNew.Location = new System.Drawing.Point(394, 311);
      this.buttonPairNew.Name = "buttonPairNew";
      this.buttonPairNew.Size = new System.Drawing.Size(105, 34);
      this.buttonPairNew.TabIndex = 4;
      this.buttonPairNew.Text = "Pair smartphone";
      this.buttonPairNew.UseVisualStyleBackColor = true;
      this.buttonPairNew.Click += new System.EventHandler(this.buttonPairNew_Click);
      // 
      // FormSettings
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonOK;
      this.ClientSize = new System.Drawing.Size(777, 497);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormSettings";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Secure Scan Outlook Extension Settings";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Label labelCertificaat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxDevices;
        private System.Windows.Forms.Button buttonPairNew;
    }
}