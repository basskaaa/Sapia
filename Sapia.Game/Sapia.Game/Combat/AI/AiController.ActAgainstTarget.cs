using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Structs;
using Sapia.Game.Types;
using Sapia.Game.Types.Enums;

namespace Sapia.Game.Combat.AI;

public partial class AiController
{
    private void ActAgainstTarget(TurnStep turn)
    {
        if (_plan?.ChosenAbility != null && CanUseAbilityAgainstTarget(_plan.ChosenAbility))
        {
            UseAbility(turn, _plan.ChosenAbility);
            return;
        }

        if (_plan == null)
        {
            return;
        }

        foreach (var ability in turn.Abilities)
        {
            if (CanUseAbilityAgainstTarget(ability.AbilityType))
            {
                UseAbility(turn, ability.AbilityType);
                return;
            }
        }
    }

    private void UseAbility(TurnStep turn, AbilityType ability)
    {
        turn.UseAbility(new TargetedAbilityUse(ability.Id, _plan!.Target.ParticipantId));
    }

    private bool CanUseAbilityAgainstTarget(AbilityType ability)
    {
        if (ability.Action == CombatActionType.Main && ability.Target == TargetType.Other)
        {
            return IsAbilityInRange(ability);
        }

        return false;
    }

    private bool IsAbilityInRange(AbilityType ability)
    {
        var d = Coord.Distance(_plan!.Target.Position, Participant.Position);

        return d <= ability.Range;
    }
}