using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class DoubleParser : IParser<double>
    {
        private DoubleParser() { }
        public static DoubleParser Instance { get; } = new DoubleParser();

        public double Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out double value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Double");
        }

        public bool TryParse(ref int index, string text, out double value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out double value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0.0;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Double");
                return false;
            }

            // Handle special cases: NaN, ∞, -∞
            if (TryParseSpecialValue(ref actualIndex, text, out value))
            {
                index = actualIndex;
                return true;
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
            double integerPart = 0.0;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                integerPart = integerPart * 10 + (text[actualIndex] - '0');
                hasDigits = true;
                actualIndex++;
            }

            // Parse fractional part
            double fractionalPart = 0.0;
            if (actualIndex < text.Length && text[actualIndex] == '.')
            {
                actualIndex++;
                double divisor = 10.0;
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
                    throw new FormatException("Invalid format of System.Double");
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
                        throw new FormatException("Invalid exponent in System.Double");
                    return false;
                }

                if (exponentNegative)
                {
                    exponent = -exponent;
                }

                value *= Math.Pow(10, exponent);
            }

            index = actualIndex;
            return true;
        }

        private bool TryParseSpecialValue(ref int index, string text, out double value)
        {
            value = 0.0;

            // Check for NaN
            if (index + 2 < text.Length && text.Substring(index, 3).Equals("NaN", StringComparison.OrdinalIgnoreCase))
            {
                value = double.NaN;
                index += 3;
                return true;
            }

            // Check for ∞ (Unicode U+221E)
            if (index < text.Length && text[index] == '∞')
            {
                value = double.PositiveInfinity;
                index += 1;
                return true;
            }

            // Check for -∞ (Unicode U+221E with a negative sign)
            if (index + 1 < text.Length && text[index] == '-' && text[index + 1] == '∞')
            {
                value = double.NegativeInfinity;
                index += 2;
                return true;
            }

            return false;
        }
    }
}
