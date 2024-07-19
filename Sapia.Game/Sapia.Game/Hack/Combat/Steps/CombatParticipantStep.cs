using Sapia.Game.Hack.Combat.Entities;

namespace Sapia.Game.Hack.Combat.Steps
{
    public abstract class CombatParticipantStep : CombatStep
    {
        public CombatParticipant Participant { get; }

        protected CombatParticipantStep(Combat combat, CombatParticipant participant) : base(combat)
        {
            Participant = participant;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: {Participant.Id}";
        }
    }
}
