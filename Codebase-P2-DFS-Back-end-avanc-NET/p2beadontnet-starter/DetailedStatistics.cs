using System;

namespace p2beadontnet_starter;

public static class DetailedStatistics
{
	// Les méthodes doivent être en public pour être appelées depuis l'extérieur de la classe
    // Elles sont en static car on n'instancie pas cette classe, et les méthodes ne vont pas dépendre d'aucun état propre à un objet

    public static decimal CalculateTotalExpenses(IReadOnlyCollection<Expense> expenses)
	{
        return expenses.Sum(expense => expense.Amount);
    }

    public static int NbMonthExpenses(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count() == 0) return 0;
        string month = "";
        List<string> months = new();
        foreach (var expense in expenses)
        {
            month = expense.Date.ToString().Remove(0, 3);
            //Console.WriteLine($"Date : {expense.Date} - Date tronquée : {month}");
            if (!months.Exists(element => element == month))
            {
                //Console.WriteLine($"Mois unique compté : {month}");
                months.Add(month);
                //Console.WriteLine($"Nombre de mois : {months.Count}");
            }
        }
        return months.Count;
    }

    public static void DisplayExpensesByCat(IReadOnlyCollection<Expense> expenses)
    {
        var expensesbyCat = ExpensesByCat(expenses);

        foreach (var expensebycat in expensesbyCat.OrderBy(expensebycat => expensebycat.Key))
        {
            Console.WriteLine($"{expensebycat.Key,-12}:{expensebycat.Value,10:0.00} €");
        }
    }

    public static Dictionary<string, decimal> ExpensesByCat(IReadOnlyCollection<Expense> expenses)
    {
        string category = "";
        List<string> unicStrings = new();
        Dictionary<string, decimal> expensesbyCat = new();

        foreach (var expense in expenses)
        {
            category = expense.Category;
            //Console.WriteLine($"Date : {expense.Date} - Date tronquée : {category}");
            if (!expensesbyCat.ContainsKey(category))
            {
                //Console.WriteLine($"Ajout de la catégorie : {category} avec le montant {expense.Amount} €");
                expensesbyCat.Add(category, expense.Amount);
                //Console.WriteLine($"La catégorie {category} a été ajoutée avec le montant {expensesbyCat[category]} €");
            }
            else
            {
                //Console.WriteLine($"La catégorie : {category} contient {expensesbyCat[category]} € et va recevoir le montant {expense.Amount} €");
                expensesbyCat[expense.Category] += expense.Amount;
                //Console.WriteLine($"La catégorie : {category} contient maintenant {expensesbyCat[category]} €");
            }
        }

        return expensesbyCat;
    }

    public static void DisplayMonthlyAverageByCat(IReadOnlyCollection<Expense> expenses)
    {
        int nbmontheExpenses = DetailedStatistics.NbMonthExpenses(expenses);
        foreach (var expensebycat in (DetailedStatistics.ExpensesByCat(expenses)).OrderBy(expensebycat => expensebycat.Key))
        {
            Console.WriteLine($"{expensebycat.Key,-12}:{expensebycat.Value / nbmontheExpenses,10:0.00} €");
        }

    }


    public static decimal MonthlyAverage(IReadOnlyCollection<Expense> expenses)
    {
        return expenses.Sum(expense => expense.Amount) / DetailedStatistics.NbMonthExpenses(expenses);
    }

}
