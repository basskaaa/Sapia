using Assets._Scripts.Input;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat
{
    public class CursorTrack : MonoBehaviour
    {
        private CombatUi _combatUi;

        void Awake()
        {
            _combatUi = FindFirstObjectByType<CombatUi>(FindObjectsInactive.Include);
        }

        private void Update()
        {
            if (GetMousePosition.TryGetCurrentRay(out var ray))
            {
                if (_combatUi.InteractionMode == InteractionMode.Move)
                {
                    if (GetMousePosition.TryProjectGroundRay(ray, out var groundPos))
                    {
                        groundPos = _combatUi.TransformWorldToCoordVector(groundPos);

                        transform.position = groundPos;
                    }

                    return;
                }
                else
                {
                    if (GetMousePosition.TryProjectRay(ray, out var worldPos))
                    {
                        transform.position = worldPos;
                    }
                }
            }
        }
    }
}