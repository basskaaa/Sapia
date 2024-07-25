using Assets._Scripts.Game;
using Nova;
using Sapia.Game.Combat;
using Sapia.Game.Combat.Steps;
using UnityEngine;

namespace Assets._Scripts.Ui.Test
{
    public class DebugControls : MonoBehaviour, ICombatListener
    {
        public GameObject actButton;

        private CombatRunner _combat;

        void Awake()
        {
            _combat = FindFirstObjectByType<CombatRunner>();
            _combat.AddListener(this);
        }

        public void Step() => _combat.Step();
        public void Act() => _combat.Act();

        public void StepChanged(Combat combat, CombatStep step)
        {
            actButton.SetActive(step is TurnStep turn &&
                                ((!turn.Participant.Character.IsPlayer && !turn.HasEnded) ||
                                (turn.Participant.Character.IsPlayer && turn.Abilities.Count == 0 && !turn.HasEnded)));

            var isEndTurn = step is TurnStep t2 && t2.Abilities.Count == 0;

            actButton.GetComponentInChildren<TextBlock>().Text = isEndTurn ? "End Turn" : "Act";
        }
    }
}