using System;
using System.Numerics;

namespace CryptographyConsoleApp
{
	public class Records
	{
        public record Comparisons(BigNumber a, BigNumber b, BigNumber m);
        public record ComparisonsBigInt(BigInteger a, BigInteger b, BigInteger m);
    }
}

