using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class TextValueMapParser<T> : IParser<T>
    {
        private static readonly Dictionary<string, T> s_textValueMap = new Dictionary<string, T>();

        protected static void Map(string text, T value)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Invalid text", nameof(text));
            }

            s_textValueMap[text] = value;
        }

        private static bool Match(int index, string text, string toMatch)
        {
            if (index + toMatch.Length - 1 >= text.Length)
            {
                return false;
            }

            var match = true;
            for (int i = 0; i < toMatch.Length; i++)
            {
                if (text[index + i] != toMatch[i])
                {
                    match = false;
                    break;
                }
            }

            return match;
        }

        private bool TryParseInternal(ref int index, string text, [NotNullWhen(true)] out T? value, bool throwOnFailure)
        {
            foreach (var map in s_textValueMap)
            {
                if (Match(index, text, map.Key))
                {
                    value = map.Value!;
                    index += map.Key.Length;
                    return true;
                }
            }

            if (throwOnFailure)
            {
                throw new FormatException();
            }

            value = default;
            return false;
        }

        public T Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out var value, true))
            {
                return value;
            }
            throw new FormatException();
        }

        public bool TryParse(ref int index, string text, [NotNullWhen(true)] out T? value)
        {
            return TryParseInternal(ref index, text, out value, false);
        }
    }
}
