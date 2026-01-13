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
            Console.WriteLine(schoolName);

            // Randomize animals
            RandomizeAnimals();

            // Assign animals to groups
            string[,] tourGroups = AssignTourGroups(groups);

            // Display tour
            DisplayTours(tourGroups);
        }

        public static void PlanAllSchools()
        {
            PlanSchoolVisit("School A");
            PlanSchoolVisit("School B", 3);
            PlanSchoolVisit("School C", 2);
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

        private static string[,] AssignTourGroups(int groups)
        {
            int animalCount = Animals.Length;
            int animalsPerGroup = (int)Math.Ceiling((double)animalCount / groups);
            string[,] tours = new string[groups, animalsPerGroup];


            // Iterate animals, prioritize assign the same number of animals per group if possible
            int i = 0;
            int j = 0;
            for (int current = 0; current < animalCount; current++)
            {
                tours[i, j] = Animals[current];
                if (i < groups - 1)
                    i++;
                else
                {
                    j++;
                    i = 0;
                }
            }
            return tours;
        }

        private static void DisplayTours(string[,] tourGroups)
        {
            for (int i = 0; i < tourGroups.GetLength(0); i++)
            {
                Console.Write($"Group {i + 1}:");
                for (int j = 0; j < tourGroups.GetLength(1); j++)
                {
                    if (tourGroups[i, j] != null)
                        Console.Write($" {tourGroups[i, j]}");
                }
                Console.WriteLine();
            }
        }
    }
}