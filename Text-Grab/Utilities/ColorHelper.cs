using System.Windows.Media;

namespace Text_Grab.Utilities;

public static class ColorHelper
{
    public static Color MediaColorFromDrawingColor(System.Drawing.Color drawingColor)
    {
        return Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
    }

    public static SolidColorBrush SolidColorBrushFromDrawingColor(System.Drawing.Color drawingColor)
    {
        return new SolidColorBrush(MediaColorFromDrawingColor(drawingColor));
    }
}