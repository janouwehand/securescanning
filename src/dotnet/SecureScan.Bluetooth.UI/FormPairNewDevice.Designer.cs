namespace SecureScan.Bluetooth.UI
{
  partial class FormPairNewDevice
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
      this.panel3 = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.panel5 = new System.Windows.Forms.Panel();
      this.panel6 = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.panel7 = new System.Windows.Forms.Panel();
      this.panel8 = new System.Windows.Forms.Panel();
      this.flow = new System.Windows.Forms.FlowLayoutPanel();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel6.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.panel7.SuspendLayout();
      this.panel8.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 402);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(800, 48);
      this.panel1.TabIndex = 2;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonOK.Location = new System.Drawing.Point(687, 6);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(105, 34);
      this.buttonOK.TabIndex = 0;
      this.buttonOK.Text = "Close";
      this.buttonOK.UseVisualStyleBackColor = true;
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.panel3);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(800, 450);
      this.panel2.TabIndex = 3;
      // 
      // panel3
      // 
      this.panel3.Controls.Add(this.panel4);
      this.panel3.Controls.Add(this.panel1);
      this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel3.Location = new System.Drawing.Point(0, 0);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(800, 450);
      this.panel3.TabIndex = 0;
      // 
      // panel4
      // 
      this.panel4.Controls.Add(this.panel6);
      this.panel4.Controls.Add(this.panel5);
      this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel4.Location = new System.Drawing.Point(0, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(800, 402);
      this.panel4.TabIndex = 3;
      // 
      // panel5
      // 
      this.panel5.BackColor = System.Drawing.SystemColors.Info;
      this.panel5.Controls.Add(this.label1);
      this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel5.Location = new System.Drawing.Point(0, 0);
      this.panel5.Name = "panel5";
      this.panel5.Size = new System.Drawing.Size(800, 76);
      this.panel5.TabIndex = 0;
      // 
      // panel6
      // 
      this.panel6.Controls.Add(this.panel8);
      this.panel6.Controls.Add(this.panel7);
      this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel6.Location = new System.Drawing.Point(0, 76);
      this.panel6.Name = "panel6";
      this.panel6.Size = new System.Drawing.Size(800, 326);
      this.panel6.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(22, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(264, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Scanning for BLE devices with Secure Scan capability";
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::SecureScan.Bluetooth.UI.Properties.Resources.wait3;
      this.pictureBox1.Location = new System.Drawing.Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(32, 62);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      // 
      // panel7
      // 
      this.panel7.BackColor = System.Drawing.Color.White;
      this.panel7.Controls.Add(this.pictureBox1);
      this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
      this.panel7.Location = new System.Drawing.Point(0, 0);
      this.panel7.Name = "panel7";
      this.panel7.Size = new System.Drawing.Size(52, 326);
      this.panel7.TabIndex = 2;
      // 
      // panel8
      // 
      this.panel8.Controls.Add(this.flow);
      this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel8.Location = new System.Drawing.Point(52, 0);
      this.panel8.Name = "panel8";
      this.panel8.Size = new System.Drawing.Size(748, 326);
      this.panel8.TabIndex = 3;
      // 
      // flow
      // 
      this.flow.Dock = System.Windows.Forms.DockStyle.Fill;
      this.flow.Location = new System.Drawing.Point(0, 0);
      this.flow.Name = "flow";
      this.flow.Padding = new System.Windows.Forms.Padding(8);
      this.flow.Size = new System.Drawing.Size(748, 326);
      this.flow.TabIndex = 0;
      // 
      // FormPairNewDevice
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Controls.Add(this.panel2);
      this.Name = "FormPairNewDevice";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Pair smartphone";
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panel5.PerformLayout();
      this.panel6.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.panel7.ResumeLayout(false);
      this.panel8.ResumeLayout(false);
      this.ResumeLayout(false);

    }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flow;
    }
}