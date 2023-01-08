namespace SecureScanOutlookAddIn
{
  partial class FormDocumentInfo
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
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.labelName = new System.Windows.Forms.Label();
      this.labelSHA1 = new System.Windows.Forms.Label();
      this.labelTimestamp = new System.Windows.Forms.Label();
      this.labelMFP = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.labelSSSHA1 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.labelSSFileName = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.labelContentType = new System.Windows.Forms.Label();
      this.label2353 = new System.Windows.Forms.Label();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 342);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(468, 48);
      this.panel1.TabIndex = 1;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonOK.Location = new System.Drawing.Point(355, 6);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(105, 34);
      this.buttonOK.TabIndex = 0;
      this.buttonOK.Text = "Close";
      this.buttonOK.UseVisualStyleBackColor = true;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.labelContentType);
      this.panel2.Controls.Add(this.label2353);
      this.panel2.Controls.Add(this.labelSSFileName);
      this.panel2.Controls.Add(this.label10);
      this.panel2.Controls.Add(this.labelSSSHA1);
      this.panel2.Controls.Add(this.label9);
      this.panel2.Controls.Add(this.label7);
      this.panel2.Controls.Add(this.labelTimestamp);
      this.panel2.Controls.Add(this.labelMFP);
      this.panel2.Controls.Add(this.labelSHA1);
      this.panel2.Controls.Add(this.labelName);
      this.panel2.Controls.Add(this.label5);
      this.panel2.Controls.Add(this.label6);
      this.panel2.Controls.Add(this.label4);
      this.panel2.Controls.Add(this.label3);
      this.panel2.Controls.Add(this.label2);
      this.panel2.Controls.Add(this.label1);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(468, 342);
      this.panel2.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(21, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(62, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Originator";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(40, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(46, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Subject:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(40, 70);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "SHA1:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(21, 108);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(114, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Document creation";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(40, 157);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(61, 13);
      this.label5.TabIndex = 6;
      this.label5.Text = "Timestamp:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.Location = new System.Drawing.Point(40, 135);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(81, 13);
      this.label6.TabIndex = 5;
      this.label6.Text = "MFP hostname:";
      // 
      // labelName
      // 
      this.labelName.AutoSize = true;
      this.labelName.Location = new System.Drawing.Point(163, 48);
      this.labelName.Name = "labelName";
      this.labelName.Size = new System.Drawing.Size(57, 13);
      this.labelName.TabIndex = 7;
      this.labelName.Text = "labelName";
      // 
      // labelSHA1
      // 
      this.labelSHA1.AutoSize = true;
      this.labelSHA1.Location = new System.Drawing.Point(163, 70);
      this.labelSHA1.Name = "labelSHA1";
      this.labelSHA1.Size = new System.Drawing.Size(57, 13);
      this.labelSHA1.TabIndex = 8;
      this.labelSHA1.Text = "labelSHA1";
      // 
      // labelTimestamp
      // 
      this.labelTimestamp.AutoSize = true;
      this.labelTimestamp.Location = new System.Drawing.Point(163, 157);
      this.labelTimestamp.Name = "labelTimestamp";
      this.labelTimestamp.Size = new System.Drawing.Size(80, 13);
      this.labelTimestamp.TabIndex = 10;
      this.labelTimestamp.Text = "labelTimestamp";
      // 
      // labelMFP
      // 
      this.labelMFP.AutoSize = true;
      this.labelMFP.Location = new System.Drawing.Point(163, 135);
      this.labelMFP.Name = "labelMFP";
      this.labelMFP.Size = new System.Drawing.Size(51, 13);
      this.labelMFP.TabIndex = 9;
      this.labelMFP.Text = "labelMFP";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.Location = new System.Drawing.Point(21, 198);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(106, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Secure document";
      // 
      // labelSSSHA1
      // 
      this.labelSSSHA1.AutoSize = true;
      this.labelSSSHA1.Location = new System.Drawing.Point(163, 276);
      this.labelSSSHA1.Name = "labelSSSHA1";
      this.labelSSSHA1.Size = new System.Drawing.Size(71, 13);
      this.labelSSSHA1.TabIndex = 13;
      this.labelSSSHA1.Text = "labelSSSHA1";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label9.Location = new System.Drawing.Point(40, 276);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(38, 13);
      this.label9.TabIndex = 12;
      this.label9.Text = "SHA1:";
      // 
      // labelSSFileName
      // 
      this.labelSSFileName.AutoSize = true;
      this.labelSSFileName.Location = new System.Drawing.Point(163, 227);
      this.labelSSFileName.Name = "labelSSFileName";
      this.labelSSFileName.Size = new System.Drawing.Size(87, 13);
      this.labelSSFileName.TabIndex = 15;
      this.labelSSFileName.Text = "labelSSFileName";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label10.Location = new System.Drawing.Point(40, 227);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(55, 13);
      this.label10.TabIndex = 14;
      this.label10.Text = "File name:";
      // 
      // labelContentType
      // 
      this.labelContentType.AutoSize = true;
      this.labelContentType.Location = new System.Drawing.Point(163, 251);
      this.labelContentType.Name = "labelContentType";
      this.labelContentType.Size = new System.Drawing.Size(90, 13);
      this.labelContentType.TabIndex = 17;
      this.labelContentType.Text = "labelContentType";
      // 
      // label2353
      // 
      this.label2353.AutoSize = true;
      this.label2353.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2353.Location = new System.Drawing.Point(40, 251);
      this.label2353.Name = "label2353";
      this.label2353.Size = new System.Drawing.Size(70, 13);
      this.label2353.TabIndex = 16;
      this.label2353.Text = "Content type:";
      // 
      // FormDocumentInfo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(468, 390);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "FormDocumentInfo";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Document info";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSHA1;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelTimestamp;
        private System.Windows.Forms.Label labelMFP;
        private System.Windows.Forms.Label labelSSSHA1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSSFileName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelContentType;
        private System.Windows.Forms.Label label2353;
    }
}