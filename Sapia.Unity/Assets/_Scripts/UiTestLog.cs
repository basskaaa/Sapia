using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTestLog : MonoBehaviour
{
    public void Log()
    {
        Debug.Log(gameObject.name);
    }
}
