using System.Text;

namespace CryptographyConsoleApp
{
	public class BigNumber
	{
        private List<int> digits;
        private bool isNegative;

        public BigNumber(string value)
        {
            digits = new List<int>();
            if (!string.IsNullOrEmpty(value))
            {
                if (value[0] == '-')
                {
                    isNegative = true;
                    value = value.Substring(1);
                }
                for (int i = 0; i < value.Length; i++)
                {
                    if (char.IsDigit(value[value.Length - 1 - i]))
                    {
                        digits.Add(int.Parse(value[value.Length - 1 - i].ToString()));
                    }
                    else
                    {
                        throw new ArgumentException("Value is not a number.");
                    }
                }
            }
        }

        public override string ToString()
        {
            if (digits.Count == 0)
            {
                return "0";
            }
            StringBuilder number = new StringBuilder();
            if (isNegative)
            {
                number.Append("-");
            }
            for (int i = 0; i < digits.Count; i++)
            {
                number.Append(digits[digits.Count - 1 - i]);
            }
                
            return number.ToString();
        }

        public static BigNumber Add(BigNumber number1, BigNumber number2)
        {
            BigNumber result = new BigNumber("");
            
            if (number1.isNegative == number2.isNegative)
            {
                int hold = 0;

                int i = 0;
                while (i < number1.digits.Count || i < number2.digits.Count || hold != 0)
                {
                    int d1 = i < number1.digits.Count ? number1.digits[i] : 0;
                    int d2 = i < number2.digits.Count ? number2.digits[i] : 0;

                    int sum = d1 + d2 + hold;

                    result.digits.Add(sum % 10);
                    hold = sum / 10;

                    i++;
                }

                result.isNegative = number1.isNegative;

                return result;
            }
            else
            {
                BigNumber absNumber1 = new BigNumber(number1.ToString().Trim('-'));
                BigNumber absNumber2 = new BigNumber(number2.ToString().Trim('-'));

                if (absNumber1 < absNumber2)
                {
                    BigNumber temp = absNumber1;
                    absNumber1 = absNumber2;
                    absNumber2 = temp;
                    result.isNegative = !number1.isNegative;
                }

                int hold = 0;

                for (int i = 0; i < absNumber1.digits.Count; i++)
                {
                    int n1 = absNumber1.digits[i];
                    int n2 = i < absNumber2.digits.Count ? absNumber2.digits[i] : 0;

                    int difference = n1 - n2 - hold;
                    if (difference < 0)
                    {
                        difference += 10;
                        hold = 1;
                    }
                    else
                    {
                        hold = 0;
                    }

                    result.digits.Add(difference);
                }
                string strRes = result.ToString().TrimStart('-', '0');
                if (string.IsNullOrEmpty(strRes))
                {
                    strRes = "0";
                }
                BigNumber tempResult = new BigNumber(strRes);
                if (tempResult.digits.Count > 0)
                {
                    tempResult.isNegative = result.isNegative;
                    result = tempResult;
                }
                return result;
            }
        }

        public static BigNumber Multiply(BigNumber number1, BigNumber number2)
        {
            BigNumber result = new BigNumber("0");
            BigNumber absNumber1 = new BigNumber(number1.ToString().Trim('-'));
            BigNumber absNumber2 = new BigNumber(number2.ToString().Trim('-'));


            for (BigNumber i = new BigNumber("0"); i < absNumber1; i = i + new BigNumber("1"))
            {
                result = result + absNumber2;
            }

            if (number1.isNegative != number2.isNegative)
            {
                result.isNegative = true;
            }
            return result;
        }

        public static BigNumber Divide(BigNumber number1, BigNumber number2)
        {
            if (number2 == new BigNumber("0"))
            {
                throw new DivideByZeroException("Divide by zero exception");
            }

            BigNumber result = new BigNumber("0");

            BigNumber absNumber1 = new BigNumber(number1.ToString().Trim('-'));
            BigNumber absNumber2 = new BigNumber(number2.ToString().Trim('-'));

            while (absNumber1 >= absNumber2)
            {
                absNumber1 = absNumber1 - absNumber2;
                result = result + new BigNumber("1");
            }

            if (number1.isNegative != number2.isNegative)
            {
                result.isNegative = true;
            }

            return result;
        }

        public static BigNumber Reminder(BigNumber number1, BigNumber number2)
        {
            if (number2 == new BigNumber("0"))
            {
                throw new DivideByZeroException("Divide by zero exception");
            }

            BigNumber result = new BigNumber("0");

            if (number1.isNegative != number2.isNegative)
            {
                result.isNegative = true;
            }

            BigNumber absNumber1 = new BigNumber(number1.ToString().Trim('-'));
            BigNumber absNumber2 = new BigNumber(number2.ToString().Trim('-'));

            while (absNumber1 >= absNumber2)
            {
                absNumber1 = absNumber1 - absNumber2;
                //result = result + new BigNumber("1");
            }

            return absNumber1;
        }

        public static BigNumber operator %(BigNumber number1, BigNumber number2)
        {
            return Reminder(number1, number2);
        }

        public static BigNumber operator /(BigNumber number1, BigNumber number2)
        {
            return Divide(number1, number2);
        }

        public static BigNumber operator *(BigNumber number1, BigNumber number2)
        {
            return Multiply(number1, number2);
        }

        public static BigNumber operator +(BigNumber number1, BigNumber number2)
        {
            return Add(number1, number2);
        }

        public static BigNumber operator -(BigNumber number1, BigNumber number2)
        {
            BigNumber number = new BigNumber(number2.ToString().Trim('-'));
            number.isNegative = !number2.isNegative;
            return Add(number1, number);
        }

        public static bool operator <(BigNumber number1, BigNumber number2)
        {
            if (number1.isNegative != number2.isNegative)
            {
                return number1.isNegative;
            }
                
            if (number1.digits.Count != number2.digits.Count)
            {
                if (number1.digits.Count < number2.digits.Count && !number1.isNegative || number1.digits.Count > number2.digits.Count && number1.isNegative)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            for (int i = 0; i < number1.digits.Count; i++)
            {

                if (number1.digits[number1.digits.Count - 1 - i] != number2.digits[number1.digits.Count - 1 - i])
                {
                    return (number1.digits[number1.digits.Count - 1 - i] < number2.digits[number1.digits.Count - 1 - i]) ^ number1.isNegative;
                }
            }

            return false;
        }

        public static bool operator >(BigNumber number1, BigNumber number2)
        {
            return (number1 != number2) && !(number1 < number2);
        }

        public static bool operator ==(BigNumber number1, BigNumber number2)
        {
            if (number1.digits.Count == 1 && number1.digits[0] == 0 && number2.digits.Count == 1 && number2.digits[0] == 0)
            {
                return true;
            }
            if (number1.isNegative != number2.isNegative || number1.digits.Count != number2.digits.Count)
            {
                return false;
            }
            for (int i = 0; i < number1.digits.Count; i++)
            {
                if (number1.digits[i] != number2.digits[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(BigNumber number1, BigNumber num2)
        {
            return !(number1 == num2);
        }

        public static bool operator <=(BigNumber number1, BigNumber number2)
        {
            return (number1 < number2) || (number1 == number2);
        }

        public static bool operator >=(BigNumber number1, BigNumber number2)
        {
            return (number1 > number2) || (number1 == number2);
        }

        public override bool Equals(object obj)
        {
            if (obj is BigNumber)
            {
                return this == (BigNumber)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

}

