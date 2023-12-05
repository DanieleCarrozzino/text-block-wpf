using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TextEmoji.@interface;
using TextEmoji.usercontrols;
using static System.Net.Mime.MediaTypeNames;

namespace TextEmoji.objects
{
    public class TextEmoji : Grid, ITextEmoji
    {
        private Manager manager = Manager.GetInstance();
        private TextEmojiImage image    = null;

        /**********
         * PUBLIC
         *********/
        public event Action<string> LinkClicked;
        public event Action<string, MouseButtonEventArgs> RightLinkClicked;
        public event Action<string, MouseButtonEventArgs> RightTextSelectedClicked;
        public event Action<Size>   SizeChildrenChanged;
        public event Action<string> SelectedChanged;
        public event Action<string> CopyTextAction;
        public event Action<string> CopyLinkAction;
        public event Action<string> OpenLinkAction;

        /***********
         * PRIVATE
         **********/
        private event Action<object> SelectAllAction;

        public TextEmoji()
        {
            //SetValue(FontSizeProperty, (int)Const.FontSize);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            Children.Add(GetTextEmojiImage());
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
            if (String.IsNullOrEmpty(text))
            {
                removeReference();
                return;
            }

            GetTextEmojiImage().Text = text;
        }

        // Text to select programmatically
        public string HighlightText
        {
            get => (string)GetValue(HighlightTextProperty);
            set => SetValue(HighlightTextProperty, value);
        }

        public static readonly DependencyProperty HighlightTextProperty = DependencyProperty.Register(
            nameof(HighlightText), typeof(string), typeof(TextEmoji), new FrameworkPropertyMetadata("",
                (o, e) => (o as TextEmoji)?.OnHighlightTextPropertyChanged(e.NewValue as string))
            { DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus });

        private void OnHighlightTextPropertyChanged(string text)
        {
            GetTextEmojiImage().HighlightText = text;
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
            GetTextEmojiImage().Size = new Size(((Size)size).Width, 0);
        }

        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register(
            nameof(FontSize), typeof(object), typeof(TextEmoji), new FrameworkPropertyMetadata("",
                (o, e) => (o as TextEmoji)?.OnFontSizePropertyChanged(e.NewValue))
            { DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus });

        private void OnFontSizePropertyChanged(object fontsize)
        {
            GetTextEmojiImage().FontSize = (int)fontsize;
        }

        private TextEmojiImage GetTextEmojiImage()
        {
            if (image == null) image = new TextEmojiImage(this);
            return image;
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
            var dialog = new TextDialog(CopyLinkAction, OpenLinkAction, link);
            Utility.OpenPopupLinkMenu(this, e.GetPosition(this), dialog);
        }

        /// <summary>
        /// Right mouse event click on a selected text
        /// </summary>
        /// <param name="selectedText"></param>
        public void rightMouseClickWithTextSelected(string selectedText, MouseButtonEventArgs e)
        {
            RightTextSelectedClicked?.Invoke(selectedText, e);
            SelectAllAction += TextEmoji_SelectAllAction;
            var dialog = new TextDialog(CopyTextAction, OpenLinkAction, SelectAllAction, selectedText);
            Utility.OpenPopupLinkMenu(this, e.GetPosition(this), dialog);
        }

        /// <summary>
        /// Right mouse event click on a selected text
        /// </summary>
        /// <param name="selectedText"></param>
        public void rightMouseClickWithTextSelectedAndLink(string selectedText, string link, MouseButtonEventArgs e)
        {
            RightTextSelectedClicked?.Invoke(selectedText, e);
            SelectAllAction += TextEmoji_SelectAllAction;
            var dialog = new TextDialog(CopyTextAction, OpenLinkAction, SelectAllAction, OpenLinkAction, selectedText, link);
            Utility.OpenPopupLinkMenu(this, e.GetPosition(this), dialog);
        }

        /// <summary>
        /// Select All catch result
        /// </summary>
        /// <param name="obj"></param>
        private void TextEmoji_SelectAllAction(object obj)
        {
            SelectAllAction -= TextEmoji_SelectAllAction;
            if (image != null) image.SelectAll();
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

        private void removeReference()
        {
            LinkClicked = null;
            RightLinkClicked = null;
            RightTextSelectedClicked = null;
            SizeChildrenChanged = null;
            SelectedChanged = null;
            CopyTextAction = null;
            CopyLinkAction = null;
            OpenLinkAction = null;
            SelectAllAction = null;

            image.removeReference();
            image = null;
        }
    }
}
