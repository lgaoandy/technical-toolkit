using CombatSystem.Enums;
using CombatSystem.Interfaces;

namespace CombatSystem.Models.Enemies
{
    public class Monster(string name = "Monster", int maxHealth = 50, int strength = 3) : ICombatant
    {
        public string Name { get; } = name;
        public int Health { get; private set; } = maxHealth;
        public int MaxHealth { get; } = maxHealth;
        public bool IsAlive { get; private set; } = true;
        public Debuff[] Debuffs { get; set; } = [];
        public int Strength { get; } = strength;

        public void Attack(ICombatant target)
        {
            Random rnd = new();

            // If frostbitten, 30% chance to freeze. Effect wores off if frozen
            if (Debuffs.Contains(Debuff.FrostBitten))
            {
                int hitChance = rnd.Next(101);
                if (hitChance < 30)
                {
                    Console.WriteLine($"{Name} is frostbitten - freezing in place and unable to attack!");
                    Debuffs = Debuffs.Where(debuff => debuff != Debuff.FrostBitten).ToArray();
                    return;
                }
            }
            
            // Standard attack
            int damage = rnd.Next(1, Strength + 1);
            Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!"); 
            target.TakeDamage(damage);
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;

            // If immolated, burns for 2% of max health
            if (Debuffs.Contains(Debuff.Immolated))
            {
                int burn = (int)Math.Ceiling(MaxHealth * 0.02);
                Health -= burn;
                Console.WriteLine($"{Name} is immolated - burning for {burn} bonus damage!");
            }

            // Check if monster is still alive
            if (Health < 0)
            {
                Health = 0;
                IsAlive = false;
                Console.WriteLine($"{Name} has been slain!");
            }
        }
        
        public void DisplayStatus()
        {
            Console.WriteLine($"{Name}: {Health}/{MaxHealth} HP.");
        }
    }
}