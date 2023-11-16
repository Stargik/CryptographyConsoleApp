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
            BigInteger slope;
            if (p1.x != p2.x)
            {
                slope = (p2.y - p1.y) * BigInteger.ModPow(p2.x - p1.x, P - 2, P);
            }
            else
            {
                slope = (3 * BigInteger.Pow(p1.x, 2) + A) * BigInteger.ModPow(2 * p1.y, P - 2, P);
            }

            BigInteger xR = BigInteger.Pow(slope, 2) - p1.x - p2.x;
            BigInteger yR = slope * (p1.x - xR) - p1.y;

            xR = (xR % P + P) % P;
            yR = (yR % P + P) % P;

            return (xR, yR);
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

