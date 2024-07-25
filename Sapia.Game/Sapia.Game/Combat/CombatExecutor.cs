﻿using Sapia.Game.Combat.Steps;

namespace Sapia.Game.Combat;

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
                var abilities = Combat.GetUsableAbilities(participant.ParticipantId);
                turn = new TurnStep(Combat, participant, abilities.ToArray());

                yield return turn;

            } while (!turn.HasEnded);

            result = Combat.CheckForComplete();

            if (!result.HasValue)
            {
                Combat.EndTurn(participant.ParticipantId);
            }
        }

        yield return new CombatFinishedStep(Combat, result.Value);
    }
}