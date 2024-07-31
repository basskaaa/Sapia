using FluentAssertions;
using Sapia.Game.Combat.Steps;

namespace Sapia.Game.Tests.Extensions;

public static class CombatExtensions
{
    public static void StepUntil(this Game.Combat.Combat combat, Func<bool> until, int maxTries = 100)
    {
        var numTries = 0;

        while (numTries++ < maxTries)
        {
            combat.ExecuteAi();
            combat.Step();

            if (until())
            {
                break;
            }
        }

        numTries.Should().BeLessThan(maxTries);
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
}