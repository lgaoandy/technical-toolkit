using System.Drawing;
using CombatSystem.Interfaces;
using Pastel;

namespace CombatSystem.Models.Characters
{
    public class Monk(string name, int constitution, int maxHealth) : Character
    {
        public override string Name { get; } = name;
        public override int Health { get; set; } = maxHealth;
        public override int MaxHealth { get; } = maxHealth;
        protected override int BaseDamage { get; } = 2;
        protected override string SpecialAbilityName { get; } = "Radiant Sunder";
        protected int StaminaMeter { get; private set; } = 0;
        public int Constitution = constitution;

        public override void TakeDamage(int amount)
        {
            StaminaMeter++;
            base.TakeDamage(amount);
        }

        protected override int CalculateAttackDamage()
        {
            Random rnd = new();

            // Formulas for max and min hit
            int minHit = BaseDamage + (int)Math.Floor(Constitution / 2.5);
            int maxHit = 2 + (int)Math.Ceiling(Constitution * 0.85);

            // Calculate damage and crit chance
            int dmg = rnd.Next(minHit, maxHit);
            int critChance = rnd.Next(0, 101) / 100;

            // Increase hit counter
            StaminaMeter += 2;

            // Output damage
            if (critChance > 0.90)
            {
                StaminaMeter += 3;
                return dmg * 2;
            }
            return dmg;
        }

        public override void SpecialAbility(ICombatant target)
        {
            if (CanCast())
            {
                int damage = CalculateAttackDamage() + (int)Math.Round(Constitution * 1.25);
                target.TakeDamage(damage);
                StaminaMeter -= 10;
                Health = Math.Min(MaxHealth, Health + Constitution);
                Console.WriteLine($"{Name} uses {SpecialAbilityName.Pastel(Color.Yellow)} on {target.Name} for {damage} damage, and healing {Constitution} HP!");
            }
        }

        public bool CanCast()
        {
            return StaminaMeter >= 10;
        }
    }
}