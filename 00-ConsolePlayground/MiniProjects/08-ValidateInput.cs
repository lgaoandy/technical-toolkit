// https://learn.microsoft.com/en-ca/training/modules/csharp-do-while/5-exercise-challenge-differentiate-while-do-statements

using Microsoft.VisualBasic;

namespace MiniProjects.ValidateInput
{
    class ValidateInput
    {
        // Project #1
        public static void AskInt(int n = 5, int m = 10)
        {
            Console.WriteLine($"Enter an integer value between {n} and {m}:");

            bool valid = false;
            while (!valid)
            {
                if (int.TryParse(Console.ReadLine(), out int num))
                {
                    if (num <= m && num >= n)
                    {
                        valid = true;
                        Console.WriteLine($"Your input value ({num}) has been accepted.");
                    }
                    else
                        Console.WriteLine($"You entered {num}. Please enter a number between 5 and 10");
                }
                else
                    Console.WriteLine("Sorry, you entered an invalid number, please try again.");
            }
        }

        // Project #2 
        public static void AskRole()
        {
            string[] roles = ["administrator", "manager", "user"];

            // Generate a list of roles
            string rolesList = "";
            for (int i = 0; i < roles.Length; i++)
            {
                if (i == roles.Length - 1)
                {
                    rolesList += "or ";
                    rolesList += char.ToUpper(roles[i][0]) + roles[i][1..];
                }
                else
                    rolesList += char.ToUpper(roles[i][0]) + roles[i][1..] + ", ";
            }

            string? input;
            while (true)
            {
                Console.WriteLine($"Enter your role name ({rolesList})");

                // Wait until a non-null input is entered
                do
                {
                    input = Console.ReadLine();
                }
                while (input == null);

                // Check if input is one of the valid roles
                if (roles.Contains(input.Trim().ToLower()))
                {
                    Console.Write($"Your input value ({input}) has been accepted.");
                    break;
                }
                Console.Write($"The role name that you entered, \"{input}\" is not valid. ");
            }
        }
    }
}