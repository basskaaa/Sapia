using Sapia.Game.Combat.Entities;
using Sapia.Game.Types.Enums;
using Sapia.Game.Types;

namespace Sapia.Game.Combat;

public class CombatAbilities
{
    private readonly Combat _combat;

    public CombatAbilities(Combat combat)
    {
        _combat = combat;
    }

    public IEnumerable<UsableAbility> GetUsableAbilities(string participantId)
    {
        if (_combat.Participants.TryGetParticipantById(participantId, out var participant) && participant.Character.IsAlive)
        {
            foreach (var ability in participant.Character.Abilities)
            {
                if (ability.HasAvailableUses && _combat.TypeData.Abilities.TryFind(ability.AbilityId, out var abilityType))
                {
                    if (participant.Status.RemainingActions.Contains(abilityType.Action))
                    {
                        yield return new(abilityType, null);
                    }
                }
            }
        }
    }

    public AbilityResult? UseAbility(string participantId, AbilityUse abilityUse)
    {
        return _combat.Try(participantId, cp =>
        {
            var availableAbility = cp.Character.Abilities.FirstOrDefault(x => x.AbilityId == abilityUse.AbilityId);

            if (availableAbility != null &&
                availableAbility.HasAvailableUses &&
                _combat.TypeData.Abilities.TryFind(abilityUse.AbilityId, out var abilityType) &&
                cp.Status.RemainingActions.Contains(abilityType.Action))
            {
                var result = ExecuteAbility(cp, abilityType, abilityUse);

                if (result.HasValue)
                {
                    if (availableAbility.UsesRemaining.HasValue)
                    {
                        availableAbility.UsesRemaining--;
                    }

                    cp.Status.RemainingActions = cp.Status.RemainingActions.Except([abilityType.Action]).ToArray();

                    return result;
                }
            }

            return null;
        });
    }

    private AbilityResult? ExecuteAbility(CombatParticipant participant, AbilityType abilityType, AbilityUse abilityUse)
    {
        var targets = GetTargets(participant, abilityType, abilityUse).ToArray();

        if (abilityType.Target != TargetType.None && targets.Length == 0)
        {
            return null;
        }

        var affectedTargets = targets.Select(x => ApplyAbilityToParticipant(participant, x, abilityType));

        return new AbilityResult(abilityType, affectedTargets.ToArray());
    }

    private AffectedParticipant ApplyAbilityToParticipant(CombatParticipant applier, CombatParticipant target, AbilityType abilityType)
    {
        if (abilityType.Damage > 0)
        {
            var damage = abilityType.Damage;

            target.Character.CurrentHealth -= damage;

            return new(target.ParticipantId, -damage);
        }

        throw new NotImplementedException();
    }

    private IEnumerable<CombatParticipant> GetTargets(CombatParticipant participant, AbilityType abilityType, AbilityUse abilityUse)
    {
        if (abilityType.Target == TargetType.Self)
        {
            yield return participant;
            yield break;
        }

        if (abilityType.Target == TargetType.Other)
        {
            if (abilityUse is TargetedAbilityUse targeted)
            {
                if (_combat.Participants.TryGetParticipantById(targeted.TargetParticipantId, out var target))
                {
                    yield return target;
                }
            }

            yield break;
        }

        throw new NotImplementedException();
    }
}