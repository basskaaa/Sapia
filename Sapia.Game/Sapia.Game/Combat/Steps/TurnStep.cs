using Sapia.Game.Combat.Entities;
using Sapia.Game.Structs;

namespace Sapia.Game.Combat.Steps;

public class TurnStep : ParticipantChoiceStep
{
    public IReadOnlyCollection<UsableAbility> Abilities { get; }

    internal bool HasEnded { get; private set; }
    internal IReadOnlyCollection<Coord>? MovementRoute { get; private set; }
    internal AbilityUse? AbilityToUse { get; private set; }

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
        var path = Combat.Movement.Pather.GetPath(Participant.Position, target, new(20));

        if (!path.HasValue)
        {
            return false;
        }

        var pathToUse = path.Value.ToNodesArray()
            .ToArray();

        if (pathToUse.Length > Participant.Status.RemainingMovement)
        {
            return false;
        }

        MovementRoute = pathToUse;

        return true;
    }

    public void UseAbility(AbilityUse use) => AbilityToUse = use;
}