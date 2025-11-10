using ClosedXML.Excel;

namespace Shannon;

static class ExcelReport
{
    public static void SaveToXlsx(
        string path,
        Dictionary<char, long> charCounts, long totalChars,
        Dictionary<(char L, char R), long> pairCounts, long totalPairs,
        double h1, double h2_pair, double h2_per_symbol,
        bool useSpaceMarker)
    {
        using var wb = new XLWorkbook();

        // ===== Лист 1: Символы =====
        var ws1 = wb.Worksheets.Add("Символы");
        ws1.Cell(1, 1).Value = "Символ";
        ws1.Cell(1, 2).Value = "Частота";
        ws1.Cell(1, 3).Value = "Вероятность";
        ws1.Cell(1, 4).Value = "Энтропия";

        int r = 2;
        foreach (var (ch, cnt) in charCounts.OrderByDescending(x => x.Value).ThenBy(x => x.Key))
        {
            double p = (double)cnt / totalChars;
            double term = EntropyMath.Term(p);
            string sym = (useSpaceMarker && ch == ' ') ? "␠" : ch.ToString();

            ws1.Cell(r, 1).Value = sym;
            ws1.Cell(r, 2).Value = cnt;
            ws1.Cell(r, 3).Value = p;
            ws1.Cell(r, 4).Value = term;
            r++;
        }

        ws1.Cell(r + 1, 1).Value = "Итого H1:";
        ws1.Cell(r + 1, 2).Value = h1;

        // оформление
        ws1.Range(1, 1, 1, 4).Style.Font.SetBold();
        ws1.SheetView.FreezeRows(1);
        ws1.Column(2).Style.NumberFormat.Format = "0";         // частота — целое
        ws1.Column(3).Style.NumberFormat.Format = "0.000000";  // вероятность
        ws1.Column(4).Style.NumberFormat.Format = "0.000000";  // вклад в энтропию
        ws1.Columns(1, 4).AdjustToContents();

        // ===== Лист 2: Биграммы =====
        var ws2 = wb.Worksheets.Add("Биграммы");
        ws2.Cell(1, 1).Value = "Биграмма";
        ws2.Cell(1, 2).Value = "Частота";
        ws2.Cell(1, 3).Value = "Вероятность";
        ws2.Cell(1, 4).Value = "Энтропия";

        r = 2;
        foreach (var (key, cnt) in pairCounts.OrderByDescending(x => x.Value)
                                             .ThenBy(x => x.Key.L).ThenBy(x => x.Key.R))
        {
            double p = totalPairs > 0 ? (double)cnt / totalPairs : 0.0;
            double term = EntropyMath.Term(p);
            string left = (useSpaceMarker && key.L == ' ') ? "␠" : key.L.ToString();
            string right = (useSpaceMarker && key.R == ' ') ? "␠" : key.R.ToString();
            string bigram = left + right;

            ws2.Cell(r, 1).Value = bigram;
            ws2.Cell(r, 2).Value = cnt;
            ws2.Cell(r, 3).Value = p;
            ws2.Cell(r, 4).Value = term;
            r++;
        }

        ws2.Cell(r + 1, 1).Value = "H2:";
        ws2.Cell(r + 1, 2).Value = h2_pair;
        ws2.Cell(r + 2, 1).Value = "H2/2:";
        ws2.Cell(r + 2, 2).Value = h2_per_symbol;

        // оформление
        ws2.Range(1, 1, 1, 4).Style.Font.SetBold();
        ws2.SheetView.FreezeRows(1);
        ws2.Column(2).Style.NumberFormat.Format = "0";
        ws2.Column(3).Style.NumberFormat.Format = "0.000000";
        ws2.Column(4).Style.NumberFormat.Format = "0.000000";
        ws2.Columns(1, 4).AdjustToContents();

        // ===== Лист 3: Сводка =====
        var ws3 = wb.Worksheets.Add("Сводка");
        ws3.Cell(1, 1).Value = "Показатель";
        ws3.Cell(1, 2).Value = "Значение";
        ws3.Cell(2, 1).Value = "Длина текста";
        ws3.Cell(2, 2).Value = totalChars;
        ws3.Cell(3, 1).Value = "Количество биграмм";
        ws3.Cell(3, 2).Value = totalPairs;
        ws3.Cell(4, 1).Value = "H1 (бит/симв)";
        ws3.Cell(4, 2).Value = h1;
        ws3.Cell(5, 1).Value = "H2 (бит/пара)";
        ws3.Cell(5, 2).Value = h2_pair;
        ws3.Cell(6, 1).Value = "H2/2 (бит/симв)";
        ws3.Cell(6, 2).Value = h2_per_symbol;

        // оформление
        ws3.Range(1, 1, 1, 2).Style.Font.SetBold();
        ws3.SheetView.FreezeRows(1);
        ws3.Column(2).Style.NumberFormat.Format = "0.000000";
        ws3.Columns(1, 2).AdjustToContents();

        wb.SaveAs(path);
    }
}