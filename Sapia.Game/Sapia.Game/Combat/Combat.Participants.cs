using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Pathing;
using Sapia.Game.Types;

namespace Sapia.Game.Combat;

public partial class Combat
{
    private readonly Dictionary<int, CombatParticipant> _participantsByInitiativeOrder;
    private readonly Dictionary<string, CombatParticipant> _participantsById;

    public IReadOnlyCollection<CombatParticipant> Participants => _participantsByInitiativeOrder.Values;

    public CombatParticipant CurrentParticipant() => _participantsByInitiativeOrder[CurrentInitiativeOrder];

    public CombatParticipant GetParticipantById(string participantId) => _participantsById[participantId];
}