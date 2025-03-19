using InterpolatedParser;

namespace TestConsole
{
    public class Length2Parser : IParser<Length2>
    {
        public Length2 Parse(ref int index, string text)
        {
            Length x = default;
            Length y = default;
            Parsing.Parse(ref index, text, $"{x},{y}");
            return new Length2(x, y);
        }

        public bool TryParse(ref int index, string text, out Length2 value)
        {
            Length x = default;
            Length y = default;
            if (Parsing.TryParse(ref index, text, $"{x},{y}"))
            {
                value = new Length2(x, y);
                return true;
            }

            value = default;
            return false;
        }
    }
}
