namespace Sapia.Game.Combat.Entities;

public class AbilityUse
{
    public AbilityUse(string abilityId)
    {
        AbilityId = abilityId;
    }

    public string AbilityId { get; }
}

public class TargetedAbilityUse : AbilityUse
{
    public string TargetParticipantId { get; }

    public TargetedAbilityUse(string abilityId, string targetParticipantId) : base(abilityId)
    {
        TargetParticipantId = targetParticipantId;
    }
}