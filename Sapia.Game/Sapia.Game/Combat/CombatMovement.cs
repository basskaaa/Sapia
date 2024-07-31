using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Pathing;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat;

public  class CombatMovement
{
    public CombatPather Pather { get; }

    private readonly Combat _combat;

    public CombatMovement(Combat combat)
    {
        _combat = combat;

        Pather = new(_combat);
    }

    public bool Move(string participantId, Coord to)
    {
        return _combat.Try(participantId, cp =>
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
}