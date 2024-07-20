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
        
        private void OnEnable()
        {
            View.UIBlock.AddGestureHandler<Gesture.OnHover, ButtonVisuals>(HandleHovered);
        }

        private void OnDisable()
        {

            View.UIBlock.RemoveGestureHandler<Gesture.OnHover, ButtonVisuals>(HandleHovered);

        }

        private void HandleHovered(Gesture.OnHover evt, ButtonVisuals visuals) => OnHover?.Invoke();


        public void LogHover()
        {
            Debug.Log("Hover");
        }
    }
}


