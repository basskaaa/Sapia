using Nova;
using NovaSamples.UIControls;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Cards
{
    public class CardHover : UIControl<ButtonVisuals>
    {
        [SerializeField] private int sortFront = 100;
        [SerializeField] private Vector2 hoverScale = new Vector2(1f, 1.05f);

        private UIBlock2D uiBlock;
        private SortGroup sortGroup;

        public bool IsHovered { get; private set; }

        private Vector2 _initialScale = Vector2.one;
        private int _initialSort;

        private void Awake()
        {
            sortGroup = GetComponentInParent<SortGroup>();
            uiBlock = sortGroup.GetComponent<UIBlock2D>();
        }

        private void OnEnable()
        {
            View.UIBlock.AddGestureHandler<Gesture.OnHover, ButtonVisuals>(HandleHovered);
            View.UIBlock.AddGestureHandler<Gesture.OnUnhover, ButtonVisuals>(HandleUnhovered);

            _initialScale = new Vector2(uiBlock.Size.Percent.x, uiBlock.Size.Percent.y);
        }

        private void OnDisable()
        {
            View.UIBlock.RemoveGestureHandler<Gesture.OnHover, ButtonVisuals>(HandleHovered);
            View.UIBlock.RemoveGestureHandler<Gesture.OnUnhover, ButtonVisuals>(HandleUnhovered);
        }

        private void HandleHovered(Gesture.OnHover evt, ButtonVisuals visuals)
        {
            IsHovered = true;

            _initialSort = sortGroup.SortingOrder;

            var cardScale = _initialScale * hoverScale;

            uiBlock.Size.XY.Percent = cardScale;
            sortGroup.SortingOrder = sortFront;
        }

        private void HandleUnhovered(Gesture.OnUnhover evt, ButtonVisuals visuals)
        {
            IsHovered = false;

            uiBlock.Size.XY.Percent = _initialScale;
            sortGroup.SortingOrder = _initialSort;
        }
    }
}


