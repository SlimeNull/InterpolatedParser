using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class ByteParser : IParser<byte>
    {
        private ByteParser() { }
        public static ByteParser Instance { get; } = new ByteParser();

        public byte Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out byte value, throwOnFailure: true))
                return value;
            throw new FormatException("Invalid format of System.Byte");
        }

        public bool TryParse(ref int index, string text, out byte value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out byte value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0;

            // Skip leading whitespace
            while (actualIndex < text.Length && char.IsWhiteSpace(text[actualIndex]))
            {
                actualIndex++;
            }

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Byte");
                return false;
            }

            // Parse digits
            bool hasDigits = false;
            int result = 0;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                int digit = text[actualIndex] - '0';

                // Check for overflow
                if (result > (byte.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too large for System.Byte");
                    return false;
                }

                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Byte");
                return false;
            }

            value = (byte)result;
            index = actualIndex;
            return true;
        }
    }
}
