namespace Sapia.Game.Characters.Configuration;

public class CharacterConfiguration
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<int, CharacterLevelConfiguration> LevelConfigurations { get; set; } = new();
}