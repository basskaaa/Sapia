using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Nova;
using NovaSamples.UIControls;

namespace NovaSamples.UIControls
{ 
    public class CardScaleOnHover : UIControl<ButtonVisuals>
    {
        public UnityEvent OnHover = null;
        public UnityEvent OnUnhover = null;
        [SerializeField] private Vector2 hoverScale = new Vector2 (1f, 1.05f);
        private Vector2 baseScale; 
        private UIBlock2D uiBlock;
        private SortGroup sortGroup;
        private int sortBase;
        private int sortFront = 10;
        
        private void OnEnable()
        {
            sortGroup = GetComponentInParent<SortGroup>();
            uiBlock = sortGroup.GetComponent<UIBlock2D>();
            View.UIBlock.AddGestureHandler<Gesture.OnHover, ButtonVisuals>(HandleHovered);
            View.UIBlock.AddGestureHandler<Gesture.OnUnhover, ButtonVisuals>(HandleUnhovered);

            GetInitialData();
        }

        private void OnDisable()
        {
            View.UIBlock.RemoveGestureHandler<Gesture.OnHover, ButtonVisuals>(HandleHovered);
            View.UIBlock.RemoveGestureHandler<Gesture.OnUnhover, ButtonVisuals>(HandleUnhovered);
        }

        private void HandleHovered(Gesture.OnHover evt, ButtonVisuals visuals) => OnHover?.Invoke();
        private void HandleUnhovered(Gesture.OnUnhover evt, ButtonVisuals visuals) => OnUnhover?.Invoke();

        private void GetInitialData()
        {
            sortBase = sortGroup.SortingOrder;
            baseScale = new Vector2(uiBlock.Size.Percent.x, uiBlock.Size.Percent.y);
            Debug.Log(baseScale.ToString());
        }

        public void Hover()
        {
            Debug.Log("Hover" + uiBlock.name);
            Vector2 cardScale = new Vector2(baseScale.x * hoverScale.x, baseScale.y * hoverScale.y);
            Debug.Log(cardScale.ToString());
            uiBlock.Size.XY.Percent = cardScale;
            sortGroup.SortingOrder = sortFront;
        }

        public void Unhover() 
        {
            Debug.Log("Unhover" + uiBlock.name);
            uiBlock.Size.XY.Percent = new Vector2(baseScale.x, baseScale.y);
            sortGroup.SortingOrder = sortBase;
        }
    }
}


