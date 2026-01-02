using System.ComponentModel.DataAnnotations;

public class ShortCodeGenerator
{

    private const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyz";
    private const int Length = 6;
    private static readonly Random randomCode = new Random();

    public static string Generate()
    {
        var chars = new char[Length];

        for (int i = 0 ; i < Length; i++)
        {
            var randomIndex = randomCode.Next(Alphabet.Length);
            chars[i] = Alphabet[randomIndex];
        }
        return new string(chars);
    }
}