using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    using System;

    public class SByteParser : IParser<sbyte>
    {
        private SByteParser() { }
        public static SByteParser Instance { get; } = new SByteParser();

        public sbyte Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out sbyte value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.SByte");
        }

        public bool TryParse(ref int index, string text, out sbyte value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out sbyte value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.SByte");
                return false;
            }

            // Parse sign
            bool negative = false;
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
            int result = 0;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                int digit = text[actualIndex] - '0';

                // Check for overflow
                if (result > (sbyte.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too large for System.SByte");
                    return false;
                }

                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.SByte");
                return false;
            }

            if (negative)
            {
                result = -result;

                // Check for underflow
                if (result < sbyte.MinValue)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too small for System.SByte");
                    return false;
                }
            }

            value = (sbyte)result;
            index = actualIndex;
            return true;
        }
    }
}
