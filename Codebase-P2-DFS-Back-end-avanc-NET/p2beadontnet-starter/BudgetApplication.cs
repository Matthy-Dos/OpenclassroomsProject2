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
        //var autoChecks = new AutoChecks();
        AutoTests.Run();
        
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
                    Console.WriteLine("");
                    DisplayHistory(expenses);
                    break;
                case 2:
                    Console.WriteLine("");
                    AddExpense(expenses);
                    break;
                case 3:
                    Console.WriteLine("\nStatistiques basiques");
                    DisplayBasicStatistics(expenses);
                    break;
                case 4:
                    Console.WriteLine("\nStatistiques complètes");
                    DisplayDetailedStatistics(expenses);
                    break;
                case 5:
                    Console.WriteLine("\nSimulation achat d'une voiture");
                    DisplayCarPurchaseSimulation(expenses);
                    break;
                case 6:
                    Console.WriteLine("\nSimulation prêt immobilier");
                    DisplayMorgageSimulation(expenses);
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
        Console.WriteLine("6. Simulation prêt immobilier");
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
        Console.WriteLine($"Dépenses totales : {DetailedStatistics.CalculateTotalExpenses(expenses)} €");
        Console.WriteLine($"Nombre de mois couverts par les dépenses : {DetailedStatistics.NbMonthExpenses(expenses)}");
        Console.WriteLine($"Dépenses totales par poste :"); DetailedStatistics.DisplayExpensesByCat(expenses);
        Console.WriteLine($"Dépenses moyennes mensuelles par poste :"); DetailedStatistics.DisplayMonthlyAverageByCat(expenses);
        Console.WriteLine($"Dépenses moyennes mensuelles : {DetailedStatistics.MonthlyAverage(expenses), 0:0.00} €");
    }


    private static void DisplayCarPurchaseSimulation(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Aucune dépense disponible, simulation d'achat de voiture impossible.");
            return;
        }

        decimal incomes = Input.ReadPositiveDecimal("Pour effectuer la simulation d'achat de véhihcule, veuillez renseigner vos revenus nets mensuels (€) : ");
        decimal price = Input.ReadPositiveDecimal("Quel est le prix d'achat du véhihcule (€) : ");
        decimal? monthlyExpenses = DetailedStatistics.MonthlyAverage(expenses);
        decimal? carSimulation = CalculateCarPurchaseSimulation(expenses, incomes, price);

        
        Console.WriteLine(
            carSimulation > 0 ? 
                $"En tenant compte de dépenses mensuelles de {monthlyExpenses,0:0.00} €, " + $"il faut compter {carSimulation,0:0} mois pour financer le véhicule." 
                : "Le projet ne peut pas être financé dans ces conditions.");


        /*if (monthlySavings > 0)
        {
            Console.WriteLine($"En tenant compte de dépenses mensuelles de {MonthlyAverage(expenses),0:0.00} €, " +
                $"il faut compter {price / monthlySavings,0:0} mois pour financer le véhicule.");
        } else { Console.WriteLine("Le projet ne peut pas être financé dans ces conditions."); }
        */

    }

    public static decimal? CalculateCarPurchaseSimulation(IReadOnlyCollection<Expense> expenses, decimal incomes, decimal price)
    {
        decimal? monthlySavings = incomes - DetailedStatistics.MonthlyAverage(expenses);
        if (monthlySavings > 0 && (monthlySavings is decimal Value))
        {
            return decimal.Round(price / Value, 0, MidpointRounding.ToPositiveInfinity); 
        } 
        return null;
    }

    private static void DisplayMorgageSimulation(IReadOnlyCollection<Expense> expenses)
    {
        if (expenses.Count == 0)
        {
            Console.WriteLine("Aucune dépense disponible. Calcul de simulation de prêt impossible");
            return;
        }

        if (!expenses.Any(expense => expense.Category == "housing"))
        {
            Console.WriteLine("Aucune dépense de logement saisie. Calcul de simulation de prêt impossible");
            return;
        }

        decimal incomes = Input.ReadPositiveDecimal("Pour effectuer la simulation de prêt immobilier, veuillez renseigner vos revenus nets mensuels (€) : ");
        decimal mortgageRate = Input.ReadDecimal("Quel est le taux annuel du crédit (%) : ");
        decimal mortgageDuration = Input.ReadPositiveDecimal("Quelle est la durée du prêt en années : ");


        decimal housingaverageExpenses = (DetailedStatistics.ExpensesByCat(expenses))["housing"] / DetailedStatistics.NbMonthExpenses(expenses);
        decimal? monthlyAverage = DetailedStatistics.MonthlyAverage(expenses);
        if (monthlyAverage is not decimal) 
        {
            Console.WriteLine($"Dépenses mensuelle impossibles à calculer, simulation de crédit impossible");
            return;
        }

        decimal monthlySavings = incomes - (decimal)monthlyAverage;
        decimal theomonthlyPaiement = monthlySavings + housingaverageExpenses;
        decimal validmonthlyPaiement = Math.Min(theomonthlyPaiement, incomes * 0.35m);
        
        

        if (validmonthlyPaiement <= 0)
        {
            Console.WriteLine($"Capacité d'emprunt insuffisante, les dépenses ({monthlyAverage} €) sont supérieures aux revenus");
            return;
        }
        Console.WriteLine("\n{0,-35}: {1,10:0.00} €", "Dépenses mensuelles (loyer inclus)", monthlyAverage);
        Console.WriteLine("{0,-35}: {1,10:0.00} €", "Dépenses mensuelles (hors loyer)", incomes - theomonthlyPaiement);
        Console.WriteLine("{0,-35}: {1,10:0.00} €", "Mensualités théoriques", theomonthlyPaiement);
        Console.WriteLine("{0,-35}: {1,10:0.00} €", "Mensualités retenues", validmonthlyPaiement);

        /*if (mortgageRate == 0)
        {
            Console.WriteLine($"La capacité d'emprunt est de {validmonthlyPaiement * mortgageDuration * 12} €");
        } else
        {
            decimal n = mortgageDuration * 12;
            decimal r = mortgageRate / 100 / 12;
            Console.WriteLine($"La capacité d'emprunt est de {validmonthlyPaiement * ( 1 - (decimal)Math.Pow((double)(1m + r), (double)(-n))) / r, 0:0.00} €");
        }*/

        Console.WriteLine($"\nLa capacité d'emprunt est de {CalculateMorgageSimulation(validmonthlyPaiement, mortgageDuration, mortgageRate), 0:0.00} €");

    }

    public static decimal CalculateMorgageSimulation (decimal validmonthlyPaiement, decimal mortgageDuration, decimal mortgageRate)
    {
        if (mortgageRate == 0)
        {
            return validmonthlyPaiement * mortgageDuration * 12;
        }
        else
        {
            decimal n = mortgageDuration * 12;
            decimal r = mortgageRate / 100 / 12;
            return validmonthlyPaiement * (1 - (decimal)Math.Pow((double)(1m + r), (double)(-n))) / r;
        }
    }

}