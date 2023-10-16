using Emoji.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
using TextEmoji.@interface;

namespace TextEmoji.objects
{
    public class TextEmojiImage : System.Windows.Controls.Image, ITextEmojiImage
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
        Point linePosition = new Point(20, 20);

        // call from xaml or from code
        private bool callFromXaml = false;
        private bool initilized = false;

        public TextEmojiImage()
        {
            Stretch = Stretch.None;
            callFromXaml = true;
        }

        public TextEmojiImage(string text)
        {
            Stretch = Stretch.None;
            Text = text;
            callFromXaml = false;
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
            linkMatches = Utiltity.CheckValidUrl(text);
            mainTextSource = HighLightText();

            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
            LostMouseCapture += OnLostMouseCapture;
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLostMouseCapture(object sender, MouseEventArgs e)
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
            MouseMove -= OnMouseMove;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Catch the move only during the selection mode
            if (!startSelected) return;

            // Get mouse position
            Point p = Mouse.GetPosition(this);

            // Get the first Character info
            lastCharacter = GetCharacterFromPoint(p);
            lastPoint = p;

            drawHighlightTextFromFirstCharacterToTheLastOne(firstCharacter, lastCharacter);
        }


        /// <summary>
        /// Main method to draw the text
        /// </summary>
        private void drawText()
        {
            Source = new DrawingImage(GetTextDest(mainTextSource));
        }

        /// <summary>
        /// Get the drawing group to draw from the TextSource
        /// </summary>
        /// <param name="textSource"></param>
        /// <returns></returns>
        private DrawingGroup GetTextDest(CustomTextSource textSource)
        {

            int textStorePosition = 0;
            textIntegers.Clear();
            linePosition.Y = 20;
            linePosition.X = 20;

            // Create a DrawingGroup object for storing formatted text.
            var textDest = new DrawingGroup();
            DrawingContext dc = textDest.Open();

            CustomTextParagraphProperties customTextParagraphProperties
                = new CustomTextParagraphProperties(new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR));
            TextFormatter formatter = TextFormatter.Create();

            // Format each line of text from the text store and draw it.
            while (textStorePosition < textSource.Text.Length && (linePosition.Y < height_object || height_object == 0))
            {
                try
                {
                    // Create a textline from the text store using the TextFormatter object.
                    using (TextLine line = formatter.FormatLine(
                        textSource,
                        textStorePosition,
                        width_object,
                        customTextParagraphProperties,
                        null))
                    {

                        // Define and draw the line
                        textIntegers.Add(textStorePosition);
                        line.Draw(dc, linePosition, InvertAxes.None);
                        textStorePosition += line.Length;
                        linePosition.Y += line.Height;


                        // DRAW EMOJI
                        //
                        // ho bisogno di torvare la posizione delle varie emoji
                        // ho la lista delle emoji all'interno di emojiCollection
                        // devo controllare che la linea che sto gestendo non contenga
                        // l'emoji che voglio disegnare

                        foreach (Match match in emojiCollection)
                        {
                            if (textStorePosition - line.Length < match.Index && textStorePosition > match.Index)
                            {
                                double distance = line.GetDistanceFromCharacterHit(new CharacterHit(match.Index, 0));
                                Point point = new(distance + 20 - 2, linePosition.Y - line.Height - 4);

                                double width = Const.FontSize + 6;
                                double height = Const.FontSize + 6;

                                var di = new DrawingImage(Emoji.Wpf.Image.RenderEmoji(match.Value, out width, out height));
                                di.Freeze();
                                Rect imageRect = new Rect(point, new Size(Const.FontSize + 6, Const.FontSize + 6));
                                dc.DrawImage(di, imageRect);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            // Persist the drawn text content.
            dc.Close();
            return textDest;
        }

        /// <summary>
        /// TODO set this method accessible from the outside
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        private void clickLink(int start, int length)
        {
            Console.WriteLine("Click - " + Text.Substring(start, length));
        }


        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            startSelected = true;
            Mouse.Capture(this);
            MouseMove += OnMouseMove;

            // Get mouse position
            Point p = Mouse.GetPosition(this);

            // Get the first Character info
            firstCharacter = GetCharacterFromPoint(p);
            firstPoint = p;

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            startSelected = false;

            // Get mouse position
            Point p = e.GetPosition(this);

            // Get the first Character info
            lastCharacter = GetCharacterFromPoint(p);
            lastPoint = p;

            // rmeove overhead
            MouseMove -= OnMouseMove;

            // if first is eqaul to the last one character
            // I perform a link click if it's a match
            if (firstCharacter == lastCharacter && linkMatches != null)
            {
                foreach (Match match in linkMatches)
                {
                    if (firstCharacter.FirstCharacterIndex >= match.Index &&
                        firstCharacter.FirstCharacterIndex <= match.Index + match.Length)
                    {
                        clickLink(match.Index, match.Length);
                    }
                }
            }
            // Draw single highlight
            else
            {
                drawHighlightTextFromFirstCharacterToTheLastOne(firstCharacter, lastCharacter);
            }

            base.OnMouseLeftButtonUp(e);
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

        private CharacterHit GetCharacterFromPoint(Point point)
        {
            // Get line position
            int index = Math.Max((int)(point.Y + (int)Const.FontSize) / (int)Const.FontSize - 1, 0);
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

        private CustomTextSource HighLightText()
        {
            List<(int, int, int)> highList = new List<(int, int, int)>();
            foreach (Match match in linkMatches)
            {
                highList.Add((match.Index, match.Length, (int)CustomTextSource.TYPE.LINK));
            }
            CustomTextSource textSource = new CustomTextSource(Text, highList);
            Source = new DrawingImage(GetTextDest(textSource));
            return textSource;
        }



        private void drawHighlightTextFromFirstCharacterToTheLastOne(CharacterHit first, CharacterHit last)
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
            Source = new DrawingImage(GetTextDest(mainTextSource));
        }

    }
}
