using Sapia.Game.Combat.Steps;
using Sapia.Game.Extensions;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.AI;

public partial class AiController
{
    private void MoveTowardsTarget(TurnStep turn)
    {
        // Reset movement target if:
        // Arrived
        // It is now obstructed
        // It is now no longer adjacent to the target
        if (_movementTarget.HasValue &&
            (_movementTarget.Value == Participant.Position ||
             Combat.Movement.Pather.IsObstructed(_movementTarget.Value) ||
             Coord.Distance(_movementTarget.Value, _target.Position) > 1))
        {
            _movementTarget = null;
        }

        var dist = Coord.Distance(Participant.Position, _target.Position);

        // Close enough to do something, no movement required
        if (dist <= 1)
        {
            _movementTarget = null;
            return;
        }

        // Move towards existing target
        if (_movementTarget.HasValue && TryToMoveTo(turn, _movementTarget.Value))
        {
            return;
        }

        // No valid movement target, or couldn't move to it
        // Try to find a new movement target adjacent to the target
        var adjacent = _target!.Position.GetAdjacent()
            .Where(x => !Combat.Movement.Pather.IsObstructed(x))
            .OrderBy(x => Coord.Distance(x, Participant.Position))
            .ToArray();

        foreach (var possibleAdjacent in adjacent)
        {
            if (TryToMoveTo(turn, possibleAdjacent))
            {
                return;
            }
        }
    }

    private bool TryToMoveTo(TurnStep turn, Coord target)
    {
        var path = Combat.Movement.Pather.GetPath(Participant.Position, target, new(20));

        if (path != null)
        {
            _movementTarget = target;

            var movementGoal = path.Value.Path.Take(Participant.Status.RemainingMovement).Last();

            return turn.TryMove(movementGoal.Node);
        }

        return false;
    }

}