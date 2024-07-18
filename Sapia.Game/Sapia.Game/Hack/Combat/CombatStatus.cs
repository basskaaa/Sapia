using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapia.Game.Hack.Combat
{
    public class CombatStatus
    {
        public int RemainingMovement { get; internal set; }
        public IReadOnlyCollection<CombatActionType> RemainingActions { get; internal set; } = Array.Empty<CombatActionType>();
    }

    public enum CombatActionType
    {
        Starter = 1,
        Main = 1,
        Free = 2,
        Reaction = 3
    }
}
