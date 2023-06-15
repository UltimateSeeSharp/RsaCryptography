using Spectre.Console;
using System.Numerics;

static class Program
{
    static string _secret = "Hallo Welt!";
    static int _p = 37; //  first prime
    static int _q = 41; //  second prime
    static int _e = 19; //  exponent
    static int _n = 0; //  product  

    static void Main(string[] args)
    {
        _p = AnsiConsole.Ask<int>("Define [aqua]first prime [[p]][/]");
        _q = AnsiConsole.Ask<int>("Define [aqua]second prime [[q]][/]");
        _e = AnsiConsole.Ask<int>("Define [aqua]exponent [[e]][/]");
        _secret = AnsiConsole.Ask<string>("Define your [aqua]secret[/]");

        _n = _p * _q;

        int[] asciiValues = _secret.ToAsciiValues();
        int[] encryptedValues = asciiValues.Encrypt();
        string decryptedText = encryptedValues.Decrypt();

        AnsiConsole.Clear();
        AnsiConsole.Markup($"You're decrypted text is [aqua]{decryptedText}[/]");
        Console.ReadKey();
    }

    static string Decrypt(this int[] encryptedValues)
    {
        char[] decryptedChars = new char[encryptedValues.Length];
        for (int i = 0; i < encryptedValues.Length; i++)
        {
            int encryptedValue = encryptedValues[i];

            BigInteger biValue = new BigInteger(encryptedValue);
            BigInteger result = BigInteger.ModPow(biValue, _e, _n);

            decryptedChars[i] = (char)result;
        }
        return new string(decryptedChars);
    }

    //static string Decrypt(this int[] encryptedValues)
    //{
    //    char[] encryptedChars = new char[encryptedValues.Length];
    //    for (int x = 0; x < encryptedValues.Length; x++)
    //    {
    //        (int p, int q) = (_p - 1, _q - 1);
    //
    //        int n = p * q;
    //        int gcd = GCD(p, q);
    //
    //        int a = ((q - 1) * (p - 1)) / gcd;
    //        int d = a + _e;
    //    }
    //    return encryptedChars.ToString();
    //}

    static int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }

    static int[] Encrypt(this int[] asciiValues)
    {
        int[] encryptedValues = new int[asciiValues.Length];
        for (int x = 0; x < asciiValues.Length; x++)
        {
            BigInteger biValue = new(asciiValues[x]);
            BigInteger result = BigInteger.Pow(biValue, _e);

            BigInteger rest = result % 1517;
            encryptedValues[x] = (int)rest;
        }
        return encryptedValues;
    }

    static int[] ToAsciiValues(this string text)
    {
        int[] asciiValues = new int[text.Length];
        for (int x = 0; x < text.Length; x++)
        {
            asciiValues[x] = (int)text[x];
        }
        return asciiValues;
    }
}