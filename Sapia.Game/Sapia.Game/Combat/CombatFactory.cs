using Sapia.Game.Characters;
using Sapia.Game.Combat.Entities;
using Sapia.Game.Structs;
using Sapia.Game.Types;

namespace Sapia.Game.Combat;

public static class CombatFactory
{
    public static CombatBag Create(ITypeDataRoot typeData, IEnumerable<CombatParticipantEntry> participants)
    {
        var builtParticipants = participants.OrderByDescending(x => x.InitiativeRoll)
            .Select((x, i) => new CombatParticipant(x.Id, x.Character, x.InitiativeRoll, i, x.Position))
            .ToArray();

        var combat = new Combat(typeData, builtParticipants);
        var executor = new CombatExecutor(combat);

        return new CombatBag(executor);
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