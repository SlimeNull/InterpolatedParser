using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpolatedParser.Parsers
{
    public class EnumParser<T> : TextValueMapParser<T>
    {
        static EnumParser()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException($"{typeof(T)} is not a Enum type");
            }

            var enumValues = (T[])Enum.GetValues(typeof(T));

            for (int i = 0; i < enumValues.Length; i++)
            {
                var name = Enum.GetName(typeof(T), enumValues[i]!);

                if (!string.IsNullOrEmpty(name))
                {
                    Map(name, enumValues[i]);
                }
            }
        }

        private EnumParser() { }
        public static EnumParser<T> Instance { get; } = new EnumParser<T>();
    }
}
