using System.Runtime.CompilerServices;

namespace InterpolatedParser
{
    public static partial class Parsing
    {
        [InterpolatedStringHandler]
        public struct RefPieceParseInterpolatedStringHandler
        {
            private int _currentIndex;
            private string _input;

            public int CurrentIndex => _currentIndex;

            public RefPieceParseInterpolatedStringHandler(
                int literalLength, int formattedCount, ref int index, string input)
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
