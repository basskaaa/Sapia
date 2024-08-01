using System;
using System.Collections.Generic;
using System.Text;

namespace Sapia.Game.Combat.Entities.Enums
{
    public enum AbilityFailureReason
    {
        Unknown,
        InvalidAbility,
        InsufficientUsesRemaining,
        InsufficientActions ,
        TargetOutOfRange  ,
        UnableToTargetSelf ,
        NoTarget
    }
}
