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
      this.panel1 = new System.Windows.Forms.Panel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.buttonGO = new System.Windows.Forms.Button();
      this.buttonAbort = new System.Windows.Forms.Button();
      this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
      this.buttonSecureScan = new System.Windows.Forms.Button();
      this.buttonEnrollSmartphone = new System.Windows.Forms.Button();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel6 = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.panel8 = new System.Windows.Forms.Panel();
      this.edtlog = new System.Windows.Forms.RichTextBox();
      this.panel7 = new System.Windows.Forms.Panel();
      this.panel10 = new System.Windows.Forms.Panel();
      this.panel9 = new System.Windows.Forms.Panel();
      this.panel5 = new System.Windows.Forms.Panel();
      this.labelPDF = new System.Windows.Forms.Label();
      this.buttonChoosePDF = new System.Windows.Forms.Button();
      this.panel3 = new System.Windows.Forms.Panel();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.labelState = new System.Windows.Forms.ToolStripStatusLabel();
      this.qrControl1 = new SecureScanMFP.QRControl();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel6.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel8.SuspendLayout();
      this.panel7.SuspendLayout();
      this.panel9.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel3.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.splitContainer1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1314, 173);
      this.panel1.TabIndex = 0;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel2);
      this.splitContainer1.Size = new System.Drawing.Size(1314, 173);
      this.splitContainer1.SplitterDistance = 700;
      this.splitContainer1.TabIndex = 0;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.buttonGO);
      this.flowLayoutPanel1.Controls.Add(this.buttonAbort);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(700, 173);
      this.flowLayoutPanel1.TabIndex = 0;
      // 
      // buttonGO
      // 
      this.buttonGO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
      this.buttonGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonGO.Location = new System.Drawing.Point(3, 3);
      this.buttonGO.Name = "buttonGO";
      this.buttonGO.Size = new System.Drawing.Size(227, 164);
      this.buttonGO.TabIndex = 0;
      this.buttonGO.Text = "Start";
      this.buttonGO.UseVisualStyleBackColor = false;
      this.buttonGO.Click += new System.EventHandler(this.buttonGO_Click);
      // 
      // buttonAbort
      // 
      this.buttonAbort.BackColor = System.Drawing.Color.Red;
      this.buttonAbort.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonAbort.Location = new System.Drawing.Point(236, 3);
      this.buttonAbort.Name = "buttonAbort";
      this.buttonAbort.Size = new System.Drawing.Size(227, 164);
      this.buttonAbort.TabIndex = 1;
      this.buttonAbort.Text = "Abort";
      this.buttonAbort.UseVisualStyleBackColor = false;
      this.buttonAbort.Click += new System.EventHandler(this.buttonAbort_Click);
      // 
      // flowLayoutPanel2
      // 
      this.flowLayoutPanel2.Controls.Add(this.buttonSecureScan);
      this.flowLayoutPanel2.Controls.Add(this.buttonEnrollSmartphone);
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
      this.flowLayoutPanel2.Size = new System.Drawing.Size(610, 173);
      this.flowLayoutPanel2.TabIndex = 1;
      // 
      // buttonSecureScan
      // 
      this.buttonSecureScan.BackColor = System.Drawing.Color.Yellow;
      this.buttonSecureScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonSecureScan.Location = new System.Drawing.Point(380, 3);
      this.buttonSecureScan.Name = "buttonSecureScan";
      this.buttonSecureScan.Size = new System.Drawing.Size(227, 164);
      this.buttonSecureScan.TabIndex = 1;
      this.buttonSecureScan.Text = "Initiate Secure Scan";
      this.buttonSecureScan.UseVisualStyleBackColor = false;
      this.buttonSecureScan.Visible = false;
      this.buttonSecureScan.Click += new System.EventHandler(this.buttonSecureScan_Click);
      // 
      // buttonEnrollSmartphone
      // 
      this.buttonEnrollSmartphone.BackColor = System.Drawing.Color.Yellow;
      this.buttonEnrollSmartphone.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonEnrollSmartphone.Location = new System.Drawing.Point(147, 3);
      this.buttonEnrollSmartphone.Name = "buttonEnrollSmartphone";
      this.buttonEnrollSmartphone.Size = new System.Drawing.Size(227, 164);
      this.buttonEnrollSmartphone.TabIndex = 2;
      this.buttonEnrollSmartphone.Text = "Enroll + Scan";
      this.buttonEnrollSmartphone.UseVisualStyleBackColor = false;
      this.buttonEnrollSmartphone.Click += new System.EventHandler(this.buttonEnrollSmartphone_Click);
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.panel6);
      this.panel2.Controls.Add(this.panel5);
      this.panel2.Controls.Add(this.panel3);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 173);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(1314, 837);
      this.panel2.TabIndex = 1;
      // 
      // panel6
      // 
      this.panel6.Controls.Add(this.panel4);
      this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel6.Location = new System.Drawing.Point(0, 48);
      this.panel6.Name = "panel6";
      this.panel6.Size = new System.Drawing.Size(1314, 757);
      this.panel6.TabIndex = 4;
      // 
      // panel4
      // 
      this.panel4.Controls.Add(this.panel8);
      this.panel4.Controls.Add(this.panel7);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel4.Location = new System.Drawing.Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Padding = new System.Windows.Forms.Padding(16);
      this.panel4.Size = new System.Drawing.Size(1314, 757);
      this.panel4.TabIndex = 2;
      // 
      // panel8
      // 
      this.panel8.Controls.Add(this.edtlog);
      this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel8.Location = new System.Drawing.Point(16, 16);
      this.panel8.Name = "panel8";
      this.panel8.Size = new System.Drawing.Size(1054, 725);
      this.panel8.TabIndex = 2;
      // 
      // edtlog
      // 
      this.edtlog.BackColor = System.Drawing.SystemColors.Control;
      this.edtlog.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.edtlog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.edtlog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.edtlog.Location = new System.Drawing.Point(0, 0);
      this.edtlog.Name = "edtlog";
      this.edtlog.Size = new System.Drawing.Size(1054, 725);
      this.edtlog.TabIndex = 0;
      this.edtlog.Text = "";
      // 
      // panel7
      // 
      this.panel7.Controls.Add(this.panel10);
      this.panel7.Controls.Add(this.panel9);
      this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel7.Location = new System.Drawing.Point(1070, 16);
      this.panel7.Name = "panel7";
      this.panel7.Size = new System.Drawing.Size(228, 725);
      this.panel7.TabIndex = 1;
      // 
      // panel10
      // 
      this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel10.Location = new System.Drawing.Point(0, 222);
      this.panel10.Name = "panel10";
      this.panel10.Size = new System.Drawing.Size(228, 503);
      this.panel10.TabIndex = 1;
      // 
      // panel9
      // 
      this.panel9.Controls.Add(this.qrControl1);
      this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel9.Location = new System.Drawing.Point(0, 0);
      this.panel9.Name = "panel9";
      this.panel9.Size = new System.Drawing.Size(228, 222);
      this.panel9.TabIndex = 0;
      // 
      // panel5
      // 
      this.panel5.Controls.Add(this.labelPDF);
      this.panel5.Controls.Add(this.buttonChoosePDF);
      this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel5.Location = new System.Drawing.Point(0, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new System.Drawing.Size(1314, 48);
      this.panel5.TabIndex = 3;
      // 
      // labelPDF
      // 
      this.labelPDF.AutoSize = true;
      this.labelPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelPDF.Location = new System.Drawing.Point(236, 14);
      this.labelPDF.Name = "labelPDF";
      this.labelPDF.Size = new System.Drawing.Size(186, 20);
      this.labelPDF.TabIndex = 1;
      this.labelPDF.Text = "Standaard testdocument";
      // 
      // buttonChoosePDF
      // 
      this.buttonChoosePDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonChoosePDF.Location = new System.Drawing.Point(3, 6);
      this.buttonChoosePDF.Name = "buttonChoosePDF";
      this.buttonChoosePDF.Size = new System.Drawing.Size(227, 36);
      this.buttonChoosePDF.TabIndex = 0;
      this.buttonChoosePDF.Text = "Choose PDF";
      this.buttonChoosePDF.UseVisualStyleBackColor = true;
      this.buttonChoosePDF.Click += new System.EventHandler(this.buttonChoosePDF_Click);
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.statusStrip1);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel3.Location = new System.Drawing.Point(0, 805);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(1314, 32);
      this.panel3.TabIndex = 1;
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.labelState});
      this.statusStrip1.Location = new System.Drawing.Point(0, 2);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
      this.statusStrip1.Size = new System.Drawing.Size(1314, 30);
      this.statusStrip1.TabIndex = 0;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(57, 25);
      this.toolStripStatusLabel1.Text = "State:";
      // 
      // labelState
      // 
      this.labelState.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelState.Name = "labelState";
      this.labelState.Size = new System.Drawing.Size(43, 25);
      this.labelState.Text = "Idle";
      // 
      // qrControl1
      // 
      this.qrControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.qrControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.qrControl1.Location = new System.Drawing.Point(0, 0);
      this.qrControl1.Name = "qrControl1";
      this.qrControl1.Size = new System.Drawing.Size(228, 222);
      this.qrControl1.TabIndex = 0;
      this.qrControl1.Visible = false;
      // 
      // FormMFP
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1314, 1010);
      this.Controls.Add(this.panel2);
      this.Controls.Add(this.panel1);
      this.Name = "FormMFP";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Secure Scanning MFP";
      this.panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel8.ResumeLayout(false);
      this.panel7.ResumeLayout(false);
      this.panel9.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonGO;
        private System.Windows.Forms.Button buttonAbort;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttonSecureScan;
        private System.Windows.Forms.RichTextBox edtlog;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel labelState;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button buttonChoosePDF;
        private System.Windows.Forms.Label labelPDF;
    private System.Windows.Forms.Button buttonEnrollSmartphone;
    private System.Windows.Forms.Panel panel8;
    private System.Windows.Forms.Panel panel7;
    private System.Windows.Forms.Panel panel10;
    private System.Windows.Forms.Panel panel9;
    private QRControl qrControl1;
  }
}