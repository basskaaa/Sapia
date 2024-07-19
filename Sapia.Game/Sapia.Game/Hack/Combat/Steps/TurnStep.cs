using Sapia.Game.Hack.Combat.Entities;

namespace Sapia.Game.Hack.Combat.Steps
{
    public class TurnStep : CombatParticipantStep
    {
        public bool HasEnded { get; private set; }

        public TurnStep(Combat combat, CombatParticipant participant) : base(combat, participant)
        {
        }

        public void EndTurn()
        {
            HasEnded = true;
        }
    }
}
