using System;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using TextEmoji.usercontrols;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Interop;
using TextEmoji.objects;

namespace TextEmoji
{
    internal static class Utility
    {

        // Create a new Popup control
        static Popup popup = new Popup();

        // Get default icon browser
        static System.Drawing.Icon browserIcon = null;

        // browser path
        static string browser = string.Empty;

        /// <summary>
        /// Get all valid links from the 
        /// passing msg
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<Match> CheckValidUrl(String msg)
        {
            try
            {
                MatchCollection matches = Regex.Matches(msg, Const.WEB_URL);
                return matches.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public static SolidColorBrush GetBrushColor(string hexColor)
        {
            SolidColorBrush backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            backgroundBrush.Freeze();

            return backgroundBrush;
        }

        public static SolidColorBrush GetSelectionBrushColor()
        {
            SolidColorBrush backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#b3d9ff"));
            backgroundBrush.Freeze();

            return backgroundBrush;
        }

        public static void OpenPopupLinkMenu(UIElement element, Point p, UIElement dialogContent)
        {
            popup.Focus();

            popup.StaysOpen             = false;
            popup.AllowsTransparency    = true;

            if(!popup.IsOpen)
                popup.PopupAnimation    = PopupAnimation.Fade;

            // add the layout
            popup.Child = dialogContent;

            // Target
            popup.PlacementTarget   = element;
            popup.Placement         = PlacementMode.Relative;

            popup.HorizontalOffset  = p.X + 5;
            popup.VerticalOffset    = p.Y + 5;

            popup.IsOpen = true;
        }

        public static void ClosePopup()
        {
            popup.IsOpen = false;
        }
    }
}
