using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEmoji.objects
{
    public class AMatch
    {
        public AMatch(string value, int index, int length)
        {
            Value   = value;
            Index   = index;
            Length  = length;
        }

        public string Value { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
    }
}
