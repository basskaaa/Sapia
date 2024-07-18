using Sapia.Game.Hack.Structs;
using Sapia.Game.Hack.Types;

namespace Sapia.Game.Hack.Combat;

public class Combat
{
    private readonly ITypeDataRoot _typeData;
    private readonly Dictionary<int, CombatParticipant> _participants;

    public Combat(ITypeDataRoot typeData, IReadOnlyCollection<CombatParticipant> participants)
    {
        _typeData = typeData;
        _participants = participants.ToDictionary(x=>x.InitiativeOrder, x=>x);

        StartNextRound();
    }

    public int CurrentInitiativeOrder { get; private set; }
    public IReadOnlyCollection<CombatParticipant> Participants => _participants.Values;

    public CombatParticipant CurrentParticipant() => _participants[0];

    public void EndTurn(string participantId)
    {
        if (CurrentParticipant().Id == participantId)
        {
            CurrentInitiativeOrder++;
            if (!_participants.ContainsKey(CurrentInitiativeOrder))
            {
                StartNextRound();
            }
        }
    }

    private void StartNextRound()
    {
        CurrentInitiativeOrder = 0;

        foreach (var combatParticipant in _participants)
        {
            combatParticipant.Value.Status = new CombatStatus
            {
                RemainingMovement = combatParticipant.Value.Character.Stats.MovementSpeed,
                RemainingActions = Enum.GetValues<CombatActionType>()
            };
        }
    }

    public bool Move(string id, Coord to)
    {
        return Try(id, cp =>
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

    private bool Try(string id, Func<CombatParticipant, bool> act)
    {
        var participant = Participants.FirstOrDefault(x => x.Id == id);

        if (participant == null)
        {
            return false;
        }

        return act(participant);
    }
}