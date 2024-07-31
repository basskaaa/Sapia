using Sapia.Game.Combat.Entities;

namespace Sapia.Game.Combat.Steps;

public abstract class ParticipantChoiceStep : ParticipantStep
{
    protected ParticipantChoiceStep(Combat combat, CombatParticipant participant) : base(combat, participant)
    {
    }
}