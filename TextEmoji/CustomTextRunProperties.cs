﻿using System;
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
            EMOJI,
        }

        public CustomTextRunProperties(STYLE style, int fontsize)
        {
            switch (style)
            {
                case STYLE.HIGHLIGHT:
                    TextDecorations = new TextDecorationCollection();
                    Pen underlinePen = new Pen(Utility.GetBrushColor("#0073e6"), 2);
                    TextDecorations.Add(new TextDecoration(
                        TextDecorationLocation.Underline, 
                        underlinePen, 1, 
                        TextDecorationUnit.FontRecommended, 
                        TextDecorationUnit.FontRecommended));
                    ForegroundBrush = Utility.GetBrushColor("#0073e6");
                    break;
                case STYLE.SELECTED:
                    BackgroundBrush = Utility.GetBrushColor("#ffd480");
                    ForegroundBrush = Brushes.Black;
                    break;
                case STYLE.BOTH:
                    TextDecorations = new TextDecorationCollection();
                    underlinePen = new Pen(Utility.GetBrushColor("#0073e6"), 2);
                    TextDecorations.Add(new TextDecoration(
                        TextDecorationLocation.Underline,
                        underlinePen, 1,
                        TextDecorationUnit.FontRecommended,
                        TextDecorationUnit.FontRecommended));
                    ForegroundBrush = Utility.GetBrushColor("#0073e6");
                    BackgroundBrush = Utility.GetBrushColor("#66ff0000");
                    break;
                case STYLE.CLEAR:
                    BackgroundBrush = Brushes.Transparent;
                    ForegroundBrush = Brushes.Black;
                    break;
                case STYLE.EMOJI:
                    ForegroundBrush = Brushes.Transparent;
                    BackgroundBrush = Brushes.Transparent;
                    break;
                default:
                    break;

            }

            Typeface            = new Typeface("Segoe UI Emoji");
            FontRenderingEmSize = fontsize;
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
