using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    using System;

    public class DecimalParser : IParser<decimal>
    {
        private DecimalParser() { }
        public static DecimalParser Instance { get; } = new DecimalParser();

        public decimal Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out decimal value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Decimal");
        }

        public bool TryParse(ref int index, string text, out decimal value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out decimal value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0m;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Decimal");
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

            // Parse integer part
            bool hasDigits = false;
            decimal integerPart = 0m;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                integerPart = integerPart * 10 + (text[actualIndex] - '0');
                hasDigits = true;
                actualIndex++;
            }

            // Parse fractional part
            decimal fractionalPart = 0m;
            if (actualIndex < text.Length && text[actualIndex] == '.')
            {
                actualIndex++;
                decimal divisor = 10m;
                while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
                {
                    fractionalPart += (text[actualIndex] - '0') / divisor;
                    divisor *= 10;
                    hasDigits = true;
                    actualIndex++;
                }
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Decimal");
                return false;
            }

            value = integerPart + fractionalPart;
            if (negative)
            {
                value = -value;
            }

            // Parse exponent part
            if (actualIndex < text.Length && (text[actualIndex] == 'e' || text[actualIndex] == 'E'))
            {
                actualIndex++;
                bool exponentNegative = false;
                if (actualIndex < text.Length && (text[actualIndex] == '-' || text[actualIndex] == '+'))
                {
                    exponentNegative = text[actualIndex] == '-';
                    actualIndex++;
                }

                int exponent = 0;
                bool hasExponentDigits = false;
                while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
                {
                    exponent = exponent * 10 + (text[actualIndex] - '0');
                    hasExponentDigits = true;
                    actualIndex++;
                }

                if (!hasExponentDigits)
                {
                    if (throwOnFailure)
                        throw new FormatException("Invalid exponent in System.Decimal");
                    return false;
                }

                if (exponentNegative)
                {
                    exponent = -exponent;
                }

                value *= (decimal)Math.Pow(10, exponent);
            }

            index = actualIndex;
            return true;
        }
    }
}
