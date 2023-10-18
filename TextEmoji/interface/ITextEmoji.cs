using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TextEmoji.@interface
{
    public interface ITextEmoji
    {

        /// <summary>
        /// Events to handle inside the parent,
        /// a link is been clicked
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        public void linkClicked(string link);

        /// <summary>
        /// Events to handle inside the parent,
        /// a link is been clicked with the right 
        /// mouse button
        /// </summary>
        /// <param name="link"></param>
        /// <param name="coordinate"></param>
        public void rightLinkClicked(string link, MouseButtonEventArgs e);

        /// <summary>
        /// Resize container 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void resize(int width, int height);

        /// <summary>
        /// Get the selectetd text after a mouse up
        /// </summary>
        /// <param name="selected"></param>
        public void Selected(string selected);

        /// <summary>
        /// Clean the entire text
        /// </summary>
        public void CleanImage();

        /// <summary>
        /// Clean the last entire text
        /// </summary>
        public void CleanLastImage();

        /// <summary>
        /// Get copied text
        /// </summary>
        /// <param name="text"></param>
        public void CopyText(string text);


    }
}
