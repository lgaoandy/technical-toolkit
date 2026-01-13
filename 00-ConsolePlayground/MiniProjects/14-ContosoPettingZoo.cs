namespace MiniProjects.ContosoPettingZoo
{
    class ContosoPettingZoo
    {
        private static string[] Animals { get; set; } = [
            "alpacas", "capybaras", "chickens", "ducks", "emus", "geese",
            "goats", "iguanas", "kangaroos", "lemurs", "llamas", "macaws",
            "ostriches", "pigs", "ponies", "rabbits", "sheep", "tortoises"
        ];

        // Reference material: https://learn.microsoft.com/en-us/training/modules/guided-project-visit-petting-zoo/
        public static void PlanSchoolVisit(string schoolName, int groups = 6)
        {
            // Randomize animals
            RandomizeAnimals();
            DisplayAnimals();
        }

        // Using the modern version of Fisher-Yates algorithm
        private static void RandomizeAnimals()
        {
            int n = Animals.Length;
            Random rnd = new();

            // Iterate starting with first index, swap index elements by the remaining indexes
            for (int i = 0; i < n; i++)
            {
                int j = rnd.Next(i, n);
                (Animals[i], Animals[j]) = (Animals[j], Animals[i]);
            }
        }

        private static void DisplayAnimals()
        {
            Console.WriteLine(String.Join(", ", Animals));
        }
    }
}