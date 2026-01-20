// Reference material: https://learn.microsoft.com/en-us/training/modules/challenge-project-debug-c-sharp-console-application/
// Personal rewrite of what was given from the project description and starter code
using System.Drawing;
using Pastel;

namespace MiniProjects.SmarterCashRegister
{
    class SmarterCashRegister
    {
        private static int[,] DailyStartingCash { get; } = { { 1, 50 }, { 5, 20 }, { 10, 10 }, { 20, 5 } };

        public static void ApplyTransactions(int transactions)
        {
            // Setup starting cash till
            int[] cashTill = LoadStartingTill();
            int transactionRevenue = 0;
            int creditRevenue = 0;
            int creditTransactionCharge = 0;

            LogTillStatus(cashTill);

            for (int i = 0; i < transactions; i++)
            {
                // Generate random transaction for transactions
                int cost = GenerateTransaction(2, 49);
                int[] cashPayment = GenerateCashPayments(cost);

                // Check if cash payment is valid
                try
                {
                    cashTill = PayCash(cashTill, cost, cashPayment);
                    LogTillStatus(cashTill);
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message.Pastel(Color.PaleVioletRed));
                    Console.WriteLine($"${cost} paid in credit card instead.");
                    creditRevenue += cost;
                    creditTransactionCharge += 2;
                }

                transactionRevenue += cost;
            }

            // Announce Till Status
            Console.WriteLine("\n--- CLOSING --");
            LogTillStatus(cashTill);
            int cashRevenue = CalculateCashValue(cashTill) - CalculateCashValue(LoadStartingTill());

            Console.WriteLine($"Total Cash Revenue: ${cashRevenue}");
            Console.WriteLine($"Total Credit Revenue: ${creditRevenue}");
            Console.WriteLine($"Credit Card Transaction Cost: ${creditTransactionCharge}");

            if (cashRevenue + creditRevenue == transactionRevenue)
                Console.WriteLine("Transaction success!".Pastel(Color.Green) + $" - ${cashRevenue} (Cash) + ${creditRevenue} (Credit) = ${transactionRevenue} (Total Cost)");
            else
                Console.WriteLine("Revenue Lost!".Pastel(Color.Red) + $" - ${cashRevenue} + ${creditRevenue} =/= ${transactionRevenue}");

            Console.WriteLine("Press any key to continue...".Pastel(Color.DimGray));
            Console.ReadKey();
        }

        static int[] PayCash(int[] cashTill, int cost, int[] cashPayment)
        {
            int cashPaymentValue = CalculateCashValue(cashPayment);
            int changeNeeded = cashPaymentValue - cost;

            // Check if cash payment can cover cost
            if (cashPaymentValue < cost)
                throw new InvalidOperationException("Cash payment did not cover item cost.");

            // Add cash payment to supply
            int[] cashSupply = (int[])cashTill.Clone();
            for (int i = 0; i < cashSupply.Length; i++)
                cashSupply[i] += cashPayment[i];

            // Give back change
            while (changeNeeded > 19 && cashSupply[3] > 0)
            {
                cashSupply[3]--;
                changeNeeded -= 20;
            }

            while (changeNeeded > 9 && cashSupply[2] > 0)
            {
                cashSupply[2]--;
                changeNeeded -= 10;
            }

            while (changeNeeded > 4 && cashSupply[1] > 0)
            {
                cashSupply[1]--;
                changeNeeded -= 5;
            }

            while (changeNeeded > 0 && cashSupply[0] > 0)
            {
                cashSupply[0]--;
                changeNeeded -= 1;
            }

            // Check if there is enough cash in till to give change
            if (changeNeeded > 0)
                throw new InvalidOperationException($"Not enough cash to give change for ${cashPaymentValue - cost}.");
            return cashSupply;
        }

        static int GenerateTransaction(int minCost, int maxCost)
        {
            Random dice = new();
            return dice.Next(minCost, maxCost);
        }

        static int[] GenerateCashPayments(int cost)
        {
            int[] cashPayment = new int[4];
            cashPayment[0] = cost % 2;                 // value is 1 when itemCost is odd, value is 0 when itemCost is even
            cashPayment[1] = (cost % 10 > 7) ? 1 : 0; // value is 1 when itemCost ends with 8 or 9, otherwise value is 0
            cashPayment[2] = (cost % 20 > 13) ? 1 : 0; // value is 1 when 13 < itemCost < 20 OR 33 < itemCost < 40, otherwise value is 0
            cashPayment[3] = (cost < 20) ? 1 : 2;  // value is 1 when itemCost < 20, otherwise value is 2
            return cashPayment;
        }

        static int[] LoadStartingTill()
        {
            int n = DailyStartingCash.GetLength(0);
            int[] cashTill = new int[n];
            for (int i = 0; i < n; i++)
                cashTill[i] = DailyStartingCash[i, 1];
            return cashTill;
        }

        static int CalculateCashValue(int[] cash)
        {
            int sum = 0;
            for (int i = 0; i < DailyStartingCash.GetLength(0); i++)
                sum += DailyStartingCash[i, 0] * cash[i];
            return sum;
        }

        static void LogTillStatus(int[] cashTill)
        {
            int sum = 0;
            Console.WriteLine("CURRENT TILL BALANCE:");
            for (int i = cashTill.Length - 1; i >= 0; i--)
            {
                string count = cashTill[i].ToString().PadLeft(5);
                string value = ("$" + DailyStartingCash[i, 0]).PadRight(3);
                int total = cashTill[i] * DailyStartingCash[i, 0];
                Console.WriteLine($"{count} x {value} = ${total}");
                sum += total;
            }
            Console.WriteLine($"{"Total".PadLeft(11)}:  ${sum}");
        }
    }
}