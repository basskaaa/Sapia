using Assets._Scripts.Input;
using Assets._Scripts.Ui;
using UnityEngine;

namespace Assets._Scripts.Debug
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
            if (GetMousePosition.Instance.TryGetCurrentRay(out Ray ray))
            {
                if (_combatUi.InteractionMode == InteractionMode.Move)
                {
                    if (GetMousePosition.Instance.TryProjectGroundRay(ray, out Vector3 groundPos))
                    {
                        groundPos = _combatUi.TransformWorldToCoordVector(groundPos);

                        transform.position = groundPos;
                    }

                    return;
                }
                else
                {
                    if (GetMousePosition.Instance.TryProjectRay(ray, out Vector3 worldPos))
                    {
                        transform.position = worldPos;
                    }
                }
            }
        }
    }
}