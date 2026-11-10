namespace p2beadontnet_starter;

public sealed class BudgetApplication
{
    private readonly CsvExpenseRepository repository;

    public BudgetApplication(CsvExpenseRepository repository)
    {
        this.repository = repository;
    }

    public void Run()
    {
        var expenses = repository.Load();
        Console.WriteLine("=== Emma Budget ===");
        Console.WriteLine($"{expenses.Count} dépense(s) chargée(s).\n");

        var running = true;
        while (running)
        {
            DisplayMenu();
            switch (Input.ReadMenuChoice())
            {
                case 1:
                    DisplayHistory(expenses);
                    break;
                case 2:
                    AddExpense(expenses);
                    break;
                case 3:
                    DisplayBasicStatistics(expenses);
                    break;
                case 4:
                    Console.WriteLine("Statistiques complètes");
                    DisplayDetailedStatistics(expenses);
                    break;
                case 5:
                    Console.WriteLine("Simulation voiture");
                    SimulateCarPurchase(expenses);
                    break;
                case 6:
                    Console.WriteLine("Simulation prêt immobilier : en cours d'implémentation.");
                    SimulateMortgage(expenses);
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Choix inconnu.");
                    break;
            }

            Console.WriteLine();
        }

        Console.WriteLine("À bientôt.");
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("1. Afficher l'historique");
        Console.WriteLine("2. Ajouter une dépense");
        Console.WriteLine("3. Statistiques de base");
        Console.WriteLine("4. Statistiques complètes");
        Console.WriteLine("5. Simulation achat d'une voiture");
        Console.WriteLine("6. Simulation prêt immobilier (en cours d'implémentation)");
        Console.WriteLine("0. Quitter");
    }

    private static void DisplayHistory(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Aucune dépense enregistrée.");
            return;
        }

        foreach (var expense in expenses.OrderByDescending(expense => expense.Date))
        {
            Console.WriteLine($"{expense.Date:dd/MM/yyyy} | {expense.Category,-12} | {expense.Description,-30} | {expense.Amount,10:0.00} €");
        }
    }

    private void AddExpense(List<Expense> expenses)
    {
        var expense = new Expense
        {
            Date = ReadDate(),
            Category = Input.ReadRequired("Catégorie : "),
            Description = Input.ReadRequired("Description : "),
            Amount = Input.ReadDecimal("Montant : ")
        };

        repository.Add(expense);
        expenses.Add(expense);
        Console.WriteLine("Dépense enregistrée.");
    }

    private static DateOnly ReadDate()
    {
        while (true)
        {
            Console.Write("Date (dd/MM/yyyy) : ");
            if (DateOnly.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", out var date))
            {
                return date;
            }

            Console.WriteLine("Saisissez une date valide au format dd/MM/yyyy.");
        }
    }

    private static void DisplayBasicStatistics(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Aucune statistique disponible.");
            return;
        }

        Console.WriteLine($"Nombre de dépenses : {expenses.Count}");
        Console.WriteLine($"Période : {expenses.Min(expense => expense.Date):dd/MM/yyyy} - {expenses.Max(expense => expense.Date):dd/MM/yyyy}");
        Console.WriteLine("Les statistiques détaillées sont à implémenter dans le cadre de l'exercice.");
    }

    private static void DisplayDetailedStatistics(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Aucune statistique disponible.");
            return;
        }

        // Utilise ici une lambda :
        // Pour chaque Expense de la collection expenses, appelle-le temporairement expense et renvoie son Amount.
        //var total = expenses.Sum(expense => expense.Amount);
        Console.WriteLine($"Dépenses totales : {expenses.Sum(expense => expense.Amount)} €");
        Console.WriteLine($"Nombre de mois couverts par les dépenses : {NbMonthExpenses(expenses)}");
        Console.WriteLine($"Dépenses totales par poste :"); DisplayExpensesByCat(expenses);
        Console.WriteLine($"Dépenses moyennes mensuelles par poste :"); DisplayMonthlyAverageByCat(expenses);
        Console.WriteLine($"Dépenses moyennes mensuelles : {MonthlyAverage(expenses), 0:0.00} €");
    }

    private static int NbMonthExpenses(IReadOnlyCollection<Expense> expenses)
    {
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
            }
        }
        return months.Count;
    }

    private static void DisplayExpensesByCat(IReadOnlyCollection<Expense> expenses)
    {
        var expensesbyCat = ExpensesByCat(expenses);

        foreach (var expensebycat in expensesbyCat.OrderBy(expensebycat => expensebycat.Key))
        {
            Console.WriteLine($"{expensebycat.Key, -12}:{expensebycat.Value, 10:0.00} €");
        }
    }

    private static Dictionary<string, decimal> ExpensesByCat(IReadOnlyCollection<Expense> expenses)
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

    private static void DisplayMonthlyAverageByCat(IReadOnlyCollection<Expense> expenses)
    {
        int nbmontheExpenses = NbMonthExpenses(expenses);
        foreach (var expensebycat in ExpensesByCat(expenses))
        {
            Console.WriteLine($"{expensebycat.Key, -12}:{expensebycat.Value / nbmontheExpenses, 10:0.00} €");
        }

    }

    private static decimal MonthlyAverage(IReadOnlyCollection<Expense> expenses)
    {
        return expenses.Sum(expense => expense.Amount) / NbMonthExpenses(expenses);
    }

    private static void SimulateCarPurchase(IReadOnlyCollection<Expense> expenses)
    {
        decimal incomes = Input.ReadDecimal("Pour effectuer la simulation d'achat de véhihcule, veuillez renseigner vos revenus nets mensuels (€) : ");
        decimal price = Input.ReadDecimal("Quel est le prix d'achat du véhihcule (€) : ");
        decimal monthlySavings = incomes - MonthlyAverage(expenses);
        if (monthlySavings > 0)
        {
            Console.WriteLine($"En tenant compte de dépenses mensuelles de {MonthlyAverage(expenses),0:0.00} €, " +
                $"il faut compter {price / monthlySavings,0:0} mois pour financer le véhicule.");
        } else { Console.WriteLine("Le projet ne peut pas être financé dans ces conditions."); }
    }

    private static void SimulateMortgage(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Aucune statistique disponible. Calcul de simulation de prêt impossible");
            return;
        }

        if (!FindExpenceCat(expenses, "housing"))
        {
            Console.WriteLine("Aucune dépense de logement disponible. Calcul de simulation de prêt impossible");
            return;
        }

        decimal incomes = Input.ReadDecimal("Pour effectuer la simulation de prêt immobilier, veuillez renseigner vos revenus nets mensuels (€) : ");
        decimal mortgageRate = Input.ReadDecimal("Quel est le taux annuel du crédit (%) : ");
        decimal mortgageDuration = Input.ReadDecimal("Quelle est la durée du prêt en années : ");

        if (mortgageDuration <= 0)
        {
            Console.WriteLine("Durée de prêt négative ou nulle, calcul de simulation de prêt impossible");
            return;
        }

        decimal housingaverageExpenses = (ExpensesByCat(expenses))["housing"] / NbMonthExpenses(expenses);
        decimal monthlyAverage = MonthlyAverage(expenses);
        decimal monthlySavings = incomes - monthlyAverage;
        decimal theomonthlyPaiement = monthlySavings + housingaverageExpenses;
        decimal validmonthlyPaiement = Math.Min(theomonthlyPaiement, incomes * 0.35m);
        
        if (validmonthlyPaiement <= 0)
        {
            Console.WriteLine($"Capacité d'emprunt insuffisante, les dépenses ({monthlyAverage} €) sont supérieures aux revenus");
            return;
        }
        Console.WriteLine($"Dépenses mensuelles :{monthlyAverage, 10:0.00} €");
        Console.WriteLine($"Mensualités théoriques :{theomonthlyPaiement,10:0.00} €");
        Console.WriteLine($"Mensualités retenues :{validmonthlyPaiement,10:0.00} €");

        if (mortgageRate == 0)
        {
            Console.WriteLine($"La capacité d'emprunt est de {validmonthlyPaiement * mortgageDuration * 12} €");
        } else
        {
            decimal n = mortgageDuration * 12;
            decimal r = mortgageRate / 100 / 12;
            Console.WriteLine($"La capacité d'emprunt est de {validmonthlyPaiement * ( 1 - (decimal)Math.Pow((double)(1m + r), (double)(-n))) / r, 0:0.00} €");
        }

    }

    private static bool FindExpenceCat(IReadOnlyCollection<Expense> expenses, string category)
    {
        foreach (var expense in expenses)
        {
            if (expense.Category == category) { return true; }
        }
        return false;
    }
}