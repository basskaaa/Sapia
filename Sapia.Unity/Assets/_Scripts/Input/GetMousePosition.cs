using Assets._Scripts.Patterns;
using UnityEngine;

namespace Assets._Scripts.Input
{
    public class GetMousePosition : Singleton<GetMousePosition>
    {
        public bool TryGetCurrentRay(out Ray ray)
        {
            if (UnityEngine.Input.mousePresent)
            {
                ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                return true;
            }
            else
            {
                ray = default;
                return false;
            }
        }

        public bool TryProjectRay(Ray ray, out Vector3 worldPos)
        {
            Plane plane = new Plane(-transform.forward, transform.position);

            if (plane.Raycast(ray, out float distance))
            {
                worldPos = ray.GetPoint(distance);
                return true;
            }
            else
            {
                worldPos = default;
                return false;
            }
        }

        public bool TryProjectGroundRay(Ray ray, out Vector3 worldPos)
        {
            Plane plane = new Plane(transform.up, 0);

            if (plane.Raycast(ray, out float distance))
            {
                worldPos = ray.GetPoint(distance);
                return true;
            }
            else
            {
                worldPos = default;
                return false;
            }
        }
    }
}
