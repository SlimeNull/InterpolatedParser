using System.Runtime.CompilerServices;

namespace InterpolatedParser
{
    public static partial class Parsing
    {
        [InterpolatedStringHandler]
        public struct RefPieceTryParseInterpolatedStringHandler
        {
            private readonly int _originIndex;

            private int _currentIndex;
            private string _input;
            private bool _failed;

            public bool OK => !_failed;
            public int CurrentIndex => _currentIndex;

            public RefPieceTryParseInterpolatedStringHandler(
                int literalLength, int formattedCount, ref int index, string input)
            {
                _originIndex = index;
                _currentIndex = index;
                _input = input;
            }

            public void AppendLiteral(string s)
            {
                if (_failed)
                {
                    _currentIndex = _originIndex;
                    return;
                }

                AppendLiteralForTryParseHandler(ref _currentIndex, ref _failed, _input, s);
            }

            public void AppendFormatted<T>(in T value)
            {
                if (_failed)
                {
                    _currentIndex = _originIndex;
                    return;
                }

                AppendFormatedForTryParseHandler(ref _currentIndex, ref _failed, _input, value);
            }
        }
    }
}
