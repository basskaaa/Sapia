using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTrack : MonoBehaviour
{
    private void Update()
    {
        if (GetMousePosition.Instance.TryGetCurrentRay(out Ray ray) && GetMousePosition.Instance.TryProjectRay(ray, out Vector3 worldPos))
        {
            transform.position = worldPos;
        }

    }
}
