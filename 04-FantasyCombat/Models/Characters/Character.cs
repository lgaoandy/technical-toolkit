using CombatSystem.Interfaces;
using CombatSystem.Enums;

namespace CombatSystem.Models.Characters
{
    public abstract class Character(string name = "Character", int maxHealth = 100) : ICombatant
    {
        public virtual string Name { get; } = name;
        public virtual int Health { get; set; } = maxHealth;
        public virtual int MaxHealth { get; } = maxHealth;
        public virtual bool IsAlive { get; private set; } = true;
        public List<Debuff> Debuffs { get; set; } = [];
        protected virtual int BaseDamage { get; } = 1;
        protected virtual string SpecialAbilityName { get; } = "Double Strike";

        // Common Methods
        protected abstract int CalculateAttackDamage();

        public virtual void Attack(ICombatant target)
        {
            int damage = CalculateAttackDamage();
            Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!");
            target.TakeDamage(damage);
        }

        public virtual void TakeDamage(int amount)
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
            if (Health <= 0)
            {
                Health = 0;
                IsAlive = false;
                Console.WriteLine($"{Name} has been slain!");
            }
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"{Name}: {Health}/{MaxHealth} HP");
        }

        public virtual void SpecialAbility(ICombatant target)
        {
            int damage = CalculateAttackDamage();
            target.TakeDamage(damage * 2);
            Console.WriteLine($"{Name} uses {SpecialAbilityName} on {target.Name} for {damage} damage!");
        }
    }
}