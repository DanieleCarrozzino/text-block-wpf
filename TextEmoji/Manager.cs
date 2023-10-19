using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        private ITextEmoji lastTextEmoji = null;


        public Manager()
        {
            initLibrary(); 
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

        private void initLibrary()
        {
            /*try
            {
                string exePath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                string dllPath = Path.Combine(exePath, "Lib", "Emoji.Wpf2.dll");
                if (File.Exists(dllPath))
                {
                    Assembly assembly = Assembly.LoadFrom(dllPath);
                }
            }
            catch (Exception ex)
            {
            }*/
        }

    }
}
