using Sapia.Game.Combat.Entities.Enums;
using Sapia.Game.Combat.Steps;

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
                var abilities = Combat.Abilities.GetUsableAbilities(participant.ParticipantId);
                turn = new(Combat, participant, abilities.ToArray());

                yield return turn;

                if (turn.MovementRoute != null)
                {
                    var path = turn.MovementRoute;

                    foreach (var coord in path)
                    {
                        var previousPosition = participant.Position;

                        if (Combat.Movement.Move(participant.ParticipantId, coord))
                        {
                            yield return new MovedStep(Combat, participant, previousPosition);
                        }
                        else
                        {
                            break;
                        }
                    }
                }else if (turn.AbilityToUse != null)
                {
                    var abilityResult = Combat.Abilities.UseAbility(participant.ParticipantId, turn.AbilityToUse);

                    if (abilityResult.HasValue)
                    {
                        yield return new AbilityUsedStep(Combat, participant, abilityResult.Value);
                    }
                }

            } while (!turn.HasEnded);

            result = Combat.CheckForComplete();

            if (!result.HasValue)
            {
                Combat.EndTurn(participant.ParticipantId);
            }
        }

        yield return new FinishedStep(Combat, result.Value);
    }
}