using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using VCardWithQr.Models;
using ZXing;
using ZXing.Common;

namespace VCardWithQr.Controllers;

public class VCardController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult GenerateQR(VCard model)
    {
        var vcardText = model.ToVCard();

        var writer = new BarcodeWriterPixelData
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = 300,
                Width = 300
            }
        };

        var pixelData = writer.Write(vcardText);

        using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
        {
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                try
                {
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                var qrCodeBase64 = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";

                ViewBag.QRCodeImage = qrCodeBase64;
            }
        }

        return View("Index", model);
    }
}
