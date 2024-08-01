using Assets._Scripts.Game;
using Assets._Scripts.Input;
using Assets._Scripts.Ui.Combat.Abstractions;
using Nova;
using NovaSamples.UIControls;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._Scripts.Ui.Combat.Cards
{
    public class CardSelect : UIControl<ButtonVisuals>
    {
        public CombatParticipantRef CurrentTarget { get; private set; }

        public ICardUsedListener CardUsedListener;

        public bool isCardSelected = false;

        private RectTransform abilityCardHolder;
        private UIBlock2D cardBlock;

        [SerializeField] private float opacityOnSelect = 0.5f;

        private Color _initialColour;
        private Vector3 _initialPosition;

        private bool isReleased = false;
        private bool hasTarget = false;
    

        private void OnEnable()
        {
            abilityCardHolder = GetComponentInParent<AbilityCardHolder>().GetComponent<RectTransform>();
            View.UIBlock.AddGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
            cardBlock = GetComponent<UIBlock2D>();

            _initialColour = cardBlock.Color;
            _initialPosition = cardBlock.Position.Value;
        }

        private void OnDisable()
        {
            View.UIBlock.RemoveGestureHandler<Gesture.OnRelease, ButtonVisuals>(HandleReleased);
        }

        private void HandleReleased(Gesture.OnRelease evt, ButtonVisuals visuals)
        {
            if (!isReleased)
            {
                isReleased = true;
                isCardSelected = false;

                ReturnToDeck();

                var render = GetComponentInChildren<CardRender>();

                if (render == null)
                {
                    UnityEngine.Debug.LogWarning($"Unable to find a {nameof(CardRender)} on {gameObject}");
                    return;
                }

                CheckTarget();
                CardUsedListener?.OnCardUsed(render.Ability, CurrentTarget);
            }
        }

        void Update()
        {
            if (isCardSelected) 
            {
                OnCardDrag();
                CheckTarget();
            }
        }
        
        private void ReturnToDeck()
        {
            transform.SetParent(abilityCardHolder, false);        
            
            cardBlock.Position.Y.Value = _initialPosition.y;
            cardBlock.Position.Z.Value = _initialPosition.z;

            cardBlock.Color = _initialColour;
        }

        public void OnCardDrag()
        {
            isReleased = false;
            transform.SetParent(abilityCardHolder.transform.parent, false);

            cardBlock.Position.XY.Value = UnityEngine.Input.mousePosition;

            if (GetMousePosition.TryGetCurrentRay(out var ray) && GetMousePosition.TryProjectRay(ray, out var worldPos))
            {
                Select();
            }
        }

        private void Select()
        {
            isCardSelected = true;

            cardBlock.Color = new Color(_initialColour.r, _initialColour.g, _initialColour.b, opacityOnSelect);
        }

        private void CheckTarget()
        {
            var mousePos = UnityEngine.Input.mousePosition;

            var ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit raycastHit;

            var isHit = Physics.Raycast(ray, out raycastHit);

            if (isHit && raycastHit.transform.CompareTag("Target") && isCardSelected && !hasTarget)
            {
                CurrentTarget = raycastHit.transform.GetComponentInParent<CombatParticipantRef>();
                HighlightTarget(raycastHit);
            }

            if (CurrentTarget != null && !raycastHit.transform.CompareTag("Target"))
            {
                UnhighlightTargets();
                CurrentTarget = null;
            }

            if (raycastHit.transform.CompareTag("Target") && !isCardSelected)
            {
                UnhighlightTargets();
            }
        }

        private void HighlightTarget(RaycastHit hit)
        {
            hit.transform.GetComponent<MeshRenderer>().enabled = true;
            hasTarget = true;
        }

        private void UnhighlightTargets()
        {
            hasTarget = false;
            var targets = FindObjectsByType<TargetCollider>(FindObjectsSortMode.None);
            foreach (var target in targets)
            {
                target.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
