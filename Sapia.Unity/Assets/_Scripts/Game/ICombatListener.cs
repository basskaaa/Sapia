using Sapia.Game.Combat;
using Sapia.Game.Combat.Steps;

namespace Assets._Scripts.Game
{
    public interface ICombatListener
    {
        void StepChanged(Combat combat, CombatStep step);
    }
}