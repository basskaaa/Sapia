using Sapia.Game.Hack.Combat.Entities;
using Sapia.Game.Hack.Structs;
using Sapia.Game.Hack.Types;
using Sapia.Game.Hack.Types.Enums;

namespace Sapia.Game.Hack.Combat;

public class Combat
{
    public int CurrentRound { get; private set; }

    private readonly ITypeDataRoot _typeData;
    private readonly Dictionary<int, CombatParticipant> _participantsByInitiativeOrder;
    private readonly Dictionary<string, CombatParticipant> _participantsById;

    public Combat(ITypeDataRoot typeData, IReadOnlyCollection<CombatParticipant> participants)
    {
        _typeData = typeData;
        _participantsByInitiativeOrder = participants.ToDictionary(x => x.InitiativeOrder, x => x);
        _participantsById = participants.ToDictionary(x => x.ParticipantId, x => x);

        StartNextRound();
    }

    public int CurrentInitiativeOrder { get; private set; }
    public IReadOnlyCollection<CombatParticipant> Participants => _participantsByInitiativeOrder.Values;

    public CombatParticipant CurrentParticipant() => _participantsByInitiativeOrder[CurrentInitiativeOrder];

    public void EndTurn(string participantId)
    {
        if (CurrentParticipant().ParticipantId == participantId)
        {
            CurrentInitiativeOrder++;
            if (!_participantsByInitiativeOrder.ContainsKey(CurrentInitiativeOrder))
            {
                StartNextRound();
            }
        }
    }

    private void StartNextRound()
    {
        CurrentRound++;
        CurrentInitiativeOrder = 0;

        foreach (var combatParticipant in _participantsByInitiativeOrder)
        {
            combatParticipant.Value.Status = new CombatStatus
            {
                RemainingMovement = combatParticipant.Value.Character.Stats.MovementSpeed,
                RemainingActions = Enum.GetValues<CombatActionType>()
            };
        }
    }

    public bool Move(string participantId, Coord to)
    {
        return Try(participantId, cp =>
        {
            var distance = Coord.Distance(cp.Position, to);

            if (distance > cp.Status.RemainingMovement)
            {
                return false;
            }

            cp.Status.RemainingMovement -= distance;
            cp.Position = to;

            return true;
        });
    }

    private bool Try(string participantId, Func<CombatParticipant, bool> act) => Try<bool>(participantId, act);

    private T? Try<T>(string participantId, Func<CombatParticipant, T> act)
    {
        if (_participantsById.TryGetValue(participantId, out var participant) && participant.Character.IsAlive)
        {
            return act(participant);
        }

        return default;
    }

    public CombatResult? CheckForComplete()
    {
        var allPlayersDead = Participants
            .Where(x => x.Character.IsPlayer)
            .All(x => !x.Character.IsAlive);

        if (allPlayersDead)
        {
            return CombatResult.PlayerDefeat;
        }

        var allOthersDead = Participants
            .Where(x => !x.Character.IsPlayer)
            .All(x => !x.Character.IsAlive);

        if (allOthersDead)
        {
            return CombatResult.PlayerVictory;
        }

        return null;
    }

    public IEnumerable<UsableAbility> GetUsableAbilities(string participantId)
    {
        if (_participantsById.TryGetValue(participantId, out var participant) && participant.Character.IsAlive)
        {
            foreach (var ability in participant.Character.Abilities)
            {
                if (ability.HasAvailableUses && _typeData.Abilities.TryFind(ability.AbilityId, out var abilityType))
                {
                    if (participant.Status.RemainingActions.Contains(abilityType.Action))
                    {
                        yield return new UsableAbility(abilityType, null);
                    }
                }
            }
        }
    }

    public AbilityResult? UseAbility(string participantId, AbilityUse abilityUse)
    {
        return Try<AbilityResult?>(participantId, cp =>
        {
            var availableAbility = cp.Character.Abilities.FirstOrDefault(x => x.AbilityId == abilityUse.AbilityId);

            if (availableAbility != null &&
                availableAbility.HasAvailableUses &&
                _typeData.Abilities.TryFind(abilityUse.AbilityId, out var abilityType) &&
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

            return new AffectedParticipant(target.ParticipantId, -damage);
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
                if (_participantsById.TryGetValue(targeted.TargetParticipantId, out var target))
                {
                    yield return target;
                }
            }

            yield break;
        }

        throw new NotImplementedException();
    }
}

public enum CombatResult
{
    PlayerDefeat,
    PlayerVictory
}