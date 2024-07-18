namespace Sapia.Game.Hack.Status;

public interface ICharacterStatus
{
    int CurrentHealth { get; set; }
    int MaxHealth { get; }
    IReadOnlyCollection<PreparedAbility> Abilities { get; }
    int TotalLevel { get; }
}