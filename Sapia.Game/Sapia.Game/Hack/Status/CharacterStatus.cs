namespace Sapia.Game.Hack.Status;

public class CharacterStatus : ICharacterStatus
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; }

    public IReadOnlyCollection<PreparedAbility> Abilities { get; }
    public Dictionary<string, int> ClassLevels { get; }

    public int TotalLevel => ClassLevels.Sum(x => x.Value);

    public CharacterStatus(IReadOnlyCollection<PreparedAbility> abilities, int maxHealth, Dictionary<string, int> classLevels)
    {
        Abilities = abilities;
        ClassLevels = classLevels;
        CurrentHealth = MaxHealth = maxHealth;
    }
}