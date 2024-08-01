using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Types;

namespace Sapia.Game.Combat.Entities;

public readonly struct AbilityAttempt
{
    public AbilityAttempt(AbilityResult result)
    {
        Result = result;
    }

    public AbilityAttempt(AbilityFailureReason failureReason)
    {
        FailureReason = failureReason;
    }

    public bool WasSuccess => Result.HasValue;

    public AbilityResult? Result { get; }
    public AbilityFailureReason? FailureReason { get; }

    public static AbilityAttempt Fail(AbilityFailureReason reason) => new(reason);

    public static AbilityAttempt Success(AbilityResult result) => new(result);
}

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