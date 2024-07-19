using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapia.Game.Hack.Combat.Entities
{
    public readonly struct AbilityUse
    {
        public AbilityUse(string abilityId)
        {
            AbilityId = abilityId;
        }

        public string AbilityId { get; }
    }

    public readonly struct TargetedAbilityUse //: AbilityUse
    {
        public string TargetParticipantId { get; }
    }
}
