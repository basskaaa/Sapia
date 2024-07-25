namespace Sapia.Game.Hack.Characters;

public class PreparedAbility
{
    public PreparedAbility(string abilityId, int? usesRemaining = null)
    {
        AbilityId = abilityId;
        UsesRemaining = usesRemaining;
    }

    public string AbilityId { get; }
    public int? UsesRemaining { get; set; }

    public bool HasAvailableUses => !UsesRemaining.HasValue || UsesRemaining.Value > 0;
}