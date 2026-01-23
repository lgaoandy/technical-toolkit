using CombatSystem.Interfaces;
using CombatSystem.Enums;
using System.Diagnostics;
using System.Drawing;
using Pastel;

// Elementalist: switch between ice and fire for optimal damage
namespace CombatSystem.Models.Characters
{
    public class Elementalist(string name = "Elementalist", int intelligence = 3, int maxHealth = 80) : Character
    {
        public override string Name { get; } = name;
        public override string Class { get; } = "Elementalist";
        public override string CoreAttribute { get; } = "Intelligence";
        public override int Health { get; set; } = maxHealth;
        public override int MaxHealth { get; } = maxHealth;
        protected override string SpecialAbilityName { get; } = "Elemental Visage";

        // Distinct properties
        public int Intelligence { get; set; } = intelligence;
        public int Mana = 20 + 5 * intelligence;
        public int MaxMana = 20 + 5 * intelligence;
        protected Element[] Elements = [Element.Fire, Element.Ice];
        public Element CurrentElement { get; private set; } = Element.Fire;
        protected bool ManaShield = false;
        protected int ManaShieldStrength = 4 + (int)Math.Floor(intelligence * 0.43);

        protected override int CalculateAttackDamage()
        {
            return BaseDamage + (int)Math.Floor(Intelligence / 3.0);
        }

        public override void TakeDamage(int amount)
        {
            // If have mana shield, absorbs amount first
            if (ManaShield)
            {
                Console.WriteLine($"{Name}'s mana shield absorbs {Math.Min(amount, ManaShieldStrength)} damage!");
                amount -= ManaShieldStrength;
                ManaShield = false;
            }
            if (amount > 0)
            {
                base.TakeDamage(amount);
            }
        }

        protected void RegenMana(int manaRegen)
        {
            Mana = Math.Min(MaxMana, Mana + manaRegen);
        }

        private int InflictIceShatter(ICombatant target)
        {
            int damage = (int)Math.Ceiling(target.MaxHealth * 0.08);
            RegenMana(3);
            target.Debuffs.Remove(Debuff.FrostBitten);
            Console.WriteLine($"{Name} shatters a frostbitten {target.Name} with fire, dealing bonus elemental {damage} damage and regained 3 mana.");
            return damage;
        }

        private int InflictExplosion(ICombatant target)
        {
            int damage = 20 + (int)Math.Floor(Intelligence * 1.8);
            target.Debuffs.Remove(Debuff.Immolated);
            Console.WriteLine($"{Name} melts an immolated {target.Name} with ice, dealing bonus explosive {damage} damage!");
            return damage;
        }

        // Modify attack to add elemental effects and mana regen on enemy death
        public override void Attack(ICombatant target)
        {
            int damage = 0;
            if (CurrentElement == Element.Fire)
            {
                if (target.Debuffs.Contains(Debuff.FrostBitten))
                    damage = InflictIceShatter(target);
                else if (target.Debuffs.Remove(Debuff.Scorched))
                    target.Debuffs.Add(Debuff.Immolated);
                else
                    target.Debuffs.Add(Debuff.Scorched);
            }
            else
            {
                if (target.Debuffs.Contains(Debuff.Immolated))
                    damage = InflictExplosion(target);
                else if (target.Debuffs.Remove(Debuff.Chilled))
                    target.Debuffs.Add(Debuff.FrostBitten);
                else
                    target.Debuffs.Add(Debuff.Chilled);
            }

            if (damage == 0)
            {
                damage = CalculateAttackDamage();
                Console.WriteLine($"{Name} attacks {target.Name} for {damage} damage!");
            }
            target.TakeDamage(damage);

            // Mana regens on enemy defeat
            if (target.Health == 0)
            {
                double regenFactor = 2.0 + 8.4711 * Math.Exp(-0.019076 * Intelligence);
                int manaRegen = (int)Math.Ceiling(target.MaxHealth / regenFactor);
                Console.WriteLine($"{Name} has slain {target.Name}, regening {manaRegen} mana.");
                RegenMana(manaRegen);
            }
        }

        // Modify special ability to swap elements
        public override void SpecialAbility(ICombatant target)
        {
            if (CanCast())
            {
                Mana -= 8;
                ManaShield = true;
                CurrentElement = CurrentElement switch
                {
                    Element.Fire => Element.Ice,
                    Element.Ice => Element.Fire,
                    _ => CurrentElement
                };
                target.TakeDamage(0);
                Console.WriteLine($"{Name} uses {SpecialAbilityName.Pastel(Color.Blue)} - gaining mana shield and swapping to {CurrentElement}!");
            }
        }

        public bool CanCast()
        {
            return Mana >= 8;
        }

        public bool CanOptimalCast(ICombatant target)
        {
            if (!CanCast())
                return false;
            if (target.Debuffs.Contains(Debuff.FrostBitten) && CurrentElement == Element.Ice)
                return true;
            if (target.Debuffs.Contains(Debuff.Immolated) && CurrentElement == Element.Fire)
                return true;
            return false;
        }
    }
}