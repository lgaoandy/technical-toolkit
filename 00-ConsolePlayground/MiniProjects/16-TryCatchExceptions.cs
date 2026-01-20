namespace MiniProjects.TryCatchExceptions
{
    class TryCatchExceptions
    {

        // Reference exercise: https://learn.microsoft.com/en-us/training/modules/implement-exception-handling-c-sharp/8-exercise-challenge-try-catch-explicit-exceptions
        public static void CatchSpecificExceptions()
        {
            OverFlowExceptionExample();
            NullReferenceExceptionExample();
            IndexOutOfRangeExceptionExample();
            DivideByZeroExceptionExample();
            Console.WriteLine("Exiting program.");
        }

        static void OverFlowExceptionExample()
        {
            try
            {
                int num1 = int.MaxValue;
                int num2 = int.MaxValue;
                int result = checked(num1 + num2);
            }
            catch (OverflowException ex)
            {
                Console.WriteLine("Error: The number is too large to be represented as an integer. " + ex.Message);
            }
        }

        static void NullReferenceExceptionExample()
        {
            try
            {
                // string? str = null;
                // int length = str.Length;
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Error: The reference is null. " + ex.Message);
            }
        }

        static void IndexOutOfRangeExceptionExample()
        {
            try
            {
                int[] numbers = new int[5];
                numbers[5] = 10;
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Error: Index out of range. " + ex.Message);
            }
        }

        static void DivideByZeroExceptionExample()
        {
            try
            {
                int num3 = 10;
                int result2 = num3 / 0;
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Error: Cannot divide by zero. " + ex.Message);
            }
        }
    }
}