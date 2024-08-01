using Sapia.Game.Combat.Steps;
using Sapia.Game.Extensions;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.AI;

public partial class AiController
{
    private void MoveTowardsTarget(TurnStep turn)
    {
        var target = _plan!.Target;
        var desiredRange = _plan.ChosenAbility?.Range ?? 1;
        
        // Reset movement target if:
        // Arrived
        // It is now obstructed
        // It is now no longer within range of the target
        if (_plan.MovementTarget.HasValue &&
            (_plan.MovementTarget.Value == Participant.Position ||
             Combat.Movement.Pather.IsObstructed(_plan.MovementTarget.Value) ||
             Coord.Distance(_plan.MovementTarget.Value, target.Position) > desiredRange))
        {
            _plan.MovementTarget = null;
        }

        var dist = Coord.Distance(Participant.Position, target.Position);

        // Close enough to do something, no movement required
        if (dist <= desiredRange)
        {
            _plan.MovementTarget = null;
            return;
        }

        // Move towards existing target
        if (_plan.MovementTarget.HasValue && TryToMoveTo(turn, _plan.MovementTarget.Value))
        {
            return;
        }

        // No valid movement target, or couldn't move to it
        // Try to find a new movement target adjacent to the target
        var adjacent = target!.Position.GetAdjacent()
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
            _plan!.MovementTarget = target;

            var movementGoal = path.Value.Path.Take(Participant.Status.RemainingMovement).Last();

            return turn.TryMove(movementGoal.Node);
        }

        return false;
    }

}