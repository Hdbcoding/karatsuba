using System;
using System.Text;

namespace Karatsuba.Multiplier
{
    class StringBigInteger
    {
        private string _value { get; set; }
        private bool _isNegative { get; set; }

        public StringBigInteger(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("value must represent an integer");
            if (IsNegative(value))
            {
                _isNegative = true;
                value = value.Substring(1);
            }
            if (IsInteger(value)) _value = value;
            else throw new ArgumentException("value must represent an integer");
        }

        private StringBigInteger(string value, bool isNegative)
        {
            _isNegative = isNegative;
            _value = value;
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

        private static StringBigInteger Add(StringBigInteger a, StringBigInteger b)
        {
            bool sameSign = a._isNegative == b._isNegative;

            if (sameSign) return new StringBigInteger(AddStrings(a._value, b._value), a._isNegative);
            else
            {
                var compare = CompareIgnoreSign(a, b);
                if (compare == 0 && b._isNegative) return new StringBigInteger("0");
                var aIsSmaller = compare < 0;
                var isNegative = aIsSmaller == b._isNegative;
                var bigger = aIsSmaller ? b : a;
                var smaller = aIsSmaller ? a : b;

                string addResult = AddStrings(bigger._value, smaller._value, true);
                return new StringBigInteger(addResult, isNegative);
            }
        }

        private static string AddStrings(string a, string b, bool subtract = false)
        {
            int i = a.Length - 1;
            int j = b.Length - 1;
            int carry = 0;
            StringBuilder sb = new StringBuilder();

            while (i >= 0 && j >= 0)
            {
                int a_i = a[i] - '0';
                int b_j = b[j] - '0';
                int value = 0;
                if (subtract)
                {
                    value = a_i - b_j + carry;
                    carry = value < 0 ? -1 : 0;
                }
                else
                {
                    value = a_i + b_j + carry;
                    carry = value / 10;
                }

                value = value % 10; // wrong calculation! this is remainder, not modulus

                sb.Insert(0, value);
                i--;
                j--;
            }
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

        public static StringBigInteger operator +(StringBigInteger a, StringBigInteger b) => Add(a, b);

        public static StringBigInteger operator -(StringBigInteger a, StringBigInteger b) => Add(a, FlipSign(b));

        public static StringBigInteger FlipSign(StringBigInteger value) => new StringBigInteger(value._value, !value._isNegative);

        public override string ToString()
        {
            return _value;
        }
    }
}