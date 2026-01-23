using CombatSystem.Interfaces;
using CombatSystem.Models.Characters;
using CombatSystem.Models.Enemies;
using CombatSystem.Enums;

namespace CombatSystem.Managers.CombatManager
{
    public class CombatManager
    {
        public static void Introduce(ICombatant i)
        {
            if (i is Character c)
                Console.WriteLine($"{c.Name} is a {c.Class} with {c.Health} HP.");
            else if (i is Monster m)
                Console.WriteLine($"{m.Name} is a monster with {m.Strength} strength and {m.Health} HP.");
            else
                Console.WriteLine($"{i.Name} enters the fray!");
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
                else if (attacker is Elementalist elementalist && elementalist.CanOptimalCast(defender))
                    elementalist.SpecialAbility(defender);
                else if (attacker is Monk monk && monk.CanOptimalCast())
                    monk.SpecialAbility(defender);
                else // Standard attack
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