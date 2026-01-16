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

        // Modify attack to add elemental effects and mana regen on enemy death
        public override void Attack(ICombatant target)
        {
            int damageBase = CalculateAttackDamage();
            int damageElemental = 0;

            if (CurrentElement == Element.Fire)
            {
                // If a frostbitten enemy endures a fire attack, deals 8% of their health as bonus elemental damage and regens 3 mana
                if (target.Debuffs.Contains(Debuff.FrostBitten))
                {
                    damageElemental = (int)Math.Ceiling(target.MaxHealth * 0.08);
                    target.Debuffs.Remove(Debuff.FrostBitten);
                    RegenMana(3);
                    Console.WriteLine($"{Name} shatters a frostbitten {target.Name} with fire, dealing bonus elemental {damageElemental} damage and regained 3 mana.");
                }

                // If enemy is already scorched, enemy becomes immolated
                if (target.Debuffs.Remove(Debuff.Scorched))
                    target.Debuffs.Add(Debuff.Immolated);
                else
                    target.Debuffs.Add(Debuff.Scorched);
            }
            else
            {
                // If an immolated enemy endures an ice attack, deals explosive damage
                if (target.Debuffs.Contains(Debuff.Immolated))
                {
                    damageElemental = 20 + (int)Math.Floor(Intelligence * 1.5);
                    target.Debuffs.Remove(Debuff.Immolated);
                    Console.WriteLine($"{Name} melts an immolated {target.Name} with ice, dealing bonus explosive {damageElemental} damage!");
                }

                // If enemy is already chilled, upgrades to frostbitten
                if (target.Debuffs.Remove(Debuff.Chilled))
                    target.Debuffs.Add(Debuff.FrostBitten);
                else
                    target.Debuffs.Add(Debuff.Chilled);
            }

            Console.WriteLine($"{Name} attacks {target.Name} for {damageBase} damage!");
            target.TakeDamage(damageBase + damageElemental);

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
                Console.WriteLine($"{Name} uses {SpecialAbilityName.Pastel(Color.Blue)} - gaining mana shield and swapping to {CurrentElement}!");
            }
        }

        public bool CanCast()
        {
            return Mana >= 8;
        }

        public bool OptimalCast(ICombatant target)
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