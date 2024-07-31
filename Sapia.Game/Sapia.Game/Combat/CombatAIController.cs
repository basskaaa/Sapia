using Sapia.Game.Combat.AI;
using Sapia.Game.Combat.Steps;

namespace Sapia.Game.Combat;

public class CombatAIController
{
    private readonly Combat _combat;

    private readonly Dictionary<string, AiController> _controllers=new();

    public CombatAIController(Combat combat)
    {
        _combat = combat;

        foreach (var participant in _combat.Participants.All)
        {
            if (participant.Character.IsNpc)
            {
                _controllers[participant.ParticipantId] = new AiController(_combat, participant.ParticipantId);
            }
        }
    }

    public bool ExecuteStep(CombatParticipantStep participantStep)
    {
        if (_controllers.TryGetValue(participantStep.Participant.ParticipantId, out var controller))
        {
            return controller.ExecuteStep(participantStep);
        }

        return false;
    }
}