namespace CryptographyConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        BigNumber number1 = new BigNumber("-401");
        BigNumber number2 = new BigNumber("-401");
        var res = number1 - number2;
        Console.WriteLine(res);
    }
}

