using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Extensions;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.AI;

public class AiController
{
    public Combat Combat { get; }
    public CombatParticipant Participant { get; }

    private int? _lastRoundActed;
    private readonly HashSet<DecisionAttempts> _decisionsThisRound = new();

    private static readonly IReadOnlyCollection<DecisionAttempts> _allDecisions = (DecisionAttempts[])Enum.GetValues(typeof(DecisionAttempts));

    private Coord? _movementTarget;
    private CombatParticipant? _target;

    public AiController(Combat combat, string participantId)
    {
        Combat = combat;
        Participant = combat.Participants[participantId];
    }

    public bool ExecuteStep(ParticipantChoiceStep participantStep)
    {
        if (!_lastRoundActed.HasValue || _lastRoundActed.Value != Combat.CurrentRound)
        {
            _lastRoundActed = Combat.CurrentRound;
            _decisionsThisRound.Clear();
        }

        _target ??= FindTarget();

        if (participantStep is TurnStep turn)
        {
            if (_target == null || HasMadeAllDecisions())
            {
                turn.EndTurn();
            }
            else
            {
                if (DoFor(DecisionAttempts.ActAgainstTarget, turn, ActAgainstTarget))
                {
                    return true;
                }

                if (DoFor(DecisionAttempts.Move, turn, MoveTowardsTarget))
                {
                    return true;
                }

                if (DoFor(DecisionAttempts.ActAgainstTargetAfterMoving, turn, ActAgainstTarget))
                {
                    return true;
                }
            }
        }

        return true;
    }

    private bool DoFor(DecisionAttempts decision,TurnStep turn, Action<TurnStep> act)
    {
        if (_decisionsThisRound.Contains(decision))
        {
            return false;
        }

        act(turn);
        _decisionsThisRound.Add(decision);

        return true;
    }

    private bool HasMadeAllDecisions()
    {
        foreach (var decision in _allDecisions)
        {
            if (!_decisionsThisRound.Contains(decision))
            {
                return false;
            }
        }

        return true;
    }

    private void ActAgainstTarget(TurnStep turn)
    {
        // TODO: try to attack or whatever
    }

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

    private CombatParticipant? FindTarget()
    {
        return Combat.Participants.All
            .Where(x => x.Character.IsPlayer && x.Character.IsAlive)
            .OrderBy(x => Coord.Distance(x.Position, Participant.Position))
            .FirstOrDefault();
    }

    enum DecisionAttempts
    {
        Move,
        ActAgainstTarget,
        ActAgainstTargetAfterMoving,
    }
}