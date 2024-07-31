using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Pathing;
using Sapia.Game.Types;

namespace Sapia.Game.Combat;

public partial class Combat
{
    public CombatPather Pather { get; }

    public int CurrentRound { get; private set; }

    private readonly ITypeDataRoot _typeData;
    private readonly Dictionary<int, CombatParticipant> _participantsByInitiativeOrder;
    private readonly Dictionary<string, CombatParticipant> _participantsById;

    public Combat(ITypeDataRoot typeData, IReadOnlyCollection<CombatParticipant> participants)
    {
        _typeData = typeData;
        _participantsByInitiativeOrder = participants.ToDictionary(x => x.InitiativeOrder, x => x);
        _participantsById = participants.ToDictionary(x => x.ParticipantId, x => x);

        Pather = new(this);

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
                RemainingActions = (CombatActionType[])Enum.GetValues(typeof(CombatActionType))
            };
        }
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

    public CombatParticipant GetParticipantById(string participantId)
    {
        throw new NotImplementedException();
    }
}

public enum CombatResult
{
    PlayerDefeat,
    PlayerVictory
}