using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static CryptographyConsoleApp.Records;

namespace CryptographyConsoleApp
{
    public class CryptographyBigInt
    {
        public static BigInteger EulerFunction(BigInteger n)
        {
            BigInteger result = BigInteger.Parse(n.ToString());

            for (BigInteger i = BigInteger.Parse("2"); i * i <= n; i = i + BigInteger.Parse("1"))
            {
                if (n % i == BigInteger.Parse("0"))
                {
                    while (n % i == BigInteger.Parse("0"))
                    {
                        n = n / i;
                    }
                    result = result - result / i;
                }
            }

            if (n > BigInteger.Parse("1"))
            {
                result = result - result / n;
            }

            return result;
        }

        public static BigInteger MobiusFunction(BigInteger n)
        {

            BigInteger result = BigInteger.Parse("1");

            for (BigInteger i = BigInteger.Parse("2"); i * i <= n; i = i + BigInteger.Parse("1"))
            {
                if (n % i == BigInteger.Parse("0"))
                {
                    if (n % (i * i) == BigInteger.Parse("0"))
                    {
                        return BigInteger.Parse("0");
                    }

                    result = result * BigInteger.Parse("-1");

                    int count = 0;

                    while (n % i == BigInteger.Parse("0"))
                    {
                        count++;
                        n = n / i;
                    }
                    if (count > 1)
                    {
                        return BigInteger.Parse("0");
                    }
                }
            }

            if (n > BigInteger.Parse("1"))
            {
                result = result * BigInteger.Parse("-1");
            }

            return result;
        }

        public static BigInteger GCD(BigInteger n1, BigInteger n2)
        {
            BigInteger n11 = BigInteger.Parse(n1.ToString());
            BigInteger n22 = BigInteger.Parse(n2.ToString());
            if (n22 == BigInteger.Parse("0"))
            {
                return n11;
            }

            while (n22 != BigInteger.Parse("0"))
            {
                BigInteger temp = BigInteger.Parse(n22.ToString());
                n22 = n11 % n22;
                n11 = temp;
            }
            return n11;
        }

        public static BigInteger LCM(BigInteger n1, BigInteger n2)
        {
            return (n1 * n2) / GCD(n1, n2);
        }

        public static BigInteger LCM(List<BigInteger> numbers)
        {
            if (numbers.Count < 2)
            {
                throw new ArgumentException("Count must be more than 1.");
            }

            BigInteger lcm = BigInteger.Parse(numbers[0].ToString());
            for (int i = 1; i < numbers.Count; i++)
            {
                lcm = LCM(lcm, numbers[i]);
            }
            return lcm;
        }

        public static BigInteger GCDExternal(BigInteger n1, BigInteger n2, out BigInteger x, out BigInteger y)
        {
            if (n1 == BigInteger.Parse("0"))
            {
                x = BigInteger.Parse("0");
                y = BigInteger.Parse("1");
                return n2;
            }
            BigInteger x2 = BigInteger.Parse("0");
            BigInteger y2 = BigInteger.Parse("0");

            BigInteger temp = GCDExternal(n2 % n1, n1, out x2, out y2);
            x = y2 - (n2 / n1) * x2;
            y = x2;
            return temp;
        }

        public static BigInteger ChineseReminder(List<ComparisonsBigInt> comparisons, out BigInteger mod)
        {
            for (int i = 0; i < comparisons.Count; i++)
            {
                for (int k = i + 1; k < comparisons.Count; k++)
                {
                    if (GCD(BigInteger.Parse(comparisons[i].m.ToString()), BigInteger.Parse(comparisons[k].m.ToString())) != BigInteger.Parse("1"))
                    {
                        throw new Exception("Equations has no solutions.");
                    }
                }
            }
            BigInteger totalM = BigInteger.Parse("1");
            for (int i = 0; i < comparisons.Count; i++)
            {
                totalM = totalM * comparisons[i].m;
            }

            List<BigInteger> Ms = new List<BigInteger>();

            for (int i = 0; i < comparisons.Count; i++)
            {
                BigInteger M = totalM / comparisons[i].m;
                Ms.Add(M);
            }

            List<BigInteger> Ns = new List<BigInteger>();

            for (int i = 0; i < comparisons.Count; i++)
            {
                BigInteger x = BigInteger.Parse("0");
                BigInteger y = BigInteger.Parse("0");
                var g = GCDExternal(Ms[i], comparisons[i].m, out x, out y);
                while (x < BigInteger.Parse("0"))
                {
                    x = x + comparisons[i].m;
                }
                Ns.Add(x);
            }
            var result = BigInteger.Parse("0");
            for (int i = 0; i < comparisons.Count; i++)
            {
                var temp = Ms[i] * Ns[i] * comparisons[i].b;
                result = result + temp;
            }
            while (result > totalM)
            {
                result = result - totalM;
            }
            while (result < BigInteger.Parse("0"))
            {
                result = result + totalM;
            }
            mod = totalM;
            return result;
        }

        public static BigInteger Pow(BigInteger n1, BigInteger n2)
        {
            if (n2 < BigInteger.Parse("0"))
            {
                return 0;
            }
            if (n2 == BigInteger.Parse("0"))
            {
                return BigInteger.Parse("1");
            }
            if (n2 == BigInteger.Parse("1") || n1 == BigInteger.Parse("0"))
            {
                return n1;
            }

            var result = BigInteger.Parse("1");
            var n11 = BigInteger.Parse(n1.ToString());
            var n22 = BigInteger.Parse(n2.ToString());

            while (n22 > BigInteger.Parse("0"))
            {
                if (n22 % BigInteger.Parse("2") == BigInteger.Parse("1"))
                {
                    result = result * n11;
                }
                n11 = n11 * n11;
                n22 = n22 / BigInteger.Parse("2");
            }

            return result;
        }

        public static BigInteger PowByModul(BigInteger n1, BigInteger n2, BigInteger m)
        {
            if (n2 < BigInteger.Parse("0"))
            {
                return 0;
            }
            if (n2 == BigInteger.Parse("0"))
            {
                return BigInteger.Parse("1");
            }
            if (n2 == BigInteger.Parse("1") || n1 == BigInteger.Parse("0"))
            {
                return n1 % m;
            }

            var result = BigInteger.Parse("1");
            var n11 = BigInteger.Parse(n1.ToString());
            var n22 = BigInteger.Parse(n2.ToString());

            while (n22 > BigInteger.Parse("0"))
            {
                if (n22 % BigInteger.Parse("2") == BigInteger.Parse("1"))
                {
                    result = (result * n11) % m;
                }
                n11 = (n11 * n11) % m;
                n22 = n22 / BigInteger.Parse("2");
            }

            return result;
        }

        public static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m;
            BigInteger y = 0, x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                BigInteger q = a / m;
                BigInteger t = m;

                m = a % m;
                a = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            if (x < 0)
                x += m0;

            return x;
        }

        public static BigInteger LegendreSymbol(BigInteger a, BigInteger p)
        {
            if (p < BigInteger.Parse("3") || p % BigInteger.Parse("2") == BigInteger.Parse("0"))
            {
                throw new ArgumentException("P is not correct.");
            }

            if (a % p == BigInteger.Parse("0"))
            {
                return BigInteger.Parse("0");
            }

            BigInteger exp = (p - BigInteger.Parse("1")) / BigInteger.Parse("2");

            BigInteger powRes = PowByModul(a, exp, p);

            BigInteger result = BigInteger.Parse("0");

            if (powRes == BigInteger.Parse("1") || powRes == p - BigInteger.Parse("1"))
            {
                if (powRes == BigInteger.Parse("1"))
                {
                    result = BigInteger.Parse("1");
                }
                else
                {
                    result = BigInteger.Parse("-1");
                }
            }
            return result;
        }

        public static BigInteger JacobiSymbol(BigInteger a, BigInteger p)
        {
            if (p <= BigInteger.Parse("0") || p % BigInteger.Parse("2") == BigInteger.Parse("0"))
            {
                throw new ArgumentException("P is not correct.");
            }

            if (GCD(a, p) != BigInteger.Parse("1"))
            {
                return BigInteger.Parse("0");
            }

            BigInteger result = BigInteger.Parse("1");

            if (a < BigInteger.Parse("0"))
            {
                a = a * BigInteger.Parse("-1");
                if (p % BigInteger.Parse("4") == BigInteger.Parse("3"))
                {
                    result = result * BigInteger.Parse("-1");
                }
            }

            while (a != BigInteger.Parse("0"))
            {
                BigInteger t = BigInteger.Parse("0");
                while (a % BigInteger.Parse("2") == BigInteger.Parse("0"))
                {
                    t = t + BigInteger.Parse("1");
                    a = a / BigInteger.Parse("2");
                }

                if (t % BigInteger.Parse("2") != BigInteger.Parse("0"))
                {
                    if (p % BigInteger.Parse("8") == BigInteger.Parse("3") || p % BigInteger.Parse("8") == BigInteger.Parse("5"))
                    {
                        result = result * BigInteger.Parse("-1");
                    }
                }

                if (a % BigInteger.Parse("4") == BigInteger.Parse("3") && p % BigInteger.Parse("4") == BigInteger.Parse("3"))
                {
                    result = result * BigInteger.Parse("-1");
                }
                BigInteger temp = a;
                a = p % temp;
                p = temp;
            }

            return result;

        }

        public static BigInteger FactorizePollard(BigInteger n, List<BigInteger> BigIntegers)
        {
            var random = new Random();
            BigInteger x = int.Parse(n.ToString()) < int.MaxValue ? BigInteger.Parse(random.Next(2, int.Parse(n.ToString())).ToString()) : BigInteger.Parse(random.Next(2, int.MaxValue).ToString());
            BigInteger y = BigInteger.Parse(x.ToString());
            BigInteger d = BigInteger.Parse("1");
            while (d == BigInteger.Parse("1"))
            {
                x = F(x, n);
                y = F(F(y, n), n);
                d = GCD(Abs(x - y), n);
            }

            if (d == n)
            {
                //BigIntegers.Add(d);
                return 0;
            }
            /*else
            {
                BigInteger p = n / d;
                FactorizePollard(d, BigIntegers);
                FactorizePollard(p, BigIntegers);
            }*/
            return d;
        }

        private static BigInteger Abs(BigInteger n)
        {
            BigInteger res = BigInteger.Parse(n.ToString());
            if (n < BigInteger.Parse("0"))
            {
                res = BigInteger.Parse(n.ToString().Trim('-'));
            }
            return res;
        }

        private static BigInteger F(BigInteger x, BigInteger n)
        {
            //BigInteger x1 = BigInteger.Parse(x.ToString());
            //BigInteger n1 = BigInteger.Parse(n.ToString());
            return ((x * x) + BigInteger.Parse("1")) % n;
        }

        public static BigInteger BabyGiantStepDiscreteLog(BigInteger a, BigInteger b, BigInteger n)
        {
            BigInteger m = Sqrt(n) + BigInteger.Parse("1");
            BigInteger g = PowByModul(a, m, n);
            BigInteger g1 = BigInteger.Parse(g.ToString());

            var table = new Dictionary<string, BigInteger>();

            for (BigInteger i = BigInteger.Parse("1"); i <= m; i = i + BigInteger.Parse("1"))
            {
                if (!table.ContainsKey(g1.ToString()))
                {
                    table.Add(g1.ToString(), i);
                }
                g1 = (g1 * g) % n;
            }

            for (BigInteger i = BigInteger.Parse("0"); i < m; i = i + BigInteger.Parse("1"))
            {
                var c = (b * PowByModul(a, i, n)) % n;
                if (table.ContainsKey(c.ToString()))
                {
                    return (table.GetValueOrDefault(c.ToString())) * m - i;
                }
            }

            return 0;
        }

        public static BigInteger Sqrt(BigInteger n)
        {
            if (n < BigInteger.Parse("0"))
            {
                return 0;
            }

            if (n < BigInteger.Parse("4"))
            {
                return n == BigInteger.Parse("0") ? BigInteger.Parse("0") : BigInteger.Parse("1");
            }


            var k = BigInteger.Parse("2") * Sqrt((n - n % BigInteger.Parse("4")) / BigInteger.Parse("4"));
            return n < Pow((k + BigInteger.Parse("1")), BigInteger.Parse("2")) ? k : k + BigInteger.Parse("1");
        }

        public static BigInteger DiscreteSqrtCipolla(BigInteger n, BigInteger p)
        {
            if (LegendreSymbol(n, p) != 1)
            {
                throw new ArgumentException("P or N is not correct.");
            }
            BigInteger a = BigInteger.Parse("0");
            BigInteger c = (a * a - n + p) % p;
            while (LegendreSymbol(c, p) == 1)
            {
                a = a + BigInteger.Parse("1");
                c = (a * a - n + p) % p;
            }

            (BigInteger, BigInteger) result = (BigInteger.Parse("1"), BigInteger.Parse("0"));
            (BigInteger, BigInteger) d = (a, BigInteger.Parse("1"));
            BigInteger nn = (p + 1) / 2;
            while (nn > 0)
            {
                if (nn % 2 != 0)
                {
                    result = ComplexMultByModul(result, d, p, c);
                }
                d = ComplexMultByModul(d, d, p, c);
                nn = nn / 2;
            }

            return result.Item1;
        }

        private static (BigInteger, BigInteger) ComplexMultByModul((BigInteger, BigInteger) n1, (BigInteger, BigInteger) n2, BigInteger p, BigInteger c)
        {
            return ((n1.Item1 * n2.Item1 + n1.Item2 * n2.Item2 * c) % p,
                    (n1.Item1 * n2.Item2 + n2.Item1 * n1.Item2) % p);
        }

        public static bool IsPrimeMillerRabin(BigInteger n, BigInteger k)
        {
            if (n == 2 || n == 3)
            {
                return true;
            }

            if (n < 2 || n % 2 == 0)
            {
                return false;
            }


            BigInteger t = n - BigInteger.Parse("1");
            BigInteger s = 0;
            var random = new Random();

            while (t % 2 == 0)
            {
                t = t / 2;
                s = s + 1;
            }

            for (BigInteger i = 0; i <= k; i++)
            {
                BigInteger m = 174;//int.TryParse(n.ToString(), out int c) ? BigInteger.Parse(random.Next(2, int.Parse(n.ToString()) - 2).ToString()) : BigInteger.Parse(random.Next(2, int.MaxValue - 2).ToString());
                BigInteger b = PowByModul(m, t, n);
                if (b != 1 && b != n - 1)
                {
                    for (int j = 0; j < s - 1; j++)
                    {
                        b = (b * b) % n;
                        if (b != n - 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static (BigInteger n, BigInteger e) RSAGeneratePublicKey(BigInteger p, BigInteger q)
        {
            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);

            var random = new Random();
            BigInteger e = int.TryParse(phi.ToString(), out int c) ? BigInteger.Parse(random.Next(2, c - 2).ToString()) : BigInteger.Parse(random.Next(2, int.MaxValue - 2).ToString());
            while (GCD(e, phi) != 1)
            {
                e = int.TryParse(phi.ToString(), out c) ? BigInteger.Parse(random.Next(2, c - 2).ToString()) : BigInteger.Parse(random.Next(2, int.MaxValue - 2).ToString());
            }
            return (n, e);
        }

        public static BigInteger RSAGenerateSecretKey(BigInteger phi, BigInteger e)
        {
            BigInteger c;
            c = GCDExternal(e, phi, out BigInteger x, out BigInteger y);
            return (x + phi) % phi;
        }

        public static BigInteger TextToNumber(string text, int baseNum)
        {
            BigInteger number = 0;
            BigInteger p = text.Length - 1;

            foreach (char c in text)
            {
                BigInteger charValue;
                if (c == ' ')
                {
                    charValue = 0;
                }
                else
                {
                    charValue = c - 'A' + 1;
                }

                number += charValue * Pow(baseNum, p);
                p--;
            }

            return number;
        }

        public static string NumberToText(BigInteger number, BigInteger baseNum)
        {
            StringBuilder text = new StringBuilder();

            while (number > 0)
            {
                BigInteger remainder = number % baseNum;
                if (remainder == 0)
                {
                    text.Insert(0, ' ');
                }
                else
                {
                    char c = (char)('A' + remainder - 1);
                    text.Insert(0, c);
                }
                number /= baseNum;
            }

            return text.ToString();
        }

        public static (BigInteger x, BigInteger y) NumberToPoint(BigInteger number, ElGamalCurveParams curve)
        {
            (BigInteger x, BigInteger y) M;
            M.x = number;
            M.y = ModInverse((BigInteger.Pow(M.x, 3) + curve.A * M.x + curve.B) % curve.P, curve.P);
            return M;
        }

        public static (BigInteger x, BigInteger y) GetInversePoint((BigInteger x, BigInteger y) S, ElGamalCurveParams curve)
        {
            BigInteger inverseY;
            if (S.y != 0)
            {
                inverseY = (curve.P - S.Item2) % curve.P;
            }
            else
            {
                inverseY = 0;
            }

            return (S.x, inverseY);
        }

        public static BigInteger RSAEncryptMessage(BigInteger m, BigInteger n, BigInteger e)
        {
            BigInteger encryptedMes = PowByModul(m, e, n);
            return encryptedMes;
        }

        public static BigInteger RSADecryptMessage(BigInteger c, BigInteger n, BigInteger d)
        {
            BigInteger decryptedMes = PowByModul(c, d, n);
            return decryptedMes;
        }



        public static void ElGamalCurve(ElGamalCurveParams curve, string message)
        {
            var random = new Random();
            BigInteger k = 3;//int.TryParse(curve.N.ToString(), out int c) ? BigInteger.Parse(random.Next(1, c - 2).ToString()) : BigInteger.Parse(random.Next(1, int.MaxValue - 2).ToString());
            Console.WriteLine("Bob's private key: " + k.ToString());
            (BigInteger x, BigInteger y) Y = curve.PointSelfSum(k, curve.G);

            Console.WriteLine("Y: (" + Y.x.ToString() + "," + Y.y.ToString() + ")");

            string inputStr = message;

            BigInteger number;
            bool isStrMes = false;

            if (!BigInteger.TryParse(inputStr, out number))
            {
                isStrMes = true;
                number = TextToNumber(inputStr, 57);
            }
            (BigInteger x, BigInteger y) M = NumberToPoint(number, curve);
            Console.WriteLine("Point decrypted: (" + M.x.ToString() + "," + M.y.ToString() + ")");

            BigInteger r = 5;// int.TryParse(curve.N.ToString(), out int z) ? BigInteger.Parse(random.Next(1, z - 2).ToString()) : BigInteger.Parse(random.Next(1, int.MaxValue - 2).ToString());
            Console.WriteLine("Alice's private key: " + r.ToString());
            var G = curve.PointSelfSum(r, curve.G);
            Console.WriteLine("G: (" + G.Item1.ToString() + "," + G.Item2.ToString() + ")");
            var D = curve.PointSelfSum(r, Y);
            Console.WriteLine("D: (" + D.Item1.ToString() + "," + D.Item2.ToString() + ")");
            var H = curve.AddPoints(M, D);
            Console.WriteLine("H: (" + H.Item1.ToString() + "," + H.Item2.ToString() + ")");
            var S = curve.PointSelfSum(k, G);
            Console.WriteLine("S: (" + S.Item1.ToString() + "," + S.Item2.ToString() + ")");


            var S1 = GetInversePoint(S, curve);
            Console.WriteLine("S1: (" + S1.Item1.ToString() + "," + S1.Item2.ToString() + ")");
            var M1 = curve.AddPoints(S1, H);
            Console.WriteLine("Point decrypted: (" + M1.Item1.ToString() + "," + M1.Item2.ToString() + ")");

            string text = M1.Item1.ToString();

            if (isStrMes)
            {
                text = NumberToText(M1.Item1, 57);
            }

            
            Console.WriteLine(text);
        }
    }
}