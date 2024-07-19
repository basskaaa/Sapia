using Sapia.Game.Hack.Characters;
using Sapia.Game.Hack.Combat;
using Sapia.Game.Hack.Combat.Steps;
using Sapia.Game.Hack.Configuration;
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

var goblinA = new SimpleCharacter("GoblinA", new CharacterStats(5))
{
    Abilities = new[] { new PreparedAbility("Slash") }
};

var goblinB = new SimpleCharacter("GoblinB", new CharacterStats(5))
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

while (num++ < 10 && combatExecution.MoveNext())
{
    var step = combatExecution.Current;

    Console.WriteLine($"{combat.CurrentRound}: {step}");

    if (step is TurnStep turn)
    {
        Console.WriteLine(t + string.Join(", ", turn.Abilities.Select(x => x.AbilityType.Id)));

        turn.EndTurn();
    }
}