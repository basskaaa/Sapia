using Nova;
using NovaSamples.UIControls;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Nova.Gesture;

public class CardSelect : UIControl<ButtonVisuals>
{
    [SerializeField] private Transform selectedCardPivot;
    [SerializeField] private GameEvent onCardRelease;

    private Transform abilityCardHolder;
    public UnityEvent OnReleased = null;
    private UIBlock2D cardBlock;


    private void OnEnable()
    {
        abilityCardHolder = GetComponentInParent<AbilityCardHolder>().transform;
        View.UIBlock.AddGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
        cardBlock = GetComponent<UIBlock2D>();
    }

    private void OnDisable()
    {
        View.UIBlock.RemoveGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
    }

    private void HandleReleased(Gesture.OnRelease evt, ButtonVisuals visuals) => OnReleased?.Invoke();

    public void OnCardDrag()
    {
        transform.SetParent(selectedCardPivot,true);
    }

    public void OnCardRelease()
    {
        transform.SetParent(abilityCardHolder, true);
        cardBlock.Position.Y = 0;
        onCardRelease.Raise();
    }

}
