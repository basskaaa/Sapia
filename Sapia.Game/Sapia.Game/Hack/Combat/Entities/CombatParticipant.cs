using Sapia.Game.Hack.Characters;
using Sapia.Game.Hack.Structs;

namespace Sapia.Game.Hack.Combat.Entities;

public class CombatParticipant
{
    public CombatParticipant(string id, ICompiledCharacter character, int initiativeRoll, int initiativeOrder, Coord position)
    {
        Character = character;
        InitiativeRoll = initiativeRoll;
        InitiativeOrder = initiativeOrder;
        Position = position;

        Id = id;

        Status = new CombatStatus();
    }

    public string Id { get; }

    public ICompiledCharacter Character { get; }
    public CombatStatus Status { get; internal set; }

    public int InitiativeRoll { get; }
    public int InitiativeOrder { get; }
    public Coord Position { get; internal set; }

}