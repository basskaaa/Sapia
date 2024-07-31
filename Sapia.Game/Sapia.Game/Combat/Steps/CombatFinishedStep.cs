using Sapia.Game.Combat.Entities.Enums;

namespace Sapia.Game.Combat.Steps;

public class CombatFinishedStep : CombatStep
{
    public CombatResult Result { get; }

    public CombatFinishedStep(Combat combat, CombatResult result) : base(combat)
    {
        Result = result;
    }

    public override string ToString()
    {
        return $"{base.ToString()}: {Result}";
    }
}