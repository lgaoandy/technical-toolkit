using System;
using System.Collections.Generic;

public class ToDoList
{
    private static int capacity = 10;
    private static string[] items = new string[capacity];
    private static int[] completed = new int[capacity];
    private static int length = 0;

    private static void DisplayItems()
    {
        if (length == 0)
        {
            Console.WriteLine("Your list is empty");
            return;
        }
        Console.WriteLine("Your Items are:");
        for (int i = 0; i < length; i++)
        {
            Console.WriteLine((completed[i] == 0 ? "[ ] "  : "[x] ") + "(" + (i + 1) + ") " + items[i]);
        }
    }

    public static void Start()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.WriteLine("\nActions:");
            Console.WriteLine("(1) View Items");
            Console.WriteLine("(2) Add Item");
            Console.WriteLine("(3) Mark Item As Completed");
            Console.WriteLine("(4) Exit");
            Console.WriteLine("\nChoose:");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n--- View Items ---");
                    DisplayItems();
                    break;

                case "2":
                    Console.WriteLine("\n--- Add Item ---");
                    Console.WriteLine("Add Item:");
                    if (length == capacity)
                    {
                        Console.WriteLine("Your list is full");
                    }
                    else
                    {
                        string? item = Console.ReadLine();
                        if (string.IsNullOrEmpty(item))
                        {
                            Console.WriteLine("Item must not be empty, try again.");
                            break;
                        }

                        items[length] = item;
                        completed[length] = 0;
                        length++;
                    }
                    break;
                case "3":
                    Console.WriteLine("\n--- Mark Item as Completed ---");
                    DisplayItems();
                    Console.WriteLine("Which item to complete:");

                    string? input = Console.ReadLine();
                    if (int.TryParse(input, out int num) && num >= 1 && num <= length + 1)
                    {
                        completed[num - 1] = 1;
                    }
                    else
                    {
                        Console.WriteLine("Valid number, try again.");
                    }
                    break;
                case "4":
                    Console.WriteLine("Exiting...");
                    isRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }

}
