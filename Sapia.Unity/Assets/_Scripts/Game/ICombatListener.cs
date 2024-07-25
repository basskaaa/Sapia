using Sapia.Game.Combat.Steps;
using Sapia.Game.Combat;

public interface ICombatListener
{
    void StepChanged(Combat combat, CombatStep step);
}