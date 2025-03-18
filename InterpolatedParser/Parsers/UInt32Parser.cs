﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class UInt32Parser : IParser<uint>
    {
        private UInt32Parser() { }
        public static UInt32Parser Instance { get; } = new UInt32Parser();

        public uint Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out uint value, throwOnFailure: true))
            {
                return value;
            }
            throw new FormatException("Invalid format of System.UInt32");
        }

        public bool TryParse(ref int index, string text, out uint value)
        {
            return TryParseInternal(ref index, text, out value, throwOnFailure: false);
        }

        private bool TryParseInternal(ref int index, string text, out uint value, bool throwOnFailure)
        {
            int actualIndex = index;
            value = 0;

            if (actualIndex >= text.Length)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.UInt32");
                return false;
            }

            // Parse digits
            bool hasDigits = false;
            uint result = 0;
            while (actualIndex < text.Length && text[actualIndex] >= '0' && text[actualIndex] <= '9')
            {
                uint digit = (uint)(text[actualIndex] - '0');

                // Check for overflow
                if (result > (uint.MaxValue - digit) / 10)
                {
                    if (throwOnFailure)
                        throw new OverflowException("Value is too large for System.UInt32");
                    return false;
                }

                result = result * 10 + digit;
                hasDigits = true;
                actualIndex++;
            }

            if (!hasDigits)
            {
                if (throwOnFailure)
                    throw new FormatException("Invalid format of System.UInt32");
                return false;
            }

            value = result;
            index = actualIndex;
            return true;
        }
    }
}
