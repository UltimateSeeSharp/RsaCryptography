using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;

static class Program
{
    static string _secret = "Hallo Welt!";
    static int _p = 37; //  first prime
    static int _q = 41; //  second prime
    static int _e = 19; //  exponent
    static int _n = 0; //  product  

    static void Main(string[] args)
    {
        //_p = AnsiConsole.Ask<int>("Define [aqua]first prime [[p]][/]");
        //_q = AnsiConsole.Ask<int>("Define [aqua]second prime [[q]][/]");
        //_e = AnsiConsole.Ask<int>("Define [aqua]exponent [[e]][/]");
        //_secret = AnsiConsole.Ask<string>("Define your [aqua]secret[/]");

        _n = _p * _q;

        int[] asciiValues = _secret.ToAsciiValues();
        int[] encryptedValues = asciiValues.Encrypt();
        string decryptedText = encryptedValues.Decrypt();

        Console.ReadKey();
    }

    static string Decrypt(this int[] encryptedValues)
    {
        AnsiConsole.Clear();
        PrintDetails();

        char[] decryptedChars = new char[encryptedValues.Length];
        for (int x = 0; x < encryptedValues.Length; x++)
        {
            int encryptedValue = encryptedValues[x];

            BigInteger biValue = new BigInteger(encryptedValue);

            BigInteger result = BigInteger.ModPow(biValue, _e, _n);
            if (x is 0)
            {
                PrintStep(1);
                AnsiConsole.MarkupLine($"Encrypted value ^ [violet]e[/] = [blue]x[/]");
                AnsiConsole.MarkupLine($"{encryptedValue} ^ {_e} = {BigInteger.Pow(encryptedValue, _e)}");
                WaitForKey();

                PrintStep(2);
                AnsiConsole.MarkupLine($"[blue]x[/] % [yellow4_1]n[/] = Decrypted value");
                AnsiConsole.MarkupLine($"{BigInteger.Pow(encryptedValue, _e)} % {_n} = {result}");

                PrintHint($"n = first prime {_p} * second prime {_q}");

                WaitForKey();
            }

            decryptedChars[x] = (char)result;
        }

        PrintValues(encryptedValues, decryptedChars.ToList().Select(x => (int)x).ToArray(), encrypt: false);
        WaitForKey();

        return new string(decryptedChars);
    }

    static int GCD(int a, int b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }

    static int[] Encrypt(this int[] asciiValues)
    {
        AnsiConsole.Clear();
        PrintDetails();

        int[] encryptedValues = new int[asciiValues.Length];
        for (int x = 0; x < asciiValues.Length; x++)
        {
            BigInteger asciiValue = new(asciiValues[x]);
            BigInteger result = BigInteger.Pow(asciiValue, _e);
            if (x is 0)
            {
                PrintStep(1);
                AnsiConsole.MarkupLine($"Ascii-Value ^ [violet]e[/] = [blue]x[/]");
                AnsiConsole.MarkupLine($"{asciiValue} ^ {_e} = {result}");
                WaitForKey();
            }

            BigInteger rest = result % _n;
            if (x is 0)
            {
                PrintStep(2);
                AnsiConsole.MarkupLine($"[blue]x[/] % [yellow4_1]n[/] = Encrypted value");
                AnsiConsole.MarkupLine($"{result} % {_n} = {result}");

                PrintHint($"n = first prime {_p} * second prime {_q}");

                WaitForKey();
            }

            encryptedValues[x] = (int)rest;
        }

        PrintValues(asciiValues, encryptedValues);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press key to start decrypting process...[/]");
        AnsiConsole.WriteLine();
        WaitForKey();

        return encryptedValues;
    }

    private static void PrintHint(string hintText)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[grey]HINT: {hintText}[/]");
    }

    private static void PrintValues(int[] values1, int[] values2, bool encrypt = true)
    {
        StringBuilder sb = new();
        Table table = new();

        if (encrypt)
        {
            PrintTitle("Encrypted values");
            table.AddColumns("Value", "Encrypted value");
        }
        else
        {
            PrintTitle("Decrypted values");
            table.AddColumns("Encrypted value", "Decrypted value", "Decrypted text");
        }

        for (int x = 0; x < values1.Length; x++)
        {
            if (encrypt)
            {
                table.AddRow(values1[x].ToString(), values2[x].ToString());
            }
            else
            {
                char value = (char)values2[x];
                sb.Append(value);
                table.AddRow(values1[x].ToString(), values2[x].ToString(), value.ToString());
            }
        }

        AnsiConsole.Write(table);

        if (!encrypt)
        {
            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine($"[aqua]Decrypted text:[/] {sb.ToString()}");
        }
    }

    private static void PrintStep(int stepNr)
    {
        if (stepNr > 1)
            AnsiConsole.WriteLine();

        AnsiConsole.MarkupLine($"[aqua]{stepNr}. Step[/]");
        AnsiConsole.WriteLine();
    }

    private static void PrintTitle(string title)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[aqua]{title}[/]");
    }

    private static void PrintDetails()
    {
        AnsiConsole.MarkupLine($"[white]First prime (p) = {_p}[/]");
        AnsiConsole.MarkupLine($"[white]Second prime (q) = {_q}[/]");
        AnsiConsole.MarkupLine($"[violet]Exponent (e) = {_e}[/]");
        AnsiConsole.MarkupLine($"[yellow4_1]Prime product (n) = {_n}[/]");

        AnsiConsole.WriteLine();
    }

    private static void WaitForKey()
    {
        Console.ReadKey();
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
}