using CombatSystem.Enums;

namespace CombatSystem.Interfaces
{
    public interface ICombatant
    {
        string Name { get; }
        int Health { get; }
        int MaxHealth { get; }
        bool IsAlive { get; }
        List<Debuff> Debuffs { get; set; }
        void Attack(ICombatant target);
        void TakeDamage(int amount);
        void DisplayStatus();
    }
}