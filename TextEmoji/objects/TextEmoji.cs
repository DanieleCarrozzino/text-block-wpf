using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TextEmoji.objects
{
    public class TextEmoji : Grid
    {
        TextEmojiImage image = null;

        public TextEmoji()
        {
            SizeChanged += TextEmoji_SizeChanged;
        }

        private void TextEmoji_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (image != null)
            {
                image.Size = new Size(e.NewSize.Width, 0);
            }
        }

        // Text to draw
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(TextEmoji), new FrameworkPropertyMetadata("",
                (o, e) => (o as TextEmoji)?.OnTextPropertyChanged(e.NewValue as string))
            { DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus });

        private void OnTextPropertyChanged(string text)
        {
            image = new TextEmojiImage(text);
            Children.Add(image);
        }

    }
}
