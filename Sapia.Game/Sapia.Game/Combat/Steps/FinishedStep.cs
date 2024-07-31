using Sapia.Game.Combat.Entities.Enums;

namespace Sapia.Game.Combat.Steps;

public class FinishedStep : CombatStep
{
    public CombatResult Result { get; }

    public FinishedStep(Combat combat, CombatResult result) : base(combat)
    {
        Result = result;
    }

    public override string ToString()
    {
        return $"{base.ToString()}: {Result}";
    }
}