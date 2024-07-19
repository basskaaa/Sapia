namespace Sapia.Game.Hack.Characters;

public interface ICompiledCharacter
{
    int CurrentHealth { get; internal set; }
    public CharacterStats Stats { get; }
    IReadOnlyCollection<PreparedAbility> Abilities { get; }
    int TotalLevel { get; }

    bool IsPlayer { get; }

    bool IsAlive => CurrentHealth > 0;
}