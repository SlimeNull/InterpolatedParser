namespace TestConsole
{
    public struct Length2
    {
        public Length2(Length x, Length y)
        {
            X = x;
            Y = y;
        }

        public Length X { get; set; }
        public Length Y { get; set; }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
