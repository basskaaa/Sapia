using Sapia.Game.Hack.Characters;
using Sapia.Game.Hack.Combat.Entities;
using Sapia.Game.Hack.Structs;
using Sapia.Game.Hack.Types;

namespace Sapia.Game.Hack.Combat;

public static class CombatFactory
{
    public static Combat Create(ITypeDataRoot typeData, IEnumerable<CombatParticipantEntry> participants)
    {
        var builtParticipants = participants.OrderBy(x => x.InitiativeRoll)
            .Select((x, i) => new CombatParticipant(x.Id, x.Character, x.InitiativeRoll, i, x.Position))
            .ToArray();

        return new Combat(typeData, builtParticipants);
    }

    public readonly struct CombatParticipantEntry
    {
        public CombatParticipantEntry(string id, ICompiledCharacter character, int initiativeRoll, Coord position)
        {
            Id = id;
            Character = character;
            InitiativeRoll = initiativeRoll;
            Position = position;
        }

        public string Id { get; }
        public ICompiledCharacter Character { get; }
        public int InitiativeRoll { get; }
        public Coord Position { get; }
    }
}