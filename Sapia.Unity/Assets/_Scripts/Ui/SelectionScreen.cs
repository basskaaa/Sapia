using Nova;
using UnityEngine;

namespace Assets._Scripts.Ui
{
    public class SelectionScreen : MonoBehaviour
    {
        private ScreenSpace screenSpace;

        void Start()
        {
            GetComponent<ScreenSpace>().enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
