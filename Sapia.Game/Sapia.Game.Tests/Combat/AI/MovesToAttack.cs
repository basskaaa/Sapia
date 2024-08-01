using FluentAssertions;
using Sapia.Game.Extensions;
using Sapia.Game.Structs;
using Sapia.Game.Tests.Configuration;
using Sapia.Game.Tests.Configuration.Scenarios;
using Sapia.Game.Tests.Extensions;
using Sapia.Game.Types;

namespace Sapia.Game.Tests.Combat.AI;

public class MovesToAttack
{
    [SapiaTheory, Theory]
    public void Moves_within_distance_of_player_in_1_turn(ITypeDataRoot typeData)
    {
        var combat = PlayerVsSkeleton.Setup(typeData);

        var p = combat.Player();
        var s = combat.Skeleton();

        combat.StepAiUntil(() =>
        {
            var d = Coord.Distance(p.Position, s.Position);

            return d <= 1;
        });

        combat.CurrentRound.Should().Be(1);

        var adjacent = p.Position.GetAdjacent().ToArray();
        adjacent.Should().Contain(s.Position);

        adjacent = s.Position.GetAdjacent().ToArray();
        adjacent.Should().Contain(p.Position);
    }

    [SapiaTheory, Theory]
    public void Moves_within_distance_of_player_over_more_than_1_turn(ITypeDataRoot typeData)
    {
        var combat = PlayerVsSkeleton.Setup(typeData);

        var p = combat.Player();
        var s = combat.Skeleton();

        s.Position = new(0, s.Character.Stats.MovementSpeed * 2);
        
        combat.StepAiUntil(() =>
        {
            var d = Coord.Distance(p.Position, s.Position);

            return d <= 1;
        });

        combat.CurrentRound.Should().BeGreaterOrEqualTo(2);

        var adjacent = p.Position.GetAdjacent().ToArray();
        adjacent.Should().Contain(s.Position);

        adjacent = s.Position.GetAdjacent().ToArray();
        adjacent.Should().Contain(p.Position);
    }
}