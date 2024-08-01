using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Entities.Enums;

namespace Sapia.Game.Combat.Steps;

public class AbilityFailedStep : ParticipantStep
{
    public AbilityUse Use { get; }
    public AbilityFailureReason Reason { get; }

    public AbilityFailedStep(Combat combat, CombatParticipant participant, AbilityUse use, AbilityFailureReason reason) : base(combat, participant)
    {
        Use = use;
        Reason = reason;
    }

    public override string ToString()
    {
        return $"{base.ToString()} - {Use.AbilityId}: {Reason}";
    }
}