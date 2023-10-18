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
        }

        public string Text;

        /// <summary>
        /// Highlight list of size and position
        /// </summary>
        public HashSet<(int, int, int)> positionList = new HashSet<(int, int, int)>();
        public HashSet<(int, int, int)> positionLink = new HashSet<(int, int, int)>();

        public int startIndex = 0;
        public int lastIndex = 0;

        public CustomTextSource(string text)
        {
            Text = text;
        }

        public CustomTextSource ClearSelection()
        {
            positionList.Clear();
            positionList.UnionWith(positionLink);
            return new CustomTextSource(Text, positionList.OrderBy(item => item.Item1).ToList());
        }

        public CustomTextSource AddSelection(int startIndex, int lastIndex)
        {
            // Create selection info
            if (lastIndex > startIndex)
            {
                this.startIndex = startIndex;
                this.lastIndex = lastIndex;
            }
            else
            {
                this.startIndex = lastIndex;
                this.lastIndex = startIndex;
            }

            // clear last selection
            positionList.Clear();
            positionList.UnionWith(positionLink);

            // Define text and position list
            positionList.Add((startIndex, lastIndex - startIndex, (int)TYPE.SELECTION));
            return new CustomTextSource(Text, positionList.OrderBy(item => item.Item1).ToList());
        }

        /// <summary>
        /// link constructor, receive a list of dimension 
        /// to know where are the link to draw
        /// </summary>
        /// <param name="list">tuple of index and length</param>
        public CustomTextSource(string text, List<(int, int, int)> list)
        {
            // Clear link list
            positionLink = list.Where(item => item.Item3 == (int)TYPE.LINK).ToHashSet();

            // Get the selection item
            // from the list
            var selectedItem = list.Find(item => item.Item3 == (int)TYPE.SELECTION);
            list.Remove(selectedItem);

            foreach ((int index, int length, int type) in list)
            {

                // skip the selected item
                //if (index == selectedItem.Item1 && selectedItem.Item2 == length && selectedItem.Item3 == type) continue;

                // I keep only the items that 
                // are finished inside the selected
                // range
                if (/*the first part is inside*/
                    (index > selectedItem.Item1 && index < selectedItem.Item1 + selectedItem.Item2) ||
                    /*the last part is inside*/
                    (index + length > selectedItem.Item1 && index + length < selectedItem.Item1 + selectedItem.Item2) ||
                    /*is totally inside*/
                    (index > selectedItem.Item1 && index + length < selectedItem.Item1 + selectedItem.Item2)
                    )
                {
                    // This item is in part or totally inside the selection range
                    // On first condition I only remove the share part from the selection range 
                    if((index > selectedItem.Item1 && index + length > selectedItem.Item1 + selectedItem.Item2))
                    {
                        Trace.WriteLine("\n************");
                        Trace.WriteLine("1 ind " + index);
                        Trace.WriteLine("1 it1 " + selectedItem.Item1);
                        Trace.WriteLine("1 it2 " + selectedItem.Item2);
                        Trace.WriteLine("************\n");

                        var item_to_add     = (index, selectedItem.Item1 + selectedItem.Item2 - index, (int)TYPE.BOTH);
                        var item_modify     = (selectedItem.Item1 + selectedItem.Item2, index + length - (selectedItem.Item1 + selectedItem.Item2), type);
                        selectedItem.Item2  = index - selectedItem.Item1;

                        Trace.WriteLine("\n************");
                        Trace.WriteLine("1 add " + item_to_add.ToString());
                        Trace.WriteLine("1 mod " + item_modify.ToString());
                        Trace.WriteLine("************\n");

                        positionList.Add(item_to_add);
                        positionList.Add(item_modify);

                    }
                    else if (index + length > selectedItem.Item1 && index < selectedItem.Item1)
                    {
                        var item_modify = (index, selectedItem.Item1 - index, type);
                        var item_to_add = (selectedItem.Item1, index + length - selectedItem.Item1, (int)TYPE.BOTH);
                        Trace.WriteLine("2 add " + item_to_add.ToString());
                        Trace.WriteLine("2 mod " + item_modify.ToString());

                        selectedItem.Item1 = index + length;
                        selectedItem.Item2 = selectedItem.Item2 - item_to_add.Item2;                     

                        positionList.Add(item_to_add);
                        positionList.Add(item_modify);
                    }
                    else
                    {
                        // It's completly inside
                        // I have to split
                        // the selection range and take
                        // only the left part of the selection
                        // for the future comparison

                        //first part of the selection
                        var first_sel = (selectedItem.Item1, index - selectedItem.Item1, (int)TYPE.SELECTION);

                        //both element
                        var item_to_add = (index, length, (int)TYPE.BOTH);

                        //last part of the selection
                        selectedItem = (index + length, selectedItem.Item2 - (index + length), (int)TYPE.SELECTION);

                        positionList.Add(item_to_add);
                        positionList.Add(first_sel);
                    }
                }
                else
                    positionList.Add((index, length, type));
            }

            positionList.Add(selectedItem);
            positionList    = positionList.ToList().OrderBy(item => item.Item1).ToHashSet();

            Trace.WriteLine(selectedItem.ToString());
            Text            = text;
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
                if (positionList.Count > 0)
                {
                    return manageHighLightText(textSourceCharacterIndex);
                }
                return new TextCharacters(Text, textSourceCharacterIndex,
                                                    Text.Length - textSourceCharacterIndex, new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR));

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
            foreach ((int index, int length, int type) in positionList)
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
                                            index - textSourceCharacterIndex, new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR));
                }

                // Se cIndex è racchiuso all'interno di index
                // e index + length allora disegno HIGHLIGHT
                if(textSourceCharacterIndex >= index && textSourceCharacterIndex <= index + length)
                {
                    CustomTextRunProperties.STYLE style;
                    if (type == (int)TYPE.LINK)
                    {
                        style = CustomTextRunProperties.STYLE.HIGHLIGHT;
                    }
                    else if (type == (int)TYPE.SELECTION)
                    {
                        style = CustomTextRunProperties.STYLE.SELECTED;
                    }
                    else
                    {
                        style = CustomTextRunProperties.STYLE.BOTH;
                    }
                    var final_length = Math.Min(Math.Max(1, length - (textSourceCharacterIndex - index)), Text.Length);
                    return new TextCharacters(Text, textSourceCharacterIndex,
                                            final_length, new CustomTextRunProperties(style));
                }

                // Improvment potrei eliminare gli elementi
                // che ho già finito di gestire, se faccio
                // la continue significa che non sono più utili
            }

            // Se cIndex è minore della dimensione del testo
            // totale devo finire di disegnare l'ultima parte 
            // di clear
            if(textSourceCharacterIndex < Text.Length)
            {
                return new TextCharacters(Text, textSourceCharacterIndex,
                                                    Text.Length - textSourceCharacterIndex, new CustomTextRunProperties(CustomTextRunProperties.STYLE.CLEAR));
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

        public string GetSelectedText()
        {
            int start = -1;
            int length = 0;
            foreach(var item in positionList)
            {
                if(item.Item3 == (int)TYPE.SELECTION || item.Item3 == (int)TYPE.BOTH)
                {
                    if(start == -1)
                        start = item.Item1;
                    length += item.Item2;
                }
            }
            if(start != -1 && length > 0)
                return Text.Substring(start, length + 1);
            return "";
        }
    }
}
