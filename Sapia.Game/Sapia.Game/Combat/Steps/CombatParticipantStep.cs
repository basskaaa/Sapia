using Sapia.Game.Combat.Entities;

namespace Sapia.Game.Combat.Steps;

public abstract class CombatParticipantStep : CombatStep
{
    public CombatParticipant Participant { get; }

    protected CombatParticipantStep(Combat combat, CombatParticipant participant) : base(combat)
    {
        Participant = participant;
    }

    public override string ToString()
    {
        return $"{GetType().Name}: {Participant.ParticipantId} ({Participant.Character.CurrentHealth} / {Participant.Character.Stats.MaxHealth})";
    }
}