using CombatSystem.Interfaces;
using CombatSystem.Enums;

namespace CombatSystem.Models.Characters
{
    public abstract class Character(string name = "Character", int maxHealth = 100) : ICombatant
    {
        public string Name { get; } = name;
        public int Health { get; private set; } = maxHealth;
        public int MaxHealth { get; } = maxHealth;
        public bool IsAlive { get; private set; } = true;
        public Debuff[] Debuffs { get; } = [];
        protected virtual int BaseDamage { get; } = 1;
        protected virtual string SpecialAbilityName { get; } = "Double Strike";

        // Common Methods
        protected abstract int CalculateAttackDamage();

        public virtual void Attack(ICombatant target)
        {
            int damage = CalculateAttackDamage();
            target.TakeDamage(damage);
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            // Check if character is still alive
            if (Health < 0)
            {
                IsAlive = false;
                Console.WriteLine($"{Name} has been slain!");
            }
            else
            {
                Console.WriteLine($"{Name} has substained damage with {Health} HP left");
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