namespace CryptographyConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        BigNumber number1 = new BigNumber("3");
        BigNumber number2 = new BigNumber("31");
        BigNumber number3 = new BigNumber("30");
        BigNumber number4 = new BigNumber("0");
        List<Records.Comparisons> cmps = new List<Records.Comparisons>
        {
            new Records.Comparisons(new BigNumber("1"), new BigNumber("5"), new BigNumber("9")),
            new Records.Comparisons(new BigNumber("1"), new BigNumber("7"), new BigNumber("8")),
            new Records.Comparisons(new BigNumber("1"), new BigNumber("3"), new BigNumber("7"))
        };
        var res = CryptographyCalculator.JacobiSymbol(number1, number2);
        var res2 = CryptographyCalculator.ChineseReminder(cmps, out number4);
        Console.WriteLine(res);
        //Console.WriteLine($"{res2} (mod {number4})");
        /*while (number3 < new BigNumber("0"))
        {
            number3 = number3 + number2;
        }
        Console.WriteLine(number3);
        Console.WriteLine(number4);*/
    }
}

