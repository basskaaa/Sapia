using System.Collections.Generic;
using System.Linq;
using Assets._Scripts.TypeData;
using Sapia.Game.Characters;
using Sapia.Game.Characters.Configuration;
using Sapia.Game.Combat;
using Sapia.Game.Structs;

namespace Assets._Scripts.Game
{
    public static class CombatStarter
    {
        public static Combat CreateCombat(IReadOnlyCollection<CombatParticipantRef> participantRefs)
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

            var typeData = TypeDataFactory.CreateTypeData();
            var characterStatusService = new CharacterService(typeData);

            var theRock = characterStatusService.CompileCharacter(theRockConfiguration, new[] { "Jab", "Slash", "Shoot" });

            ICompiledCharacter CreateSkeleton() => new SimpleCharacter("Skeleton", new CharacterStats(3))
            {
                Abilities = new[] { new PreparedAbility("Slash") }
            };

            var participants = participantRefs.Select(x =>
            {
                var pos = new Coord((int)x.transform.position.x, (int)x.transform.position.z);
                var id = x.ParticipantId;

                var character = x.ParticipantId == "Player" ? theRock : CreateSkeleton();

                return new CombatFactory.CombatParticipantEntry(id, character, id == "Player" ? 20 : 5, pos);
            });

            var combat = CombatFactory.Create(typeData, participants);

            return combat;
        }
    }
}