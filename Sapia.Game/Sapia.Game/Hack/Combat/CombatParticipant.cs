using Sapia.Game.Hack.Status;

namespace Sapia.Game.Hack.Combat;

public class CombatParticipant
{
    public CombatParticipant(string id, ICharacterStatus status, int initiativeRoll, int initiativeOrder)
    {
        Status = status;
        InitiativeRoll = initiativeRoll;
        InitiativeOrder = initiativeOrder;

        Id = id;
    }

    public string Id { get; }

    public ICharacterStatus Status { get; }
    public int InitiativeRoll { get; }
    public int InitiativeOrder { get; }
}