using System.Windows;
using Text_Grab.Controls;

namespace Text_Grab.Models;

public class WordBorderInfo
{
    public string Word { get; set; } = string.Empty;
    public Rect BorderRect { get; set; } = Rect.Empty;
    public int LineNumber { get; set; }
    public int ResultColumnID { get; set; }
    public int ResultRowID { get; set; }
    public string MatchingBackground { get; set; } = "Transparent";
    public bool IsBarcode { get; set; }

    public WordBorderInfo()
    {

    }

    public WordBorderInfo(WordBorder wordBorder)
    {
        Word = wordBorder.Word;
        LineNumber = wordBorder.LineNumber;
        ResultColumnID = wordBorder.ResultColumnID;
        ResultRowID = wordBorder.ResultRowID;
        MatchingBackground = wordBorder.MatchingBackground.ToString();
        IsBarcode = wordBorder.IsBarcode;
        BorderRect = new()
        {
            X = wordBorder.Left,
            Y = wordBorder.Top,
            Width = wordBorder.Width,
            Height = wordBorder.Height
        };
    }
}
