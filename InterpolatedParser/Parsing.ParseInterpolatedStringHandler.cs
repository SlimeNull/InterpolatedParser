using System.Runtime.CompilerServices;
using InterpolatedParser.Parsers;

namespace InterpolatedParser
{
    public static partial class Parsing
    {
        [InterpolatedStringHandler]
        public struct ParseInterpolatedStringHandler
        {
            private int _currentIndex;
            private string _input;

            public ParseInterpolatedStringHandler(
                int literalLength, int formattedCount, string input)
            {
                _currentIndex = 0;
                _input = input;
            }

            public void AppendLiteral(string s)
            {
                AppendLiteralForParseHandler(ref _currentIndex, _input, s);
            }

            public void AppendFormatted<T>(in T value)
            {
                AppendFormatedForParseHandler(ref _currentIndex, _input, value);
            }
        }
    }
}
