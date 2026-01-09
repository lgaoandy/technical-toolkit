
namespace MiniProjects.DataConversion
{
    class DataConversion
    {
        /*
            DECODES 
            Intakes: a string array with a mix of numeric values and strings
                - if a value is numeric, add to a sum
                - if a value is a string, concatenate to form a message
            Output: consoles the sum of numeric values and the text message separately
            
            Reference:
            https://learn.microsoft.com/en-us/training/modules/csharp-convert-cast/4-challenge
        */
        public static void Decode(string[] values)
        {
            double sum = 0;
            string message = "";

            foreach (string value in values)
            {
                if (double.TryParse(value, out double num))
                    sum += num;
                else
                    message += value;
            }

            Console.WriteLine($"Message: {message}");
            Console.WriteLine($"Total: {sum}");
        }

        // https://learn.microsoft.com/en-us/training/modules/csharp-convert-cast/6-challenge-2
        public static void Divide()
        {
            int value1 = 11;
            decimal value2 = 6.2m;
            float value3 = 4.3f;

            int result1 = Convert.ToInt32(value1 / value2);
            Console.WriteLine($"Divide value1 by value2, display the result as an int: {result1}");

            decimal result2 = value2 / (decimal)value3;
            Console.WriteLine($"Divide value2 by value3, display the result as a decimal: {result2}");

            float result3 = value3 / value1;
            Console.WriteLine($"Divide value3 by value1, display the result as a float: {result3}");
        }
    }
}