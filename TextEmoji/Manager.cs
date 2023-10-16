using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

    }
}
