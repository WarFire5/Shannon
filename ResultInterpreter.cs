namespace Shannon;

public record Interpretation(
    string RelationSummary,   // соотношение H₁ и H₂/2
    string EntropyLevel,      // высокая/средняя/низкая по H₁
    string? AtypicalNote,     // нетипичный случай, если есть
    string[] Hints            // предупреждения и примечания
);

static class ResultInterpreter
{
    public static Interpretation Analyze(long sampleSize, int uniqueSymbols, double h1, double h2PerPair, double h2PerSymbol)
    {
        var hints = new List<string>();
        string? atypical = null;

        // качество выборки
        if (sampleSize < 200) hints.Add("Предупреждение: выборка очень мала (меньше 200 знаков), поэтому оценки H могут быть шумными.");
        else if (sampleSize < 2000) hints.Add("Примечание: выборка средней длины (меньше 2000 знаков), поэтому учитывайте возможную погрешность оценки.");

        // соотношение H₁ и H₂/2
        string relation;
        if (sampleSize < 2)
        {
            relation = "Слишком маленькая выборка для интерпретации.";
        }
        else if (h2PerSymbol > h1 + 1e-9)
        {
            relation = "H₂/2 оказался больше H₁ — нетипично (обычно H₂/2 ≤ H₁).";
            atypical = "Возможные причины: малая выборка, шум оценок, особенности текста или предобработки.";
        }
        else
        {
            double rel = h1 > 0 ? (h1 - h2PerSymbol) / h1 : 0.0;
            if (Math.Abs(rel) <= 0.05)
                relation = "H₁ ≈ H₂/2: слабые зависимости между соседними символами, текст ближе к бессвязному.";
            else if (rel > 0.15)
                relation = "H₂/2 заметно меньше H₁: сильные зависимости между соседними символами, текст хорошо предсказуем по контексту.";
            else
                relation = "H₂/2 немного меньше H₁: умеренные зависимости между соседними символами, присутствует естественная языковая структура.";
        }

        // уровень энтропии по H₁
        string level = (h1 >= 3.8) ? "H₁ высокая: распределение ближе к равномерному, низкая избыточность."
                      : (h1 >= 2.2) ? "H₁ средняя: умеренная избыточность, типично для реальных текстов."
                                    : "H₁ низкая: текст сильно избыточен (много повторов, узкий набор символов или жёсткие шаблоны).";

        // справка
        hints.Add($"Представленные выводы актуальны для выборки размером {sampleSize} знаков, в которой число уникальных символов {uniqueSymbols}.");

        return new Interpretation(relation, level, atypical, hints.ToArray());
    }
}