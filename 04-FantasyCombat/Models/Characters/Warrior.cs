using CombatSystem.Interfaces;

// Warrior: basic attacks charges fervor, which can be used to release devastating attacks
namespace CombatSystem.Models.Characters
{
    public class Warrior(string name = "Warrior", int strength = 3, int maxHealth = 130) : Character
    {
        public override string Name { get; } = name;
        public override int Health { get; set; } = maxHealth;
        public override int MaxHealth { get; } = maxHealth;
        protected override int BaseDamage { get; } = 2;
        protected override string SpecialAbilityName { get; } = "Heaven's Smash";

        public int Strength { get; set; } = strength;
        protected int Fervor { get; private set; } = 0;
        protected int MaxFervor { get; } = 100;

        private void IncreaseFervor()
        {
            Fervor = Math.Min(Fervor + 9 + Strength, MaxFervor);
        }

        public override void Attack(ICombatant target)
        {
            base.Attack(target);
            IncreaseFervor();
        }

        protected override int CalculateAttackDamage()
        {
            Random rnd = new();
            int damage = rnd.Next(BaseDamage, BaseDamage + Strength + 1);
            double fervorMult = 1 + Fervor / 400;
            return (int)Math.Floor(damage * fervorMult);
        }

        public override bool SpecialAbility(ICombatant target)
        {
            if (Fervor == MaxFervor)
            {
                Fervor = 0;
                int damage = (BaseDamage + Strength) * 4;
                target.TakeDamage(damage);
                Console.WriteLine($"{Name} uses {SpecialAbilityName} - dealing a massive {damage} critical hit!");
                return true;
            }
            return false;
        }
    }
}