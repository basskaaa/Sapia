using Sapia.Game.Types;

namespace Assets._Scripts.TypeData
{
    public static class TypeDataFactory
    {
        private static TestTypeDataProvider<AbilityType> CreateAbilitiesTypeData()
        {
            var slash = new AbilityType
            {
                Id = "Slash",
                Description = "Slash your opponent with slashiness"

            };
            var jab = new AbilityType
            {
                Id = "Jab",
                Damage = 2,
                Description = "Jab your opponent with jabjab"
            };
            var whirlwind = new AbilityType
            {
                Id = "Whirlwind",
                Description = "Whoooosh don't forget helicopter noises"
            };

            return new(slash, jab, whirlwind);
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
}