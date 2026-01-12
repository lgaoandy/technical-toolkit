// Following code is my work for the guided project offered in microsoft (omitting their guidance)
using System.Drawing;
using Pastel;

namespace MiniProjects.DogDonation
{
    class DogDonation
    {
        static string AskUser(string prompt = "")
        {
            string? userResponse;
            while (true)
            {
                Console.Write(prompt);
                userResponse = Console.ReadLine();
                if (userResponse == null || userResponse.Trim().Length == 0)
                    Console.Write("You must enter something...");
                else
                {
                    Console.WriteLine();
                    return userResponse.Trim().ToLower();
                }
            }
        }

        // Reference material: https://learn.microsoft.com/en-us/training/modules/guided-project-work-variable-data-c-sharp/2-prepare
        public static void RunGuided()
        {
            // #1 the ourAnimals array will store the following: 
            string animalSpecies = "";
            string animalID = "";
            string animalAge = "";
            string animalPhysicalDescription = "";
            string animalPersonalityDescription = "";
            string animalNickname = "";
            string suggestedDonation = "";

            // #2 variables that support data entry
            int maxPets = 8;
            string? readResult;
            string menuSelection = "";
            decimal decimalDonation = 0.00m;

            // #3 array used to store runtime data, there is no persisted data
            string[,] ourAnimals = new string[maxPets, 7];

            // #4 create sample data ourAnimals array entries
            for (int i = 0; i < maxPets; i++)
            {
                switch (i)
                {
                    case 0:
                        animalSpecies = "dog";
                        animalID = "d1";
                        animalAge = "2";
                        animalPhysicalDescription = "medium sized cream colored female golden retriever weighing about 45 pounds. housebroken.";
                        animalPersonalityDescription = "loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.";
                        animalNickname = "lola";
                        suggestedDonation = "85.00";
                        break;

                    case 1:
                        animalSpecies = "dog";
                        animalID = "d2";
                        animalAge = "9";
                        animalPhysicalDescription = "large reddish-brown male golden retriever weighing about 85 pounds. housebroken.";
                        animalPersonalityDescription = "loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.";
                        animalNickname = "gus";
                        suggestedDonation = "49.99";
                        break;

                    case 2:
                        animalSpecies = "cat";
                        animalID = "c3";
                        animalAge = "1";
                        animalPhysicalDescription = "small white female weighing about 8 pounds. litter box trained.";
                        animalPersonalityDescription = "friendly";
                        animalNickname = "snow";
                        suggestedDonation = "40.00";
                        break;

                    case 3:
                        animalSpecies = "cat";
                        animalID = "c4";
                        animalAge = "3";
                        animalPhysicalDescription = "Medium sized, long hair, yellow, female, about 10 pounds. Uses litter box.";
                        animalPersonalityDescription = "A people loving cat that likes to sit on your lap.";
                        animalNickname = "Lion";
                        suggestedDonation = "";
                        break;

                    default:
                        animalSpecies = "";
                        animalID = "";
                        animalAge = "";
                        animalPhysicalDescription = "";
                        animalPersonalityDescription = "";
                        animalNickname = "";
                        suggestedDonation = "";
                        break;

                }

                if (!decimal.TryParse(suggestedDonation, out decimalDonation))
                {
                    decimalDonation = 45.00m; // if suggestedDonation NOT a number, default to 45.00
                }

                ourAnimals[i, 0] = "ID #: " + animalID;
                ourAnimals[i, 1] = "Species: " + animalSpecies;
                ourAnimals[i, 2] = "Age: " + animalAge;
                ourAnimals[i, 3] = "Nickname: " + animalNickname;
                ourAnimals[i, 4] = "Physical description: " + animalPhysicalDescription;
                ourAnimals[i, 5] = "Personality: " + animalPersonalityDescription;
                ourAnimals[i, 6] = $"Suggested Donation: {decimalDonation:C2}";

            }

            // #5 display the top-level menu options
            do
            {
                // NOTE: the Console.Clear method is throwing an exception in debug sessions
                Console.WriteLine("Welcome to the Contoso PetFriends app. Your main menu options are:");
                Console.WriteLine(" 1. List all of our current pet information");
                Console.WriteLine(" 2. Display all dogs with a specified characteristic");
                Console.WriteLine();
                Console.WriteLine("Enter your selection number (or type Exit to exit the program)");

                readResult = Console.ReadLine();
                if (readResult != null)
                {
                    menuSelection = readResult.ToLower();
                }

                // use switch-case to process the selected menu option
                switch (menuSelection)
                {
                    case "1":
                        // list all pet info
                        for (int i = 0; i < maxPets; i++)
                        {
                            if (ourAnimals[i, 0] != "ID #: ")
                            {
                                Console.WriteLine();
                                for (int j = 0; j < 7; j++)
                                {
                                    Console.WriteLine(ourAnimals[i, j]);
                                }
                            }
                        }
                        Console.WriteLine("\n\rPress the Enter key to continue");
                        readResult = Console.ReadLine();

                        break;

                    case "2":
                        // Gather user input for the pet characteristic search
                        readResult = AskUser("Please enter what characteristics you're looking for: ");

                        // Loop through animals and identify dogs
                        string[] matchingIds = new string[8];
                        int m = 0;
                        for (int i = 0; i < maxPets; i++)
                        {
                            if (ourAnimals[i, 1].Contains("dog"))
                            {
                                // Search each dog's pet description for a term match
                                string dogDescription = ourAnimals[i, 4].ToLower() + " " + ourAnimals[i, 5].ToLower();
                                if (dogDescription.Contains(readResult))
                                {
                                    matchingIds[m] = ourAnimals[i, 0];
                                    m++;
                                }
                            }
                        }

                        // Display the dogs that matches
                        if (m > 0)
                            for (int i = 0; i < m; i++)
                            {
                                for (int j = 0; j < maxPets; j++)
                                {
                                    if (ourAnimals[j, 0] == matchingIds[i])
                                    {
                                        for (int k = 0; k < 7; k++)
                                        {
                                            Console.WriteLine(ourAnimals[j, k]);
                                        }
                                        Console.WriteLine();
                                    }
                                }
                            }
                        else
                            Console.WriteLine("There are no matching animal with your characteristic.");
                        Console.WriteLine("\n\rPress any key to continue.");
                        Console.ReadKey();

                        break;

                    default:
                        break;
                }

            } while (menuSelection != "exit");

        }

        // Add overloaded method to force user to choice an acceptable answer
        static string AskUser(string[] answers, string prompt = "")
        {
            string? userResponse;
            while (true)
            {
                Console.Write(prompt);
                userResponse = Console.ReadLine();

                if (userResponse == null || userResponse.Trim().Length == 0)
                {
                    Console.Write("You must enter something... ");
                    continue;
                }

                userResponse = userResponse.Trim().ToLower();
                if (answers.Contains(userResponse))
                    return userResponse;
                else
                    Console.Write("Your response is not an available option. Try again... ");
            }
        }

        static void PressAnyKeyToContinue()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            Console.WriteLine();
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\n" + " MAIN MENU OPTIONS ".PadLeft(24, '-').PadRight(36, '-'));
            Console.WriteLine("1.".PadLeft(4).PadRight(6) + "List all our current pet information");
            Console.WriteLine("2.".PadLeft(4).PadRight(6) + "Search dogs by characteristic");
            Console.WriteLine("3.".PadLeft(4).PadRight(6) + "Enter (3 or exit) to leave program");
        }

        static void SearchAnimation(string dogName, string trait)
        {
            string[] icons = [".", "..", "..."];
            foreach (string icon in icons)
            {
                Console.Write($"\rSearching our dog {dogName} for {trait}{icon} ");
                Thread.Sleep(200);
            }
            Console.Write($"\r{new String(' ', Console.BufferWidth)}"); // Clears the current terminal tab
        }

        static void DisplayDog(string[,] dogs, int i)
        {
            Console.WriteLine($"{dogs[i, 1]} (ID# {dogs[i, 0]})");
            Console.WriteLine($"{dogs[i, 2]} years old");
            Console.WriteLine($"Physical Description: {dogs[i, 3]}");
            Console.WriteLine($"Personality: {dogs[i, 4]}");
            Console.WriteLine($"Fee: {dogs[i, 5]}\n");
        }

        static void DisplayDogs(string[,] dogs)
        {
            Console.WriteLine();
            for (int i = 0; i < dogs.GetLength(0); i++)
                DisplayDog(dogs, i);
            PressAnyKeyToContinue();
        }

        static void SearchForTraits(string[,] dogs)
        {
            // Prompt user for search in comma separated list
            string userResponse = AskUser("\nIn a comma-separated list, Enter the traits you are seeking: ");
            string[] traits = userResponse.Split(',');
            int n = traits.Length;

            // Trim and lowercase all traits
            for (int i = 0; i < n; i++)
                traits[i] = traits[i].Trim().ToLower();

            int count = 0;
            for (int i = 0; i < dogs.GetLength(0); i++)
            {
                bool match = false;
                for (int j = 0; j < n; j++) // Checks if any traits match current dog
                {
                    string dogDescription = dogs[i, 3].Trim().ToLower() + " " + dogs[i, 4].Trim().ToLower();
                    SearchAnimation(dogs[i, 1], traits[j]);
                    if (dogDescription.Contains(traits[j]))
                    {
                        Console.WriteLine($"\rOur dog {dogs[i, 1]} is a {traits[j].Pastel(Color.Gold)} match!");
                        match = true;
                    }
                }

                if (match) // If match, displays their traits
                {
                    count++;
                    Console.WriteLine();
                    DisplayDog(dogs, i);
                }
            }

            if (count == 0)
                Console.WriteLine("There are no dogs with your characteristics!");
            PressAnyKeyToContinue();
        }


        // Reference material: https://learn.microsoft.com/en-us/training/modules/challenge-project-work-variable-data-c-sharp/2-prepare
        public static void RunChallenge()
        {
            // Rewritting starter code - start with initializing animals at declaration
            string[,] ourDogs = new string[,]
            {
                {
                    "d1",
                    "Lola",
                    "2",
                    "Medium sized cream colored female golden retriever weighing about 45 pounds. housebroken.",
                    "Loves to have her belly rubbed and likes to chase her tail. gives lots of kisses.",
                    "$85.00"
                },
                {
                    "d2",
                    "Gus",
                    "9",
                    "Large reddish-brown male golden retriever weighing about 85 pounds. housebroken.",
                    "Loves to have his ears rubbed when he greets you at the door, or at any time! loves to lean-in and give doggy hugs.",
                    "$49.99"
                },
            };

            string menuSelection;
            Console.WriteLine("Welcome to the Contoso PetFriends app!");

            while (true)
            {
                DisplayMainMenu();
                menuSelection = AskUser(["1", "2", "3", "exit"], "Enter your selection: ");

                switch (menuSelection)
                {
                    case "1":
                        DisplayDogs(ourDogs);
                        break;
                    case "2":
                        SearchForTraits(ourDogs);
                        break;
                    case "3":
                    case "exit":
                        return;
                }
            }
        }
    }
}