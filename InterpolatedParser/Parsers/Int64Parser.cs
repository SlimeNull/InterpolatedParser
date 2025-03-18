namespace InterpolatedParser.Parsers
{
    public class Int64Parser : IParser<long>
    {
        private Int64Parser() { }
        public static Int64Parser Instance { get; } = new Int64Parser();

        public long Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out long value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.Int64");
        }

        public bool TryParse(ref int index, string text, out long value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out long value, bool throwOnFailure)
        {
            value = 0;
            int actualIndex = index;
            bool negative = false;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.Int64");
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
                if (value > (long.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                    {
                        throw new OverflowException("Value is too large for System.Int64");
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
                    throw new FormatException("Invalid format of System.Int64");
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
