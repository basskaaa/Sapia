using Sapia.Game.Structs.Dice;
using Sapia.Game.Types;

namespace Sapia.Game.Combat.Entities;

public readonly struct UsableAbility
{
    public UsableAbility(AbilityType abilityType, IDiceValue? bonus)
    {
        AbilityType = abilityType;
        Bonus = bonus;
    }

    public AbilityType AbilityType { get; }
    public IDiceValue? Bonus { get; }
}