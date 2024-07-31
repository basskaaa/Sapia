using Sapia.Game.Combat.Entities;

namespace Sapia.Game.Combat.Steps;

public abstract class ParticipantStep : CombatStep
{
    public CombatParticipant Participant { get; }

    protected ParticipantStep(Combat combat, CombatParticipant participant) : base(combat)
    {
        Participant = participant;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: {Participant.ParticipantId} ({Participant.Character.CurrentHealth} / {Participant.Character.Stats.MaxHealth} @{Participant.Position})";
    }
}