using FluentAssertions;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Tests.Configuration;
using Sapia.Game.Tests.Configuration.Scenarios;
using Sapia.Game.Tests.Extensions;
using Sapia.Game.Types;

namespace Sapia.Game.Tests.Combat.AI.Regression;

public class FailedAbilities
{
    [SapiaData, Theory]
    public void Wont_try_to_use_main_abilities_multiple_times(ITypeDataRoot typeData)
    {
        var combat = PlayerVsSkeleton.Setup(typeData);

        var p = combat.Player();
        var s = combat.Skeleton();

        s.Position = (0, 1);

        combat.SkipTurnIfPlayer();

        // First step uses the ability
        var step = combat.GetNextStep();
        step.Should().BeOfType<AbilityUsedStep>();

        combat.StepUntil(() => combat.CurrentStep is TurnStep turn && turn.Participant.Character.IsPlayer,
        () =>
        {
            combat.CurrentStep.Should().NotBeOfType<AbilityFailedStep>();
        });
    }
}