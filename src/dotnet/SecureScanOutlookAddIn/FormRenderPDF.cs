using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using PdfiumViewer;

namespace SecureScanOutlookAddIn
{
  public partial class FormRenderPDF : Form
  {
    private PdfViewer viewer;
    private PdfDocument document;

    public Stream DocumentStream { get; set; }

    public FormRenderPDF() => InitializeComponent();

    protected override void OnShown(EventArgs e) => RenderPDF();

    protected override void OnClosing(CancelEventArgs e)
    {
      viewer?.Dispose();
      viewer = null;
      document?.Dispose();
      document = null;
      DocumentStream?.Dispose();
      DocumentStream = null;
    }

    private void RenderPDF()
    {
      if (DocumentStream == null)
      {
        return;
      }

      // Create the PdfViewer control
      viewer = new PdfViewer
      {
        // Set the viewer's Dock property to Fill so it will take up the entire form
        Dock = DockStyle.Fill,
        ShowToolbar = false,
        ZoomMode = PdfViewerZoomMode.FitWidth
      };

      // Add the viewer to the form      
      Controls.Add(viewer);

      //DocumentStream = File.OpenRead(@"C:\dev\securescanning\data\MFP-test-document-to-protect.pdf");

      document = PdfDocument.Load(DocumentStream);
      viewer.Document = document;
    }

  }
}
