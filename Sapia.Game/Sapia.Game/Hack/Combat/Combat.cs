using Sapia.Game.Hack.Combat.Entities;
using Sapia.Game.Hack.Structs;
using Sapia.Game.Hack.Types;

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
        _participantsById = participants.ToDictionary(x => x.Id, x => x);

        StartNextRound();
    }

    public int CurrentInitiativeOrder { get; private set; }
    public IReadOnlyCollection<CombatParticipant> Participants => _participantsByInitiativeOrder.Values;

    public CombatParticipant CurrentParticipant() => _participantsByInitiativeOrder[CurrentInitiativeOrder];

    public void EndTurn(string participantId)
    {
        if (CurrentParticipant().Id == participantId)
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

    private bool Try(string participantId, Func<CombatParticipant, bool> act)
    {
        if (_participantsById.TryGetValue(participantId, out var participant) && participant.Character.IsAlive)
        {
            return act(participant);
        }

        return false;
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
        if (_participantsById.TryGetValue(participantId, out var participant))
        {
            foreach (var ability in participant.Character.Abilities)
            {
                var hasUses = !ability.UsesRemaining.HasValue || ability.UsesRemaining.Value > 0;

                if (hasUses && _typeData.Abilities.TryFind(ability.AbilityId, out var abilityType))
                {
                    if (participant.Status.RemainingActions.Contains(abilityType.Action))
                    {
                        yield return new UsableAbility(abilityType, null);
                    }
                }
            }
        }
    }
}

public enum CombatResult
{
    PlayerDefeat,
    PlayerVictory
}