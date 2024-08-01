using FluentAssertions;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;

namespace Sapia.Game.Tests.Extensions;

public static class CombatExtensions
{
    public static void StepUntil(this Game.Combat.Combat combat, Func<bool> until, Action? perStep = null, int maxTries = 100)
    {
        var numTries = 0;

        while (numTries++ < maxTries)
        {
            combat.ExecuteAi();
            combat.Step();

            perStep?.Invoke();

            if (until())
            {
                break;
            }
        }

        numTries.Should().BeLessThan(maxTries, "too many tries were attempted");
    }

    public static void StepAiUntil(this Game.Combat.Combat combat, Func<bool> until, int maxTries = 100)
    {
        combat.StepUntil(until, combat.SkipTurnIfPlayer, maxTries);
    }

    public static void SkipTurn(this Game.Combat.Combat combat)
    {
        if (combat.CurrentStep is TurnStep turn)
        {
            turn.EndTurn();
            combat.Step();
        }
    }

    public static void SkipTurnIf(this Game.Combat.Combat combat, Func<TurnStep, bool> ifTurn)
    {
        if (combat.CurrentStep is TurnStep turn && ifTurn(turn))
        {
            combat.SkipTurn();
        }
    }

    public static void SkipTurnIfPlayer(this Game.Combat.Combat combat)
    {
        combat.SkipTurnIf(t => t.Participant.Character.IsPlayer);
    }

    public static void SkipTurnIfNpc(this Game.Combat.Combat combat)
    {
        combat.SkipTurnIf(t => t.Participant.Character.IsNpc);
    }

    public static CombatParticipant Player(this Game.Combat.Combat combat) => combat.Participants["Player"];
    public static CombatParticipant GoblinArcher(this Game.Combat.Combat combat) => combat.Participants["GoblinArcher"];
    public static CombatParticipant Skeleton(this Game.Combat.Combat combat) => combat.Participants["Skeleton"];
}