using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class UInt16Parser : IParser<ushort>
    {
        private UInt16Parser() { }
        public static UInt16Parser Instance { get; } = new UInt16Parser();

        public ushort Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out ushort value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.UInt16");
        }

        public bool TryParse(ref int index, string text, out ushort value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out ushort value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.UInt16");
                return false;
            }

            // Parse digits
            bool hasDigits = false;
            int result = 0;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                int digit = text[actualIndex] - '0';

                // Check for overflow
                if (result > (ushort.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too large for System.UInt16");
                    return false;
                }

                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.UInt16");
                return false;
            }

            value = (ushort)result;
            index = actualIndex;
            return true;
        }
    }
}
