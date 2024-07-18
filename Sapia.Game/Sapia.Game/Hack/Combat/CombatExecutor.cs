using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sapia.Game.Hack.Combat.Steps;

namespace Sapia.Game.Hack.Combat
{
    public class CombatExecutor
    {
        public CombatExecutor(Combat combat)
        {
            Combat = combat;
        }

        public Combat Combat { get; }

        public CombatStep Next()
        {
            throw new NotImplementedException();
        }
    }
}
