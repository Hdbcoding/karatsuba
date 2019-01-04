using System;
using System.Text;

namespace Karatsuba.Multiplier
{
    class StringBigInteger
    {
        private string _value { get; set; }

        public StringBigInteger(string value)
        {
            if (IsInteger(value)) _value = value;
            else throw new ArgumentException("value must represent an integer");
        }

        private static bool IsInteger(string value)
        {
            foreach (var c in value)
            {
                if (!char.IsDigit(c)) return false;
            }

            return true;
        }

        private static string Add(string a, string b)
        {
            StringBuilder sb = new StringBuilder();
            int i = a.Length - 1;
            int j = b.Length - 1;
            int carry = 0;

            while (i >= 0 && j >= 0)
            {
                int a_i = a[i] - '0';
                int b_j = b[j] - '0';
                int value = a_i + b_j + carry;
                carry = value / 10;
                value = value % 10;
                sb.Insert(0, value);
                i--;
                j--;
            }

            return sb.ToString();
        }

        public static StringBigInteger operator+(StringBigInteger a, StringBigInteger b){
            var value = Add(a._value, b._value);
            return new StringBigInteger(value);
        }

        public override string ToString(){
            return _value;
        }
    }
}