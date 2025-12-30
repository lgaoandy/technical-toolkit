using CombatSystem.Interfaces;

// Warrior: basic attacks charges fervor, which can be used to release devastating attacks
namespace CombatSystem.Models.Characters
{
    public class Warrior : Character
    {
        protected new int BaseDamage { get; } = 2;
        protected new string SpecialAbilityName { get; } = "Earth Smash";

        protected int Strength { get; set; } = 3;
        protected int Fervor { get; private set; } = 0;
        protected int MaxFervor { get; } = 100;

        private void IncreaseFervor()
        {
            Fervor = Math.Min(Fervor + 12, MaxFervor);
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

        public override void SpecialAbility(ICombatant target)
        {
            if (Fervor == MaxFervor)
            {
                Fervor = 0;
                int damage = (BaseDamage + Strength) * 4;
                target.TakeDamage(damage);
                Console.WriteLine($"{Name} uses {SpecialAbilityName} - dealing a massive {damage} critical hit!");
            }
            else
            {
                Console.WriteLine($"Cannot use {SpecialAbilityName} - not enough fervor!");
            }
        }
    }
}