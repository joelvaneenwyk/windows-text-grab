using System.Drawing;
using Text_Grab.Models;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using ZXing.Windows.Compatibility;
using static ZXing.Rendering.SvgRenderer;
using Color = System.Drawing.Color;

namespace Text_Grab.Utilities;

public static class BarcodeUtilities
{

    public static OcrOutput TryToReadBarcodes(Bitmap bitmap)
    {
        BarcodeReader barcodeReader = new()
        {
            AutoRotate = true,
            Options = new DecodingOptions { TryHarder = true }
        };

        Result result = barcodeReader.Decode(bitmap);

        string resultString = string.Empty;
        if (result is not null)
            resultString = result.Text;

        return new OcrOutput
        {
            Kind = OcrOutputKind.Barcode,
            RawOutput = resultString,
            SourceBitmap = bitmap,
        };
    }

    public static Bitmap GetQrCodeForText(string text, ErrorCorrectionLevel correctionLevel)
    {
        BitmapRenderer bitmapRenderer = new();
        bitmapRenderer.Foreground = Color.Black;
        bitmapRenderer.Background = Color.White;

        BarcodeWriter barcodeWriter = new()
        {
            Format = BarcodeFormat.QR_CODE,
            Renderer = bitmapRenderer,
        };

        EncodingOptions encodingOptions = new()
        {
            Width = 500,
            Height = 500,
            Margin = 5,
        };
        encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, correctionLevel);
        barcodeWriter.Options = encodingOptions;

        Bitmap bitmap = barcodeWriter.Write(text);

        return bitmap;
    }

    public static SvgImage GetSvgQrCodeForText(string text, ErrorCorrectionLevel correctionLevel)
    {
        BarcodeWriterSvg barcodeWriter = new()
        {
            Format = BarcodeFormat.QR_CODE,
            Renderer = new SvgRenderer()
        };

        EncodingOptions encodingOptions = new()
        {
            Width = 500,
            Height = 500,
            Margin = 5,
        };
        encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, correctionLevel);
        barcodeWriter.Options = encodingOptions;

        SvgImage svg = barcodeWriter.Write(text);

        return svg;
    }
}
