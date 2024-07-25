using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class UiTestLog : MonoBehaviour
    {
        public void Log()
        {
            UnityEngine.Debug.Log(gameObject.name);
        }
    }
}
