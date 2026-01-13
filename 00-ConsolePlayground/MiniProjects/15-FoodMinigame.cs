// Reference material: https://learn.microsoft.com/en-us/training/modules/challenge-project-create-mini-game/2-prepare
namespace MiniProjects.FoodMinigame
{
    class FoodMinigame
    {
        public static void Run()
        {
            Random random = new Random();
            Console.CursorVisible = false;
            int height = Console.WindowHeight - 1;
            int width = Console.WindowWidth - 5;
            bool shouldExit = false;

            // Console position of the player
            int playerX = 0;
            int playerY = 2;

            // Console position of the food
            int foodX = 0;
            int foodY = 0;

            // Available player and food strings
            string[] states = ["('-')", "(^-^)", "(X_X)"];
            string[] foods = ["🍙", "🍣", "🍕"];

            // Current player string displayed in the Console
            string player = states[0];

            // Index of the current food
            int food = 0;

            InitializeGame();
            while (!shouldExit)
            {
                if (TerminalResized())
                {
                    Console.Clear();
                    Console.Write("Console was resized. Program exiting...");
                    break;
                }

                if (PlayerIsFaster())
                    Move(3, false);
                else if (PlayerIsSick())
                    FreezePlayer();
                else
                    Move(otherKeysExit: false);

                if (GotFood())
                {
                    ChangePlayer();
                    ShowFood();
                }
            }

            bool PlayerIsFaster()
            {
                return player.Equals(states[1]);
            }

            bool PlayerIsSick()
            {
                return player.Equals(states[2]);
            }

            // Returns true if the Terminal was resized 
            bool TerminalResized()
            {
                return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
            }

            // Displays random food at a random location
            void ShowFood()
            {
                do
                {
                    // Update food position to a random location
                    foodX = random.Next(0, width - 1);
                    foodY = random.Next(0, height - 1);
                } while (GotFood());

                // Update food to a random index
                food = random.Next(0, foods.Length);

                // Display the food at the location
                Console.SetCursorPosition(foodX, foodY);
                Console.Write(foods[food]);
            }

            bool GotFood()
            {
                return playerX - 1 <= foodX && playerX + 4 >= foodX && playerY == foodY;
            }

            // Changes the player to match the food consumed
            void ChangePlayer()
            {
                player = states[food];
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(player);
            }

            // Temporarily stops the player from moving
            void FreezePlayer()
            {
                Thread.Sleep(1000);
                player = states[0];
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(player);
            }

            // Reads directional input from the Console and moves the player
            void Move(int speed = 1, bool otherKeysExit = false)
            {
                int lastX = playerX;
                int lastY = playerY;

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        playerY--;
                        break;
                    case ConsoleKey.DownArrow:
                        playerY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        playerX -= speed;
                        break;
                    case ConsoleKey.RightArrow:
                        playerX += speed;
                        break;
                    case ConsoleKey.Escape:
                        shouldExit = true;
                        break;
                    default:
                        shouldExit = otherKeysExit;
                        break;
                }

                // Clear the characters at the previous position
                Console.SetCursorPosition(lastX, lastY);
                for (int i = 0; i < player.Length; i++)
                {
                    Console.Write(" ");
                }

                // Keep player position within the bounds of the Terminal window
                playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
                playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

                // Draw the player at the new location
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(player);
            }

            // Clears the console, displays the food and player
            void InitializeGame()
            {
                Console.Clear();
                ShowFood();
                Console.SetCursorPosition(playerX, playerY);
                Console.Write(player);
            }
        }
    }
}