using Sapia.Game.Combat.AI.Entities;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.AI;

public partial class AiController
{
    public Combat Combat { get; }
    public CombatParticipant Participant { get; }

    private int? _lastRoundActed;

    private AiTurn _turnState = new();

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
            _turnState = new();
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

    private bool DoFor(DecisionAttempts decision, TurnStep turn, Action<TurnStep> act)
    {
        if (_turnState.Decisions.Contains(decision))
        {
            return false;
        }

        act(turn);
        _turnState.Decisions.Add(decision);

        return true;
    }

    private bool HasMadeAllDecisions()
    {
        foreach (var decision in _allDecisions)
        {
            if (!_turnState.Decisions.Contains(decision))
            {
                return false;
            }
        }

        return true;
    }
    
    private CombatParticipant? FindTarget()
    {
        return Combat.Participants.All
            .Where(x => x.Character.IsPlayer && x.Character.IsAlive)
            .OrderBy(x => Coord.Distance(x.Position, Participant.Position))
            .FirstOrDefault();
    }
}