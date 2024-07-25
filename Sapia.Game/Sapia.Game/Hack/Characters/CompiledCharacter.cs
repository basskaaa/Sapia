namespace Sapia.Game.Hack.Characters;

public class CompiledCharacter : ICompiledCharacter
{
    public int CurrentHealth { get; set; }
    public CharacterStats Stats { get; }

    public bool IsPlayer => true;

    public IReadOnlyCollection<PreparedAbility> Abilities { get; }
    public Dictionary<string, int> ClassLevels { get; }

    public int TotalLevel => ClassLevels.Sum(x => x.Value);

    public CompiledCharacter(IReadOnlyCollection<PreparedAbility> abilities, CharacterStats stats, Dictionary<string, int> classLevels)
    {
        Abilities = abilities;
        ClassLevels = classLevels;
        Stats = stats;
        CurrentHealth = stats.MaxHealth;
    }
}

public class CharacterStats
{
    public int MaxHealth { get; internal set; }
    public int MovementSpeed { get; internal set; } = 6;

    public CharacterStats(){ }

    public CharacterStats(int maxHealth)
    {
        MaxHealth = maxHealth;
    }
}