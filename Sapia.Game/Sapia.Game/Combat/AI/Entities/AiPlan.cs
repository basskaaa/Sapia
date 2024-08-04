using Sapia.Game.Combat.Entities;
using Sapia.Game.Structs;
using Sapia.Game.Types.Entities;

namespace Sapia.Game.Combat.AI.Entities;

public class AiPlan
{
    public AiPlan(CombatParticipant target, AbilityType? chosenAbility)
    {
        Target = target;
        ChosenAbility = chosenAbility;
    }

    public CombatParticipant Target { get; }
    public AbilityType? ChosenAbility { get; }

    public Coord? MovementTarget { get; set; }
}