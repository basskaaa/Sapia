namespace Sapia.Game.Hack.Characters;

public class SimpleCharacter : ICompiledCharacter
{
    public SimpleCharacter(string name, CharacterStats stats, int totalLevel = 1)
    {
        Name = name;
        Stats = stats;
        TotalLevel = totalLevel;
        CurrentHealth = stats.MaxHealth;
    }

    public string Name { get; }
    public int CurrentHealth { get; set; }
    public CharacterStats Stats { get; }
    public IReadOnlyCollection<PreparedAbility> Abilities { get; set; } = Array.Empty<PreparedAbility>();
    public int TotalLevel { get; }
    public bool IsPlayer => false;
}