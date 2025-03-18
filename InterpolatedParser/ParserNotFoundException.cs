namespace InterpolatedParser
{
    public class ParserNotFoundException : Exception
    {
        public ParserNotFoundException(Type requiredType, string message) : base(message)
        {
            RequiredType = requiredType;
        }

        public ParserNotFoundException(Type requiredType) : this(requiredType, $"No parser found for {requiredType}")
        { }

        public Type RequiredType { get; }
    }
}
