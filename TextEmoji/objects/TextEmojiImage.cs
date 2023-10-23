using Emoji.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
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
        private int width_object  = 300;
        private int height_object = 300;

        // Info characters on first and last mouse click
        private CharacterHit firstCharacter     = new CharacterHit(-1, 0);
        private CharacterHit lastCharacter      = new CharacterHit(-1, 0);
        private CharacterHit newFirstCharacter  = new CharacterHit(-1, 0);
        private CharacterHit newLastCharacter   = new CharacterHit(-1, 0);

        // Info point of first and last mouse click
        private Point firstPoint;
        private Point lastPoint;
        private Point newLastPoint;
        private Point newFirstPoint;

        // Define the selection mode
        private bool startSelected = false;

        // Margin text
        Point linePosition = new Point(0, 0);

        // Parent
        private ITextEmoji parent = null;

        // Selected Text
        private string selectedText = "";

        // Highlight text
        private AMatch highlightMatch = new AMatch("", -1, -1);

        //------------
        //
        // ACTION
        //
        //------------
        public event Action<string> LinkClicked;
        public event Action<string, MouseButtonEventArgs> RightLinkClicked;
        public event Action<string, MouseButtonEventArgs> RightTextSelectedClicked;
        public event Action<string> SelectedChanged;


        public TextEmojiImage(ITextEmoji parent)
        {
            this.Focusable      = true;
            Visibility          = Visibility.Collapsed;
            HorizontalAlignment = HorizontalAlignment.Left;
            this.parent         = parent;
            this.Cursor         = Cursors.IBeam;
        }

        // Text to draw
        private string _text = "";
        public string Text{ 
            get 
            {
                return _text;
            } 
            set
            {
                _text = value;
                Init(value);
            }
        }

        public string HighlightText
        {
            set => OnHighlightTextChange(value);
        }

        public Size Size
        {
            set => OnSizeChanged(value);
        }

        private int fontsize = (int)Const.FontSize;
        public int FontSize
        {
            set => OnFontSizeChanged(value);
        }

        private void OnSizeChanged(Size size)
        {
            width_object  = (int)size.Width;
            height_object = (int)size.Height;
            InvalidateVisual();
        }

        private void OnFontSizeChanged(int font)
        {
            fontsize = font;
            if (mainTextSource != null) mainTextSource.fontsize = font;
            InvalidateVisual();
        }

        private void OnHighlightTextChange(string highlight)
        {
            highlightMatch.Value = highlight;
            if (Text.ToLower().Contains(highlight.ToLower()))
            {
                highlightMatch.Index    = Text.ToLower().IndexOf(highlight.ToLower());
                highlightMatch.Length   = highlight.Length - 1;

                if(mainTextSource != null)
                    mainTextSource.AddHighlight(highlightMatch);

                InvalidateVisual();
            }
            else if (highlight.Length > 0 && Text.Length > 0)
            {
                highlightMatch.Index    = -1;
                highlightMatch.Value    = "";
                highlightMatch.Length   = -1;

                if (mainTextSource != null)
                    mainTextSource.RemoveHighlight();

                InvalidateVisual();
            }
        }

        public void Init(string text)
        {
            emojiCollection     = EmojiData.MatchOne.Matches(text);
            linkMatches         = Utility.CheckValidUrl(text);
            HighLightText();

            MouseUp += OnMouseUp;
            EventManager.RegisterClassHandler(typeof(Window), Keyboard.KeyDownEvent, new KeyEventHandler(OnKeyDown), true);
        }

        public double LineHeight() { return fontsize * 1.33; }

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
            stopTimer();
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

        private (CharacterHit, Point) GetCharacterFromPoint(Point point)
        {
            // Get line position
            int index = (int)Math.Max(((point.Y) + LineHeight()) / LineHeight() - 1, 0);
            if (index >= textIntegers.Count) return (new CharacterHit(mainTextSource.Text.Length - 1, 0), /*TODO resolve this*/new Point(0, 0));

            int storePosition = textIntegers[index];

            // Create text line
            CustomTextParagraphProperties customTextParagraphProperties
                = new CustomTextParagraphProperties(fontsize);
            TextFormatter formatter = TextFormatter.Create();
            using (TextLine line = formatter.FormatLine(
                        mainTextSource,
                        storePosition,
                        width_object,
                        customTextParagraphProperties,
                        null))
            {
                var hit = line.GetCharacterHitFromDistance(point.X);
                return (hit, new Point(line.GetDistanceFromCharacterHit(hit), 0));
            }
        }

        private void HighLightText()
        {
            List<(int, int, int)> listDifferentStyle = new List<(int, int, int)>();
            foreach (Match match in linkMatches)
            {
                listDifferentStyle.Add((match.Index, match.Length, (int)CustomTextSource.TYPE.LINK));
            }
            foreach (Match match in emojiCollection)
            {
                listDifferentStyle.Add((match.Index, match.Length, (int)CustomTextSource.TYPE.EMOJI));
            }
            if(highlightMatch.Value.Length > 0 && Text.ToLower().Contains(highlightMatch.Value.ToLower()))
            {
                highlightMatch.Index    = Text.ToLower().IndexOf(highlightMatch.Value.ToLower());
                highlightMatch.Length   = highlightMatch.Value.Length - 1;
                listDifferentStyle.Add((
                    highlightMatch.Index,
                    highlightMatch.Length,
                    (int)CustomTextSource.TYPE.HIGHLIGHT));
            }

            mainTextSource = new CustomTextSource(Text, listDifferentStyle, fontsize);
            this.Visibility = Visibility.Visible;
        }



        private (int, int) drawHighlightTextFromFirstCharacterToTheLastOne()
        {
            newFirstPoint   = firstPoint;
            newLastPoint    = lastPoint;
            newFirstCharacter   = firstCharacter;
            newLastCharacter    = lastCharacter;   

            if (firstCharacter.FirstCharacterIndex > lastCharacter.FirstCharacterIndex)
            {
                newFirstCharacter   = newLastCharacter;
                newLastCharacter    = firstCharacter;

                var tmp         = newFirstPoint;
                newFirstPoint   = newLastPoint;
                newLastPoint    = tmp;
            }
            InvalidateVisual();
            return (0, 0);
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
                = new CustomTextParagraphProperties(new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR, fontsize), fontsize);
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

                        /*****************
                         * Draw selection
                         *****************/
                        var start_bound = Math.Max(newFirstCharacter.FirstCharacterIndex, textSourcePosition);
                        var end_bound   = Math.Min(newLastCharacter.FirstCharacterIndex, textSourcePosition + line.Length) + 1;

                        if (start_bound != end_bound && end_bound - start_bound > 0)
                        {
                            var list = line.GetTextBounds(start_bound, end_bound - start_bound);
                            foreach (var bound in list)
                            {
                                Rect rec = new Rect(bound.Rectangle.X, linePosition.Y, bound.Rectangle.Width, bound.Rectangle.Height);

                                if (newFirstCharacter.FirstCharacterIndex == start_bound)
                                {
                                    dc.DrawGeometry(Utility.GetSelectionBrushColor(), null, createGeometryLeftRounded(rec));
                                }
                                else if (newLastCharacter.FirstCharacterIndex == end_bound)
                                {
                                    dc.DrawGeometry(Utility.GetSelectionBrushColor(), null, createGeometryRightRounded(rec));
                                }
                                else
                                {
                                    dc.DrawRectangle(Utility.GetSelectionBrushColor(), null, rec);
                                }
                            }
                        }
                        //CustomSelection();

                        /************
                         * Draw line
                         ************/
                        line.Draw(dc, linePosition, InvertAxes.None);

                        // define the width of the draw
                        if (line.Width > width) width = (int)line.Width;

                        textSourcePosition  += line.Length;
                        linePosition.Y      += line.Height;

                        /*************
                         * Draw emoji
                         *************/
                        // ho bisogno di torvare la posizione delle varie emoji
                        // ho la lista delle emoji all'interno di emojiCollection
                        // devo controllare che la linea che sto gestendo non contenga
                        // l'emoji che voglio disegnare
                        foreach (Match match in emojiCollection)
                        {
                            if (textSourcePosition - line.Length <= match.Index && textSourcePosition > match.Index)
                            {
                                double distance = line.GetDistanceFromCharacterHit(new CharacterHit(match.Index, 0));
                                Point point = new(distance - 2, linePosition.Y - line.Height - 4);

                                double width_o = LineHeight() + 6;
                                double height_o = LineHeight() + 6;

                                var di = new DrawingImage(Emoji.Wpf.Image.RenderEmoji(match.Value, out width_o, out height_o));
                                di.Freeze();
                                Rect imageRect = new Rect(point, new Size(LineHeight() + 6, LineHeight() + 6));
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


        /// <summary>
        /// Old custom selection
        /// </summary>
        private void CustomSelection()
        {
            // if line contains the start of the selection 
            // it means textSourcePosition < firstCharacterPosition
            // and textSourcePosition + line.Length > firstCharacterPosition
            /*if (textSourcePosition <= newFirstCharacter.FirstCharacterIndex 
                && textSourcePosition + line.Length > newFirstCharacter.FirstCharacterIndex)
            {
                double pointx       = Math.Min(Math.Max(newFirstPoint.X, 0), line.Width);
                double width_rect   = line.Width - pointx;

                // if the line contains also the end of selection
                if (newLastCharacter.FirstCharacterIndex < textSourcePosition + line.Length)
                {
                    double x    = Math.Min(newLastPoint.X, line.Width);
                    width_rect  = Math.Abs(x - pointx);

                    Rect rec    = new Rect(pointx, linePosition.Y, width_rect, line.Height);
                    dc.DrawRoundedRectangle(Utility.GetBrushColor("#b3d9ff"), null, rec, Const.CornerRect, Const.CornerRect);
                }
                else
                {
                    Rect rec = new Rect(pointx, linePosition.Y, width_rect, line.Height);
                    dc.DrawGeometry(Utility.GetBrushColor("#b3d9ff"), null, createGeometryLeftRounded(rec));
                }
            }
            // If the line contains the end of the selection
            // it means lastCharacterPosition > textSourcePosition
            // and textSourcePosition + line.Length > lastCharacterPosition
            else if (textSourcePosition < newLastCharacter.FirstCharacterIndex
                && textSourcePosition + line.Length > newLastCharacter.FirstCharacterIndex)
            {
                double pointx = Math.Min(line.Width, newLastPoint.X);

                Rect rec = new Rect(0, linePosition.Y, pointx, line.Height);
                dc.DrawGeometry(Utility.GetBrushColor("#b3d9ff"), null, createGeometryRightRounded(rec));
            }
            // The selection contains the entire line
            // it means firstCharacterPosition < textSourcePosition
            // and lastCharacterPosition > textSourcePosition + line.Length
            else if (newFirstCharacter.FirstCharacterIndex < textSourcePosition
                && textSourcePosition + line.Length < newLastCharacter.FirstCharacterIndex)
            {
                Rect rec = new Rect(0, linePosition.Y, line.Width, line.Height);
                dc.DrawRectangle(Utility.GetBrushColor("#b3d9ff"), null, rec);
            }*/
        }


        private StreamGeometry createGeometryLeftRounded(Rect rectangleRect)
        {
            // Define the radius for the rounded corners
            double cornerRadius = Const.CornerRect;

            // Create a StreamGeometry to define the custom shape
            StreamGeometry roundedRectangleGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = roundedRectangleGeometry.Open())
            {
                double x = rectangleRect.X;
                double y = rectangleRect.Y;
                double width    = rectangleRect.Width;
                double height   = rectangleRect.Height;

                // Define the figure with a rounded left corner
                ctx.BeginFigure(new Point(x + cornerRadius, y), true, true);
                ctx.ArcTo(new Point(x, y + cornerRadius), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Counterclockwise, true, false);
                ctx.LineTo(new Point(x, y + height - cornerRadius), true, false);
                ctx.ArcTo(new Point(x + cornerRadius, y + height), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Counterclockwise, true, false);
                ctx.LineTo(new Point(x + width, y + height), true, false);
                ctx.LineTo(new Point(x + width, y), true, false);
            }
            return roundedRectangleGeometry;
        }

        private StreamGeometry createGeometryRightRounded(Rect rectangleRect)
        {
            // Define the radius for the rounded corners
            double cornerRadius = Const.CornerRect;

            // Create a StreamGeometry to define the custom shape
            StreamGeometry roundedRectangleGeometry = new StreamGeometry();
            using (StreamGeometryContext ctx = roundedRectangleGeometry.Open())
            {
                double x = rectangleRect.X;
                double y = rectangleRect.Y;
                double width    = rectangleRect.Width;
                double height   = rectangleRect.Height;

                // Define the figure with a rounded right corner
                ctx.BeginFigure(new Point(x, y), true, true);
                ctx.LineTo(new Point(x + width - cornerRadius, y), true, false);
                ctx.ArcTo(new Point(x + width, y + cornerRadius), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(new Point(x + width, y + height - cornerRadius), true, false);
                ctx.ArcTo(new Point(x + width - cornerRadius, y + height), new Size(cornerRadius, cornerRadius), 0, false, SweepDirection.Clockwise, true, false);
                ctx.LineTo(new Point(x, y + height), true, false);
            }

            return roundedRectangleGeometry;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            // Start selection mode
            startSelected = true;

            // Capture mouse events
            this.CaptureMouse();

            // Get mouse position
            Point p = Mouse.GetPosition(this);

            // Clean last selection
            if (parent != null)
                parent.CleanLastImage();

            // Get the first Character info
            (firstCharacter, firstPoint)  = GetCharacterFromPoint(p);

            // To capture the mouse
            e.Handled = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            removeMouseCapture();

            // Get mouse position
            Point p = e.GetPosition(this);

            // Get the first Character info
            (lastCharacter, lastPoint) = GetCharacterFromPoint(p);

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
                // to receive copy keyboard command
                Keyboard.Focus(this);

                // re draw the ihighlight text
                var indexes     = drawHighlightTextFromFirstCharacterToTheLastOne();
                selectedText    = mainTextSource.Text.Substring(indexes.Item1, indexes.Item2 + 1);
                selectedChanged(selectedText);
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {

            // right click on link or text
            // Get mouse position
            Point p = e.GetPosition(this);

            // Get the first Character info
            CharacterHit    character;
            Point           point;
            (character, point) = GetCharacterFromPoint(p);

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
            /*if (!string.IsNullOrEmpty(GetSelectedText()))
            {*/
                rightMouseClickWithSelectedText(GetSelectedText(), e);
                //e.Handled = true;
                //return;
            //}

            base.OnMouseRightButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!startSelected) return;

            ManageMouseMoveOutOfBorder(e);

            // Get mouse position
            Point p = Mouse.GetPosition(this);

            // Get the first Character info
            (lastCharacter, lastPoint) = GetCharacterFromPoint(p);
            //lastPoint = p;

            // draw
            drawHighlightTextFromFirstCharacterToTheLastOne();

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
                selectedText = GetSelectedText();
                if (!selectedText.Equals(""))
                {
                    if (parent != null) parent.CopyText(selectedText);
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }

        public string GetSelectedText()
        {
            if(firstCharacter.FirstCharacterIndex >= 0 
                && lastCharacter.FirstCharacterIndex >= 0 
                && (lastCharacter.FirstCharacterIndex + firstCharacter.FirstCharacterIndex) != 0)
            {
                int start  = firstCharacter.FirstCharacterIndex;
                int length = lastCharacter.FirstCharacterIndex - firstCharacter.FirstCharacterIndex;
                if(length < 0)
                {
                    length = Math.Abs(length);
                    start = lastCharacter.FirstCharacterIndex;
                }


                bool add = false;
                foreach(Match match in emojiCollection)
                {
                    if(start + length == match.Index)
                    {
                        length += match.Length;
                        add = true;
                    }
                }
                if (!add) length += 1;
                return mainTextSource.Text.Substring(start, length);
            }
            return "";
        }

        public void SelectAll()
        {
            firstCharacter  = new CharacterHit(0, 0);
            lastCharacter   = new CharacterHit(Text.Length - 1, 0);

            newFirstCharacter   = firstCharacter;
            newLastCharacter    = lastCharacter;

            InvalidateVisual();
        }

        //***************
        //
        // CLEAN methods
        //
        //***************

        public void CleanImage()
        {
            selectedText    = "";

            newFirstCharacter = new CharacterHit(-1, 0);
            newLastCharacter  = new CharacterHit(-1, 0);

            firstCharacter  = new CharacterHit(-1, 0);
            lastCharacter   = new CharacterHit(-1, 0);
            InvalidateVisual();
        }


        //*******************
        //
        // SCROLLABLE logic
        //
        //*******************

        private ScrollViewer parentScrollable   = null;
        private bool IsParentScrollable         = true;

        private void ManageMouseMoveOutOfBorder(MouseEventArgs e)
        {
            if (!IsParentScrollable) return;
            parentScrollable ??= getScrollableParent();
            if (parentScrollable == null) return;
            
            var p = e.GetPosition(parentScrollable);
            if (p.Y > parentScrollable.ActualHeight || p.Y < 0)
            {
                up      = p.Y < 0;
                delta   = Math.Abs(p.Y) / 30;
                if (!timerRunning())
                    MoveScroll();
            }
            else stopTimer();
        }

        private bool up      = false;
        private double delta = 2;
        private DispatcherTimer timer;

        private void MoveScroll()
        {
            getTimer().Start();            
        }

        private DispatcherTimer getTimer()
        {
            timer ??= new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(14);
            timer.Tick += (sender, e) =>
            {
                Point pointReferToParent = this.TranslatePoint(new Point(0, 0), parentScrollable);

                if (pointReferToParent.Y + this.ActualHeight > parentScrollable.ActualHeight && !up)
                {
                    parentScrollable.ScrollToVerticalOffset(parentScrollable.VerticalOffset + Math.Min(delta, pointReferToParent.Y + this.ActualHeight - parentScrollable.ActualHeight));
                }
                else if (pointReferToParent.Y < 0 && up)
                {
                    parentScrollable.ScrollToVerticalOffset(parentScrollable.VerticalOffset - Math.Min(delta, Math.Abs(pointReferToParent.Y)));
                }
                else timer.Stop();
            };
            return timer;
        }

        private void stopTimer()
        {
            if (timer != null) timer.Stop();
        }

        private bool timerRunning()
        {
            if (timer != null) return timer.IsEnabled;
            return false;
        }

        private ScrollViewer getScrollableParent()
        {
            DependencyObject parent = VisualTreeHelper.GetParent(this);

            while (parent != null)
            {
                if (parent is ScrollViewer scrollInfo)
                {
                    IsParentScrollable = true;
                    return scrollInfo;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            IsParentScrollable = false;
            return null;
        }

    }
}
