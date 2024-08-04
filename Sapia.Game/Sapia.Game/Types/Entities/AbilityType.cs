﻿using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Types.Enums;

namespace Sapia.Game.Types.Entities;

public class AbilityType : TypeDataWithDescription
{
    public CombatActionType Action { get; set; } = CombatActionType.Main;
    public TargetType Target { get; set; } = TargetType.Other;

    // Only used when Target = TargetType.Other
    public bool CanTargetSelf { get; set; } = false;

    public int Damage { get; set; } = 1;

    public int Range { get; set; } = 1;

    public int Rank { get; set; } = 0;
}