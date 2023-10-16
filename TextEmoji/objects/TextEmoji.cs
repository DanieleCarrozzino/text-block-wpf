using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TextEmoji.@interface;

namespace TextEmoji.objects
{
    public class TextEmoji : Grid, ITextEmoji
    {
        private TextEmojiImage image    = null;
        public event Action<string> LinkClicked;
        public event Action<string> RightLinkClicked;
        public event Action<Size>   SizeChildrenChanged;

        public TextEmoji()
        {
            SizeChanged         += TextEmoji_SizeChanged;
            HorizontalAlignment = HorizontalAlignment.Stretch;
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
            image = new TextEmojiImage(text, this);
            Children.Add(image);
        }

        /// <summary>
        /// Event to handle the left mouse click on a link
        /// </summary>
        /// <param name="link"></param>
        public void linkClicked(string link)
        {
            LinkClicked?.Invoke(link);
        }

        /// <summary>
        /// Resize the parent with the size of the text
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void resize(int width, int height)
        {
            SizeChildrenChanged?.Invoke(new Size(width, height));
        }

        /// <summary>
        /// Right mouse event click on a link
        /// </summary>
        /// <param name="link"></param>
        public void rightLinkClicked(string link)
        {
            RightLinkClicked?.Invoke(link);
        }
    }
}
