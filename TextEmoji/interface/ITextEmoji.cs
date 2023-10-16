using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void rightLinkClicked(string link);

        /// <summary>
        /// Resize container 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void resize(int width, int height);

    }
}
