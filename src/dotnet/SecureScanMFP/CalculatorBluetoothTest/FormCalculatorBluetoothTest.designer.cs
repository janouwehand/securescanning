namespace SecureScanMFP.CalculatorBluetoothTest
{
  partial class FormCalculatorBluetoothTest
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
      this.label4 = new System.Windows.Forms.Label();
      this.ResultText = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.Operand2Text = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.Operand1Text = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.OperationText = new System.Windows.Forms.TextBox();
      this.edtlog = new System.Windows.Forms.RichTextBox();
      this.PublishButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(309, 155);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(32, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "result";
      // 
      // ResultText
      // 
      this.ResultText.Location = new System.Drawing.Point(381, 152);
      this.ResultText.Name = "ResultText";
      this.ResultText.Size = new System.Drawing.Size(132, 20);
      this.ResultText.TabIndex = 18;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(309, 111);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 17;
      this.label3.Text = "operand 2";
      // 
      // Operand2Text
      // 
      this.Operand2Text.Location = new System.Drawing.Point(381, 108);
      this.Operand2Text.Name = "Operand2Text";
      this.Operand2Text.Size = new System.Drawing.Size(132, 20);
      this.Operand2Text.TabIndex = 16;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(309, 74);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "operand 1";
      // 
      // Operand1Text
      // 
      this.Operand1Text.Location = new System.Drawing.Point(381, 71);
      this.Operand1Text.Name = "Operand1Text";
      this.Operand1Text.Size = new System.Drawing.Size(132, 20);
      this.Operand1Text.TabIndex = 14;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(309, 39);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(51, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "operation";
      // 
      // OperationText
      // 
      this.OperationText.Location = new System.Drawing.Point(381, 36);
      this.OperationText.Name = "OperationText";
      this.OperationText.Size = new System.Drawing.Size(132, 20);
      this.OperationText.TabIndex = 12;
      // 
      // edtlog
      // 
      this.edtlog.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.edtlog.Location = new System.Drawing.Point(0, 217);
      this.edtlog.Name = "edtlog";
      this.edtlog.Size = new System.Drawing.Size(1011, 333);
      this.edtlog.TabIndex = 11;
      this.edtlog.Text = "";
      // 
      // PublishButton
      // 
      this.PublishButton.Location = new System.Drawing.Point(26, 14);
      this.PublishButton.Name = "PublishButton";
      this.PublishButton.Size = new System.Drawing.Size(135, 62);
      this.PublishButton.TabIndex = 10;
      this.PublishButton.Text = "Start";
      this.PublishButton.UseVisualStyleBackColor = true;
      this.PublishButton.Click += new System.EventHandler(this.PublishButton_Click);
      // 
      // FormCalculatorBluetoothTest
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1011, 550);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.ResultText);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.Operand2Text);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.Operand1Text);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.OperationText);
      this.Controls.Add(this.edtlog);
      this.Controls.Add(this.PublishButton);
      this.Name = "FormCalculatorBluetoothTest";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Calculator Bluetooth Test";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ResultText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Operand2Text;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Operand1Text;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox OperationText;
        private System.Windows.Forms.RichTextBox edtlog;
        private System.Windows.Forms.Button PublishButton;
    }
}