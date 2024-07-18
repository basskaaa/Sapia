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
    }
}