using Sapia.Game.Hack.Combat.Steps;

namespace Sapia.Game.Hack.Combat;

public class CombatExecutor
{
    public CombatExecutor(Combat combat)
    {
        Combat = combat;
    }

    public Combat Combat { get; }

    public IEnumerator<CombatStep> Execute()
    {
        yield return new CombatStartStep(Combat);

        CombatResult? result = null;

        while (!result.HasValue)
        {
            var participant = Combat.CurrentParticipant();
            TurnStep turn;

            do
            {
                var abilities = Combat.GetUsableAbilities(participant.Id);
                turn = new TurnStep(Combat, participant, abilities.ToArray());

                yield return turn;

            } while (!turn.HasEnded);

            result = Combat.CheckForComplete();

            if (!result.HasValue)
            {
                Combat.EndTurn(participant.Id);
            }
        }

        yield return new CombatFinishedStep(Combat, result.Value);
    }
}