using InterpolatedParser;

namespace TestConsole
{
    internal class Program
    {
        static Program()
        {
            Parsing.RegisterParser(new TwoDoubleParser());
            Parsing.RegisterParser(new LengthUnitParser());
            Parsing.RegisterParser(new LengthParser());
            Parsing.RegisterParser(new Length2Parser());
        }

        static void Main(string[] args)
        {
            var someInteger = 0;
            var someEnum = FileAccess.Read;
            var someFloat1 = 0.0;
            var someFloat2 = 0.0;
            var someFloat3 = 0.0;
            Parsing.Parse("abc-123,Write,2e5,NaN,-∞", $"abc{someInteger},{someEnum},{someFloat1},{someFloat2},{someFloat3}");

            Console.WriteLine(someInteger);
            Console.WriteLine(someEnum);
            Console.WriteLine(someFloat1);
            Console.WriteLine(someFloat2);
            Console.WriteLine(someFloat3);

            TwoDouble twoDouble = default;
            Parsing.Parse("2323,2434", $"{twoDouble}");

            Console.WriteLine(twoDouble);

            Length2 length2 = default;
            int i = 0;
            Parsing.Parse(ref i, "2346mm,34nm", $"{length2}");
            Console.WriteLine(length2);
        }
    }
}
