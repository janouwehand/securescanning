using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Net.Codecrete.QrCodeGenerator;

namespace SecureScanMFP
{
  public partial class QRControl : UserControl
  {
    public QRControl() => InitializeComponent();

    public byte[] QRKey { get; private set; }

    public static Bitmap ByteToImage(byte[] data)
    {
      using (var mStream = new MemoryStream())
      {
        mStream.Write(data, 0, Convert.ToInt32(data.Length));
        var bm = new Bitmap(mStream, false);
        return bm;
      }
    }

    public void ShowQR(byte[] qrKey)
    {
      QRKey = qrKey;
      var str = Convert.ToBase64String(qrKey);
      var qr = QrCode.EncodeText(str, QrCode.Ecc.Medium);
      var png = qr.ToPng(6, 1);
      pictureBoxQR.Image = ByteToImage(png);
      Visible = true;
    }
  }
}
