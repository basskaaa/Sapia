using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePosition : Singleton<GetMousePosition>
{
    public bool TryGetCurrentRay(out Ray ray)
    {
        if (Input.mousePresent) 
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}
