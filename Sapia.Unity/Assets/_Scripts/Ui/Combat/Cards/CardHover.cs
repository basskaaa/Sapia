using Nova;
using NovaSamples.UIControls;
using UnityEngine;

namespace Assets._Scripts.Ui.Combat.Cards
{
    public class CardHover : UIControl<ButtonVisuals>
    {
        [SerializeField] private int sortFront = 100;
        [SerializeField] private Vector2 hoverScale = new Vector2(1.1f, 1.1f);

        private UIBlock2D uiBlock;
        private SortGroup sortGroup;

        public bool IsHovered { get; private set; }

        private Length3 _initialSize;
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

            _initialSize = uiBlock.Size;
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

            uiBlock.Size = Length3.FixedValue(_initialSize.X.Value * hoverScale.x, _initialSize.Y.Value * hoverScale.y, _initialSize.Z.Value);
            sortGroup.SortingOrder = sortFront;
        }

        private void HandleUnhovered(Gesture.OnUnhover evt, ButtonVisuals visuals)
        {
            Unhover();
        }

        public void Unhover()
        {
            IsHovered = false;

            uiBlock.Size = _initialSize;
            sortGroup.SortingOrder = _initialSort;
        }
    }
}


