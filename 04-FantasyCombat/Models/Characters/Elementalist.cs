using CombatSystem.Interfaces;

// Elementalist: switch between ice and fire for optimal damage
namespace CombatSystem.Models.Characters
{
    public class Elementalist : Character
    {
        protected new string SpecialAbilityName { get; } = "Swap Element";

        // Distinct properties
        protected int Intelligence { get; set; } = 3;
        public int Mana = 50;
        public int MaxMana = 50;
        protected string[] Elements = ["Ice", "Fire"];
        protected int CurrentElementIndex = 0;

        private string GetCurrentElement()
        {
            return Elements[CurrentElementIndex];
        }

        protected override int CalculateAttackDamage()
        {
            throw new NotImplementedException();
        }

        
    }
}