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

        public static SolidColorBrush GetSelectionBrushColor(string hexColor)
        {
            SolidColorBrush backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexColor));
            backgroundBrush.Freeze();

            return backgroundBrush;
        }

        public static void OpenPopupLinkMenu(UIElement element, Point p, UIElement dialogContent)
        {
            Trace.WriteLine("1");
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
            Trace.WriteLine("2");
        }

        public static void ClosePopup()
        {
            popup.IsOpen = false;
        }


        static private string GetDefaultWebBrowser()
        {
            if (!string.IsNullOrEmpty(browser)) return browser;

            // The registry key path for default web browser
            const string keyPath = @"HTTP\shell\open\command";

            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        // Read the (default) string value
                        browser = key.GetValue(null) as string;
                        if (!string.IsNullOrEmpty(browser))
                        {
                            // Extract the browser's executable from the command
                            int startIndex = browser.IndexOf('"');
                            int endIndex = browser.IndexOf('"', startIndex + 1);

                            if (startIndex >= 0 && endIndex >= 0)
                            {
                                browser = browser.Substring(startIndex + 1, endIndex - startIndex - 1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any potential exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return browser;
        }

        static public System.Drawing.Icon GetDefaultBrowserIcon()
        {
            if(browserIcon != null) return browserIcon;

            string browserPath = GetDefaultWebBrowser();

            if (!string.IsNullOrEmpty(browserPath) && File.Exists(browserPath))
            {
                try
                {
                    browserIcon = System.Drawing.Icon.ExtractAssociatedIcon(browserPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error extracting icon: " + ex.Message);
                }
            }

            return browserIcon;
        }
    }
}
