using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextEmoji
{
    internal static class Utiltity
    {

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

    }
}
