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

        public int startIndex = 0;
        public int lastIndex = 0;

        public CustomTextSource(string text)
        {
            Text = text;
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

            // Remove last selection
            (int, int, int) itemToRemove        = (-1, -1, -1);
            (int, int, int) itemToRemoveBoth    = (-1, -1, -1);
            foreach (var item in positionList)
            {
                if (item.Item3 == (int)TYPE.SELECTION)
                {
                    itemToRemove = item;
                }
                else if (item.Item3 == (int)TYPE.BOTH)
                {
                    itemToRemoveBoth = item;

                    // Nel caso il Both sia successivo al suo parent
                    // aggiungo il both al precedente
                    // per sapere se è il precedente il suo parent 
                    // controllo che positonList[index - 1]
                    // (positionList - 1) index + length >= (item) index
                    
                    /*var i = positionList.IndexOf(item);
                    if (positionList[i].Item1 + positionList[i].Item2 >= item.Item1)
                    {
                        // il parent è il precedente
                    }
                    else
                    {
                        // il parent è il successivo
                    }*/
                }
            }
            positionList.Remove(itemToRemove);
            positionList.Remove(itemToRemoveBoth);

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
            // Genero il both in base alla selezione
            // e la presenza di link

            // es. 
            // item1(10, 5,  0)
            // item2(19, 20, 0)
            // item3(24, 35, 1)
            //
            // item1(10, 5,  0)
            // item2(19, 20, 0)
            // item3(24, 3,  1)
            //
            // In questo caso l'item 3 è la selezione
            // e sta coprendo parte del link di item2
            //
            // tra l'index 24 con lunghezza 16 dovrà comparire 
            // un ulteriore item con type 2 == BOTH
            //
            //
            // ciclo sulla lista alla ricerca della selezione
            // salvandomi l'item precedente e nel caso la selezione
            // sia sopra a un link spezzo la selezione in 2 item e 
            // diminuisco la lunghezza del link precedente

            int i = 0;
            (int, int, int) previous = (-1, -1, -1);
            (int, int, int) next     = (-1, -1, -1);

            foreach ((int item1, int item2, int item3) in list)
            {
                // Get the next value inside the list
                if (i < list.Count - 1)
                {
                    next = list[i + 1];
                }
                else next = (-1, -1, -1);

                // Trovo la selezione
                // e controllo che non si
                // sovrapponga
                if (item3 == (int)TYPE.SELECTION)
                {
                    if (previous.Item1 + previous.Item2 > item1)
                    {
                        // Si sovrappone

                        // Modifico quello precedente
                        positionList.Remove(previous);
                        positionList.Add((previous.Item1, item1 - previous.Item1 - 1, previous.Item3));

                        // Aggiungo il both
                        var length = Math.Min((previous.Item1 + previous.Item2) - item1, item2);
                        positionList.Add((item1, length, (int)TYPE.BOTH));

                        // Aggiungo il seleeted se ne è avanzato
                        if(item2 > length)
                        {
                            // prendendo l'esempio di prima
                            // in questo caso avremo.
                            //
                            // item1(10, 5,  0)
                            // item2(19, 5,  0)
                            // item3(24, 15, 2)
                            // item4(39, 24, 1)
                            //
                            // io devo aggiungere l'item 4
                            var new_select_index = item1 + length;
                            var new_select_length = item2 - length;
                            positionList.Add((new_select_index, new_select_length, (int)TYPE.SELECTION));
                        }
                    }
                    else if (item1 + item2 > next.Item1 && next.Item1 >= 0)
                    {
                        // In questo caso la selezione è sovrapposta
                        // a un elemento successivo all'interno
                        // della lista
                        //
                        // Devo andare a disegnare la selection dallo
                        // start di questo elemento fino allo start
                        // dell'elemento successivo
                        //
                        // TODO nel caso lo racchiudi del tutto
                        positionList.Add((item1, next.Item1 - item1, (int)TYPE.SELECTION));

                        // Adesso disgeno il both venutosi a creare
                        // tra il next e l'attuale
                        // considero se la selezione racchiude tutto 
                        // il link

                        int length = Math.Min( next.Item2, (item1 + item2) - next.Item1 );
                        positionList.Add((next.Item1, length, (int)TYPE.BOTH));

                        // Nel caso sia avanzato qualcosa dalla sovrapposizione
                        // disegno il link rimanente con i valori di next
                        if(length < next.Item2)
                        {
                            // In questo caso devo aggiungere il link 
                            // partendo dalla fine del both
                            // quindi start = next.item1 + length
                            // e lunghezza prendo la lunghezza di next
                            // e ci tolgo la differenza tra lo start di next e 
                            // la nuova start
                            positionList.Add((next.Item1 + length, next.Item2 - length, (int)TYPE.LINK));
                        }

                    }
                    else 
                    {
                        positionList.Add((item1, item2, item3));
                    }
                }
                else
                {
                    positionList.Add((item1, item2, item3));
                }
                previous = (item1, item2, item3);
                i++;
            }

            positionList = positionList.ToList().OrderBy(item => item.Item1).ToHashSet();
            Text = text;
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
                    return new TextCharacters(Text, textSourceCharacterIndex,
                                            length + 1 - (textSourceCharacterIndex - index), new CustomTextRunProperties(style));
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

            // Ho finito di gestire i link
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
