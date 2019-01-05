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

        public static StringBigInteger operator +(StringBigInteger a, StringBigInteger b) => Add(a, b);

        public static StringBigInteger operator -(StringBigInteger a, StringBigInteger b) => Add(a, FlipSign(b));

        public static StringBigInteger FlipSign(StringBigInteger value) => new StringBigInteger(value._value, !value._isNegative);

        public override string ToString()
        {
            return (_isNegative ? "-" : "") + _value;
        }
    }
}