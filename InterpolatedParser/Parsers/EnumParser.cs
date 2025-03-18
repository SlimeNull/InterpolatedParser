using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class EnumParser<T> : IParser<T>
    {
        private static readonly string?[] _enumNames;
        private static readonly T[] _enumValues;

        static EnumParser()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException($"{typeof(T)} is not a Enum type");
            }

            _enumValues = (T[])Enum.GetValues(typeof(T));
            _enumNames = new string?[_enumValues.Length];

            for (int i = 0; i < _enumValues.Length; i++)
            {
                _enumNames[i] = Enum.GetName(typeof(T), _enumValues[i]);
            }
        }

        private EnumParser() { }
        public static EnumParser<T> Instance { get; } = new EnumParser<T>();

        public T Parse(ref int index, string text)
        {
            if (TryParseInternal(ref index, text, out var value, true))
            {
                return value;
            }
            throw new FormatException($"Invalid format of {typeof(T)}");
        }

        public bool TryParse(ref int index, string text, out T value)
        {
            return TryParseInternal(ref index, text, out value, false);
        }

        private bool TryParseInternal(ref int index, string text, out T value, bool throwOnFailure)
        {
            for (int i = 0; i < _enumNames.Length; i++)
            {
                var name = _enumNames[i];

                if (name == null ||
                    index + name.Length - 1 >= text.Length)
                {
                    continue;
                }

                var match = true;
                for (int j = 0; j < name.Length; j++)
                {
                    if (text[index + j] != name[j])
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    index += name.Length;
                    value = _enumValues[i];
                    return true;
                }
            }

            if (throwOnFailure)
            {
                throw new FormatException($"Invalid format of {typeof(T)}");
            }

            value = default;
            return false;
        }
    }
}
