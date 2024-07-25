using Assets._Scripts.Ui.Test;
using UnityEngine;

namespace Assets._Scripts.Game
{
    public class CombatRunner : MonoBehaviour
    {
        private DebugText _debug;

        void Start()
        {
            _debug = FindFirstObjectByType<DebugText>();

            ShowDebugText("Combat runner has started");
        }

        private void ShowDebugText(params string[] text) => _debug.Show(text);
    }
}
