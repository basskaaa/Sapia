using UnityEngine;

namespace Assets._Scripts.Ui.Combat
{
    public class UiTestLog : MonoBehaviour
    {
        public void Log()
        {
            UnityEngine.Debug.Log(gameObject.name);
        }
    }
}
