namespace Shannon;

static class EntropyMath
{
    public static Dictionary<char, long> CountChars(string text)
    {
        var dict = new Dictionary<char, long>();
        foreach (char c in text)
        {
            dict.TryGetValue(c, out long v);
            dict[c] = v + 1;
        }
        return dict;
    }

    public static Dictionary<(char L, char R), long> CountBigrams(string text)
    {
        var dict = new Dictionary<(char, char), long>();
        for (int i = 0; i + 1 < text.Length; i++)
        {
            var key = (text[i], text[i + 1]);
            dict.TryGetValue(key, out long v);
            dict[key] = v + 1;
        }
        return dict;
    }

    public static double ShannonEntropy(IEnumerable<long> counts, long total)
    {
        const double ln2 = 0.6931471805599453;
        double h = 0.0;
        foreach (var cnt in counts)
        {
            if (cnt <= 0) continue;
            double p = (double)cnt / total;
            h += -p * (Math.Log(p) / ln2);
        }
        return h;
    }

    public static double Term(double p)
    {
        if (p <= 0) return 0.0;
        const double ln2 = 0.6931471805599453;
        return -p * (Math.Log(p) / ln2);
    }
}