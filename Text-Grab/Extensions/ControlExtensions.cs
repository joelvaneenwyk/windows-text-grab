using System.Windows;
using System.Windows.Controls;

namespace Text_Grab;

public static class ControlExtensions
{



    public static double GetHorizontalScaleFactor(this Viewbox viewbox)
    {
        if (viewbox.Child is not FrameworkElement childElement)
            return 1.0;

        double outsideWidth = viewbox.ActualWidth;
        double insideWidth = childElement.ActualWidth;

        return outsideWidth / insideWidth;
    }

    public static Rect GetAbsolutePlacement(this FrameworkElement element, bool relativeToScreen = false)
    {
        var absolutePos = element.PointToScreen(new Point(0, 0));
        if (relativeToScreen)
        {
            return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
        }
        var posMW = Application.Current.MainWindow.PointToScreen(new Point(0, 0));
        absolutePos = new Point(absolutePos.X - posMW.X, absolutePos.Y - posMW.Y);
        return new Rect(absolutePos.X, absolutePos.Y, element.ActualWidth, element.ActualHeight);
    }
}