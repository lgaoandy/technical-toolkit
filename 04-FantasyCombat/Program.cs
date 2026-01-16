using System.Drawing;
using CombatSystem.Interfaces;
using CombatSystem.Managers.CombatManager;
using CombatSystem.Models.Characters;
using CombatSystem.Models.Enemies;
using Pastel;

namespace CombatSystem.Program
{
    class Program
    {
        static void Main()
        {
            Warrior gunnar = new("Gunnar", 3, 140);
            Monk collin = new("Collin", 7, 110);
            Elementalist terra = new("Terra", 12, 100);
            Warrior doyum = new("Doyum", 5, 210);

            Monster scarecrow = new("Scarecrow", 30, 1);
            Monster giantRat = new("Giant Rat", 30, 3);
            Monster bigBadWolf = new("Big Bad Wolf", 40, 7);
            Monster sewerWorm = new("Sewer Worm", 70, 5);

            Console.WriteLine("\n===== Match 1 =====".Pastel(Color.White));
            ICombatant winner1 = CombatManager.Fight(gunnar, scarecrow);

            Console.WriteLine("\n===== Match 2 =====".Pastel(Color.White));
            ICombatant winner2 = CombatManager.Fight(collin, giantRat);

            Console.WriteLine("\n===== Match 3 =====".Pastel(Color.White));
            ICombatant winner3 = CombatManager.Fight(terra, bigBadWolf);

            Console.WriteLine("\n===== Match 4 =====".Pastel(Color.White));
            ICombatant winner4 = CombatManager.Fight(doyum, sewerWorm);

            Console.WriteLine("\n===== Match 5 =====".Pastel(Color.White));
            ICombatant winner5 = CombatManager.Fight(winner1, winner2);

            Console.WriteLine("\n===== Match 6 =====".Pastel(Color.White));
            ICombatant winner6 = CombatManager.Fight(winner3, winner4);

            Console.WriteLine("\n===== Final Match =====".Pastel(Color.White));
            CombatManager.Fight(winner5, winner6);
        }
    }
}