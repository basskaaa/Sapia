using UnityEngine;

namespace Assets._Scripts.Ui.Combat
{
    public class UtilityActions : MonoBehaviour
    {
        public GameObject buttons;
        private CombatUi _combatUi;

        void Awake()
        {
            _combatUi = GetComponentInParent<CombatUi>();

            Hide();
        }

        public void Hide()
        {
            buttons.SetActive(false);
        }

        public void Show()
        {
            buttons.SetActive(true);
        }

        public void ToggleMoveMode()
        {
            _combatUi.ChangeInteractionMode(_combatUi.InteractionMode == InteractionMode.Move ? InteractionMode.Ready : InteractionMode.Move);
        }

        public void EndTurn()
        {
            _combatUi.EndTurn();
        }
    }
}