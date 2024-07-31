using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Pathing;
using Sapia.Game.Structs;
using Sapia.Game.Types;
using System.IO;

namespace Sapia.Game.Combat;

public class CombatParticipants
{
    public CombatParticipant this[string participantId] => ById[participantId];

    internal Dictionary<int, CombatParticipant> ByInitiativeOrder { get; }
    internal Dictionary<string, CombatParticipant> ById { get; }

    public IReadOnlyCollection<CombatParticipant> All => ByInitiativeOrder.Values;

    public CombatParticipants(IReadOnlyCollection<CombatParticipant> participants)
    {
        ByInitiativeOrder = participants.ToDictionary(x => x.InitiativeOrder, x => x);
        ById = participants.ToDictionary(x => x.ParticipantId, x => x);
    }

    internal CombatParticipant GetParticipantByInitiativeOrder(int initiativeOrder) => ByInitiativeOrder[initiativeOrder];

    public CombatParticipant? GetParticipantAtPosition(Coord coord)
    {
        foreach (var combatParticipant in All)
        {
            if (combatParticipant.Character.IsAlive && combatParticipant.Position == coord)
            {
                return combatParticipant;
            }
        }

        return null;
    }

    public bool TryGetParticipantById(string participantId, out CombatParticipant participant) => ById.TryGetValue(participantId, out participant);
}