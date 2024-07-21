using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static Nova.Gesture;

public class CardSelect : UIControl<ButtonVisuals>
{
    [SerializeField] private GameEvent onCardRelease;
    public bool isCardSelected = false;

    private Transform selectedCardPivot;
    private Transform abilityCardHolder;
    public UnityEvent OnReleased = null;
    private UIBlock2D cardBlock;

    private Vector3 cardPos;
    private Vector3 pivotPos;

    [SerializeField] private float opacityOnSelect = 0.5f;
    private Color color;
    private RadialGradient gradient;
    private Color border;
    


    private void OnEnable()
    {
        abilityCardHolder = GetComponentInParent<AbilityCardHolder>().transform;
        View.UIBlock.AddGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
        cardBlock = GetComponent<UIBlock2D>();
        selectedCardPivot = FindObjectOfType<SelectedCardPivot>().transform;

        GetInitPosData();
        GetInitColorData();
    }

    private void OnDisable()
    {
        View.UIBlock.RemoveGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
    }

    private void HandleReleased(Gesture.OnRelease evt, ButtonVisuals visuals) => OnReleased?.Invoke();

    private void Update()
    {
        if (!isCardSelected)
        {
            return;
        }

        if (isCardSelected) 
        {
            OnCardDrag();
        }
    }
    public void OnCardRelease()
    {
        CheckTarget();

        SetInitPos();
        SetInitColor();

        isCardSelected = false;
        transform.SetParent(abilityCardHolder, true);
        onCardRelease.Raise();
    }

    public void OnCardDrag()
    {
        transform.SetParent(selectedCardPivot, true);

        if (GetMousePosition.Instance.TryGetCurrentRay(out Ray ray) && GetMousePosition.Instance.TryProjectRay(ray, out Vector3 worldPos))
        {
            selectedCardPivot.position = worldPos;
            SetSelectedColor();
            isCardSelected = true;
        }
    }

    public void GetInitPosData()
    {
        pivotPos = selectedCardPivot.position;
        cardPos = cardBlock.transform.position;
    }

    private void SetInitPos()
    {
        cardBlock.transform.position = cardPos;
        selectedCardPivot.position = pivotPos;
    }

    public void GetInitColorData()
    {
        color = cardBlock.Color;
        gradient = cardBlock.Gradient;
        border = cardBlock.Border.Color;
    }

    private void SetSelectedColor()
    {
        cardBlock.Color = new Color(color.r, color.g, color.b, opacityOnSelect);
    }

    private void SetInitColor()
    {
        cardBlock.Color = color;
    }

    private void CheckTarget()
    {
        Vector3 mousePos = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit raycastHit;

        bool isHit = Physics.Raycast(ray, out raycastHit);

        if (raycastHit.transform.CompareTag("Target"))
        {
            Debug.Log(raycastHit.transform.name);
            //raycastHit.transform.AddComponent<Outline>();
        }
    }
}
