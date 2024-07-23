using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.E) && canInteract)
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) 
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) 
                { 
                    interactObj.OnInteract();
                }
            }
        }
    }
}
