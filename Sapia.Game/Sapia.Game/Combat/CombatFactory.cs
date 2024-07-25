using Sapia.Game.Characters;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Structs;
using Sapia.Game.Types;

namespace Sapia.Game.Combat;

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