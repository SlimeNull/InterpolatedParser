using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class BooleanParser : IParser<bool>
    {
        private BooleanParser() { }
        public static BooleanParser Instance { get; } = new BooleanParser();

        public bool Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out bool value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Boolean");
        }

        public bool TryParse(ref int index, string text, out bool value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out bool value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = false;

            // Check for "true"
            if (actualIndex + 3 < text.Length &&
                text[actualIndex] == 't' &&
                text[actualIndex + 1] == 'r' &&
                text[actualIndex + 2] == 'u' &&
                text[actualIndex + 3] == 'e')
            {
                value = true;
                index = actualIndex + 4;
                return true;
            }

            // Check for "false"
            if (actualIndex + 4 < text.Length &&
                text[actualIndex] == 'f' &&
                text[actualIndex + 1] == 'a' &&
                text[actualIndex + 2] == 'l' &&
                text[actualIndex + 3] == 's' &&
                text[actualIndex + 4] == 'e')
            {
                value = false;
                index = actualIndex + 5;
                return true;
            }

            // If neither "true" nor "false" is found, parsing fails
            if (throwOnFailure)
            {
                throw new FormatException("Invalid format of System.Boolean");
            }
            return false;
        }
    }
}
