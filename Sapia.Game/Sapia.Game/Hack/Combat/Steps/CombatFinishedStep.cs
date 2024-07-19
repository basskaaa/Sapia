namespace Sapia.Game.Hack.Combat.Steps
{
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
}
