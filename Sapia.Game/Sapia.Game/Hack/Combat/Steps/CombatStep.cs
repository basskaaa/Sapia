﻿namespace Sapia.Game.Hack.Combat.Steps
{
    public abstract class CombatStep
    {
        protected CombatStep(Combat combat)
        {
            Combat = combat;
        }

        public Combat Combat { get; }
    }
}
