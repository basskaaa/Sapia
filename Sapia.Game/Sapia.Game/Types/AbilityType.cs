using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Types.Enums;

namespace Sapia.Game.Types;

public class AbilityType : TypeDataWithDescription
{
    public CombatActionType Action { get; set; } = CombatActionType.Main;
    public TargetType Target { get; set; } = TargetType.Other;

    public int Damage { get; set; } = 1;
}