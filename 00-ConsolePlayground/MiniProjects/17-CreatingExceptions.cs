namespace MiniProjects.CreatingExceptions
{
    // Reference material: https://learn.microsoft.com/en-us/training/modules/create-throw-exceptions-c-sharp/4-exercise-challenge-create-throw-exception
    class CreatingExceptions
    {
        public static void Run()
        {
            string[][] userEnteredValues = [
                ["1", "2", "3"],
                ["1", "two", "3"],
                ["0", "1", "2"],
            ];

            try
            {
                Workflow1(userEnteredValues);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during 'Workflow1'.");
                Console.WriteLine(ex.Message);
            }
        }

        // Refactored
        static void Process1(string[] userEntries)
        {
            foreach (string userValue in userEntries)
            {
                if (!int.TryParse(userValue, out int valueEntered))
                    throw new FormatException("Invalid data. User input values must be valid integers.");
                if (valueEntered == 0)
                    throw new DivideByZeroException("Invalid data. User input values must be non-zero values.");
            }
            Console.WriteLine("'Process1' completed successfully.");
            Console.WriteLine();
        }

        static void Workflow1(string[][] userEnteredValues)
        {
            foreach (string[] userEntries in userEnteredValues)
            {
                try
                {
                    Process1(userEntries);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("'Process1' encountered an issue, process aborted.");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }
            }
            Console.WriteLine("'Workflow1' completed successfully.");
        }
    }
}