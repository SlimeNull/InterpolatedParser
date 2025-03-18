﻿using InterpolatedParser;

namespace TestConsole
{
    internal class Program
    {
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
        }
    }
}
