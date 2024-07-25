namespace Sapia.Game.Hack.Configuration;

public class CharacterConfiguration
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<int, LevelConfiguration> LevelConfigurations { get; set; } = new();
}