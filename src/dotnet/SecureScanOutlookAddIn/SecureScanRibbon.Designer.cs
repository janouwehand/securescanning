namespace SecureScanOutlookAddIn
{
  partial class SecureScanRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public SecureScanRibbon()
        : base(Globals.Factory.GetRibbonFactory())
    {
      InitializeComponent();
    }

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
      Microsoft.Office.Tools.Ribbon.RibbonDialogLauncher ribbonDialogLauncherImpl1 = this.Factory.CreateRibbonDialogLauncher();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecureScanRibbon));
      this.tabSecureScan = this.Factory.CreateRibbonTab();
      this.groupSecureScan = this.Factory.CreateRibbonGroup();
      this.buttonReadSecureDocument = this.Factory.CreateRibbonButton();
      this.tabSecureScan.SuspendLayout();
      this.groupSecureScan.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabSecureScan
      // 
      this.tabSecureScan.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
      this.tabSecureScan.ControlId.OfficeId = "TabMail";
      this.tabSecureScan.Groups.Add(this.groupSecureScan);
      this.tabSecureScan.Label = "TabMail";
      this.tabSecureScan.Name = "tabSecureScan";
      // 
      // groupSecureScan
      // 
      this.groupSecureScan.DialogLauncher = ribbonDialogLauncherImpl1;
      this.groupSecureScan.Items.Add(this.buttonReadSecureDocument);
      this.groupSecureScan.Label = "Secure Scan";
      this.groupSecureScan.Name = "groupSecureScan";
      this.groupSecureScan.DialogLauncherClick += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.groupSecureScan_DialogLauncherClick);
      // 
      // buttonReadSecureDocument
      // 
      this.buttonReadSecureDocument.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.buttonReadSecureDocument.Description = "ndfndfndfndfndfndfndfdfnddfndfndndf";
      this.buttonReadSecureDocument.Image = global::SecureScanOutlookAddIn.Properties.Resources.document_key5;
      this.buttonReadSecureDocument.Label = "Read Secure Document";
      this.buttonReadSecureDocument.Name = "buttonReadSecureDocument";
      this.buttonReadSecureDocument.ShowImage = true;
      this.buttonReadSecureDocument.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.button1_Click);
      // 
      // SecureScanRibbon
      // 
      this.Name = "SecureScanRibbon";
      this.RibbonType = resources.GetString("$this.RibbonType");
      this.Tabs.Add(this.tabSecureScan);
      this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.SecureScanRibbon_Load);
      this.tabSecureScan.ResumeLayout(false);
      this.tabSecureScan.PerformLayout();
      this.groupSecureScan.ResumeLayout(false);
      this.groupSecureScan.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    internal Microsoft.Office.Tools.Ribbon.RibbonTab tabSecureScan;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupSecureScan;
    internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonReadSecureDocument;
  }

  partial class ThisRibbonCollection
  {
    internal SecureScanRibbon SecureScanRibbon
    {
      get { return this.GetRibbon<SecureScanRibbon>(); }
    }
  }
}
