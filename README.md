# InterpolatedParser

Simple value parser with interpolated string.

Example:

```cs
using InterpolatedParsing;

string input = "x is 69";

int x = 0;
Parsing.Parse(input, $"x is {x}");

Console.WriteLine(x); // Prints 69.
```

## Built-In Support

Byte, UInt16, UInt32, UInt64, SByte, Int16, Int32, Int64, Single, Double, BigInteger, Decimal, Boolean, Enum is supported by default

## Add custom type support

Write a parser:

```cs
public record struct TwoDouble(double X, double Y);

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
```

Register parser:

```cs
Parsing.RegisterParser(new TwoDoubleParser());
```

Use:

```cs
TwoDouble twoDouble = default;
Parsing.Parse("2323,2434", $"{twoDouble}");

Console.WriteLine(twoDouble);  // Prints TwoDouble { X = 2323, Y = 2434 }
```

