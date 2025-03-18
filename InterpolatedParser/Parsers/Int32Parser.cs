using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class Int32Parser : IParser<int>
    {
        private Int32Parser() { }
        public static Int32Parser Instance { get; } = new Int32Parser();

        public int Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out int value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Int32");
        }

        public bool TryParse(ref int index, string text, out int value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out int value, bool throwOnFailure)
        {
            value = 0;
            int actualIndex = index;
            bool negative = false;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Int16");
                return false;
            }

            // Handle optional sign
            if (text[actualIndex] == '-')
            {
                negative = true;
                actualIndex++;
            }
            else if (text[actualIndex] == '+')
            {
                actualIndex++;
            }

            // Parse digits
            bool hasDigits = false;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                int digit = text[actualIndex] - '0';

                // Check for overflow
                if (value > (int.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                    {
                        throw new OverflowException("Value is too large for System.Int32");
                    }

                    return false;
                }

                value = value * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                {
                    throw new FormatException("Invalid format of System.Int32");
                }

                return false;
            }

            if (negative)
            {
                value = -value;
            }

            index = actualIndex;
            return true;
        }
    }
}
