namespace p2beadontnet_starter;

internal static class Program
{
    private static void Main(string[] args)
    {
        string inputFile = args.Length > 0
            ? args[0]
            : "emma_expenses.csv";
        var repository = new CsvExpenseRepository(inputFile);
        var application = new BudgetApplication(repository);
        application.Run();
    }
}
