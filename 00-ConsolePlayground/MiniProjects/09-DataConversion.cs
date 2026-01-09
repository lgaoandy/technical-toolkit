// https://learn.microsoft.com/en-us/training/modules/csharp-convert-cast/4-challenge

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
    }
}