using System;
using System.ComponentModel;
using System.Diagnostics;

namespace p2beadontnet_starter;

public static class AutoTests
{
	public static void Run()
	{
        ///////////////////////////////////////////////////
        //////////// Création des jeux de test ////////////
        ///////////////////////////////////////////////////

        // Liste vide
        var emptylistExpenses = new List<Expense>
        {
        };

        // Liste de 1 dépenses vide
        var emptyExpenses = new List<Expense>
        {
            new Expense { }
        };

        // Liste de dépenses avec une seule dépense
        var oneExpenses = new List<Expense>
        {
            new Expense {
            Date = new DateOnly(2026, 1, 1),
            Category = "housing",
            Description = "Loyer",
            Amount = 500.00m
            }
        };

        // Liste de dépense complète : plusieurs mois sur plusieurs années, montants positifs, négatifs, nuls , plusieurs catégories
        var notemptyExpenses = new List<Expense>
        {
            new Expense
            {
                Date = new DateOnly(2026, 1, 2),
                Category = "housing",
                Description = "Loyer",
                Amount = 500m
            },
            new Expense
            {
                Date = new DateOnly(2025, 1, 1),
                Category = "leisure",
                Description = "Remboursement ciné",
                Amount = -24.0m
            },
            new Expense
            {
                Date = new DateOnly(2026, 1, 1),
                Category = "food",
                Description = "Courses",
                Amount = 0.00m
            },
            new Expense
            {
                Date = new DateOnly(2026, 2, 1),
                Category = "food",
                Description = "Courses",
                Amount = 13.25m
            }
        };


        ///////////////////////////////////////////////////
        /////////////// Tests des fonctions ///////////////
        ///////////////////////////////////////////////////

        // Test de la fonction CalculateTotalExpenses
        if (DetailedStatistics.CalculateTotalExpenses(emptylistExpenses) != 0m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with emptylistExpenses, résultat : {DetailedStatistics.CalculateTotalExpenses(emptylistExpenses)}");
        if (DetailedStatistics.CalculateTotalExpenses(emptyExpenses) != 0m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with emptyExpenses, résultat : {DetailedStatistics.CalculateTotalExpenses(emptyExpenses)}");
        if (DetailedStatistics.CalculateTotalExpenses(oneExpenses) != 500m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with  oneExpenses, résultat : {DetailedStatistics.CalculateTotalExpenses(oneExpenses)}");
        if (DetailedStatistics.CalculateTotalExpenses(notemptyExpenses) != 489.25m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with  notemptyExpenses, résultat : {DetailedStatistics.CalculateTotalExpenses(notemptyExpenses)}");


        // Test de la fonction NbMonthExpenses
        if (DetailedStatistics.NbMonthExpenses(emptylistExpenses) != 0) Console.WriteLine($"Erreur Test NbMonthExpenses() with emptylistExpenses, résultat : {DetailedStatistics.NbMonthExpenses(emptylistExpenses)}");
        if (DetailedStatistics.NbMonthExpenses(emptyExpenses) != 1) Console.WriteLine($"Erreur Test NbMonthExpenses() with emptyExpenses, résultat : {DetailedStatistics.NbMonthExpenses(emptyExpenses)}"); 
        if (DetailedStatistics.NbMonthExpenses(oneExpenses) != 1) Console.WriteLine($"Erreur Test NbMonthExpenses() with oneExpenses, résultat : {DetailedStatistics.NbMonthExpenses(oneExpenses)}");
        if (DetailedStatistics.NbMonthExpenses(notemptyExpenses) != 3) Console.WriteLine($"Erreur Test NbMonthExpenses() with notemptyExpenses, résultat : {DetailedStatistics.NbMonthExpenses(notemptyExpenses)}");

        
        // Test de la fonction ExpensesByCat
        if (DetailedStatistics.ExpensesByCat(emptylistExpenses).Count != 0) Console.WriteLine($"Erreur Test ExpensesByCat() with emptylistExpenses, résultat : {DetailedStatistics.ExpensesByCat(emptylistExpenses).Count}");
        
        if (DetailedStatistics.ExpensesByCat(emptyExpenses).Count != 1) Console.WriteLine($"Erreur Test ExpensesByCat().Count with emptyExpenses, résultat : {DetailedStatistics.ExpensesByCat(emptyExpenses).Count}");
        if (DetailedStatistics.ExpensesByCat(emptyExpenses)[""] != 0m) { Console.WriteLine($"Erreur Test ExpensesByCat() with emptyExpenses, résultats : {DetailedStatistics.ExpensesByCat(emptyExpenses)[""]}"); }

        if (DetailedStatistics.ExpensesByCat(oneExpenses).Count != 1) Console.WriteLine($"Erreur Test ExpensesByCat().Count with oneExpenses, résultat : {DetailedStatistics.ExpensesByCat(oneExpenses)}");
        if (DetailedStatistics.ExpensesByCat(oneExpenses)["housing"] != 500m) { Console.WriteLine($"Erreur Test ExpensesByCat() with oneExpenses, résultats housing : {DetailedStatistics.ExpensesByCat(oneExpenses)["housing"]}"); }

        if (DetailedStatistics.ExpensesByCat(notemptyExpenses).Count != 3) Console.WriteLine($"Erreur Test ExpensesByCat().Count with notemptyExpenses, résultat : {DetailedStatistics.ExpensesByCat(notemptyExpenses).Count}");
        if (DetailedStatistics.ExpensesByCat(notemptyExpenses)["housing"] != 500m ||
            DetailedStatistics.ExpensesByCat(notemptyExpenses)["leisure"] != -24m ||
            DetailedStatistics.ExpensesByCat(notemptyExpenses)["food"] != 13.25m) { Console.WriteLine($"Erreur Test ExpensesByCat() with notemptyExpenses, résultats housing : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["housing"]}, leisure : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["leisure"]}, food : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["food"]}"); }


        // Test de la fonction MonthlyAverageByCat
        if (decimal.Round(DetailedStatistics.MonthlyAverageByCat(oneExpenses)["housing"],2) != 500m)
                Console.WriteLine($"Erreur Test MonthlyAverageByCat with oneExpenses, résultat : {DetailedStatistics.MonthlyAverageByCat(oneExpenses)["housing"]}");

        if (decimal.Round(DetailedStatistics.MonthlyAverageByCat(notemptyExpenses)["housing"], 2) != 166.67m)
            Console.WriteLine($"Erreur Test MonthlyAverageByCat with notemptyExpenses, résultat : {DetailedStatistics.MonthlyAverageByCat(notemptyExpenses)["housing"]}");
        if (decimal.Round(DetailedStatistics.MonthlyAverageByCat(notemptyExpenses)["leisure"], 2) != -8m)
            Console.WriteLine($"Erreur Test MonthlyAverageByCat with notemptyExpenses, résultat : {DetailedStatistics.MonthlyAverageByCat(notemptyExpenses)["leisure"]}"); 
        if (decimal.Round(DetailedStatistics.MonthlyAverageByCat(notemptyExpenses)["food"], 2) != 4.42m)
            Console.WriteLine($"Erreur Test MonthlyAverageByCat with notemptyExpenses, résultat : {DetailedStatistics.MonthlyAverageByCat(notemptyExpenses)["food"]}");


        // Test de la fonction MonthlyAverage
        decimal? nullableDecimal = DetailedStatistics.MonthlyAverage(oneExpenses);
        if ( ((nullableDecimal is not decimal notnullableMonthlyAverageoneExpenses) 
            ? null 
            : decimal.Round(notnullableMonthlyAverageoneExpenses, 2)) != 500.00m)
            Console.WriteLine($"Erreur Test MonthlyAverage with oneExpenses, résultat : {nullableDecimal}");
        
        nullableDecimal = DetailedStatistics.MonthlyAverage(notemptyExpenses);
        if ( ((nullableDecimal is not decimal notnullableMonthlyAveragenotemptyExpenses) 
            ? null
            : decimal.Round(notnullableMonthlyAveragenotemptyExpenses, 2)) != 163.08m)
            Console.WriteLine($"Erreur Test MonthlyAverage with notemptyExpenses, résultat : {nullableDecimal}");

        
        // Test de la fonction CalculateCarPurchaseSimulation
        if (BudgetApplication.CalculateCarPurchaseSimulation(emptylistExpenses, 2000, 20000) != null)
            Console.WriteLine($"Erreur Test CalculateCarPurchaseSimulation with emptylistExpenses, résultat : {BudgetApplication.CalculateCarPurchaseSimulation(emptylistExpenses, 2000, 20000)}");

        if (BudgetApplication.CalculateCarPurchaseSimulation(oneExpenses, 2000, 20000) != 14m)
            Console.WriteLine($"Erreur Test CalculateCarPurchaseSimulation with oneExpenses, résultat : {BudgetApplication.CalculateCarPurchaseSimulation(oneExpenses, 2000, 20000)}");

        if (BudgetApplication.CalculateCarPurchaseSimulation(notemptyExpenses, 2000, 20000) != 11m)
            Console.WriteLine($"Erreur Test CalculateCarPurchaseSimulation with notemptyExpenses, résultat : {BudgetApplication.CalculateCarPurchaseSimulation(notemptyExpenses, 2000, 20000)}");


        // Test de la fonction CalculateMorgageSimulation
        if (Decimal.Round(BudgetApplication.CalculateMorgageSimulation(700, 20, 0), 2) != 168000.00m)
            Console.WriteLine($"Erreur Test CalculateMorgageSimulation with r=0, résultat : {Decimal.Round(BudgetApplication.CalculateMorgageSimulation(700, 20, 0), 2)}");
        if (Decimal.Round(BudgetApplication.CalculateMorgageSimulation(700, 20, 2), 2) != 138371.82m) 
            Console.WriteLine($"Erreur Test CalculateMorgageSimulation with r!=0, résultat : {Decimal.Round(BudgetApplication.CalculateMorgageSimulation(700, 20, 2), 2)}");
    }

}
