using System.Globalization;
using System.Numerics;

namespace CryptographyConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        BigInteger number1 = BigInteger.Parse("14757089754479");
        BigInteger number2 = BigInteger.Parse("7320887974667");
        BigInteger number3 = BigInteger.Parse("3257");
        List<Records.ComparisonsBigInt> cmps = new List<Records.ComparisonsBigInt>
        {
            new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("2"), BigInteger.Parse("5")),
            new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("4"), BigInteger.Parse("7")),
            new Records.ComparisonsBigInt(BigInteger.Parse("1"), BigInteger.Parse("6"), BigInteger.Parse("11"))
        };
        
        List<BigInteger> numbers = new List<BigInteger>();
        //var res2 = FactorizePollard(298839521);//CryptographyCalculator.FactorizePollard(number1, numbers);
        //Console.WriteLine(res2);
        try
        {
            var res = CryptographyBigInt.RSAGeneratePublicKey(number1, number2);
            Console.WriteLine("n: " + res.n);
            Console.WriteLine("e: " + res.e);
            Console.WriteLine("phi: " + (number1 - 1) * (number2 - 1));
            var d = CryptographyBigInt.RSAGenerateSecretKey((number1 - 1) * (number2 - 1), res.e);
            Console.WriteLine("d: " + d);

            string inputMes = "Test message";

            var mesNum = CryptographyBigInt.TextToNumber(inputMes, 57);
            Console.WriteLine("m: " + mesNum);

            var encryptedMes = CryptographyBigInt.RSAEncryptMessage(mesNum, res.n, res.e);
            Console.WriteLine("c: " + encryptedMes);

            var decryptedMes = CryptographyBigInt.RSADecryptMessage(encryptedMes, res.n, d);
            Console.WriteLine("m: " + decryptedMes);

            var mes = CryptographyBigInt.NumberToText(decryptedMes, 57);
            Console.WriteLine(mes);

            BigInteger p = BigInteger.Parse("0DB7C2ABF62E35E668076BEAD208B", NumberStyles.AllowHexSpecifier);
            BigInteger a = BigInteger.Parse("0DB7C2ABF62E35E668076BEAD2088", NumberStyles.AllowHexSpecifier);
            BigInteger b = BigInteger.Parse("0659EF8BA043916EEDE8911702B22", NumberStyles.AllowHexSpecifier);
            BigInteger x = BigInteger.Parse("09487239995A5EE76B55F9C2F098", NumberStyles.AllowHexSpecifier);
            BigInteger y = BigInteger.Parse("0A89CE5AF8724C0A23E0E0FF77500", NumberStyles.AllowHexSpecifier);
            BigInteger n = BigInteger.Parse("0DB7C2ABF62E35E7628DFAC6561C5", NumberStyles.AllowHexSpecifier);

            ElGamalCurveParams curve = new ElGamalCurveParams(p, a, b, x, y, n);
            CryptographyBigInt.ElGamalCurve(curve, inputMes);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        /*foreach (var item in numbers)
        {
            Console.WriteLine(item);
        }*/
        //Console.WriteLine(res2);
        //Console.WriteLine($"{res} (mod {number6})");
        /*while (number3 < new BigNumber("0"))
        {
            number3 = number3 + number2;
        }
        Console.WriteLine(number3);
        Console.WriteLine(number4);*/
    }


}

