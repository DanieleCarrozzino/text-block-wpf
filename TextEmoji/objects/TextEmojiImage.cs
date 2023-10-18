using Emoji.Wpf2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using TextEmoji.@interface;

namespace TextEmoji.objects
{
    public class TextEmojiImage : FrameworkElement, ITextEmojiImage
    {
        // Lines positions
        private List<int> textIntegers = new List<int>();
        // Links matches
        private List<Match> linkMatches;
        // Main text source
        private CustomTextSource mainTextSource;
        // Emoji matches
        private MatchCollection emojiCollection;

        // Object dimensions
        private int width_object = 300;
        private int height_object = 300;

        // Info characters on first and last mouse click
        private CharacterHit firstCharacter;
        private CharacterHit lastCharacter;

        // Info point of first and last mouse click
        private Point firstPoint;
        private Point lastPoint;

        // Define the selection mode
        private bool startSelected = false;

        // Margin text
        Point linePosition = new Point(0, 0);

        // call from xaml or from code
        private bool callFromXaml = false;
        private bool initilized = false;

        // Parent
        private ITextEmoji parent = null;

        // Selected Text
        private string selectedText = "";

        //------------
        //
        // ACTION
        //
        //------------
        public event Action<string> LinkClicked;
        public event Action<string, MouseButtonEventArgs> RightLinkClicked;
        public event Action<string, MouseButtonEventArgs> RightTextSelectedClicked;
        public event Action<string> SelectedChanged;

        public TextEmojiImage()
        {
            Visibility          = Visibility.Collapsed;
            callFromXaml        = true;
            HorizontalAlignment = HorizontalAlignment.Left;
            this.Cursor         = Cursors.IBeam;
        }

        public TextEmojiImage(string text, ITextEmoji parent)
        {
            Visibility          = Visibility.Collapsed;
            Text                = text;
            callFromXaml        = false;
            HorizontalAlignment = HorizontalAlignment.Left;
            this.parent         = parent;
            this.Cursor         = Cursors.IBeam;
        }

        // Text to draw
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(TextEmojiImage), new FrameworkPropertyMetadata("",
                (o, e) => (o as TextEmojiImage)?.OnTextPropertyChanged(e.NewValue as string))
            { DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus });

        private void OnTextPropertyChanged(string text)
        {
            if (callFromXaml || initilized)
                Init(text);
        }


        public Size Size
        {
            set
            {
                OnSizeChanged(value);
            }
        }

        private void OnSizeChanged(Size size)
        {
            width_object = (int)size.Width;
            height_object = (int)size.Height;

            if (!callFromXaml && !initilized)
                Init(Text);
            else
                drawText();
        }

        public void Init(string text)
        {
            initilized = true;
            emojiCollection = EmojiData.MatchOne.Matches(text);
            linkMatches = Utility.CheckValidUrl(text);
            HighLightText();

            MouseUp += OnMouseUp;
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
        }

        /// <summary>
        /// Remove mouse capture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            removeMouseCapture();
        }

        /// <summary>
        /// Remove mouse capture 
        /// and the selection mode
        /// </summary>
        private void removeMouseCapture()
        {
            startSelected = false;
            Mouse.Capture(null);
        }


        /// <summary>
        /// Main method to draw the text
        /// </summary>
        private void drawText()
        {
            InvalidateVisual();
        }


        /// <summary>
        /// TODO set this method accessible from the outside
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void clickLink(int start, int length)
        {
            if(parent != null)  parent.linkClicked(mainTextSource.Text.Substring(start, length));
            else                LinkClicked?.Invoke(mainTextSource.Text.Substring(start, length));
        }

        /// <summary>
        /// TODO set this method accessible from the outside
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void rightclickLink(int start, int length, MouseButtonEventArgs e)
        {
            if (parent != null) parent.rightLinkClicked(mainTextSource.Text.Substring(start, length), e);
            else RightLinkClicked?.Invoke(mainTextSource.Text.Substring(start, length), e);
        }

        /// <summary>
        /// Right click with a selected text
        /// </summary>
        /// <param name="selectedText"></param>
        private void rightMouseClickWithSelectedText(string selectedText, MouseButtonEventArgs e)
        {
            if (parent != null) parent.rightMouseClickWithTextSelected(selectedText, e);
            else RightTextSelectedClicked?.Invoke(selectedText, e);
        }

        private void selectedChanged(string text)
        {
            if (parent != null) parent.Selected(text);
            else SelectedChanged?.Invoke(text);
        }

        private CharacterHit GetCharacterFromPoint(Point point)
        {
            // Get line position
            int index = Math.Max(((int)(point.Y) + (int)Const.LineHeight) / (int)Const.LineHeight - 1, 0);
            if (index >= textIntegers.Count) return new CharacterHit(mainTextSource.Text.Length - 1, 0);

            int storePosition = textIntegers[index];

            // Create text line
            CustomTextParagraphProperties customTextParagraphProperties
                = new CustomTextParagraphProperties();
            TextFormatter formatter = TextFormatter.Create();
            using (TextLine line = formatter.FormatLine(
                        mainTextSource,
                        storePosition,
                        width_object,
                        customTextParagraphProperties,
                        null))
            {
                return line.GetCharacterHitFromDistance(point.X);
            }
        }

        private void HighLightText()
        {
            List<(int, int, int)> highList = new List<(int, int, int)>();
            foreach (Match match in linkMatches)
            {
                highList.Add((match.Index, match.Length, (int)CustomTextSource.TYPE.LINK));
            }
            mainTextSource = new CustomTextSource(Text, highList);
            this.Visibility = Visibility.Visible;
        }



        private (int, int) drawHighlightTextFromFirstCharacterToTheLastOne(CharacterHit first, CharacterHit last)
        {
            var length = last.FirstCharacterIndex - first.FirstCharacterIndex;
            var start = first.FirstCharacterIndex;

            if (length < 0)
            {
                start = last.FirstCharacterIndex;
                var tmp = firstPoint;
                firstPoint = lastPoint;
                lastPoint = tmp;
            }

            mainTextSource = mainTextSource.AddSelection(start, start + Math.Abs(length));
            InvalidateVisual();
            return (start, Math.Abs(length));
        }

        //*****************
        //
        // OVERRIDE methods
        //
        //*****************

        protected override void OnRender(DrawingContext dc)
        {
            int textSourcePosition = 0;
            textIntegers.Clear();
            linePosition.Y = 0;
            linePosition.X = 0;
            int width = 0;

            CustomTextParagraphProperties customTextParagraphProperties
                = new CustomTextParagraphProperties(new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR));
            TextFormatter formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            while (textSourcePosition < mainTextSource.Text.Length && (linePosition.Y < height_object || height_object == 0))
            {
                try
                {
                    // Create a textline from the text store using the TextFormatter object.
                    using (TextLine line = formatter.FormatLine(
                        mainTextSource,
                        textSourcePosition,
                        width_object,
                        customTextParagraphProperties,
                        null))
                    {

                        // Define and draw the line
                        textIntegers.Add(textSourcePosition);
                        line.Draw(dc, linePosition, InvertAxes.None);

                        // define the width of the draw
                        if (line.Width > width) width = (int)line.Width;

                        textSourcePosition += line.Length;
                        linePosition.Y += line.Height;

                        // DRAW EMOJI
                        //
                        // ho bisogno di torvare la posizione delle varie emoji
                        // ho la lista delle emoji all'interno di emojiCollection
                        // devo controllare che la linea che sto gestendo non contenga
                        // l'emoji che voglio disegnare

                        foreach (Match match in emojiCollection)
                        {
                            if (textSourcePosition - line.Length < match.Index && textSourcePosition > match.Index)
                            {
                                double distance = line.GetDistanceFromCharacterHit(new CharacterHit(match.Index, 0));
                                Point point = new(distance - 2, linePosition.Y - line.Height - 4);

                                double width_o = Const.LineHeight + 6;
                                double height_o = Const.LineHeight + 6;

                                var di = new DrawingImage(Emoji.Wpf2.Image.RenderEmoji(match.Value, out width_o, out height_o));
                                di.Freeze();
                                Rect imageRect = new Rect(point, new Size(Const.LineHeight + 6, Const.LineHeight + 6));
                                dc.DrawImage(di, imageRect);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
                finally
                {
                    if (parent != null)
                        parent.resize(width, (int)linePosition.Y);
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // Start selection mode
            startSelected = true;

            // Capture mouse events
            this.CaptureMouse();


            // Get focus to enable the keydown
            this.Focus();

            // Get mouse position
            Point p = Mouse.GetPosition(this);

            // Get the first Character info
            firstCharacter = GetCharacterFromPoint(p);
            firstPoint = p;

            // Clean last selection
            if (parent != null)
                parent.CleanLastImage();

            // To capture the mouse
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            removeMouseCapture();

            // Get mouse position
            Point p = e.GetPosition(this);

            // Get the first Character info
            lastCharacter = GetCharacterFromPoint(p);
            lastPoint = p;

            // if first is eqaul to the last one character
            // I perform a link click if it's a match
            if (firstCharacter.FirstCharacterIndex == lastCharacter.FirstCharacterIndex)
            {
                if (linkMatches != null)
                {
                    foreach (Match match in linkMatches)
                    {
                        if (firstCharacter.FirstCharacterIndex >= match.Index &&
                            firstCharacter.FirstCharacterIndex <= match.Index + match.Length)
                        {
                            clickLink(match.Index, match.Length);
                            return;
                        }
                    }
                }
                CleanImage();
            }
            // Draw single highlight
            else
            {
                var indexes     = drawHighlightTextFromFirstCharacterToTheLastOne(firstCharacter, lastCharacter);
                selectedText    = mainTextSource.Text.Substring(indexes.Item1, indexes.Item2 + 1);
                selectedChanged(selectedText);
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {

            // right click on link or text
            // Get mouse position
            Point p = e.GetPosition(this);

            // Get the first Character info
            var character = GetCharacterFromPoint(p);

            if (linkMatches != null)
            {
                foreach (Match match in linkMatches)
                {
                    if (character.FirstCharacterIndex >= match.Index &&
                        character.FirstCharacterIndex <= match.Index + match.Length)
                    {
                        rightclickLink(match.Index, match.Length, e);
                        e.Handled = true;
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(mainTextSource.GetSelectedText()))
            {
                rightMouseClickWithSelectedText(mainTextSource.GetSelectedText(), e);
                e.Handled = true;
            }

            base.OnMouseRightButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!startSelected) return;

            // Get mouse position
            Point p = Mouse.GetPosition(this);

            // Get the first Character info
            lastCharacter = GetCharacterFromPoint(p);
            lastPoint = p;

            // draw
            drawHighlightTextFromFirstCharacterToTheLastOne(firstCharacter, lastCharacter);

            base.OnMouseMove(e);
        }

        protected override void OnLostMouseCapture(MouseEventArgs e)
        {
            removeMouseCapture();
            base.OnLostMouseCapture(e);
        }


        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            //TODO
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                selectedText = mainTextSource.GetSelectedText();
                if (!selectedText.Equals(""))
                {
                    //Clipboard.SetText(selectedText);
                    if (parent != null) parent.CopyText(selectedText);
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        //***************
        //
        // CLEAN methods
        //
        //***************

        public void CleanImage()
        {
            selectedText = "";
            mainTextSource = mainTextSource.ClearSelection();
            InvalidateVisual();
        }

    }
}
