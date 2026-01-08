// https://learn.microsoft.com/en-ca/training/modules/csharp-for/3-exercise-challenge-for-statements

namespace MiniProjects.FizzBuzz
{
    class FizzBuzz
    {
        public static void Run(int count)
        {
            for (int i = 1; i < count + 1; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                    Console.WriteLine($"{i} - FizzBuzz");
                else if (i % 3 == 0)
                    Console.WriteLine($"{i} - Fizz");
                else if (i % 5 == 0)
                    Console.WriteLine($"{i} - Buzz");
                else
                    Console.WriteLine(i);
            }
        }
    }
}