using Sapia.Game.Hack.Combat.Entities;

namespace Sapia.Game.Hack.Combat.Steps;

public class TurnStep : CombatParticipantStep
{
    public IReadOnlyCollection<UsableAbility> Abilities { get; }

    public bool HasEnded { get; private set; }

    public TurnStep(Combat combat, CombatParticipant participant, IReadOnlyCollection<UsableAbility> abilities) : base(combat, participant)
    {
        Abilities = abilities;
    }

    public void EndTurn()
    {
        HasEnded = true;
    }

    public AbilityResult? UseAbility(AbilityUse use) => Combat.UseAbility(Participant.ParticipantId, use);
}