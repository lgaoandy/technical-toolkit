using CombatSystem.Interfaces;
using CombatSystem.Models.Characters;
using CombatSystem.Models.Enemies;
using CombatSystem.Enums;

namespace CombatSystem.Managers.CombatManager
{
    public class CombatManager
    {
        public static void Introduce(ICombatant c)
        {
            if (c is Warrior w)
                Console.WriteLine($"{w.Name} is a warrior with {w.Strength} strength and {w.Health} HP.");
            else if (c is Elementalist e)
                Console.WriteLine($"{e.Name} is a {e.Intelligence} intelligence elementalist and {e.Health} HP.");
            else if (c is Monster m)
                Console.WriteLine($"{m.Name} is a monster with {m.Strength} strength and {m.Health} HP.");
            else if (c is Monk k)
                Console.WriteLine($"{k.Name} is a monk with {k.Constitution} constitution and {k.Health} HP.");
            else
                Console.WriteLine($"{c.Name} enters the fray!");
        }

        public static ICombatant Fight(ICombatant c1, ICombatant c2)
        {
            int round = 0;
            ICombatant attacker = c1;
            ICombatant defender = c2;

            // Introduce contestants
            Introduce(c1);
            Introduce(c2);

            while (c1.IsAlive && c2.IsAlive)
            {
                // Announce every 2 rounds
                if (round % 2 == 0)
                    Console.WriteLine($"----- Round {round / 2 + 1} -----");

                // Use special ability if possible
                if (attacker is Warrior warrior && warrior.CanCast())
                    warrior.SpecialAbility(defender);
                else if (attacker is Elementalist elementalist && elementalist.OptimalCast(defender))
                    elementalist.SpecialAbility(defender);
                else if (attacker is Monk monk && monk.CanCast())
                    monk.SpecialAbility(defender);

                // Standard attack
                else
                    attacker.Attack(defender);

                // Announce every exchange
                if (round % 2 == 1)
                {
                    c1.DisplayStatus();
                    c2.DisplayStatus();
                }

                // Swap turns
                (attacker, defender) = (defender, attacker);
                round++;
            }

            // Annouce winner
            Console.WriteLine($"{defender.Name} is victorious with {defender.Health} HP remaining!");
            return defender;
        }
    }
}