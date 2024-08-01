using System.Runtime.InteropServices;
using FluentAssertions;
using Sapia.Game.Extensions;
using Sapia.Game.Structs;
using Sapia.Game.Tests.Configuration;
using Sapia.Game.Tests.Configuration.Scenarios;
using Sapia.Game.Tests.Extensions;
using Sapia.Game.Types;

namespace Sapia.Game.Tests.Combat.AI;

public class KillsThePlayer
{
    [SapiaData, Theory]
    public void Kills_in_melee(ITypeDataRoot typeData)
    {
        var combat = PlayerVsSkeleton.Setup(typeData);

        var p = combat.Player();
        var s = combat.Skeleton();
        
        combat.StepAiUntil(() => !p.Character.IsAlive);

        var adjacent = p.Position.GetAdjacent().ToArray();
        adjacent.Should().Contain(s.Position);

        adjacent = s.Position.GetAdjacent().ToArray();
        adjacent.Should().Contain(p.Position);
    }

    [SapiaData, Theory]
    public void Kills_with_ranged_without_moving(ITypeDataRoot typeData)
    {
        var combat = PlayerVsGoblinArcher.Setup(typeData);

        var p = combat.Player();
        var g = combat.GoblinArcher();

        var initialPosition = new Coord(0, combat.TypeData.Abilities.Get("Shoot").Range);

        g.Position = initialPosition;

        combat.StepAiUntil(() => !p.Character.IsAlive);

        g.Position.Should().Be(initialPosition);
    }

    [SapiaData, Theory]
    public void Moves_into_range_to_kill(ITypeDataRoot typeData)
    {
        var combat = PlayerVsGoblinArcher.Setup(typeData);

        var p = combat.Player();
        var g = combat.GoblinArcher();

        var initialPosition = new Coord(0, combat.TypeData.Abilities.Get("Shoot").Range + 2);

        g.Position = initialPosition;

        combat.StepAiUntil(() => !p.Character.IsAlive);

        g.Position.Should().NotBe(initialPosition);
    }
}