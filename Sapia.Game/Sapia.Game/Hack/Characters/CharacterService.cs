using Sapia.Game.Hack.Configuration;
using Sapia.Game.Hack.Types;

namespace Sapia.Game.Hack.Characters;

public class CharacterService
{
    private readonly ITypeDataRoot _typeData;

    public CharacterService(ITypeDataRoot typeData)
    {
        _typeData = typeData;
    }

    public CompiledCharacter CompileCharacter(CharacterConfiguration configuration, IReadOnlyCollection<string> chosenAbilities)
    {
        var maxHealth = 0;

        var classLevels = new Dictionary<string, int>();

        foreach (var levelConfiguration in configuration.LevelConfigurations)
        {
            if (_typeData.Classes.TryFind(levelConfiguration.Value.ClassId, out var classType))
            {
                maxHealth += classType.HealthPerLevel;

                if (!classLevels.ContainsKey(classType.Id))
                {
                    classLevels[classType.Id] = 0;
                }

                classLevels[classType.Id]++;
            }
        }

        var allowedAbilities = new HashSet<string>();

        foreach (var classLevel in classLevels)
        {
            if (_typeData.Classes.TryFind(classLevel.Key, out var classType))
            {
                foreach (var abilitiesAtLevel in classType.AbilitiesAtLevels)
                {
                    if (abilitiesAtLevel.Value <= classLevel.Value)
                    {
                        allowedAbilities.Add(abilitiesAtLevel.Key);
                    }
                }
            }
        }

        var abilities = new List<PreparedAbility>();

        foreach (var ability in allowedAbilities)
        {
            if (_typeData.Abilities.TryFind(ability, out var abilityType))
            {
                // TODO: limit uses etc
                abilities.Add(new(ability));
            }
        }

        var stats = new CharacterStats
        {
            MaxHealth = maxHealth
        };

        return new(abilities, stats, classLevels);
    }
}