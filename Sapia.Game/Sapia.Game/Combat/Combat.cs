using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Combat.Pathing;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Types;

namespace Sapia.Game.Combat;

public class Combat
{
    private readonly CombatExecutor _executor;
    private readonly IEnumerator<CombatStep> _execution;

    public CombatStep? CurrentStep => _execution.Current;

    public int CurrentRound { get; private set; }
    public int CurrentInitiativeOrder { get; private set; }

    public ITypeDataRoot TypeData { get; }

    public CombatParticipants Participants { get; }
    public CombatMovement Movement { get; }
    public CombatAbilities Abilities { get; }

    public Combat(ITypeDataRoot typeData, IReadOnlyCollection<CombatParticipant> participants)
    {
        TypeData = typeData;

        Participants = new(participants);
        Movement = new(this);
        Abilities = new(this);

        StartNextRound();

        _executor = new CombatExecutor(this);
        _execution = _executor.Execute();
    }

    public bool Step() => _execution.MoveNext();

    internal void EndTurn(string participantId)
    {
        if (CurrentParticipant().ParticipantId == participantId)
        {
            CurrentInitiativeOrder++;
            if (!Participants.ByInitiativeOrder.ContainsKey(CurrentInitiativeOrder))
            {
                StartNextRound();
            }
        }
    }

    internal CombatParticipant CurrentParticipant() => Participants.GetParticipantByInitiativeOrder(CurrentInitiativeOrder);

    private void StartNextRound()
    {
        CurrentRound++;
        CurrentInitiativeOrder = 0;

        foreach (var combatParticipant in Participants.ByInitiativeOrder)
        {
            combatParticipant.Value.Status = new()
            {
                RemainingMovement = combatParticipant.Value.Character.Stats.MovementSpeed,
                RemainingActions = (CombatActionType[])Enum.GetValues(typeof(CombatActionType))
            };
        }
    }

    internal bool Try(string participantId, Func<CombatParticipant, bool> act) => Try<bool>(participantId, act);

    internal T? Try<T>(string participantId, Func<CombatParticipant, T> act)
    {
        if (Participants.TryGetParticipantById(participantId, out var participant) && participant.Character.IsAlive)
        {
            return act(participant);
        }

        return default;
    }

    public CombatResult? CheckForComplete()
    {
        var allPlayersDead = Participants
            .All
            .Where(x => x.Character.IsPlayer)
            .All(x => !x.Character.IsAlive);

        if (allPlayersDead)
        {
            return CombatResult.PlayerDefeat;
        }

        var allOthersDead = Participants
            .All
            .Where(x => !x.Character.IsPlayer)
            .All(x => !x.Character.IsAlive);

        if (allOthersDead)
        {
            return CombatResult.PlayerVictory;
        }

        return null;
    }
}