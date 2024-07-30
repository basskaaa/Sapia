using Sapia.Game.Types;

namespace Sapia.Game.Combat.Entities;

public readonly struct AbilityResult
{
    public AbilityResult(AbilityType ability, params AffectedParticipant[] affectedParticipants)
    {
        Ability = ability;
        AffectedParticipants = affectedParticipants;
    }

    public AbilityType Ability { get; }
    public IReadOnlyCollection<AffectedParticipant> AffectedParticipants { get; }
}

public readonly struct AffectedParticipant
{
    public AffectedParticipant(string participantId, int? healthChange)
    {
        ParticipantId = participantId;
        HealthChange = healthChange;
    }

    public string ParticipantId { get; }
    public int? HealthChange { get; }
}