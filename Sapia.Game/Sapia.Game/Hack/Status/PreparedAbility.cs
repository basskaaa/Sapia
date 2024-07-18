namespace Sapia.Game.Hack.Status;

public class PreparedAbility
{
    public PreparedAbility(string abilityId, int? usesRemaining = null)
    {
        AbilityId = abilityId;
        UsesRemaining = usesRemaining;
    }

    public string AbilityId { get; }
    public int? UsesRemaining { get; set; }
}