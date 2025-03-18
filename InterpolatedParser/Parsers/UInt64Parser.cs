using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class UInt64Parser : IParser<ulong>
    {
        private UInt64Parser() { }
        public static UInt64Parser Instance { get; } = new UInt64Parser();

        public ulong Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out ulong value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.UInt64");
        }

        public bool TryParse(ref int index, string text, out ulong value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out ulong value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.UInt64");
                return false;
            }

            // Parse digits
            bool hasDigits = false;
            ulong result = 0;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                ulong digit = (ulong)(text[actualIndex] - '0');

                // Check for overflow
                if (result > (ulong.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too large for System.UInt64");
                    return false;
                }

                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.UInt64");
                return false;
            }

            value = result;
            index = actualIndex;
            return true;
        }
    }
}
