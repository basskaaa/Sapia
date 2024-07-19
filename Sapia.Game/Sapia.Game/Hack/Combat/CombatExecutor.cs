using Sapia.Game.Hack.Combat.Steps;

namespace Sapia.Game.Hack.Combat
{
    public class CombatExecutor
    {
        public CombatExecutor(Combat combat)
        {
            Combat = combat;
        }

        public Combat Combat { get; }

        public IEnumerator<CombatStep> Execute()
        {
            CombatResult? result = null;

            while (!result.HasValue)
            {
                var participant = Combat.CurrentParticipant();
                var turn = new TurnStep(Combat, participant);

                while (!turn.HasEnded)
                {
                    yield return turn;
                }

                result = Combat.CheckForComplete();
            }

            yield return new CombatFinishedStep(Combat, result.Value);
        }
    }
}
