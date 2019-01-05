using System;
using System.Text;

namespace Karatsuba.Multiplier
{
    class StringBigInteger
    {
        private string _value { get; set; }
        private bool _isNegative { get; set; }

        private int Length => _value.Length;

        public StringBigInteger(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value must represent an integer");
            if (IsNegative(value))
            {
                _isNegative = true;
                value = value.Substring(1);
            }
            if (IsInteger(value)) _value = TrimLeadingZeroes(value);
            else throw new ArgumentException("value must represent an integer");
        }

        private StringBigInteger(string value, bool isNegative)
        {
            _isNegative = isNegative;
            _value = TrimLeadingZeroes(value);
        }

        private static bool IsInteger(string value)
        {
            foreach (var c in value)
                if (!char.IsDigit(c)) return false;

            return true;
        }

        private static bool IsNegative(string value)
        {
            return value[0] == '-';
        }

        private static string TrimLeadingZeroes(string value)
        {
            int lastLeadingZero = -1;
            int i = 0;
            while (i < value.Length && value[i] == '0')
                lastLeadingZero = i++;
            value = value.Substring(lastLeadingZero + 1);
            if (value == "") value = "0";
            return value;
        }

        private static StringBigInteger Add(StringBigInteger a, StringBigInteger b)
        {
            bool sameSign = a._isNegative == b._isNegative;
            var compare = CompareIgnoreSign(a, b);
            var aIsSmaller = compare < 0;
            var bigger = aIsSmaller ? b : a;
            var smaller = aIsSmaller ? a : b;
            if (sameSign)
            {
                var addResult = AddStrings(bigger._value, smaller._value);
                return new StringBigInteger(addResult, a._isNegative);
            }
            else if (compare == 0 && b._isNegative) return new StringBigInteger("0");
            else
            {
                var isNegative = aIsSmaller == b._isNegative;
                string addResult = AddStrings(bigger._value, smaller._value, true);
                return new StringBigInteger(addResult, isNegative);
            }
        }

        //note: a is always bigger
        private static string AddStrings(string a, string b, bool subtract = false)
        {
            var sb = new StringBuilder();
            int i = a.Length - 1;
            int j = b.Length - 1;
            int carry = 0;
            while (i >= 0)
            {
                int a_i = a[i] - '0' + carry;
                int b_j = j >= 0 ? b[j] - '0' : 0;
                int value = 0;
                if (subtract)
                {
                    value = a_i - b_j;
                    carry = value < 0 ? -1 : 0;
                }
                else
                {
                    value = a_i + b_j;
                    carry = value / 10;
                }

                value = value % 10;
                if (value < 0) value += 10;

                sb.Insert(0, value);
                i--;
                j--;
            }

            if (carry > 0) sb.Insert(0, 1);

            return sb.ToString();
        }

        private static int CompareIgnoreSign(StringBigInteger a, StringBigInteger b)
        {
            if (a._value.Length > b._value.Length) return 1;
            if (a._value.Length < b._value.Length) return -1;
            for (int i = 0; i < a._value.Length; i++)
            {
                var compare = a._value[i] - b._value[i];
                if (compare != 0) return compare;
            }
            return 0;
        }

        public static StringBigInteger FlipSign(StringBigInteger value) => new StringBigInteger(value._value, !value._isNegative);

        public static StringBigInteger operator +(StringBigInteger a, StringBigInteger b) => Add(a, b);

        public static StringBigInteger operator -(StringBigInteger a, StringBigInteger b) => Add(a, FlipSign(b));

        public static StringBigInteger FullKWrapper(StringBigInteger a, StringBigInteger b)
        {
            var isNegative = a._isNegative ^ b._isNegative;
            var posA = a._isNegative ? FlipSign(a) : a;
            var posB = b._isNegative ? FlipSign(b) : b;
            var result = KMultiply(a, b);
            return isNegative ? FlipSign(result) : result;
        }

        public static StringBigInteger KMultiply(StringBigInteger x, StringBigInteger y)
        {
            if (x._value == "0" || y._value == "0") return new StringBigInteger("0");
            if (x._value == "1") return y;
            if (y._value == "1") return x;
            if (x.Length == 1 && y.Length == 1) return SingleDigitMultiply(x, y);
            var compare = CompareIgnoreSign(x, y);
            var bigger = compare >= 0 ? x : y;
            var smaller = compare >= 0 ? y : x;
            var n = bigger.Length;
            var n_2 = n / 2;
            var a = Top(bigger, n_2, n);
            var b = Bottom(bigger, n_2, n);
            var c = Top(smaller, n_2, n);
            var d = Bottom(smaller, n_2, n);

            Console.WriteLine($"... computing {x} * {y}");
            Console.WriteLine($"... length of bigger number: {n}");
            Console.WriteLine($"... a: {a}, b: {b}, c: {c}, d: {d}");
            var ac = KMultiply(a,c);
            var bd = KMultiply(b,d);
            var ad = KMultiply(a,d);
            var bc = KMultiply(b,c);
            var ad_bc = ad + bc; // KMultiply(a + b, c + d) - ac - bd;
            Console.WriteLine($"... ac: {ac}, bd: {bd}, ad+bc: {ad_bc}");

            var ac_n = MultiplyByTens(ac, n);
            var ad_bc_n_2 = MultiplyByTens(ad_bc, n_2);
            var result = ac_n + ad_bc_n_2 + bd;
            Console.WriteLine($"... final result: {ac_n} + {ad_bc_n_2} + {bd} = {result}");

            return result;
        }

        private static StringBigInteger SingleDigitMultiply(StringBigInteger x, StringBigInteger y)
            => new StringBigInteger(((x._value[0] - '0') * (y._value[0] - '0')).ToString());

        private static StringBigInteger Top(StringBigInteger x, int n_2, int n)
        {
            var m = n_2 - (n - x.Length);
            if (m == 0) return new StringBigInteger("0");
            if (m >= x.Length) return x;
            return new StringBigInteger(x._value.Substring(0, m));
        }

        private static StringBigInteger Bottom(StringBigInteger x, int n_2, int n)
        {
            var m = n_2 - (n - x.Length);
            if (m == 0) return x;
            if (m >= x.Length) return new StringBigInteger("0");
            return new StringBigInteger(x._value.Substring(m));
        }

        private static StringBigInteger MultiplyByTens(StringBigInteger ac, int n)
        {
            if (ac._value == "0") return ac;
            var value = ac._value.PadRight(n + ac.Length, '0');
            return new StringBigInteger(value);
        }

        public static StringBigInteger operator *(StringBigInteger a, StringBigInteger b) => FullKWrapper(a, b);

        public override string ToString()
        {
            return (_isNegative ? "-" : "") + _value;
        }
    }
}