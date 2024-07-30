using Sapia.Game.Extensions;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.Pathing;

public class CombatPather : IPathManager<Coord>
{
    public IReadOnlyCollection<Coord> Obstructed => _obstructed;

    private readonly HashSet<Coord> _obstructed = new();
    private readonly Combat _combat;

    public CombatPather(Combat combat)
    {
        _combat = combat;
    }

    public IReadOnlyCollection<Coord> OutboundPaths(Coord node)
    {
        IEnumerable<Coord> Inner()
        {
            foreach (var coord in node.GetAdjacent())
            {
                if (_obstructed.Contains(coord) || _combat.GetParticipantAtPosition(coord) != null)
                {
                    continue;
                }

                yield return coord;
            }
        }

        return Inner().ToArray();
    }

    public (float actualToNext, float heuristicToGoal) GetCosts(Coord currentNode, Coord nextNode, Coord goal)
    {
        var toNext = Coord.Distance(currentNode, nextNode);
        var toGoal = Coord.Distance(nextNode, goal);

        return (toNext, toGoal);
    }

    public bool EqualsOtherNode(Coord a, Coord b) => a == b;

    public void Obstruct(params Coord[] positions)
    {
        foreach (var pos in positions)
        {
            _obstructed.Add(pos);
        }
    }
}