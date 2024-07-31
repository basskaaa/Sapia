using Sapia.Game.Combat.Entities;
using Sapia.Game.Combat.Steps;

namespace Sapia.Game.Combat.AI;

public class AiController
{
    public Combat Combat { get; }
    public CombatParticipant Participant { get; }

    public AiController(Combat combat, string participantId)
    {
        Combat = combat;
        Participant = combat.Participants[participantId];
    }

    public bool ExecuteStep(CombatParticipantStep participantStep)
    {
        throw new NotImplementedException();
    }
}