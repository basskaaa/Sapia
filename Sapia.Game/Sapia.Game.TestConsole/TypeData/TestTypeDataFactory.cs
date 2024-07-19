using Sapia.Game.Hack.Types;

namespace Sapia.Game.TestConsole.TypeData;

public static class TypeDataFactory
{
    private static TestTypeDataProvider<AbilityType> CreateAbilitiesTypeData()
    {
        var slash = new AbilityType
        {
            Id = "Slash"
        };
        var parry = new AbilityType
        {
            Id = "Jab"
        };
        var whirlwind = new AbilityType
        {
            Id = "Whirlwind"
        };

        return new(slash, parry, whirlwind);
    }

    private static TestTypeDataProvider<ClassType> CreateClassTypeData()
    {
        var fighter = new ClassType
        {
            Id = "Fighter",
            HealthPerLevel = 5,
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