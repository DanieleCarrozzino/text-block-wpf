using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TextEmoji.@interface;
using TextEmoji.usercontrols;

namespace TextEmoji.objects
{
    public class TextEmoji : Grid, ITextEmoji
    {
        private Manager manager = Manager.GetInstance();
        private TextEmojiImage image    = null;
        public event Action<string> LinkClicked;
        public event Action<string, MouseButtonEventArgs> RightLinkClicked;
        public event Action<string, MouseButtonEventArgs> RightTextSelectedClicked;
        public event Action<Size>   SizeChildrenChanged;
        public event Action<string> SelectedChanged;
        public event Action<string> CopyTextAction;
        public event Action<string> CopyLinkAction;
        public event Action<string> OpenLinkAction;

        public TextEmoji()
        {
            //SizeChanged += TextEmoji_SizeChanged;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        private void TextEmoji_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeChanged -= TextEmoji_SizeChanged;
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

        public Size SizeContainer
        {
            get => (Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            nameof(SizeContainer), typeof(object), typeof(TextEmoji), new FrameworkPropertyMetadata("",
                (o, e) => (o as TextEmoji)?.OnSizePropertyChanged(e.NewValue))
            { DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus });

        private void OnSizePropertyChanged(object size)
        {
            if (image != null)
                image.Size = new Size(((Size)size).Width, 0);
        }

        //***************
        //
        // CLEAN methods
        //
        //***************

        // Ogni volta che seleziono, evidenzio o modifico
        // un textEmoji devo salvarmelo all'interno
        // del manager per poterlo "pulire" nel momento in
        // cui inizio a "modificarne" un altro
        //
        // es. se selezino un testo in uno di loro
        // la selezione vecchia deve scomparire

        /// <summary>
        /// Clean from selection
        /// </summary>
        public void CleanImage()
        {
            // Evento sporco
            if (image == null) return;

            image.CleanImage();
        }

        /// <summary>
        /// Clean from mouse down
        /// </summary>
        public void CleanLastImage()
        {
            manager.cleanLastTextEmoji();
        }



        //*****************
        //
        // EVENTS methods
        //
        //*****************

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
            Width = width;
            Height = height;
            SizeChildrenChanged?.Invoke(new Size(width, height));
        }

        /// <summary>
        /// Right mouse event click on a link
        /// </summary>
        /// <param name="link"></param>
        public void rightLinkClicked(string link, MouseButtonEventArgs e)
        {
            RightLinkClicked?.Invoke(link, e);
            var dialog = new LinkDialog(CopyLinkAction, OpenLinkAction, link);
            Utility.OpenPopupLinkMenu(this, e.GetPosition(this), dialog);
        }

        /// <summary>
        /// Right mouse event click on a selected text
        /// </summary>
        /// <param name="selectedText"></param>
        public void rightMouseClickWithTextSelected(string selectedText, MouseButtonEventArgs e)
        {
            RightTextSelectedClicked?.Invoke(selectedText, e);
            var dialog = new TextDialog(selectedText);
            Utility.OpenPopupLinkMenu(this, e.GetPosition(this), dialog);
        }

        /// <summary>
        /// Get the selected text after a mouse up 
        /// over the textemoji object
        /// </summary>
        /// <param name="selected"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Selected(string selected)
        {
            manager.saveLastTextEmojiModifiedAndCleanThePreviousOne(this);
            SelectedChanged?.Invoke(selected);
        }

        /// <summary>
        /// Get the copied text
        /// </summary>
        /// <param name="text"></param>
        public void CopyText(string text)
        {
            CopyTextAction?.Invoke(text);
        }
    }
}
