using System;
using System.Drawing;
using System.Numerics;

namespace CryptographyConsoleApp
{
	public class ElGamalCurveParams
	{
		public BigInteger P { get; set; }
        public BigInteger A { get; set; }
        public BigInteger B { get; set; }
        public (BigInteger x, BigInteger y) G { get; set; }
        public BigInteger N { get; set; }
        public BigInteger H { get; set; }

        public ElGamalCurveParams(BigInteger p, BigInteger a, BigInteger b, BigInteger x, BigInteger y, BigInteger n)
        {
            P = p;
            A = a;
            B = b;
            G = (x, y);
            N = n;
        }

        public (BigInteger, BigInteger) AddPoints((BigInteger x, BigInteger y) p1, (BigInteger x, BigInteger y) p2)
        {
            if (p1.x == p2.x && p1.y != p2.y)
            {
                return (-1, -1);
            }

            BigInteger s;
            if (p1.x != p2.x)
            {
                s = (p2.y - p1.y) * BigInteger.ModPow(p2.x - p1.x, P - 2, P);
            }
            else
            {
                s = (3 * BigInteger.Pow(p1.x, 2) + A) * BigInteger.ModPow(2 * p1.y, P - 2, P);
            }

            BigInteger x = BigInteger.Pow(s, 2) - p1.x - p2.x;
            BigInteger y = s * (p1.x - x) - p1.y;

            x = (x % P + P) % P;
            y = (y % P + P) % P;

            return (x, y);
        }

        public (BigInteger, BigInteger) PointSelfSum(BigInteger k, (BigInteger, BigInteger) p)
        {
            var res = p;
            for (BigInteger i = 1; i < k; i++)
            {
                res = AddPoints(res, p);
            }

            return res;
        }
    }
}

