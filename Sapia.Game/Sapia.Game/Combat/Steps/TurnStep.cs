using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Pathing;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.Steps;

public class TurnStep : CombatParticipantStep
{
    public IReadOnlyCollection<UsableAbility> Abilities { get; }

    public bool HasEnded { get; private set; }
    public IReadOnlyCollection<Coord>? MovementRoute { get; private set; }

    public TurnStep(Combat combat, CombatParticipant participant, IReadOnlyCollection<UsableAbility> abilities) : base(combat, participant)
    {
        Abilities = abilities;
    }

    public void EndTurn()
    {
        HasEnded = true;
    }

    public bool TryMove(Coord target)
    {
        var path = Combat.Pather.GetPath(Participant.Position, target, new AStarSettings(20));

        if (!path.HasValue)
        {
            return false;
        }

        var pathToUse = path.Value.ToNodesArray()
            .Where(x => x != Participant.Position)
            .ToArray();

        if (pathToUse.Length > Participant.Status.RemainingMovement)
        {
            return false;
        }

        MovementRoute = pathToUse;

        return true;
    }

    public AbilityResult? UseAbility(AbilityUse use) => Combat.UseAbility(Participant.ParticipantId, use);
}