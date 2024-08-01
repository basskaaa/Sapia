using UnityEngine;

namespace Assets._Scripts.Input
{
    interface IInteractable
    {
        public void OnInteract();
    }

    public class Interactor : MonoBehaviour
    {
        public Transform InteractorSource;
        public float InteractRange;
        [HideInInspector] public bool canInteract = true;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                var r = new Ray(InteractorSource.position, InteractorSource.forward);
                if (Physics.Raycast(r, out var hitInfo, InteractRange)) 
                {
                    if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) 
                    { 
                        interactObj.OnInteract();
                    }
                }
            }
        }
    }
}