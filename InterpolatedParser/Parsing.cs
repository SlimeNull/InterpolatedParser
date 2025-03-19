using InterpolatedParser.Parsers;
using InterpolatedParser.Utilities;

namespace InterpolatedParser
{
    public static partial class Parsing
    {
        private static readonly Dictionary<Type, IParser> s_parsers = new Dictionary<Type, IParser>();

        static Parsing()
        {
            RegisterParser(ByteParser.Instance);
            RegisterParser(UInt16Parser.Instance);
            RegisterParser(UInt32Parser.Instance);
            RegisterParser(UInt64Parser.Instance);
            RegisterParser(SByteParser.Instance);
            RegisterParser(Int16Parser.Instance);
            RegisterParser(Int32Parser.Instance);
            RegisterParser(Int64Parser.Instance);
            RegisterParser(SingleParser.Instance);
            RegisterParser(DoubleParser.Instance);
            RegisterParser(DecimalParser.Instance);
            RegisterParser(BigIntegerParser.Instance);
            RegisterParser(BooleanParser.Instance);
        }

        public static void RegisterParser<T>(IParser<T> parser)
        {
            s_parsers[typeof(T)] = parser;
        }

        public static void Parse(ref int index, string input,
            [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument(nameof(index), nameof(input))]
            RefPieceParseInterpolatedStringHandler template)
        {

        }

        public static void Parse(int index, string input,
            [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument(nameof(index), nameof(input))]
            PieceParseInterpolatedStringHandler template)
        {

        }

        public static void Parse(string input,
            [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument(nameof(input))]
            ParseInterpolatedStringHandler template)
        {

        }

        public static bool TryParse(ref int index, string input,
            [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument(nameof(index), nameof(input))]
            RefPieceTryParseInterpolatedStringHandler template)
        {
            return template.OK;
        }

        public static bool TryParse(int index, string input,
            [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument(nameof(index), nameof(input))]
            PieceTryParseInterpolatedStringHandler template)
        {
            return template.OK;
        }

        public static bool TryParse(string input,
            [System.Runtime.CompilerServices.InterpolatedStringHandlerArgument(nameof(input))]
            TryParseInterpolatedStringHandler template)
        {
            return template.OK;
        }

        private static void AppendLiteralForParseHandler(ref int index, string text, string literal)
        {
            for (int i = 0; i < literal.Length; i++)
            {
                if (index + i >= text.Length ||
                    text[index + i] != literal[i])
                {
                    throw new FormatException();
                }
            }

            index += literal.Length;
        }

        private static void AppendFormattedForParseHandler<T>(ref int index, string text, in T value)
        {
            if (index >= text.Length)
            {
                throw new FormatException();
            }

            if (!s_parsers.TryGetValue(typeof(T), out var parser))
            {
                if (typeof(T).IsEnum)
                {
                    parser = EnumParser<T>.Instance;
                }
                else
                {
                    throw new ParserNotFoundException(typeof(T));
                }
            }

            var actualParser = (IParser<T>)parser;
            var parsedValue = actualParser.Parse(ref index, text);

            UnsafeUtils.AsRef(value) = parsedValue;
        }

        private static void AppendLiteralForTryParseHandler(ref int index, ref bool failed, string text, string literal)
        {
            for (int i = 0; i < literal.Length; i++)
            {
                if (index + i >= text.Length ||
                    text[index + i] != literal[i])
                {
                    failed = true;
                    return;
                }
            }

            index += literal.Length;
        }

        private static void AppendFormattedForTryParseHandler<T>(ref int index, ref bool failed, string text, in T value)
        {
            if (index >= text.Length)
            {
                failed = true;
                return;
            }

            if (!s_parsers.TryGetValue(typeof(T), out var parser))
            {
                if (typeof(T).IsEnum)
                {
                    parser = EnumParser<T>.Instance;
                }
                else
                {
                    failed = true;
                    return;
                }
            }

            var actualParser = (IParser<T>)parser;
            var parsedValue = actualParser.Parse(ref index, text);

            UnsafeUtils.AsRef(value) = parsedValue;
        }
    }
}
