using System.Globalization;
using System.Numerics;
using CryptographyConsoleApp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cryptography.Tests;

[TestFixture]
public class CryptographyTests
{
    [SetUp]
    public void Setup()
    {

    }

    [TestCase("1", "1")]
    [TestCase("24", "8")]
    [TestCase("49", "42")]
    [TestCase("99", "60")]
    public void EulerFunctionTest(string nStr, string expectedStr)
    {
        var n = BigInteger.Parse(nStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.EulerFunction(n);
        Assert.That(result, Is.EqualTo(expected));
    }


    [TestCase("1", "1")]
    [TestCase("5", "-1")]
    [TestCase("99", "0")]
    [TestCase("100", "0")]
    public void MobiusFunctionTest(string nStr, string expectedStr)
    {
        var n = BigInteger.Parse(nStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.MobiusFunction(n);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(LCMCases))]
    public void LCMTest(List<BigInteger> numbers, BigInteger expected)
    {
        var result = CryptographyBigInt.LCM(numbers);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(ChineseReminderCases))]
    public void ChineseReminderTest(List<Records.ComparisonsBigInt> cmps, BigInteger expected, BigInteger expectedMod)
    {
        var result = CryptographyBigInt.ChineseReminder(cmps, out BigInteger mod);
        Assert.That((result, mod), Is.EqualTo((expected, expectedMod)));
    }

    [TestCase("64", "3", "1")]
    [TestCase("49", "7", "0")]
    [TestCase("137", "5", "-1")]
    [TestCase("137", "11", "1")]
    public void LegendreSymbolTest(string aStr, string pStr, string expectedStr)
    {
        var a = BigInteger.Parse(aStr);
        var p = BigInteger.Parse(pStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.LegendreSymbol(a, p);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("5", "7", "-1")]
    [TestCase("5362", "73", "-1")]
    [TestCase("99", "55", "0")]
    [TestCase("143", "25", "1")]
    public void JacobiSymbolTest(string aStr, string pStr, string expectedStr)
    {
        var a = BigInteger.Parse(aStr);
        var p = BigInteger.Parse(pStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.JacobiSymbol(a, p);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("63889", "7")]
    [TestCase("2047", "23")]
    [TestCase("2587481", "13")]
    [TestCase("148741225841577", "3")]
    public void FactorizePollardTest(string nStr, string expectedStr)
    {
        var n = BigInteger.Parse(nStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.FactorizePollard(n);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("3", "13", "17", "20")]
    [TestCase("4558765", "517", "654", "388")]
    [TestCase("98687", "362825", "765548", "489124")]
    [TestCase("4567654", "9", "25", "17")]
    public void BabyGiantStepDiscreteLogTest(string aStr, string bStr, string nStr, string expectedStr)
    {
        var a = BigInteger.Parse(aStr);
        var b = BigInteger.Parse(bStr);
        var n = BigInteger.Parse(nStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.BabyGiantStepDiscreteLog(a, b, n);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("10", "13", "6")]
    [TestCase("557", "2131", "1807")]
    [TestCase("2", "31", "8")]
    [TestCase("2", "7", "4")]
    public void DiscreteSqrtCipollaTest(string aStr, string pStr, string expectedStr)
    {
        var a = BigInteger.Parse(aStr);
        var p = BigInteger.Parse(pStr);
        var expected = BigInteger.Parse(expectedStr);

        var result = CryptographyBigInt.DiscreteSqrtCipolla(a, p);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("655149807493", "20", true)]
    [TestCase("12379599778027", "20", true)]
    [TestCase("4860596084479", "20", true)]
    [TestCase("9477", "20", false)]
    public void IsPrimeMillerRabinTest(string nStr, string kStr, bool expected)
    {
        var n = BigInteger.Parse(nStr);
        var k = BigInteger.Parse(kStr);

        var result = CryptographyBigInt.IsPrimeMillerRabin(n, k);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("14757089754479", "7320887974667", "Test message", "Test message")]
    [TestCase("4860596084479", "655149807493", "Hello word", "Hello word")]
    public void RSATest(string pStr, string qStr, string inputMes, string expected)
    {
        var p = BigInteger.Parse(pStr);
        var q = BigInteger.Parse(qStr);

        var publiKey = CryptographyBigInt.RSAGeneratePublicKey(p, q);
        Console.WriteLine("n: " + publiKey.n);
        Console.WriteLine("e: " + publiKey.e);
        Console.WriteLine("phi: " + (p - 1) * (q - 1));
        var d = CryptographyBigInt.RSAGenerateSecretKey((p - 1) * (q - 1), publiKey.e);
        Console.WriteLine("d: " + d);

        var mesNum = CryptographyBigInt.TextToNumber(inputMes, 57);
        Console.WriteLine("m: " + mesNum);

        var encryptedMes = CryptographyBigInt.RSAEncryptMessage(mesNum, publiKey.n, publiKey.e);
        Console.WriteLine("c: " + encryptedMes);

        var decryptedMes = CryptographyBigInt.RSADecryptMessage(encryptedMes, publiKey.n, d);
        Console.WriteLine("m: " + decryptedMes);

        var result = CryptographyBigInt.NumberToText(decryptedMes, 57);
        Console.WriteLine(result);

        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCaseSource(nameof(ElGamalCurveCases))]
    public void ElGamalCurveTest(ElGamalCurveParams curve, string inputMes, string expected)
    {

        var result = CryptographyBigInt.ElGamalCurve(curve, inputMes);
        Console.WriteLine(result);

        Assert.That(result, Is.EqualTo(expected));
    }

    public static object[] LCMCases =
    {
        new object[] { new List<BigInteger> { new BigInteger(4), new BigInteger(48), new BigInteger(32) }, new BigInteger(96) },
        new object[] { new List<BigInteger> { new BigInteger(45), new BigInteger(68), new BigInteger(32), new BigInteger(74), new BigInteger(27) }, new BigInteger(2717280) }
    };

    public static object[] ChineseReminderCases =
    {
        new object[] {
            new List<Records.ComparisonsBigInt>
            {
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("1"), BigInteger.Parse("2")),
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("1"), BigInteger.Parse("3")),
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("9"), BigInteger.Parse("11"))
            },
            new BigInteger(31),
            new BigInteger(66)
        },
        new object[] {
            new List<Records.ComparisonsBigInt>
            {
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("2"), BigInteger.Parse("5")),
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("3"), BigInteger.Parse("7")),
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("5"), BigInteger.Parse("11"))
            },
            new BigInteger(192),
            new BigInteger(385)
        },
        new object[] {
            new List<Records.ComparisonsBigInt>
            {
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("5"), BigInteger.Parse("9")),
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("7"), BigInteger.Parse("8")),
                new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("3"), BigInteger.Parse("7"))
            },
            new BigInteger(311),
            new BigInteger(504)
        }
    };

    public static object[] ElGamalCurveCases =
    {
        new object[] {
            new ElGamalCurveParams(
                BigInteger.Parse("0DB7C2ABF62E35E668076BEAD208B", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0DB7C2ABF62E35E668076BEAD2088", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0659EF8BA043916EEDE8911702B22", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("09487239995A5EE76B55F9C2F098", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0A89CE5AF8724C0A23E0E0FF77500", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0DB7C2ABF62E35E7628DFAC6561C5", NumberStyles.AllowHexSpecifier)
            ),
            "Hello word",
            "Hello word"
        },
                new object[] {
            new ElGamalCurveParams(
                BigInteger.Parse("0DB7C2ABF62E35E668076BEAD208B", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0DB7C2ABF62E35E668076BEAD2088", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0659EF8BA043916EEDE8911702B22", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("09487239995A5EE76B55F9C2F098", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0A89CE5AF8724C0A23E0E0FF77500", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0DB7C2ABF62E35E7628DFAC6561C5", NumberStyles.AllowHexSpecifier)
            ),
            "125",
            "125"
        },
        new object[] {
            new ElGamalCurveParams(
                BigInteger.Parse("0DB7C2ABF62E35E668076BEAD208B", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("06127C24C05F38A0AAAF65C0EF02C", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("051DEF1815DB5ED74FCC34C85D709", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("04BA30AB5E892B4E1649DD0928643", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0ADCD46F5882E3747DEF36E956E97", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("036DF0AAFD8B8D7597CA10520D04B", NumberStyles.AllowHexSpecifier)
            ),
            "Test message",
            "Test message"
        },
        new object[] {
            new ElGamalCurveParams(
                BigInteger.Parse("0DB7C2ABF62E35E668076BEAD208B", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("06127C24C05F38A0AAAF65C0EF02C", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("051DEF1815DB5ED74FCC34C85D709", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("04BA30AB5E892B4E1649DD0928643", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("0ADCD46F5882E3747DEF36E956E97", NumberStyles.AllowHexSpecifier),
                BigInteger.Parse("036DF0AAFD8B8D7597CA10520D04B", NumberStyles.AllowHexSpecifier)
            ),
            "347",
            "347"
        },
    };
}
