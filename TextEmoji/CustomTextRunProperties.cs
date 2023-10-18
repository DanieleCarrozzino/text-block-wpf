using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using System.Windows.Media;
using System.Windows;

namespace TextEmoji
{
    public class CustomTextRunProperties : TextRunProperties
    {

        public enum STYLE
        {
            HIGHLIGHT,
            SELECTED,
            BOTH,
            CLEAR,
        }

        public CustomTextRunProperties(STYLE style)
        {
            switch (style)
            {
                case STYLE.HIGHLIGHT:
                    TextDecorations = new TextDecorationCollection();
                    Pen underlinePen = new Pen(Utility.GetSelectionBrushColor("#0073e6"), 2);
                    TextDecorations.Add(new TextDecoration(
                        TextDecorationLocation.Underline, 
                        underlinePen, 1, 
                        TextDecorationUnit.FontRecommended, 
                        TextDecorationUnit.FontRecommended));
                    ForegroundBrush = Utility.GetSelectionBrushColor("#0073e6");
                    break;
                case STYLE.SELECTED:
                    BackgroundBrush = Utility.GetSelectionBrushColor("#b3d9ff");
                    ForegroundBrush = Brushes.Black;
                    break;
                case STYLE.BOTH:
                    TextDecorations = new TextDecorationCollection();
                    underlinePen = new Pen(Utility.GetSelectionBrushColor("#0073e6"), 2);
                    TextDecorations.Add(new TextDecoration(
                        TextDecorationLocation.Underline,
                        underlinePen, 1,
                        TextDecorationUnit.FontRecommended,
                        TextDecorationUnit.FontRecommended));
                    ForegroundBrush = Utility.GetSelectionBrushColor("#0073e6");
                    //BackgroundBrush = Utility.GetSelectionBrushColor("#e6f2ff");
                    BackgroundBrush = Utility.GetSelectionBrushColor("#66ff0000");
                    break;
                case STYLE.CLEAR:
                    BackgroundBrush = Brushes.Transparent;
                    ForegroundBrush = Brushes.Black;
                    break;
                default:
                    break;

            }

            Typeface = new Typeface("Segoe UI");
            FontRenderingEmSize = Const.FontSize;
        }


        public override Typeface Typeface { get; }
        public override double FontRenderingEmSize { get; }
        public override Brush ForegroundBrush { get; }
        public override Brush BackgroundBrush { get; } // Default background brush
        public override CultureInfo CultureInfo => CultureInfo.CurrentUICulture; // Default culture info
        public override double FontHintingEmSize => 6.0; // Default font hinting size
        public override TextDecorationCollection TextDecorations { get; } // No text decorations
        public override TextEffectCollection TextEffects => null; // No text effects
    }
}
