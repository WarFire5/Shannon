using System.Text;

namespace Shannon;

static class TextPreprocessor
{
    public static string Clean(string input)
    {
        input = input.ToLowerInvariant();
        var sb = new StringBuilder(input.Length);

        foreach (char ch in input)
        {
            char c = ch switch
            {
                '\t' or '\r' or '\n' => ' ',
                'ё' => 'е',
                'ъ' => 'ь',
                _ => ch
            };

            if (c == ' ' || IsLatinLower(c) || IsCyrillicLower(c))
                sb.Append(c);
        }

        return NormalizeSpaces(sb.ToString());
    }

    private static bool IsLatinLower(char c) => c is >= 'a' and <= 'z';
    private static bool IsCyrillicLower(char c) => c is >= 'а' and <= 'я';

    private static string NormalizeSpaces(string s)
    {
        var sb = new StringBuilder(s.Length);
        bool prevSpace = false;
        foreach (char c in s)
        {
            if (c == ' ')
            {
                if (!prevSpace) sb.Append(' ');
                prevSpace = true;
            }
            else
            {
                sb.Append(c);
                prevSpace = false;
            }
        }
        return sb.ToString().Trim();
    }
}