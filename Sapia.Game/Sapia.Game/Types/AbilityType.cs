using Sapia.Game.Combat.Entities;
using Sapia.Game.Types.Enums;

namespace Sapia.Game.Types;

public class AbilityType : TypeData
{
    public CombatActionType Action { get; set; } = CombatActionType.Main;
    public TargetType Target { get; set; } = TargetType.Other;

    public int Damage { get; set; } = 1;
}