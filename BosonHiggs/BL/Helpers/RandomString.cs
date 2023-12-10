namespace BosonHiggsApi.BL.Helpers;

public static class RandomString
{
    #region StaticProperties
    private static Random Random { get; } = new();

    private static string Numbers { get; } = "0123456789";

    private static string LowerAlphas { get; } = "abcdefghijklmnopqrstuvwxyz";

    private static string LowerSpecials { get; } = "bcdfghjkmnpqrstvwxyzaeouaeouaeouaeou";

    private static string UpperAlphas { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static string Alphas { get; } = $"{LowerAlphas}{UpperAlphas}";

    private static string AlphaNumbers { get; } = $"{Numbers}{LowerAlphas}{UpperAlphas}";
    #endregion

    public static string AlphaNumeric(int length)
    {
        return Generate(AlphaNumbers, length);
    }

    public static string Alpha(int length)
    {
        return Generate(Alphas, length);
    }

    public static string UpperAlpha(int length)
    {
        return Generate(UpperAlphas, length);
    }

    public static string Mix(int specialsLen, int numericLen)
    {
        return $"{LowerSpecial(specialsLen)}{Numeric(numericLen)}";
    }

    public static string LowerSpecial(int length)
    {
        return Generate(LowerSpecials, length);
    }

    public static string LowerAlpha(int length)
    {
        return Generate(LowerAlphas, length);
    }

    public static string Numeric(int length)
    {
        return Generate(Numbers, length);
    }

    private static string Generate(string source, int length)
    {
        return new(Enumerable.Repeat(source, length)
            .Select(x => x[Random.Next(x.Length)]).ToArray());
    }
}