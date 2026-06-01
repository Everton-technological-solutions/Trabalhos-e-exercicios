using ClosedXML.Excel;

namespace CrudExcelApp;

public static class ExcelDatabase
{
    private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.xlsx");

    private static readonly Dictionary<string, string[]> SheetColumns = new()
    {
        ["Veiculos"] = new[] { "ID", "Data", "Placa", "Horario" },
        ["Fornecedores"] = new[] { "ID", "Data", "Nome", "RG", "Placa", "NomeEmpresa", "NotaFiscal", "HoraChegada", "HoraEntrada", "HoraSaida", "Setor" },
        ["CaminhoesCasa"] = new[] { "ID", "Placa", "Motorista", "DataSaida", "HoraSaida", "KmSaida", "DataEntrada", "HoraEntrada", "KmEntrada", "Destino", "NotasFiscais" },
        ["Chaves"] = new[] { "ID", "Identificacao", "Responsavel", "DataRetirada", "DataDevolucao", "Status" }
    };

    public static void SetFilePath(string path)
    {
        _filePath = path;
    }

    public static string GetFilePath() => _filePath;

    public static void InitializeWorkbook()
    {
        if (File.Exists(_filePath))
            return;

        CreateWorkbookWithSchema(_filePath);
    }

    public static List<Dictionary<string, string>> ListRows(string sheetName)
    {
        var rows = new List<Dictionary<string, string>>();
        if (!File.Exists(_filePath))
            return rows;

        using var workbook = new XLWorkbook(_filePath);
        var ws = workbook.Worksheet(sheetName);
        var columns = SheetColumns[sheetName];
        int lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

        for (int r = 2; r <= lastRow; r++)
        {
            var dict = new Dictionary<string, string>();
            for (int c = 0; c < columns.Length; c++)
            {
                dict[columns[c]] = ws.Cell(r, c + 1).GetString();
            }
            rows.Add(dict);
        }
        return rows;
    }

    public static int InsertRow(string sheetName, Dictionary<string, string> data)
    {
        using var workbook = new XLWorkbook(_filePath);
        var ws = workbook.Worksheet(sheetName);
        var columns = SheetColumns[sheetName];
        int lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

        // Calculate new ID
        int maxId = 0;
        for (int r = 2; r <= lastRow; r++)
        {
            var val = ws.Cell(r, 1).GetString();
            if (int.TryParse(val, out int id) && id > maxId)
                maxId = id;
        }
        int newId = maxId + 1;

        int newRow = lastRow + 1;
        ws.Cell(newRow, 1).Value = newId;
        for (int c = 1; c < columns.Length; c++)
        {
            if (data.ContainsKey(columns[c]))
                ws.Cell(newRow, c + 1).Value = data[columns[c]];
        }
        workbook.Save();
        return newId;
    }

    public static void UpdateRow(string sheetName, int id, Dictionary<string, string> data)
    {
        using var workbook = new XLWorkbook(_filePath);
        var ws = workbook.Worksheet(sheetName);
        var columns = SheetColumns[sheetName];
        int lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

        for (int r = 2; r <= lastRow; r++)
        {
            var val = ws.Cell(r, 1).GetString();
            if (int.TryParse(val, out int rowId) && rowId == id)
            {
                for (int c = 1; c < columns.Length; c++)
                {
                    if (data.ContainsKey(columns[c]))
                        ws.Cell(r, c + 1).Value = data[columns[c]];
                }
                workbook.Save();
                return;
            }
        }
    }

    public static void DeleteRow(string sheetName, int id)
    {
        using var workbook = new XLWorkbook(_filePath);
        var ws = workbook.Worksheet(sheetName);
        int lastRow = ws.LastRowUsed()?.RowNumber() ?? 1;

        for (int r = 2; r <= lastRow; r++)
        {
            var val = ws.Cell(r, 1).GetString();
            if (int.TryParse(val, out int rowId) && rowId == id)
            {
                ws.Row(r).Delete();
                workbook.Save();
                return;
            }
        }
    }

    private static void CreateWorkbookWithSchema(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);

        using var workbook = new XLWorkbook();
        foreach (var kvp in SheetColumns)
        {
            var ws = workbook.Worksheets.Add(kvp.Key);
            for (int i = 0; i < kvp.Value.Length; i++)
            {
                ws.Cell(1, i + 1).Value = kvp.Value[i];
                ws.Cell(1, i + 1).Style.Font.Bold = true;
            }
            ws.SheetView.FreezeRows(1);
        }
        workbook.SaveAs(filePath);
    }
}
