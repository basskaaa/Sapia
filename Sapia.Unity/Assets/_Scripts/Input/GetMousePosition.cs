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
}
