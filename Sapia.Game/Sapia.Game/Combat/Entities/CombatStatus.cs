namespace Sapia.Game.Combat.Entities;

public class CombatStatus
{
    public int RemainingMovement { get; internal set; }
    public IReadOnlyCollection<CombatActionType> RemainingActions { get; internal set; } = Array.Empty<CombatActionType>();
}

public enum CombatActionType
{
    Starter = 1,
    Main = 2,
    Free = 3,
    Reaction = 4
}