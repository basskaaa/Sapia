using Sapia.Game.Combat.Entities;

namespace Sapia.Game.Combat.Steps;

public abstract class ParticipantChoiceStep : CombatParticipantStep
{
    protected ParticipantChoiceStep(Combat combat, CombatParticipant participant) : base(combat, participant)
    {
    }
}