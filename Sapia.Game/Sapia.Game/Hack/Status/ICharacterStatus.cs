namespace Sapia.Game.Hack.Status;

public interface ICharacterStatus
{
    int CurrentHealth { get; set; }
    public CharacterStats Stats { get; }
    IReadOnlyCollection<PreparedAbility> Abilities { get; }
    int TotalLevel { get; }
}