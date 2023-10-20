using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Security.Permissions;
using System.Diagnostics;

namespace TextEmoji
{
    internal class CustomTextSource : TextSource
    {
        public enum TYPE
        {
            LINK = 0,
            SELECTION = 1,
            BOTH = 2,
            EMOJI = 3,
        }

        public string Text;

        /// <summary>
        /// Highlight list of size and position
        /// </summary>
        public HashSet<(int, int, int)> positionCustomStyle = new HashSet<(int, int, int)>();

        public int startIndex = 0;
        public int lastIndex = 0;

        public int fontsize = (int)Const.FontSize;

        /// <summary>
        /// link constructor, receive a list of dimension 
        /// to know where are the link to draw
        /// </summary>
        /// <param name="list">tuple of index and length</param>
        public CustomTextSource(string text, List<(int, int, int)> list, int fontsize)
        {
            // Clear link list
            this.fontsize       = fontsize;
            positionCustomStyle = list.ToHashSet();
            Text                = text;
        }

        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            // Check or ended condition
            if (textSourceCharacterIndex < 0)
            {
                throw new ArgumentOutOfRangeException("textSourceCharacterIndex", "Value must be greater than 0.");
            }
            if (textSourceCharacterIndex >= Text.Length)
            {
                return new TextEndOfParagraph(1);
            }

            // Create and return a TextCharacters object, which is formatted according to
            // the current layout and rendering properties.
            if (textSourceCharacterIndex < Text.Length)
            {
                // Link and selected mode
                if (positionCustomStyle.Count > 0)
                {
                    return manageHighLightText(textSourceCharacterIndex);
                }
                return new TextCharacters(Text, textSourceCharacterIndex,
                                                    Text.Length - textSourceCharacterIndex, new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR, fontsize));
            }

            // Return an end-of-paragraph indicator if there is no more text source.
            return new TextEndOfParagraph(1);
        }

        /// <summary>
        /// draw highlight text
        /// </summary>
        /// <param name="textSourceCharacterIndex">last position</param>
        /// <returns></returns>
        private TextRun manageHighLightText(int textSourceCharacterIndex)
        {
            foreach ((int index, int length, int type) in positionCustomStyle)
            {
                // Se cIndex è maggiore di index + length
                // posso saltare questo elemento perchè già gestito
                if(textSourceCharacterIndex > index + length)
                {
                    continue;
                }

                // Se cIndex è minore di index allora disegno clear
                // perchè sono sicuro che se sto gestendo questo
                // punto quello precedente è già stato gestito
                if(textSourceCharacterIndex < index)
                {
                    return new TextCharacters(Text, textSourceCharacterIndex,
                                            index - textSourceCharacterIndex, new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR, fontsize));
                }

                // Se cIndex è racchiuso all'interno di index
                // e index + length allora disegno HIGHLIGHT
                if(textSourceCharacterIndex >= index && textSourceCharacterIndex <= index + length)
                {
                    CustomTextRunProperties.STYLE style = CustomTextRunProperties.STYLE.CLEAR;
                    if (type == (int)TYPE.LINK)
                    {
                        style = CustomTextRunProperties.STYLE.HIGHLIGHT;
                    }
                    else if(type == (int)TYPE.EMOJI)
                    {
                        style = CustomTextRunProperties.STYLE.EMOJI;
                    }
                    var final_length = Math.Min(Math.Max(1, length - (textSourceCharacterIndex - index)), Text.Length);
                    return new TextCharacters(Text, textSourceCharacterIndex,
                                            final_length, new CustomTextRunProperties(style, fontsize));
                }
            }

            // Se cIndex è minore della dimensione del testo
            // totale devo finire di disegnare l'ultima parte 
            // di clear
            if(textSourceCharacterIndex < Text.Length)
            {
                return new TextCharacters(Text, textSourceCharacterIndex,
                                                    Text.Length - textSourceCharacterIndex, new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR, fontsize));
            }

            return new TextEndOfParagraph(1);
        }

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
        {
            throw new NotImplementedException();
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            // If you don't have text effects, you can return the same character index.
            return textSourceCharacterIndex;
        }
    }
}
