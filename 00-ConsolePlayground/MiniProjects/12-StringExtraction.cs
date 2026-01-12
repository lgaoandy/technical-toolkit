namespace MiniProjects.StringExtraction
{
    class StringExtraction
    {
        static string ExtractBoundaries(string input, string start, string end)
        {
            int indexStart = input.IndexOf(start) + start.Length;
            int indexEnd = input.IndexOf(end);
            return input[indexStart..indexEnd];
        }

        // https://learn.microsoft.com/en-us/training/modules/csharp-modify-content/5-exercise-challenge-extract-replace-remove-data        
        public static void ExtractHTMLExercise()
        {
            const string input = "<div><h2>Widgets &trade;</h2><span>5000</span></div>";

            // Set quantity to the value in between <span> and </span> tags
            string quantity = ExtractBoundaries(input, "<span>", "</span>");

            // Set output to the value in between <div> and </div> tags;
            string output = ExtractBoundaries(input, "<div>", "</div>");

            // Replace HTML character
            output = output.Replace("&trade;", "&reg;");

            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Output: {output}");
        }
    }
}