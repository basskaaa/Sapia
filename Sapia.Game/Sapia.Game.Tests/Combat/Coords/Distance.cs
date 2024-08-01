using FluentAssertions;
using Sapia.Game.Extensions;
using Sapia.Game.Structs;
using Sapia.Game.Tests.Configuration;

namespace Sapia.Game.Tests.Combat.Coords;

public class Distance
{
    [Fact]
    public void Vertical_distance_is_as_expected()
    {
        DistanceIs1((0,0), (0,1));

        DistanceIs1((0, 0), (0, -1));
            
        DistanceIs1((5, 0), (5, -1));
        DistanceIs1((8, -5), (8, -4));
    }

    [Fact]
    public void Horizontal_distance_is_as_expected()
    {
        DistanceIs1((0, 0), (1, 0));

        DistanceIs1((0, 0), (-1, 0));

        DistanceIs1((5, -1), (6, -1));
        DistanceIs1((8, -5), (9, -5));
    }

    [Fact]
    public void Diagonal_distance_is_as_expected()
    {
        DistanceIs1((0, 0), (1, 1));
        DistanceIs1((0, 0), (-1, 1));


        DistanceIs1((5, -1), (6, 0));
        DistanceIs1((8, -5), (9, -4));
    }
        
    [SapiaData, Theory]
    public void Adjacent_distance_is_as_expected(Coord pos)
    {
        foreach (var coord in pos.GetAdjacent())
        {
            DistanceIs1(coord,pos);
            DistanceIs1(pos, coord);
        }
    }
        
    private void DistanceIs1(Coord a, Coord b)
    {
        var d = Coord.Distance(a, b);

        d.Should().Be(1);
    }
}