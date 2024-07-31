using System;
using System.Collections.Generic;
using System.Text;
using Sapia.Game.Combat.Entities;

namespace Sapia.Game.Combat.AI
{
    public class AiController
    {
        public Combat Combat { get; }
        public CombatParticipant Participant { get; }

        public AiController(Combat combat, string participantId)
        {
            Combat = combat;
            Participant = combat.GetParticipantById(participantId);
        }
    }
}
