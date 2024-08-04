namespace Sapia.Game.Types.Entities;

public class ClassType : TypeData
{
    public Dictionary<string, int> AbilitiesAtLevels { get; set; } = new();

    public int HealthPerLevel { get; set; } = 1;
}