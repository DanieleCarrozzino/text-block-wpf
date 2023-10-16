using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextEmoji.@interface
{
    internal interface ITextEmojiImage
    {

        /// <summary>
        /// Change this size to resize the entire block
        /// </summary>
        public Size Size { set; }

        /// <summary>
        /// Main text to draw during the init phase
        /// </summary>
        public string Text { set; get; }


    }
}
