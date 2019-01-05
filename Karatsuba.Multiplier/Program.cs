using System;

namespace Karatsuba.Multiplier
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new StringBigInteger("100");
            var d = new StringBigInteger("25");
            PrintAllOperations(c, d);

            var a = new StringBigInteger("54");
            var b = new StringBigInteger("12");
            PrintAllOperations(a, b);

            var a0 = new StringBigInteger("5454");
            var b0 = new StringBigInteger("1212");
            PrintAllOperations(a0, b0);

            // var a1 = new StringBigInteger("321");
            // var b1 = new StringBigInteger("345");
            // PrintAllOperations(a1, b1);

            // var a2 = new StringBigInteger("54321");
            // var b2 = new StringBigInteger("12345");
            // PrintAllOperations(a2, b2);

            // var a3 = new StringBigInteger("1234");
            // var b3 = new StringBigInteger("321");
            // PrintAllOperations(a3, b3);

            // var a4 = new StringBigInteger("1234");
            // var b4 = new StringBigInteger("123");
            // PrintAllOperations(a4, b4);

            var a5 = new StringBigInteger("3141592653589793238462643383279502884197169399375105820974944592");
            var b5 = new StringBigInteger("2718281828459045235360287471352662497757247093699959574966967627");
            PrintAllOperations(a5, b5);
        }

        static void PrintAllOperations(StringBigInteger a, StringBigInteger b)
        {
            // PrintAdd(a, b);
            // PrintSubtract(a, b);
            // PrintSubtract(b, a);
            PrintMultiply(a, b);
        }

        static void PrintAdd(StringBigInteger a, StringBigInteger b)
        {
            Console.WriteLine($"{a} + {b} = {a + b}");
        }

        static void PrintSubtract(StringBigInteger a, StringBigInteger b)
        {
            Console.WriteLine($"{a} - {b} = {a - b}");
        }

        private static void PrintMultiply(StringBigInteger a, StringBigInteger b)
        {
            Console.WriteLine($"{a} * {b} = {a * b}");
        }
    }
}
