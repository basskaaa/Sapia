using System;
using System.Collections.Generic;
using System.Text;
using Sapia.Game.Combat.Entities;

namespace Sapia.Game.Combat.Steps;

public class AbilityUsedStep : ParticipantStep
{
    public AbilityResult Result { get; }

    public AbilityUsedStep(Combat combat, CombatParticipant participant, AbilityResult result) : base(combat, participant)
    {
        Result = result;
    }

    public override string ToString()
    {
        return $"{base.ToString()} - {Result.Ability.Id}";
    }
}