namespace TestConsole
{
    public struct Length
    {
        public Length(double value, LengthUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public double Value { get; set; }
        public LengthUnit Unit { get; }

        public static string? GetUnitText(LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.Nanometer => "nm",
                LengthUnit.Micrometer => "um",
                LengthUnit.Millimeter => "mm",
                LengthUnit.Centimeter => "cm",
                LengthUnit.Decimeter => "dm",
                LengthUnit.Meter => "m",
                LengthUnit.Kilometer => "km",
                _ => null
            };
        }

        public override string ToString()
        {
            return $"{Value}{GetUnitText(Unit)}";
        }
    }
}
