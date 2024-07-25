using Sapia.Game.Combat;
using Sapia.Game.Combat.Steps;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class CombatUi : MonoBehaviour
    {
        private AbilityCardHolder _cards;

        void Awake()
        {
            _cards = GetComponentInChildren<AbilityCardHolder>();
        }

        public void StepChanged(Combat combat, CombatStep step)
        {
            if (step is TurnStep turn && turn.Participant.Character.IsPlayer)
            {
                _cards.Show(turn.Abilities);
            }
        }
    }
}