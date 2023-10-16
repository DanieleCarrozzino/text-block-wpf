using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using System.Windows.Media;
using System.Windows;

namespace TextEmoji
{
    internal class CustomTextParagraphProperties : TextParagraphProperties
    {
        public CustomTextParagraphProperties()
        {
            TextAlignment = TextAlignment.Left; // Set the default text alignment.
            LineSpacing = 6.0; // Set the default line spacing.
            FlowDirection = FlowDirection.LeftToRight; // Set the default flow direction.
            Indent = 0; // Set the default indentation.
            LineHeight = Const.FontSize; // Set the default line height.


            TextDecoration textDecoration = new TextDecoration
            {
                Location = TextDecorationLocation.Underline, // Set the location (Underline, OverLine, Baseline, Strikethrough)
                Pen = new Pen(Brushes.Transparent, 0), // Set the pen (brush and thickness) for the decoration.
            };

            // Create a TextDecorationCollection and add the TextDecoration to it.
            TextDecorationCollection textDecorations = new TextDecorationCollection();
            textDecorations.Add(textDecoration);

            TextDecorations = textDecorations;
            TextMarkerProperties = null; // Set the default text marker properties (can be null).

            TextWrapping = TextWrapping.Wrap; // Set the default text wrapping.
            FirstLineInParagraph = false; // Set the default for the first line in the paragraph.
            DefaultTextRunProperties = new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR);
        }

        public CustomTextParagraphProperties(CustomTextRunProperties custom)
        {
            TextAlignment = TextAlignment.Left; // Set the default text alignment.
            LineSpacing = 6.0; // Set the default line spacing.
            FlowDirection = FlowDirection.LeftToRight; // Set the default flow direction.
            Indent = 0; // Set the default indentation.
            LineHeight = Const.FontSize; // Set the default line height.


            TextDecoration textDecoration = new TextDecoration
            {
                Location = TextDecorationLocation.Underline, // Set the location (Underline, OverLine, Baseline, Strikethrough)
                Pen = new Pen(Brushes.Transparent, 0), // Set the pen (brush and thickness) for the decoration.
            };

            // Create a TextDecorationCollection and add the TextDecoration to it.
            TextDecorationCollection textDecorations = new TextDecorationCollection();
            textDecorations.Add(textDecoration);

            TextDecorations = textDecorations;
            TextMarkerProperties = null; // Set the default text marker properties (can be null).

            TextWrapping = TextWrapping.Wrap; // Set the default text wrapping.
            FirstLineInParagraph = false; // Set the default for the first line in the paragraph.
            DefaultTextRunProperties = custom;
        }

        public override TextAlignment TextAlignment { get; }
        public double LineSpacing { get; set; }
        public override FlowDirection FlowDirection { get; }
        public override double Indent { get; }
        public override double LineHeight { get; }
        public override TextMarkerProperties TextMarkerProperties { get; }
        public override TextWrapping TextWrapping { get; }
        public override bool FirstLineInParagraph { get; }
        public override TextRunProperties DefaultTextRunProperties { get; }
        public override TextDecorationCollection TextDecorations { get; }
    }

}
