using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class Int16Parser : IParser<short>
    {
        private Int16Parser() { }
        public static Int16Parser Instance { get; } = new Int16Parser();

        public short Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out short value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Int16");
        }

        public bool TryParse(ref int index, string text, out short value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out short value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Int16");
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
                if (result > (short.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too large for System.Int16");
                    return false;
                }

                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Int16");
                return false;
            }

            if (negative)
            {
                result = -result;

                // Check for underflow
                if (result < short.MinValue)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too small for System.Int16");
                    return false;
                }
            }

            value = (short)result;
            index = actualIndex;
            return true;
        }
    }
}
