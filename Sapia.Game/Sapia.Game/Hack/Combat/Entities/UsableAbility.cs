﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sapia.Game.Hack.Types;
using Sapia.Game.Structs.Dice;

namespace Sapia.Game.Hack.Combat.Entities;

public readonly struct UsableAbility
{
    public UsableAbility(AbilityType abilityType, IDiceValue? bonus)
    {
        AbilityType = abilityType;
        Bonus = bonus;
    }

    public AbilityType AbilityType { get; }
    public IDiceValue? Bonus { get; }
}