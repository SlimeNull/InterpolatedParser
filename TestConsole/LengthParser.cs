using InterpolatedParser;

namespace TestConsole
{
    public class LengthParser : IParser<Length>
    {
        public Length Parse(ref int index, string text)
        {
            double value = 0;
            LengthUnit unit = default;
            Parsing.Parse(ref index, text, $"{value}{unit}");

            return new Length(value, unit);
        }

        public bool TryParse(ref int index, string text, out Length value)
        {
            double lengthValue = 0;
            LengthUnit unit = default;
            if (Parsing.TryParse(ref index, text, $"{lengthValue}{unit}"))
            {
                value = new Length(lengthValue, unit);
                return true;
            }

            value = default;
            return false;
        }
    }
}
