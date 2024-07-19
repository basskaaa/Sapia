using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapia.Game.Hack.Combat.Entities
{
    public readonly struct AbilityResult
    {
        public AbilityResult(params AffectedParticipant[] affectedParticipants)
        {
            AffectedParticipants = affectedParticipants;
        }

        public IReadOnlyCollection<AffectedParticipant> AffectedParticipants { get; }
    }

    public readonly struct AffectedParticipant
    {
        public AffectedParticipant(string participantId, int? healthChange)
        {
            ParticipantId = participantId;
            HealthChange = healthChange;
        }

        public string ParticipantId { get; }
        public int? HealthChange { get; }
    }
}
