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
                    Pen underlinePen = new Pen(Brushes.Blue, 2);
                    TextDecorations.Add(new TextDecoration(
                        TextDecorationLocation.Underline, 
                        underlinePen, 1, 
                        TextDecorationUnit.FontRecommended, 
                        TextDecorationUnit.FontRecommended));
                    ForegroundBrush = Brushes.Blue;
                    break;
                case STYLE.SELECTED:
                    BackgroundBrush = Brushes.LightBlue;
                    ForegroundBrush = Brushes.Black;
                    break;
                case STYLE.BOTH:
                    TextDecorations = new TextDecorationCollection();
                    underlinePen = new Pen(Brushes.Blue, 2);
                    TextDecorations.Add(new TextDecoration(
                        TextDecorationLocation.Underline,
                        underlinePen, 1,
                        TextDecorationUnit.FontRecommended,
                        TextDecorationUnit.FontRecommended));
                    ForegroundBrush = Brushes.Blue;
                    BackgroundBrush = Brushes.Orange;
                    break;
                case STYLE.CLEAR:
                    BackgroundBrush = Brushes.Transparent;
                    ForegroundBrush = Brushes.Black;
                    break;
                default:
                    break;

            }

            Typeface = new Typeface("Arial");
            FontRenderingEmSize = 16;
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
