using InterpolatedParser;

namespace TestConsole
{
    public class TwoDoubleParser : IParser<TwoDouble>
    {
        public TwoDouble Parse(ref int index, string text)
        {
            double x = 0;
            double y = 0;
            Parsing.Parse(ref index, text, $"{x},{y}");
            return new TwoDouble(x, y);
        }

        public bool TryParse(ref int index, string text, out TwoDouble value)
        {
            double x = 0;
            double y = 0;
            if (Parsing.TryParse(ref index, text, $"{x},{y}"))
            {
                value = new TwoDouble(x, y);
                return true;
            }

            value = default;
            return false;
        }
    }
}
