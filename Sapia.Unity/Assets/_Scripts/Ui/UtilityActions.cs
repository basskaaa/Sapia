using UnityEditor.Rendering;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class UtilityActions : MonoBehaviour
    {
        private CombatUi _combatUi;

        void Awake()
        {
            _combatUi = GetComponentInParent<CombatUi>();

            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void ToggleMoveMode()
        {
            _combatUi.ChangeInteractionMode(_combatUi.InteractionMode == InteractionMode.Move ? InteractionMode.Ready : InteractionMode.Move);
        }
    }
}