using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailProject
{
    public static class GlobalVariable
    {
        public static int Income = 0;
        public static int Tip = 0;
        public static int customerNumber = 0;
        public static byte Day = 1;
        public static Dictionary<string, int> CocktailHaveDone = new Dictionary<string, int>();

        public static void Reset()
        {
            Income = 0;
            customerNumber = 0;
            Day = 1;
            CocktailHaveDone.Clear();
        }

        public static void AddIncome(int amount)
        {
            Income += amount;
        }

        public static void AddTip(int amount)
        {
            Tip += amount;
        }

        public static void AddCustomer()
        {
            customerNumber++;
        }

        public static void AddCocktailDone(string cocktailName, int income)
        {
            // Ensure the dictionary exists
            if (CocktailHaveDone == null)
                CocktailHaveDone = new Dictionary<string, int>();

            // Add or update the cocktail income
            if (CocktailHaveDone.ContainsKey(cocktailName))
            {
                CocktailHaveDone[cocktailName] += income;
            }
            else
            {
                CocktailHaveDone[cocktailName] = income;
            }
        }


        public static void NextDay()
        {
            Day++;
            Tip = 0;
            customerNumber = 0;
            Income = 0;
            CocktailHaveDone = new Dictionary<string, int>();
        }

        public static void DebugPrint()
        {
            Console.WriteLine($"Day: {Day}, Income: {Income}, Tip: {Tip}, Customers: {customerNumber}");
            foreach (var cocktail in CocktailHaveDone)
            {
                Console.WriteLine($"Cocktail: {cocktail.Key}, Income: {cocktail.Value}");
            }
        }

        public static string DebugPrintString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Day: {Day}, \nIncome: {Income}, \nTip: {Tip}, \nCustomers: {customerNumber}");
            foreach (var cocktail in CocktailHaveDone)
            {
                sb.AppendLine($"Cocktail: {cocktail.Key}, Income: {cocktail.Value}");
            }
            return sb.ToString();
        }

    }
}
