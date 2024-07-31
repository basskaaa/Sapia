using System;
using System.Collections.Generic;
using System.Text;
using Sapia.Game.Combat.Steps;
using Sapia.Game.Types;

namespace Sapia.Game.Combat
{
    public class CombatBag
    {
        public Combat Combat => _executor.Combat;
        public CombatStep? CurrentStep => _execution.Current;
        public ITypeDataRoot TypeData => Combat.TypeData;

        private readonly CombatExecutor _executor;
        private readonly IEnumerator<CombatStep> _execution;

        internal CombatBag(CombatExecutor executor)
        {
            _executor = executor;
            _execution = executor.Execute();
        }

        public bool Step() => _execution.MoveNext();
    }
}
