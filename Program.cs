namespace CrudExcelApp;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        ExcelDatabase.InitializeWorkbook();
        Application.Run(new FormHome());
    }
}
