using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TextEmoji.@interface;
using TextEmoji.objects;

namespace TextEmoji
{
    internal class Manager
    {

        private static Manager instance = null;
        public static Manager GetInstance()
        {
            instance ??= new Manager();
            return instance;
        }

        private ITextEmoji lastTextEmoji = null;

        public void saveLastTextEmojiModifiedAndCleanThePreviousOne(ITextEmoji textEmoji)
        {
            // Same object selection
            if (textEmoji == lastTextEmoji) return;

            if(lastTextEmoji != null)
            {
                lastTextEmoji.CleanImage();
            }
            lastTextEmoji = textEmoji;
        }

        public void cleanLastTextEmoji()
        {
            if(lastTextEmoji != null) lastTextEmoji.CleanImage();
        }

    }
}
