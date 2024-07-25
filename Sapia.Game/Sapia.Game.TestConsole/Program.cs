using Sapia.Game.Characters;
using Sapia.Game.Characters.Configuration;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Sapia.Game.TestConsole.TypeData;

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

var theRock = characterStatusService.CompileCharacter(theRockConfiguration, ["Jab", "Slash"]);

var goblinA = new SimpleCharacter("GoblinA", new CharacterStats(3))
{
    Abilities = new[] { new PreparedAbility("Slash") }
};

var goblinB = new SimpleCharacter("GoblinB", new CharacterStats(4))
{
    Abilities = new[] { new PreparedAbility("Slash") }
};

var combat = CombatFactory.Create(typeData, new[]
{
    new CombatFactory.CombatParticipantEntry("Player", theRock, 5, (0,0)),
    new CombatFactory.CombatParticipantEntry("GoblinA", goblinA, 2, (1,0)),
    new CombatFactory.CombatParticipantEntry("GoblinB", goblinB, 10, (0,1)),
});

var executor = new CombatExecutor(combat);

var combatExecution = executor.Execute();

var num = 0;

var t = "  ";

CombatParticipant? FindTargetFor(CombatParticipant participant)
{
    foreach (var other in combat.Participants)
    {
        if (other.Character.IsPlayer != participant.Character.IsPlayer && other.Character.IsAlive)
        {
            return other;
        }
    }

    return null;
}

while (combatExecution.MoveNext())
{
    var step = combatExecution.Current;

    Console.WriteLine($"{combat.CurrentRound}: {step}");

    if (step is TurnStep turn)
    {
        if (turn.Abilities.Any())
        {
            var ability = turn.Abilities.Last();

            var target = FindTargetFor(turn.Participant);

            if (target != null)
            {
                var result = turn.UseAbility(new TargetedAbilityUse(ability.AbilityType.Id, target.ParticipantId));

                if (result.HasValue)
                {
                    var targetInfo = result.Value.AffectedParticipants.Select(x => $"{x.ParticipantId} {x.HealthChange}").ToArray();
                    Console.WriteLine(t + $"{result.Value.Ability.Id}: {string.Join(", ", targetInfo)}");
                }
            }
        }

        turn.EndTurn();
    }
}