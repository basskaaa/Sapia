using UnityEngine;

namespace Assets._Scripts.Input
{
    public static class GetMousePosition 
    {
        public static bool TryGetCurrentRay(out Ray ray)
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

        public static bool TryProjectRay(Ray ray, out Vector3 worldPos)
        {
            var plane = new Plane(-Vector3.forward, 0);

            if (plane.Raycast(ray, out var distance))
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

        public static bool TryProjectGroundRay(Ray ray, out Vector3 worldPos)
        {
            var plane = new Plane(Vector3.up, 0);

            if (plane.Raycast(ray, out var distance))
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
