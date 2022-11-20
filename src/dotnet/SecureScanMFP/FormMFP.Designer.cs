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
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.edtlog = new System.Windows.Forms.RichTextBox();
      this.panel3 = new System.Windows.Forms.Panel();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.labelState = new System.Windows.Forms.ToolStripStatusLabel();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel4.SuspendLayout();
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
      this.panel1.Size = new System.Drawing.Size(992, 173);
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
      this.splitContainer1.Size = new System.Drawing.Size(992, 173);
      this.splitContainer1.SplitterDistance = 753;
      this.splitContainer1.TabIndex = 0;
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.Controls.Add(this.buttonGO);
      this.flowLayoutPanel1.Controls.Add(this.buttonAbort);
      this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(753, 173);
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
      this.buttonGO.Text = "GO";
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
      this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
      this.flowLayoutPanel2.Size = new System.Drawing.Size(235, 173);
      this.flowLayoutPanel2.TabIndex = 1;
      // 
      // buttonSecureScan
      // 
      this.buttonSecureScan.BackColor = System.Drawing.Color.Yellow;
      this.buttonSecureScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonSecureScan.Location = new System.Drawing.Point(5, 3);
      this.buttonSecureScan.Name = "buttonSecureScan";
      this.buttonSecureScan.Size = new System.Drawing.Size(227, 164);
      this.buttonSecureScan.TabIndex = 1;
      this.buttonSecureScan.Text = "Initiate Secure Scan";
      this.buttonSecureScan.UseVisualStyleBackColor = false;
      this.buttonSecureScan.Click += new System.EventHandler(this.buttonSecureScan_Click);
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.panel4);
      this.panel2.Controls.Add(this.panel3);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 173);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(992, 449);
      this.panel2.TabIndex = 1;
      // 
      // panel4
      // 
      this.panel4.Controls.Add(this.edtlog);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel4.Location = new System.Drawing.Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Padding = new System.Windows.Forms.Padding(16);
      this.panel4.Size = new System.Drawing.Size(992, 417);
      this.panel4.TabIndex = 2;
      // 
      // edtlog
      // 
      this.edtlog.BackColor = System.Drawing.SystemColors.Control;
      this.edtlog.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.edtlog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.edtlog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.edtlog.Location = new System.Drawing.Point(16, 16);
      this.edtlog.Name = "edtlog";
      this.edtlog.Size = new System.Drawing.Size(960, 385);
      this.edtlog.TabIndex = 0;
      this.edtlog.Text = "";
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.statusStrip1);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel3.Location = new System.Drawing.Point(0, 417);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(992, 32);
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
      this.statusStrip1.Size = new System.Drawing.Size(992, 30);
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
      // FormMFP
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(992, 622);
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
      this.panel4.ResumeLayout(false);
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
    }
}