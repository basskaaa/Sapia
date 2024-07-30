using Sapia.Game.Combat.Entities;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat;

public partial class Combat
{
    public bool Move(string participantId, Coord to)
    {
        return Try(participantId, cp =>
        {
            var distance = Coord.Distance(cp.Position, to);

            if (distance > cp.Status.RemainingMovement)
            {
                return false;
            }

            cp.Status.RemainingMovement -= distance;
            cp.Position = to;

            return true;
        });
    }

    public CombatParticipant? GetParticipantAtPosition(Coord coord)
    {
        foreach (var combatParticipant in Participants)
        {
            if (combatParticipant.Character.IsAlive && combatParticipant.Position == coord)
            {
                return combatParticipant;
            }
        }

        return null;
    }
}