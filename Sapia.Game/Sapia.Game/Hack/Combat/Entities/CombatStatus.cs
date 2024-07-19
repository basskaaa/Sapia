namespace Sapia.Game.Hack.Combat.Entities
{
    public class CombatStatus
    {
        public int RemainingMovement { get; internal set; }
        public IReadOnlyCollection<CombatActionType> RemainingActions { get; internal set; } = Array.Empty<CombatActionType>();
    }

    public enum CombatActionType
    {
        Starter = 1,
        Main = 1,
        Free = 2,
        Reaction = 3
    }
}
