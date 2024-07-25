using Assets._Scripts.Game;
using UnityEngine;

namespace Assets._Scripts.Ui.Test
{
    public class DebugControls : MonoBehaviour
    {
        private CombatRunner _combat;

        void Awake()
        {
            _combat = FindFirstObjectByType<CombatRunner>();
        }

        public void Step() => _combat.Step();
        public void Act() => _combat.Act();
    }
}