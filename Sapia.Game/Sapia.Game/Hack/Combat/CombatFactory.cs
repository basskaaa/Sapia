using Sapia.Game.Hack.Status;
using Sapia.Game.Hack.Types;

namespace Sapia.Game.Hack.Combat;

public static class CombatFactory
{
    public static Combat Create(ITypeDataRoot typeData, IEnumerable<(ICharacterStatus character, string id, int initiativeRoll)> participants)
    {
        var builtParticipants = participants.OrderBy(x => x.initiativeRoll)
            .Select((x, i) => new CombatParticipant(x.id, x.character, x.initiativeRoll, i))
            .ToArray();

        return new Combat(typeData, builtParticipants);
    }
}