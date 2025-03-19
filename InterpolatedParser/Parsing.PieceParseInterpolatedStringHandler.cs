using System.Runtime.CompilerServices;
using InterpolatedParser.Parsers;

namespace InterpolatedParser
{
    public static partial class Parsing
    {
        [InterpolatedStringHandler]
        public struct PieceParseInterpolatedStringHandler
        {
            private int _currentIndex;
            private string _input;

            public PieceParseInterpolatedStringHandler(
                int literalLength, int formattedCount, int index, string input)
            {
                _currentIndex = index;
                _input = input;
            }

            public void AppendLiteral(string s)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (_currentIndex + i >= _input.Length ||
                        _input[_currentIndex + i] != s[i])
                    {
                        throw new FormatException();
                    }
                }

                _currentIndex += s.Length;
            }

            public void AppendFormatted<T>(in T value)
            {
                AppendFormattedForParseHandler(ref _currentIndex, _input, value);
            }
        }
    }
}
