namespace InterpolatedParser
{
    public interface IParser
    {

    }

    public interface IParser<T> : IParser
    {
        T Parse(ref int index, string text);
        bool TryParse(ref int index, string text, out T value);
    }
}
