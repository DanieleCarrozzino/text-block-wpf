using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using TextEmoji.@interface;
using TextEmoji.objects;

namespace TextEmoji
{
    public class Manager
    {

        private static Manager instance = null;
        public static Manager GetInstance()
        {
            instance ??= new Manager();
            return instance;
        }

        // Last text emoji to call to clean
        public ITextEmoji lastTextEmoji = null;

        // Cache Emoji
        public Dictionary<string, DrawingImage> cacheEmoji = new Dictionary<string, DrawingImage>();


        public Manager()
        {
        }

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
