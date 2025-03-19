using InterpolatedParser.Parsers;

namespace TestConsole
{
    public class LengthUnitParser : TextValueMapParser<LengthUnit>
    {
        static LengthUnitParser()
        {
            Map("nm", LengthUnit.Nanometer);
            Map("um", LengthUnit.Micrometer);
            Map("mm", LengthUnit.Millimeter);
            Map("cm", LengthUnit.Centimeter);
            Map("dm", LengthUnit.Decimeter);
            Map("m", LengthUnit.Meter);
            Map("km", LengthUnit.Kilometer);
        }
    }
}
