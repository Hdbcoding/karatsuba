using System;

namespace Karatsuba.Multiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new StringBigInteger("12345678901234567890123456789012345678901234567890123456789012345678901234567890");
            var b = new StringBigInteger("12345678901234567890123456789012345678901234567890123456789012345678901234567890");
            PrintAddAndSubtract(a, b);

            var a2 = new StringBigInteger("54321");
            var b2 = new StringBigInteger("12345");
            PrintAddAndSubtract(a2, b2);

            var a3 = new StringBigInteger("1234");
            var b3 = new StringBigInteger("321");
            PrintAddAndSubtract(a3, b3);

            var a4 = new StringBigInteger("1234");
            var b4 = new StringBigInteger("123");
            PrintAddAndSubtract(a4, b4);
        }

        static void PrintAddAndSubtract(StringBigInteger a, StringBigInteger b)
        {
            PrintAdd(a, b);
            PrintSubtract(a, b);
            PrintSubtract(b, a);
        }

        static void PrintAdd(StringBigInteger a, StringBigInteger b)
        {
            Console.WriteLine($"{a} + {b} = {a + b}");
        }

        static void PrintSubtract(StringBigInteger a, StringBigInteger b)
        {
            Console.WriteLine($"{a} - {b} = {a - b}");
        }
    }
}
