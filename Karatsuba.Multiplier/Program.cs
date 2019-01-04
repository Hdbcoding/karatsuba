using System;

namespace Karatsuba.Multiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
            var a = new StringBigInteger("12345678901234567890123456789012345678901234567890123456789012345678901234567890");
            var b = new StringBigInteger("12345678901234567890123456789012345678901234567890123456789012345678901234567890");
            Console.WriteLine(a + b);
        }
    }
}
