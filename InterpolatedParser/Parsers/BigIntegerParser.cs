using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    using System;
    using System.Numerics;

    public class BigIntegerParser : IParser<BigInteger>
    {
        private BigIntegerParser() { }
        public static BigIntegerParser Instance { get; } = new BigIntegerParser();

        public BigInteger Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out BigInteger value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Numerics.BigInteger");
        }

        public bool TryParse(ref int index, string text, out BigInteger value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out BigInteger value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = BigInteger.Zero;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Numerics.BigInteger");
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
            BigInteger result = BigInteger.Zero;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                int digit = text[actualIndex] - '0';
                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Numerics.BigInteger");
                return false;
            }

            if (negative)
            {
                result = -result;
            }

            value = result;
            index = actualIndex;
            return true;
        }
    }
}
