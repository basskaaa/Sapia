using Sapia.Game.Hack.Combat.Entities;
using Sapia.Game.Hack.Types.Enums;

namespace Sapia.Game.Hack.Types;

public class AbilityType : TypeData
{
    public CombatActionType Action { get; set; } = CombatActionType.Main;
    public TargetType Target { get; set; } = TargetType.Other;
}