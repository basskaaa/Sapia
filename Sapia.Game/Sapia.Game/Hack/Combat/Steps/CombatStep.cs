using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapia.Game.Hack.Combat.Steps
{
    public abstract class CombatStep
    {
        protected CombatStep(Combat combat)
        {
            Combat = combat;
        }

        public Combat Combat { get; }
    }

    public class FinishedStep : CombatStep
    {
        public FinishedStep(Combat combat) : base(combat)
        {
        }
    }
}
