using CombatSystem.Interfaces;
using CombatSystem.Enums;

// Elementalist: switch between ice and fire for optimal damage
namespace CombatSystem.Models.Characters
{
    public class Elementalist(string name = "Elementalist", int Intelligence = 3, int maxHealth = 110) : Character
    {
        public new string Name { get; } = name;
        public new int Health { get; private set; } = maxHealth;
        public new int MaxHealth { get; } = maxHealth;
        protected new string SpecialAbilityName { get; } = "Swap Element";

        // Distinct properties
        protected int Intelligence { get; set; } = Intelligence;
        public int Mana = 20 + 5 * Intelligence;
        public int MaxMana = 20 + 5 * Intelligence;
        protected Element[] Elements = [Element.Fire, Element.Ice];
        protected Element CurrentElement = Element.Fire;

        protected override int CalculateAttackDamage()
        {
            return BaseDamage + (int)Math.Floor(Intelligence / 2.0);
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
                    damageElemental = (int)Math.Ceiling(target.Health * 1.08);
                    target.Debuffs.Replace(Debuff.FrostBitten, Debuff.Scorched);
                    RegenMana(3);
                    Console.WriteLine($"{Name} shatters a frostbitten {target.Name} with fire, dealing bonus elemental {damageElemental} damage and regained 3 mana.");
                }
                // If enemy is already scorched, enemy becomes immolated
                else if (target.Debuffs.Contains(Debuff.Scorched))
                {
                    target.Debuffs.Replace(Debuff.Scorched, Debuff.Immolated);
                }
            }
            else
            {
                // If an immolated enemy endures an ice attack, deals 40 explosive damage
                if (target.Debuffs.Contains(Debuff.Immolated))
                {
                    damageElemental = 40;
                    target.Debuffs.Replace(Debuff.Immolated, Debuff.Chilled);
                    Console.WriteLine($"{Name} melts an immolated {target.Name} with ice, dealing bonus explosive {damageElemental} damage!");
                }
                // If enemy is already chilled, upgrades to frostbitten
                else if (target.Debuffs.Contains(Debuff.Chilled))
                {
                    target.Debuffs.Replace(Debuff.Chilled, Debuff.FrostBitten);
                }
            }

            target.TakeDamage(damageBase + damageElemental);

            // Mana regens on enemy defeat
            if (target.Health == 0)
            {
                double regenFactor = 2.0 + 8.4711 * Math.Exp(-0.019076 * Intelligence);
                int manaRegen = (int)Math.Ceiling(target.Health / regenFactor);
                RegenMana(manaRegen);
                Console.WriteLine($"{Name} has slain {target.Name}, regening {manaRegen} mana.");
            }
        }

        // Modify special ability to swap elements
        public override void SpecialAbility(ICombatant target)
        {
            CurrentElement = CurrentElement switch
            {
                Element.Fire => Element.Ice,
                Element.Ice => Element.Fire,
                _ => CurrentElement
            };
        }
        
    }
}