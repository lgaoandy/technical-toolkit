namespace MiniProjects.ArrayManipulation
{
    class ArrayManipulation
    {
        // https://learn.microsoft.com/en-us/training/modules/csharp-arrays-operations/5-challenge-1
        public static string ReverseWords(string message)
        {
            string[] words = message.Split(" ");
            string result = "";

            // Reverse each word in the message
            foreach (string word in words)
            {
                char[] chars = word.ToCharArray();
                Array.Reverse(chars);
                result += String.Join("", chars) + " ";
            }

            Console.WriteLine(result);
            return result;
        }

        // https://learn.microsoft.com/en-us/training/modules/csharp-arrays-operations/7-challenge-2
        public static void SortId(string stream)
        {
            // Assuming input is comma separated
            string[] ids = stream.Split(",");

            // Sort array
            Array.Sort(ids);

            // Check if IDs are valid (4 letter length)
            foreach (string id in ids)
            {
                Console.Write(id);
                if (id.Length != 4)
                    Console.WriteLine($"\t- Error");
                else
                    Console.WriteLine();
            }
        }
    }
}