using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapia.Game.Hack.Combat.Steps
{
    public abstract class CombatParticipantStep : CombatStep
    {
        public CombatParticipant Participant { get; }

        protected CombatParticipantStep(Combat combat, CombatParticipant participant) : base(combat)
        {
            Participant = participant;
        }
    }
}
