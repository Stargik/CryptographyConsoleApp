using System;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static CryptographyConsoleApp.Records;

namespace CryptographyConsoleApp
{
	public class CryptographyCalculator
	{
		public static BigNumber EulerFunction(BigNumber n)
		{
            BigNumber result = new BigNumber(n.ToString());

            for (BigNumber i = new BigNumber("2"); i * i <= n; i = i + new BigNumber("1"))
            {
                if (n % i == new BigNumber("0"))
                {
                    while (n % i == new BigNumber("0"))
                    {
                        n = n / i;
                    }
                    result = result - result / i;
                }
            }

            if (n > new BigNumber("1"))
            {
                result = result - result / n;
            }

            return result;
        }

        public static BigNumber MobiusFunction(BigNumber n)
        {

            BigNumber result = new BigNumber("1");

            for (BigNumber i = new BigNumber("2"); i * i <= n; i = i + new BigNumber("1"))
            {
                if (n % i == new BigNumber("0"))
                {
                    if (n % (i * i) == new BigNumber("0"))
                    {
                        return new BigNumber("0");
                    }

                    result = result * new BigNumber("-1");

                    int count = 0;

                    while (n % i == new BigNumber("0"))
                    {
                        count++;
                        n = n / i;
                    }
                    if (count > 1)
                    {
                        return new BigNumber("0");
                    }
                }
            }

            if (n > new BigNumber("1"))
            {
                result = result * new BigNumber("-1");
            }

            return result;
        }

        public static BigNumber GCD(BigNumber n1, BigNumber n2)
        {
            while (n2 != new BigNumber("0"))
            {
                BigNumber temp = new BigNumber(n2.ToString());
                n2 = n1 % n2;
                n1 = temp;
            }
            return n1;
        }

        public static BigNumber LCM(BigNumber n1, BigNumber n2)
        {
            return (n1 * n2) / GCD(n1, n2);
        }

        public static BigNumber LCM(List<BigNumber> numbers)
        {
            if (numbers.Count < 2)
            {
                throw new ArgumentException("Count must be more than 1.");
            }

            BigNumber lcm = new BigNumber(numbers[0].ToString());
            for (int i = 1; i < numbers.Count; i++)
            {
                lcm = LCM(lcm, numbers[i]);
            }
            return lcm;
        }

        public static BigNumber GCDExternal(BigNumber n1, BigNumber n2, out BigNumber x, out BigNumber y)
        {
            if (n1 == new BigNumber("0"))
            {
                x = new BigNumber("0");
                y = new BigNumber("1");
                return n2;
            }
            BigNumber x2 = new BigNumber("0");
            BigNumber y2 = new BigNumber("0");

            BigNumber temp = GCDExternal(n2 % n1, n1, out x2, out y2);
            x = y2 - (n2 / n1) * x2;
            y = x2;
            return temp;
        }

        public static BigNumber ChineseReminder(List<Comparisons> comparisons, out BigNumber mod)
        {
            for (int i = 0; i < comparisons.Count; i++)
            {
                for (int k =  i + 1; k < comparisons.Count; k++)
                {
                    if (GCD(new BigNumber(comparisons[i].m.ToString()), new BigNumber(comparisons[k].m.ToString())) != new BigNumber("1"))
                    {
                        throw new Exception("Equations has no solutions.");
                    }
                }
            }
            BigNumber totalM = new BigNumber("1");
            for (int i = 0; i < comparisons.Count; i++)
            {
                totalM = totalM * comparisons[i].m;
            }

            List<BigNumber> Ms = new List<BigNumber>();

            for (int i = 0; i < comparisons.Count; i++)
            {
                BigNumber M = totalM / comparisons[i].m;
                Ms.Add(M);
            }

            List<BigNumber> Ns = new List<BigNumber>();

            for (int i = 0; i < comparisons.Count; i++)
            {
                BigNumber x = new BigNumber("0");
                BigNumber y = new BigNumber("0");
                var g = GCDExternal(Ms[i], comparisons[i].m, out x, out y);
                while (x < new BigNumber("0"))
                {
                    x = x + comparisons[i].m;
                }
                Ns.Add(x);
            }
            var result = new BigNumber("0");
            for (int i = 0; i < comparisons.Count; i++)
            {
                var temp = Ms[i] * Ns[i] * comparisons[i].b;
                result = result + temp;
            }
            while (result > totalM)
            {
                result = result - totalM;
            }
            while (result < new BigNumber("0"))
            {
                result = result + totalM;
            }
            mod = totalM;
            return result;
        }

        public static BigNumber PowByModul(BigNumber n1, BigNumber n2, BigNumber m)
        {
            if (n2 < new BigNumber("0"))
            {
                return null;
            }
            if (n2 == new BigNumber("0"))
            {
                return new BigNumber("1");
            }
            if (n2 == new BigNumber("1") || n1 == new BigNumber("0"))
            {
                return n1 % m;
            }
                
            var result = new BigNumber("1");

            while (n2 > new BigNumber("0"))
            {
                if (n2 % new BigNumber("2") == new BigNumber("1"))
                {
                    result = (result * n1) % m;
                }
                n1 = (n1 * n1) % m;
                n2 = n2 / new BigNumber("2");
            }

            return result;
        }

        public static BigNumber LegendreSymbol(BigNumber a, BigNumber p)
        {
            if (p < new BigNumber("3") || p % new BigNumber("2") == new BigNumber("0"))
            {
                throw new ArgumentException("P is not correct.");
            }

            if (a % p == new BigNumber("0"))
            {
                return new BigNumber("0");
            }

            BigNumber exp = (p - new BigNumber("1")) / new BigNumber("2");

            BigNumber powRes = PowByModul(a, exp, p);

            BigNumber result = new BigNumber("0");

            if (powRes == new BigNumber("1") || powRes == p - new BigNumber("1"))
            {
                if (powRes == new BigNumber("1"))
                {
                    result = new BigNumber("1");
                }
                else
                {
                    result = new BigNumber("-1");
                }
            }
            return result;
        }

        public static BigNumber JacobiSymbol(BigNumber a, BigNumber p)
        {
            if (p <= new BigNumber("0") || p % new BigNumber("2") == new BigNumber("0"))
            {
                throw new ArgumentException("P is not correct.");
            }

            if (GCD(a, p) != new BigNumber("1"))
            {
                return new BigNumber("0");
            }

            BigNumber result = new BigNumber("1");

            if (a < new BigNumber("0"))
            {
                a = a * new BigNumber("-1");
                if (p % new BigNumber("4") == new BigNumber("3"))
                {
                    result = result * new BigNumber("-1");
                }
            }

            while (a != new BigNumber("0"))
            {
                BigNumber t = new BigNumber("0");
                while (a % new BigNumber("2") == new BigNumber("0"))
                {
                    t = t + new BigNumber("1");
                    a = a / new BigNumber("2");
                }

                if (t % new BigNumber("2") != new BigNumber("0"))
                {
                    if (p % new BigNumber("8") == new BigNumber("3") || p % new BigNumber("8") == new BigNumber("5"))
                    {
                        result = result * new BigNumber("-1");
                    }
                }

                if (a % new BigNumber("4") == new BigNumber("3") && p % new BigNumber("4") == new BigNumber("3"))
                {
                    result = result * new BigNumber("-1");
                }
                BigNumber temp = a;
                a = p % temp;
                p = temp;
            }
            
            return result;

        }
    }
}

