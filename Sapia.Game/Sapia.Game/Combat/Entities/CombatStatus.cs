using Sapia.Game.Combat.Entities.Enums;

namespace Sapia.Game.Combat.Entities;

public class CombatStatus
{
    public int RemainingMovement { get; internal set; }
    public IReadOnlyCollection<CombatActionType> RemainingActions { get; internal set; } = Array.Empty<CombatActionType>();
}