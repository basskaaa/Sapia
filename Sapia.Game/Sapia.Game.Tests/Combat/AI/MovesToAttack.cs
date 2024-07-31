using FluentAssertions;
using Sapia.Game.Tests.Configuration;
using Sapia.Game.Tests.Configuration.Scenarios;
using Sapia.Game.Types;

namespace Sapia.Game.Tests.Combat.AI;

public class MovesToAttack
{
    [SapiaTheory, Theory]
    public void Moves_within_distance_of_player_in_1_turn(ITypeDataRoot typeData)
    {
        var combat = PlayerVsGoblin.Setup(typeData);

        1.Should().Be(2);
    }
}