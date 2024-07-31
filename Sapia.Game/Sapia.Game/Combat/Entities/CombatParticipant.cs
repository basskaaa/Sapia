using System.Diagnostics;
using Sapia.Game.Characters;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.Entities;

[DebuggerDisplay("{ParticipantId}")]
public class CombatParticipant
{
    public CombatParticipant(string participantId, ICompiledCharacter character, int initiativeRoll, int initiativeOrder, Coord position)
    {
        Character = character;
        InitiativeRoll = initiativeRoll;
        InitiativeOrder = initiativeOrder;
        Position = position;

        ParticipantId = participantId;

        Status = new();
    }

    public string ParticipantId { get; }

    public ICompiledCharacter Character { get; }
    public CombatStatus Status { get; internal set; }

    public int InitiativeRoll { get; }
    public int InitiativeOrder { get; }
    public Coord Position { get; internal set; }

}