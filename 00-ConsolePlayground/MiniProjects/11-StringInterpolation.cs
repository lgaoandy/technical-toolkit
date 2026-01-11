namespace MiniProjects.StringInterpolation
{
    // https://learn.microsoft.com/en-us/training/modules/csharp-format-strings/5-challenge
    class StringInterpolation
    {
        public static void PrintShares()
        {
            string customerName = "Ms. Barros";
            string currentProduct = "Magic Yield";
            int currentShares = 2975000;
            decimal currentReturn = 0.1275m;
            decimal currentProfit = 55000000.0m;
            string newProduct = "Glorious Future";
            decimal newReturn = 0.13125m;
            decimal newProfit = 63000000.0m;

            Console.WriteLine($"Dear {customerName},");
            Console.WriteLine($"As a customer of our {currentProduct} offering we are excited to tell you about a new financial product that would dramatically increase your return.\n");
            Console.WriteLine($"Currently, you own {currentShares:N2} shares at a return of {currentReturn:P}.\n");
            Console.WriteLine($"Our new product, {newProduct} offers a return of {newReturn:P}.  Given your current volume, your potential profit would be {newProfit:C2}.\n");
            Console.WriteLine($"Here's a quick comparison:\n");

            Console.WriteLine(
                currentProduct.PadRight(20) +
                $"{currentReturn:P}".PadRight(10) +
                $"{currentProfit:C}"
            );
            Console.WriteLine(
                newProduct.PadRight(20) +
                $"{newReturn:P}".PadRight(10) +
                $"{newProfit:C}"
            );
        }
    }
}