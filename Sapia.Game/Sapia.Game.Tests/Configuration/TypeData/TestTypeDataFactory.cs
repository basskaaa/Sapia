using Sapia.Game.Types.Entities;

namespace Sapia.Game.Tests.Configuration.TypeData;

public static class TypeDataFactory
{
    private static TestTypeDataProvider<AbilityType> CreateAbilitiesTypeData()
    {
        var slash = new AbilityType
        {
            Id = "Slash"
        };
        var jab = new AbilityType
        {
            Id = "Jab",
            Damage = 2
        };
        var whirlwind = new AbilityType
        {
            Id = "Whirlwind"
        };
        var shoot = new AbilityType
        {
            Id = "Shoot",
            Range = 6
        };

        return new(slash, jab, whirlwind, shoot);
    }

    private static TestTypeDataProvider<ClassType> CreateClassTypeData()
    {
        var fighter = new ClassType
        {
            Id = "Fighter",
            HealthPerLevel = 8,
            AbilitiesAtLevels = new()
            {
                {"Slash", 1},
                {"Jab", 1},
                {"Whirlwind", 1}
            }
        };

        return new(fighter);
    }

    public static TestTypeDataRoot CreateTypeData()
    {
        var abilities = CreateAbilitiesTypeData();

        var classes = CreateClassTypeData();

        return new(abilities, classes);
    }
}