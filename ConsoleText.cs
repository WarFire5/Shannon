namespace Shannon;

static class ConsoleText
{
    public static void FileNotFound(string file, string dir)
    {
        Console.WriteLine($"Файл \"{file}\" не найден. Добавьте его в проект.");
        Console.WriteLine($"Текущая папка запуска: {dir}");
    }

    public static void ReadingFile(string path) => Console.WriteLine($"Читаю: {path}");

    public static void ReportRawLength(int len) =>
        Console.WriteLine($"\nСимволов в заданном тексте: {len}.");

    public static void PreprocessIntro()
    {
        Console.WriteLine("\n— Подготовка текста —");
        Console.WriteLine("Удаляем табуляцию и переносы строк; приводим всё к нижнему регистру;" +
            "\nбуквы ё→е и ъ→ь; оставляем только буквы и пробелы.");
    }

    public static void ReportPreparedLength(int len) =>
        Console.WriteLine($"\nЧисло символов после обработки исходного текста: {len}.");

    public static void EmptyAfterPreprocess() =>
        Console.WriteLine("\nТекст пуст после предобработки.");

    public static void AskLimitPrompt() =>
        Console.Write("\nУкажите количество символов, для которого хотите произвести вычисления: ");

    public static void WarnTooLarge(int available) =>
        Console.WriteLine($"\nВ тексте только {available} знаков — расчёт будет проведён с доступным количеством символов.");

    public static void ErrorNotNumber()
    {
        Console.Beep();
        Console.Write("\nОшибка: введено не число. Введите число больше 2: ");
    }

    public static void ErrorTooSmall()
    {
        Console.Beep();
        Console.Write("\nОшибка: слишком маленькое значение. Введите число больше 2: ");
    }

    public static void PrintFormulas()
    {
        Console.WriteLine("\n— Расчёт частот, вероятностей и энтропий —");
        Console.WriteLine("  fᵢ   — частота появления символа i (число вхождений).");
        Console.WriteLine("  pᵢ   = fᵢ / N, где N — длина подготовленного текста.");
        Console.WriteLine("  pᵢ   — вероятность появления символа i в тексте длиной N.");
        Console.WriteLine("  fᵢⱼ  — частота появления пары символов (i, j) в перекрывающихся биграммах.");
        Console.WriteLine("  pᵢⱼ  = fᵢⱼ / (N − 1), так как в тексте из N символов есть N−1 биграмм.");
        Console.WriteLine("  pᵢⱼ  — вероятность появления пары символов (i, j) в биграммах.");
        Console.WriteLine("  H₁   = −Σ pᵢ · log₂ pᵢ");
        Console.WriteLine("  H₁   — энтропия Шеннона для одиночных символов.");
        Console.WriteLine("  H₂   = −Σ pᵢⱼ · log₂ pᵢⱼ.");
        Console.WriteLine("  H₂   — энтропия Шеннона для пар символов.");
        Console.WriteLine("  H₂/2 — оценка энтропии на один символ с учётом связей между соседними символами.");
    }

    public static void PrintResultsSummary(long n, long m, double h1, double h2, double h2half)
    {
        Console.WriteLine("\n— Подведение итогов —");
        Console.WriteLine($"В результате работы с подготовленным текстом из {n} символов имеем следующие данные:");
        Console.WriteLine($"Количество пар: {m};");
        Console.WriteLine($"Оценка энтропии с использованием частот отдельных символов (H₁): {h1:F6} бит на символ;");
        Console.WriteLine($"Оценка энтропии с использованием частот пар символов (H₂): {h2:F6} бит на пару;");
        Console.WriteLine($"Оценка энтропии с учётом связей между соседними символами (H₂/2): {h2half:F6} бит на символ в паре.");
    }

    public static void PrintInterpretation(Interpretation it)
    {
        Console.WriteLine("\n— Интерпретация результатов —");
        Console.WriteLine("• " + it.RelationSummary);
        Console.WriteLine("• " + it.EntropyLevel);
        if (!string.IsNullOrWhiteSpace(it.AtypicalNote))
            Console.WriteLine("• " + it.AtypicalNote);
        foreach (var h in it.Hints)
            Console.WriteLine(h);
    }

    public static void ExcelIntro()
    {
        Console.WriteLine("\n— Формирование отчёта в Excel —");
        Console.WriteLine("• Лист 'Символы' — показатели частот, вероятностей и энтропии для каждого встречающегося в заданном тексте символа;");
        Console.WriteLine("• Лист 'Биграммы' — показатели частот, вероятностей и энтропии для каждой каждого встречающейся в заданном тексте пары символов;");
        Console.WriteLine("• Лист 'Сводка' — общие результаты.");
    }

    public static void ExcelSaved(string path) =>
        Console.WriteLine($"\nРезультаты сохранены в: {path}");
}