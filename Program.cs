using Shannon;
using System.Text;

class Program
{
    private const string FileName = "text.txt";
    private const bool UseSpaceMarker = true;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string? path = FileLocator.FindFileUpwards(FileName);
        if (path is null)
        {
            ConsoleText.FileNotFound(FileName, Directory.GetCurrentDirectory());
            return;
        }

        ConsoleText.ReadingFile(path);
        string raw = File.ReadAllText(path, Encoding.UTF8);

        // исходная длина
        ConsoleText.ReportRawLength(raw.Length);

        // подготовка текста
        ConsoleText.PreprocessIntro();
        string prepared = TextPreprocessor.Clean(raw);
        if (prepared.Length == 0)
        {
            ConsoleText.EmptyAfterPreprocess();
            return;
        }

        // длина после подготовки
        ConsoleText.ReportPreparedLength(prepared.Length);

        // спросить лимит и взять подвыборку из подготовленного текста
        ConsoleText.AskLimitPrompt();
        int limit = UserInput.AskLimit(prepared.Length);
        string text = prepared.Length > limit ? prepared[..limit] : prepared;

        ConsoleText.PrintFormulas();

        // расчёт по символам
        var charCounts = EntropyMath.CountChars(text);
        long n = text.Length;
        double h1 = EntropyMath.ShannonEntropy(charCounts.Values, n);

        // расчёт по биграммам
        var pairCounts = EntropyMath.CountBigrams(text);
        long m = n - 1;
        double h2_pair = m > 0 ? EntropyMath.ShannonEntropy(pairCounts.Values, m) : 0.0;
        double h2_per_symbol = m > 0 ? h2_pair / 2.0 : 0.0;

        // вывод результатов
        ConsoleText.PrintResultsSummary(n, m, h1, h2_pair, h2_per_symbol);

        // интерпретация результатов
        var interpretation = ResultInterpreter.Analyze(
            sampleSize: n,
            uniqueSymbols: charCounts.Count,
            h1: h1,
            h2PerPair: h2_pair,
            h2PerSymbol: h2_per_symbol
        );
        ConsoleText.PrintInterpretation(interpretation);

        // Excel-отчёт
        string outDir = Path.GetDirectoryName(path)!;
        string xlsxPath = Path.Combine(outDir, "entropy_results.xlsx");
        ConsoleText.ExcelIntro();
        ExcelReport.SaveToXlsx(
            xlsxPath,
            charCounts, n,
            pairCounts, m,
            h1, h2_pair, h2_per_symbol,
            UseSpaceMarker);
        ConsoleText.ExcelSaved(xlsxPath);
    }
}