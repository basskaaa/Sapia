using Sapia.Game.Characters;
using Sapia.Game.Characters.Configuration;
using Sapia.Game.Combat;
using Sapia.Game.Types;

namespace Sapia.Game.Tests.Configuration.Scenarios;

public static class PlayerVsSkeleton
{
    public static Game.Combat.Combat Setup(ITypeDataRoot typeData)
    {
        var theRockConfiguration = new CharacterConfiguration
        {
            Name = "Dwayne 'The Rock' Johnson",
            LevelConfigurations = new()
            {
                {1, new()
                {
                    ClassId = "Fighter"
                }},
                {2, new()
                {
                    ClassId = "Fighter"
                }}
            }
        };

        var characterStatusService = new CharacterService(typeData);

        var theRock = characterStatusService.CompileCharacter(theRockConfiguration, ["Jab", "Slash"]);

        var skeleton = new SimpleCharacter("Skeleton", new(3))
        {
            Abilities = new[] { new PreparedAbility("Slash") }
        };

        var combat = CombatFactory.Create(typeData, new[]
        {
            new CombatFactory.CombatParticipantEntry("Player", theRock, 5, (0,0)),
            new CombatFactory.CombatParticipantEntry("Skeleton", skeleton, 2, (5,0))
        });

        // Always step again to skip the start of combat step
        combat.Step();

        return combat;
    }
}