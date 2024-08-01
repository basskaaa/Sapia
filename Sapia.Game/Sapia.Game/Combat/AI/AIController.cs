using Sapia.Game.Combat.AI.Entities;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.AI;

public partial class AiController
{
    public Combat Combat { get; }
    public CombatParticipant Participant { get; }

    private int? _lastRoundActed;

    // A plan for the AI to pursue this combat, possibly over many turns
    // Requires a valid target and can be cleared and reset if needed
    // A plan might only be to move towards the target and use an ability, clearing the plan once the ability is used
    // This will cause the plan to be recalculated
    private AiPlan? _plan;

    // Represents what the AI has done on this turn
    private AiTurn _turnState = new();

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

        if (participantStep is TurnStep turn)
        {
            _plan ??= MaintainPlan(turn);

            if (_plan == null || _turnState.HasMadeAllDecisions())
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
    
    private AiPlan? MaintainPlan(TurnStep turn)
    {
        if (PlanIsValid(_plan, turn))
        {
            return _plan;
        }

        var target = FindTarget();

        if (target != null)
        {
            // TODO: better choosing of action e.g shuffling 
            var mainActions = turn.Abilities.Where(x => x.AbilityType.Action == CombatActionType.Main).ToArray();

            return new AiPlan(target, mainActions.Length == 0 ? null : mainActions.First().AbilityType);
        }

        return null;
    }

    private bool PlanIsValid(AiPlan? plan, TurnStep turn)
    {
        if (plan == null)
        {
            return false;
        }

        if (plan.Target.Character.IsAlive)
        {
            return false;
        }

        // If the ability can't be used then clear the plan
        // This might clear the plan on the current turn which is ok because it will replan next turn
        if (plan.ChosenAbility != null && turn.Abilities.All(x => x.AbilityType.Id != plan.ChosenAbility.Id))
        {
            return false;
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