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
        if (DetailedStatistics.CalculateTotalExpenses(emptylistExpenses) != 0m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with one expense, résultat : {DetailedStatistics.NbMonthExpenses(emptylistExpenses)}");
        if (DetailedStatistics.CalculateTotalExpenses(emptyExpenses) != 0m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with one expense, résultat : {DetailedStatistics.NbMonthExpenses(emptyExpenses)}");
        if (DetailedStatistics.CalculateTotalExpenses(oneExpenses) != 500m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with  notemptyExpenses, résultat : {DetailedStatistics.NbMonthExpenses(oneExpenses)}");
        if (DetailedStatistics.CalculateTotalExpenses(notemptyExpenses) != 489.25m) Console.WriteLine($"Erreur Test CalculateTotalExpenses() with  notemptyExpenses, résultat : {DetailedStatistics.NbMonthExpenses(notemptyExpenses)}");


        // Test de la fonction NbMonthExpenses
        if (DetailedStatistics.NbMonthExpenses(emptylistExpenses) != 0) Console.WriteLine($"Erreur Test NbMonthExpenses() with emptylistExpenses, résultat : {DetailedStatistics.NbMonthExpenses(emptylistExpenses)}");
        if (DetailedStatistics.NbMonthExpenses(emptyExpenses) != 1) Console.WriteLine($"Erreur Test NbMonthExpenses() with notemptyExpenses, résultat : {DetailedStatistics.NbMonthExpenses(emptyExpenses)}"); 
        if (DetailedStatistics.NbMonthExpenses(oneExpenses) != 1) Console.WriteLine($"Erreur Test NbMonthExpenses() with one expense, résultat : {DetailedStatistics.NbMonthExpenses(oneExpenses)}");
        if (DetailedStatistics.NbMonthExpenses(notemptyExpenses) != 3) Console.WriteLine($"Erreur Test NbMonthExpenses() with notemptyExpenses, résultat : {DetailedStatistics.NbMonthExpenses(notemptyExpenses)}");

        
        // Test de la fonction ExpensesByCat
        if (DetailedStatistics.ExpensesByCat(emptylistExpenses).Count != 0) Console.WriteLine($"Erreur Test ExpensesByCat() with emptylistExpenses, résultat : {DetailedStatistics.ExpensesByCat(emptylistExpenses)}");
        
        if (DetailedStatistics.ExpensesByCat(emptyExpenses).Count != 1) Console.WriteLine($"Erreur Test ExpensesByCat() with emptyExpenses, résultat : {DetailedStatistics.ExpensesByCat(emptyExpenses)}");
        if (DetailedStatistics.ExpensesByCat(emptyExpenses)[""] != 0m) { Console.WriteLine($"Erreur Test ExpensesByCat() with one expense, résultats housing : {DetailedStatistics.ExpensesByCat(emptyExpenses)["housing"]}"); }

        if (DetailedStatistics.ExpensesByCat(oneExpenses).Count != 1) Console.WriteLine($"Erreur Test ExpensesByCat() with one expense, résultat : {DetailedStatistics.ExpensesByCat(oneExpenses)}");
        if (DetailedStatistics.ExpensesByCat(oneExpenses)["housing"] != 500m) { Console.WriteLine($"Erreur Test ExpensesByCat() with one expense, résultats housing : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["housing"]}"); }

        if (DetailedStatistics.ExpensesByCat(notemptyExpenses).Count != 3) Console.WriteLine($"Erreur Test ExpensesByCat() with notemptyExpenses, résultat : {DetailedStatistics.ExpensesByCat(notemptyExpenses)}");
        if (DetailedStatistics.ExpensesByCat(notemptyExpenses)["housing"] != 500m ||
            DetailedStatistics.ExpensesByCat(notemptyExpenses)["leisure"] != -24m ||
            DetailedStatistics.ExpensesByCat(notemptyExpenses)["food"] != 13.25m) { Console.WriteLine($"Erreur Test ExpensesByCat() with notemptyExpenses, résultats housing : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["housing"]}, leisure : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["leisure"]}, food : {DetailedStatistics.ExpensesByCat(notemptyExpenses)["food"]}"); }
    }

}
