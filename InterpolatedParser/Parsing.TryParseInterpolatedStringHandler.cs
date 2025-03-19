using System.Runtime.CompilerServices;

namespace InterpolatedParser
{
    public static partial class Parsing
    {
        [InterpolatedStringHandler]
        public struct TryParseInterpolatedStringHandler
        {
            private int _currentIndex;
            private string _input;
            private bool _failed;

            public bool OK => !_failed;

            public TryParseInterpolatedStringHandler(
                int literalLength, int formattedCount, string input)
            {
                _currentIndex = 0;
                _input = input;
            }

            public void AppendLiteral(string s)
            {
                if (_failed)
                {
                    return;
                }

                AppendLiteralForTryParseHandler(ref _currentIndex, ref _failed, _input, s);
            }

            public void AppendFormatted<T>(in T value)
            {
                if (_failed)
                {
                    return;
                }

                AppendFormattedForTryParseHandler(ref _currentIndex, ref _failed, _input, value);
            }
        }
    }
}
